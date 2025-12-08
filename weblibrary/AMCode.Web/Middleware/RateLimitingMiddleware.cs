using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AMCode.Web.RateLimiting;

namespace AMCode.Web.Middleware;

/// <summary>
/// Middleware for enforcing rate limits on API requests
/// </summary>
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private readonly RateLimitingSettings _settings;

    public RateLimitingMiddleware(
        RequestDelegate next,
        ILogger<RateLimitingMiddleware> logger,
        IOptions<RateLimitingSettings> settings)
    {
        _next = next;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip rate limiting for excluded paths
        if (IsExcludedPath(context.Request.Path))
        {
            await _next(context);
            return;
        }

        // Resolve scoped service from HttpContext (can't inject scoped services in middleware constructor)
        var rateLimitingService = context.RequestServices.GetRequiredService<RateLimiting.IRateLimitingService>();

        // Extract client identifier
        var clientKey = RateLimiting.ClientIdentifierExtractor.ExtractClientIdentifier(context);

        // Get policy for this endpoint
        var policy = GetPolicyForEndpoint(context.Request.Path, context.Request.Method);

        // Check rate limit
        var result = await rateLimitingService.CheckRateLimitAsync(clientKey, policy);

        // Set rate limit headers
        context.Response.Headers["X-RateLimit-Limit"] = result.Limit.ToString();
        context.Response.Headers["X-RateLimit-Remaining"] = result.Remaining.ToString();
        context.Response.Headers["X-RateLimit-Reset"] = result.ResetTime.ToString();

        if (!result.IsAllowed)
        {
            _logger.LogWarning(
                "Rate limit exceeded for client {ClientKey} on path {Path}. Retry after {RetryAfter} seconds",
                clientKey, context.Request.Path, result.RetryAfter);

            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            context.Response.ContentType = "application/json";
            context.Response.Headers["Retry-After"] = result.RetryAfter.ToString();

            var errorResponse = new
            {
                error = "Rate limit exceeded",
                message = "Too many requests. Please try again later.",
                retryAfter = result.RetryAfter
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
            return;
        }

        await _next(context);
    }

    private bool IsExcludedPath(PathString path)
    {
        if (_settings.ExcludedPaths == null || _settings.ExcludedPaths.Count == 0)
        {
            return false;
        }

        return _settings.ExcludedPaths.Any(excludedPath =>
            path.StartsWithSegments(excludedPath, StringComparison.OrdinalIgnoreCase));
    }

    private RateLimiting.RateLimitPolicy GetPolicyForEndpoint(PathString path, string method)
    {
        // Check for endpoint-specific override
        if (_settings.EndpointOverrides != null)
        {
            foreach (var overrideEntry in _settings.EndpointOverrides)
            {
                if (path.StartsWithSegments(overrideEntry.Key, StringComparison.OrdinalIgnoreCase))
                {
                    var overridePolicy = overrideEntry.Value;
                    
                    // If a named policy is specified, use it
                    if (!string.IsNullOrWhiteSpace(overridePolicy.PolicyName) && 
                        _settings.Policies != null &&
                        _settings.Policies.TryGetValue(overridePolicy.PolicyName, out var namedPolicy))
                    {
                        return namedPolicy;
                    }

                    // Otherwise, use the override policy directly (merge with defaults)
                    return new RateLimiting.RateLimitPolicy
                    {
                        PermitLimit = overridePolicy.PermitLimit > 0 ? overridePolicy.PermitLimit : _settings.DefaultPolicy.PermitLimit,
                        Window = overridePolicy.Window.TotalSeconds > 0 ? overridePolicy.Window : _settings.DefaultPolicy.Window,
                        Algorithm = overridePolicy.Algorithm != RateLimiting.RateLimitAlgorithm.SlidingWindow || overridePolicy.PermitLimit > 0 || overridePolicy.Window.TotalSeconds > 0 
                            ? overridePolicy.Algorithm 
                            : _settings.DefaultPolicy.Algorithm,
                        QueueLimit = overridePolicy.QueueLimit > 0 ? overridePolicy.QueueLimit : _settings.DefaultPolicy.QueueLimit,
                        AutoReplenishment = _settings.DefaultPolicy.AutoReplenishment,
                        QueueProcessingOrder = _settings.DefaultPolicy.QueueProcessingOrder
                    };
                }
            }
        }

        // Use default policy
        return _settings.DefaultPolicy;
    }
}
