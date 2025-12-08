namespace AMCode.Web.RateLimiting;

/// <summary>
/// Service for checking and enforcing rate limits
/// </summary>
public interface IRateLimitingService
{
    /// <summary>
    /// Checks if a request is allowed based on the rate limit policy
    /// </summary>
    /// <param name="key">Client identifier key (e.g., "user:user-123" or "ip:192.168.1.1")</param>
    /// <param name="policy">Rate limit policy to apply</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Rate limit result indicating if request is allowed and remaining quota</returns>
    Task<RateLimitResult> CheckRateLimitAsync(string key, RateLimitPolicy policy, CancellationToken cancellationToken = default);
}
