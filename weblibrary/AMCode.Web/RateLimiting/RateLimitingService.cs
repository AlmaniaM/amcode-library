using System.Collections.Concurrent;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Logging;

namespace AMCode.Web.RateLimiting;

/// <summary>
/// Service for enforcing rate limits using .NET's built-in rate limiting
/// </summary>
public class RateLimitingService : IRateLimitingService
{
    private readonly ILogger<RateLimitingService> _logger;
    private readonly ConcurrentDictionary<string, PartitionedRateLimiter<string>> _rateLimiters = new();

    public RateLimitingService(ILogger<RateLimitingService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<RateLimitResult> CheckRateLimitAsync(string key, RateLimitPolicy policy, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or empty", nameof(key));
        }

        if (policy == null)
        {
            throw new ArgumentNullException(nameof(policy));
        }

        try
        {
            var rateLimiter = GetOrCreateRateLimiter(policy);
            var lease = rateLimiter.AttemptAcquire(key, permitCount: 1);

            var result = new RateLimitResult
            {
                IsAllowed = lease.IsAcquired,
                Limit = policy.PermitLimit
            };

            if (lease.IsAcquired)
            {
                // Calculate reset time
                var resetTime = DateTimeOffset.UtcNow.Add(policy.Window);
                result.ResetTime = resetTime.ToUnixTimeSeconds();
                
                // For acquired leases, we need to estimate remaining
                // This is approximate since .NET rate limiters don't expose exact remaining count
                result.Remaining = policy.PermitLimit - 1; // Approximate
            }
            else
            {
                // Request was rate limited
                if (lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    result.RetryAfter = (int)retryAfter.TotalSeconds;
                }
                else
                {
                    // Fallback: calculate retry after based on window
                    result.RetryAfter = (int)policy.Window.TotalSeconds;
                }

                var resetTime = DateTimeOffset.UtcNow.Add(TimeSpan.FromSeconds(result.RetryAfter));
                result.ResetTime = resetTime.ToUnixTimeSeconds();
                result.Remaining = 0;
            }

            lease.Dispose();
            return Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking rate limit for key {Key}", key);
            // Fail open - allow request if rate limit check fails
            return Task.FromResult(new RateLimitResult
            {
                IsAllowed = true,
                Limit = policy.PermitLimit,
                Remaining = policy.PermitLimit,
                ResetTime = DateTimeOffset.UtcNow.Add(policy.Window).ToUnixTimeSeconds(),
                RetryAfter = 0
            });
        }
    }

    private PartitionedRateLimiter<string> GetOrCreateRateLimiter(RateLimitPolicy policy)
    {
        // Use a composite key that includes the policy parameters to ensure different policies get different limiters
        var limiterKey = $"{policy.PermitLimit}:{policy.Window.TotalSeconds}:{policy.Algorithm}";

        return _rateLimiters.GetOrAdd(limiterKey, _ => CreateRateLimiter(policy));
    }

    private PartitionedRateLimiter<string> CreateRateLimiter(RateLimitPolicy policy)
    {
        return policy.Algorithm switch
        {
            RateLimitAlgorithm.SlidingWindow => PartitionedRateLimiter.Create<string, string>(
                partitionKey => RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey,
                    _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = policy.PermitLimit,
                        Window = policy.Window,
                        QueueProcessingOrder = policy.QueueProcessingOrder,
                        QueueLimit = policy.QueueLimit,
                        AutoReplenishment = policy.AutoReplenishment
                    })),

            RateLimitAlgorithm.TokenBucket => PartitionedRateLimiter.Create<string, string>(
                partitionKey => RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey,
                    _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = policy.PermitLimit,
                        ReplenishmentPeriod = policy.Window,
                        TokensPerPeriod = policy.PermitLimit,
                        QueueProcessingOrder = policy.QueueProcessingOrder,
                        QueueLimit = policy.QueueLimit,
                        AutoReplenishment = policy.AutoReplenishment
                    })),

            RateLimitAlgorithm.FixedWindow => PartitionedRateLimiter.Create<string, string>(
                partitionKey => RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey,
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = policy.PermitLimit,
                        Window = policy.Window,
                        QueueProcessingOrder = policy.QueueProcessingOrder,
                        QueueLimit = policy.QueueLimit,
                        AutoReplenishment = policy.AutoReplenishment
                    })),

            RateLimitAlgorithm.ConcurrencyLimiter => PartitionedRateLimiter.Create<string, string>(
                partitionKey => RateLimitPartition.GetConcurrencyLimiter(
                    partitionKey,
                    _ => new ConcurrencyLimiterOptions
                    {
                        PermitLimit = policy.PermitLimit,
                        QueueProcessingOrder = policy.QueueProcessingOrder,
                        QueueLimit = policy.QueueLimit
                    })),

            _ => throw new ArgumentException($"Unknown rate limit algorithm: {policy.Algorithm}", nameof(policy))
        };
    }
}
