using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace AMCode.Web.RateLimiting;

/// <summary>
/// Extracts client identifiers from HTTP context for rate limiting
/// </summary>
public static class ClientIdentifierExtractor
{
    /// <summary>
    /// Extracts client identifier in priority order:
    /// 1. Authenticated User ID (from JWT claims)
    /// 2. API Key (from X-API-Key header)
    /// 3. IP Address (from HttpContext)
    /// </summary>
    /// <param name="context">HTTP context</param>
    /// <returns>Client identifier in format "{type}:{identifier}"</returns>
    public static string ExtractClientIdentifier(HttpContext context)
    {
        // Priority 1: Authenticated User ID
        var userIdClaim = context.User?.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && !string.IsNullOrWhiteSpace(userIdClaim.Value))
        {
            return $"user:{userIdClaim.Value}";
        }

        // Priority 2: API Key
        if (context.Request.Headers.TryGetValue("X-API-Key", out var apiKey) && !string.IsNullOrWhiteSpace(apiKey))
        {
            return $"apikey:{apiKey}";
        }

        // Priority 3: IP Address
        var ipAddress = GetClientIpAddress(context);
        return $"ip:{ipAddress}";
    }

    /// <summary>
    /// Gets the client IP address from the HTTP context
    /// </summary>
    /// <param name="context">HTTP context</param>
    /// <returns>Client IP address or "unknown" if not available</returns>
    private static string GetClientIpAddress(HttpContext context)
    {
        // Check for forwarded IP (when behind proxy/load balancer)
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
        {
            var ip = forwardedFor.ToString().Split(',')[0].Trim();
            if (!string.IsNullOrWhiteSpace(ip))
            {
                return ip;
            }
        }

        if (context.Request.Headers.TryGetValue("X-Real-IP", out var realIp))
        {
            var ip = realIp.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(ip))
            {
                return ip;
            }
        }

        // Fallback to connection remote IP
        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}
