namespace AMCode.Web.RateLimiting;

/// <summary>
/// Result of a rate limit check
/// </summary>
public class RateLimitResult
{
    /// <summary>
    /// Whether the request is allowed
    /// </summary>
    public bool IsAllowed { get; set; }

    /// <summary>
    /// Number of requests remaining in the current window
    /// </summary>
    public int Remaining { get; set; }

    /// <summary>
    /// Unix timestamp when the rate limit window resets
    /// </summary>
    public long ResetTime { get; set; }

    /// <summary>
    /// Number of seconds to wait before retrying (for 429 responses)
    /// </summary>
    public int RetryAfter { get; set; }

    /// <summary>
    /// Maximum number of requests allowed in the window
    /// </summary>
    public int Limit { get; set; }
}
