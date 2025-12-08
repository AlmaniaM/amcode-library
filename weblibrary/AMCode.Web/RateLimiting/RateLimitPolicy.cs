using System.Threading.RateLimiting;

namespace AMCode.Web.RateLimiting;

/// <summary>
/// Configuration for a rate limit policy
/// </summary>
public class RateLimitPolicy
{
    /// <summary>
    /// Maximum number of requests allowed in the window
    /// </summary>
    public int PermitLimit { get; set; } = 100;

    /// <summary>
    /// Time window for the rate limit (e.g., "00:15:00" for 15 minutes)
    /// </summary>
    public TimeSpan Window { get; set; } = TimeSpan.FromMinutes(15);

    /// <summary>
    /// Rate limiting algorithm to use
    /// </summary>
    public RateLimitAlgorithm Algorithm { get; set; } = RateLimitAlgorithm.SlidingWindow;

    /// <summary>
    /// Maximum number of queued requests (for burst handling)
    /// </summary>
    public int QueueLimit { get; set; } = 10;

    /// <summary>
    /// Whether tokens should auto-replenish
    /// </summary>
    public bool AutoReplenishment { get; set; } = true;

    /// <summary>
    /// Order for processing queued requests
    /// </summary>
    public QueueProcessingOrder QueueProcessingOrder { get; set; } = QueueProcessingOrder.OldestFirst;
}

/// <summary>
/// Rate limiting algorithm types
/// </summary>
public enum RateLimitAlgorithm
{
    /// <summary>
    /// Sliding window algorithm - smooth distribution over time
    /// </summary>
    SlidingWindow,

    /// <summary>
    /// Token bucket algorithm - allows bursts up to limit
    /// </summary>
    TokenBucket,

    /// <summary>
    /// Fixed window algorithm - resets at fixed intervals
    /// </summary>
    FixedWindow,

    /// <summary>
    /// Concurrency limiter - limits concurrent requests
    /// </summary>
    ConcurrencyLimiter
}
