namespace AMCode.Web.RateLimiting;

/// <summary>
/// Configuration settings for rate limiting
/// </summary>
public class RateLimitingSettings
{
    /// <summary>
    /// Default rate limit policy applied to all endpoints
    /// </summary>
    public RateLimitPolicy DefaultPolicy { get; set; } = new RateLimitPolicy
    {
        PermitLimit = 100,
        Window = TimeSpan.FromMinutes(15),
        Algorithm = RateLimitAlgorithm.SlidingWindow,
        QueueLimit = 10
    };

    /// <summary>
    /// Named policies that can be referenced by name
    /// </summary>
    public Dictionary<string, RateLimitPolicy>? Policies { get; set; }

    /// <summary>
    /// Endpoint-specific rate limit overrides
    /// Key is the path prefix (e.g., "/api/auth/login")
    /// Value is the policy override or policy name
    /// </summary>
    public Dictionary<string, EndpointRateLimitOverride>? EndpointOverrides { get; set; }

    /// <summary>
    /// Paths that are excluded from rate limiting
    /// </summary>
    public List<string>? ExcludedPaths { get; set; }
}

/// <summary>
/// Endpoint-specific rate limit override configuration
/// </summary>
public class EndpointRateLimitOverride
{
    /// <summary>
    /// Name of a predefined policy to use
    /// </summary>
    public string? PolicyName { get; set; }

    /// <summary>
    /// Maximum number of requests allowed (overrides policy if set)
    /// </summary>
    public int PermitLimit { get; set; }

    /// <summary>
    /// Time window for the rate limit (overrides policy if set)
    /// </summary>
    public TimeSpan Window { get; set; }

    /// <summary>
    /// Rate limiting algorithm to use
    /// </summary>
    public RateLimitAlgorithm Algorithm { get; set; } = RateLimitAlgorithm.SlidingWindow;

    /// <summary>
    /// Maximum number of queued requests
    /// </summary>
    public int QueueLimit { get; set; } = 10;
}
