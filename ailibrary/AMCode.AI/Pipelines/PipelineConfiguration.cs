namespace AMCode.AI.Pipelines;

/// <summary>
/// Configuration for a specific AI pipeline.
/// Bound from appsettings.json AI:Pipelines:{PipelineName} section.
/// Allows per-task provider/model selection without code changes.
/// </summary>
public class PipelineConfiguration
{
    /// <summary>
    /// Primary provider name (e.g., "OpenAI", "Anthropic", "GroqCloud")
    /// </summary>
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    /// Model override for the primary provider (e.g., "gpt-4o-mini", "claude-sonnet-4-5")
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Fallback provider name, used if primary fails
    /// </summary>
    public string? FallbackProvider { get; set; }

    /// <summary>
    /// Fallback model override
    /// </summary>
    public string? FallbackModel { get; set; }

    /// <summary>
    /// Maximum tokens for AI responses
    /// </summary>
    public int? MaxTokens { get; set; }

    /// <summary>
    /// Temperature for response creativity (0.0 = deterministic, 2.0 = very creative)
    /// </summary>
    public float? Temperature { get; set; }

    /// <summary>
    /// Maximum number of retries on failure
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// Request timeout override
    /// </summary>
    public TimeSpan? Timeout { get; set; }

    /// <summary>
    /// Provider name for embeddings (for RAG pipelines)
    /// </summary>
    public string? EmbeddingProvider { get; set; }

    /// <summary>
    /// Embedding model override (for RAG pipelines)
    /// </summary>
    public string? EmbeddingModel { get; set; }
}
