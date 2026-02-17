using System.Collections.Concurrent;

namespace AMCode.AI.Pipelines.RecipeSearch.RateLimiting;

/// <summary>
/// In-memory sliding window rate limit tracker per provider.
/// Tracks API call timestamps to determine if a provider is currently rate-limited.
/// </summary>
public class RateLimitTracker
{
    private readonly ConcurrentDictionary<string, ProviderRateLimit> _limits = new();

    /// <summary>
    /// Register a provider's rate limit configuration.
    /// Must be called at startup for each rate-limited provider.
    /// </summary>
    /// <param name="providerName">Provider name (e.g., "Spoonacular")</param>
    /// <param name="maxCalls">Maximum calls allowed within the window</param>
    /// <param name="window">Sliding window duration</param>
    public void Configure(string providerName, int maxCalls, TimeSpan window)
    {
        _limits[providerName] = new ProviderRateLimit(maxCalls, window);
    }

    /// <summary>
    /// Check if a provider is currently rate-limited (at or over its call budget).
    /// </summary>
    public bool IsRateLimited(string providerName)
    {
        if (!_limits.TryGetValue(providerName, out var limit))
            return false; // No limit configured = unlimited

        limit.PruneExpired();
        return limit.CallTimestamps.Count >= limit.MaxCalls;
    }

    /// <summary>
    /// Record a call to a provider. Call this after a successful API call.
    /// </summary>
    public void RecordCall(string providerName)
    {
        if (!_limits.TryGetValue(providerName, out var limit))
            return;

        limit.CallTimestamps.Enqueue(DateTime.UtcNow);
        limit.PruneExpired();
    }

    /// <summary>
    /// Get remaining calls for a provider before rate limit is hit.
    /// Returns null if no limit is configured.
    /// </summary>
    public int? GetRemainingCalls(string providerName)
    {
        if (!_limits.TryGetValue(providerName, out var limit))
            return null;

        limit.PruneExpired();
        return Math.Max(0, limit.MaxCalls - limit.CallTimestamps.Count);
    }

    private class ProviderRateLimit
    {
        public int MaxCalls { get; }
        public TimeSpan Window { get; }
        public ConcurrentQueue<DateTime> CallTimestamps { get; } = new();

        public ProviderRateLimit(int maxCalls, TimeSpan window)
        {
            MaxCalls = maxCalls;
            Window = window;
        }

        public void PruneExpired()
        {
            var cutoff = DateTime.UtcNow - Window;
            while (CallTimestamps.TryPeek(out var oldest) && oldest < cutoff)
            {
                CallTimestamps.TryDequeue(out _);
            }
        }
    }
}
