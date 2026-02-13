using System.Text.Json;

namespace AMCode.AI.Models;

/// <summary>
/// Result from a general AI text completion request
/// </summary>
public class AICompletionResult
{
    /// <summary>
    /// The generated text response
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// Whether the request completed successfully
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Error message if the request failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// The AI provider that processed this request
    /// </summary>
    public string Provider { get; set; } = string.Empty;
    
    /// <summary>
    /// Model used for generation
    /// </summary>
    public string? Model { get; set; }
    
    /// <summary>
    /// Reason why generation stopped (stop, length, content_filter, etc.)
    /// </summary>
    public string? FinishReason { get; set; }
    
    /// <summary>
    /// Usage statistics for this request
    /// </summary>
    public AIUsageStats Usage { get; set; } = new();
    
    /// <summary>
    /// Time taken for the request
    /// </summary>
    public TimeSpan Duration { get; set; }
    
    /// <summary>
    /// Timestamp when the request was processed
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Raw response from the provider (for debugging)
    /// </summary>
    public string? RawResponse { get; set; }
    
    /// <summary>
    /// Creates a successful result
    /// </summary>
    public static AICompletionResult Ok(string content, string provider, AIUsageStats? usage = null) => new()
    {
        Content = content,
        Success = true,
        Provider = provider,
        Usage = usage ?? new AIUsageStats()
    };
    
    /// <summary>
    /// Creates a failed result
    /// </summary>
    public static AICompletionResult Fail(string error, string provider) => new()
    {
        Success = false,
        ErrorMessage = error,
        Provider = provider
    };
}

/// <summary>
/// Result from a structured JSON response request
/// </summary>
/// <typeparam name="T">The expected response type</typeparam>
public class AIJsonResult<T> where T : class
{
    /// <summary>
    /// The parsed/deserialized response object
    /// </summary>
    public T? Data { get; set; }
    
    /// <summary>
    /// Whether the request and parsing completed successfully
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Error message if the request or parsing failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// The raw JSON string response
    /// </summary>
    public string? RawJson { get; set; }
    
    /// <summary>
    /// The AI provider that processed this request
    /// </summary>
    public string Provider { get; set; } = string.Empty;
    
    /// <summary>
    /// Usage statistics
    /// </summary>
    public AIUsageStats Usage { get; set; } = new();
    
    /// <summary>
    /// Time taken for the request
    /// </summary>
    public TimeSpan Duration { get; set; }
    
    /// <summary>
    /// Timestamp when processed
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Creates a successful result
    /// </summary>
    public static AIJsonResult<T> Ok(T data, string provider, string? rawJson = null, AIUsageStats? usage = null) => new()
    {
        Data = data,
        Success = true,
        Provider = provider,
        RawJson = rawJson,
        Usage = usage ?? new AIUsageStats()
    };
    
    /// <summary>
    /// Creates a failed result
    /// </summary>
    public static AIJsonResult<T> Fail(string error, string provider, string? rawJson = null) => new()
    {
        Success = false,
        ErrorMessage = error,
        Provider = provider,
        RawJson = rawJson
    };
}

/// <summary>
/// Result from a chat conversation request
/// </summary>
public class AIChatResult
{
    /// <summary>
    /// The assistant's response message
    /// </summary>
    public AIChatMessage Message { get; set; } = new() { Role = AIChatRole.Assistant };
    
    /// <summary>
    /// Whether the request completed successfully
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// The AI provider that processed this request
    /// </summary>
    public string Provider { get; set; } = string.Empty;
    
    /// <summary>
    /// Finish reason
    /// </summary>
    public string? FinishReason { get; set; }

    /// <summary>
    /// Tool calls requested by the AI (when the AI wants to invoke functions)
    /// </summary>
    public List<AIToolCall>? ToolCalls { get; set; }

    /// <summary>
    /// Whether the response contains tool calls that need to be executed
    /// </summary>
    public bool HasToolCalls => ToolCalls?.Count > 0;

    /// <summary>
    /// Usage statistics
    /// </summary>
    public AIUsageStats Usage { get; set; } = new();

    /// <summary>
    /// Time taken
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Creates a successful result
    /// </summary>
    public static AIChatResult Ok(string content, string provider, AIUsageStats? usage = null) => new()
    {
        Message = AIChatMessage.Assistant(content),
        Success = true,
        Provider = provider,
        Usage = usage ?? new AIUsageStats()
    };
    
    /// <summary>
    /// Creates a failed result
    /// </summary>
    public static AIChatResult Fail(string error, string provider) => new()
    {
        Success = false,
        ErrorMessage = error,
        Provider = provider
    };
}

/// <summary>
/// Result from an embedding generation request
/// </summary>
public class AIEmbeddingResult
{
    /// <summary>
    /// Generated embeddings (one per input text)
    /// </summary>
    public List<float[]> Embeddings { get; set; } = new();
    
    /// <summary>
    /// Whether the request completed successfully
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// The AI provider
    /// </summary>
    public string Provider { get; set; } = string.Empty;
    
    /// <summary>
    /// Model used for embeddings
    /// </summary>
    public string? Model { get; set; }
    
    /// <summary>
    /// Dimensions of each embedding vector
    /// </summary>
    public int Dimensions { get; set; }
    
    /// <summary>
    /// Usage statistics
    /// </summary>
    public AIUsageStats Usage { get; set; } = new();
    
    /// <summary>
    /// Time taken
    /// </summary>
    public TimeSpan Duration { get; set; }
    
    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Creates a successful result
    /// </summary>
    public static AIEmbeddingResult Ok(List<float[]> embeddings, string provider, int dimensions, AIUsageStats? usage = null) => new()
    {
        Embeddings = embeddings,
        Success = true,
        Provider = provider,
        Dimensions = dimensions,
        Usage = usage ?? new AIUsageStats()
    };
    
    /// <summary>
    /// Creates a failed result
    /// </summary>
    public static AIEmbeddingResult Fail(string error, string provider) => new()
    {
        Success = false,
        ErrorMessage = error,
        Provider = provider
    };
}

/// <summary>
/// Result from an image/vision analysis request
/// </summary>
public class AIVisionResult
{
    /// <summary>
    /// The AI's analysis/response about the image(s)
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// Whether the request completed successfully
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// The AI provider
    /// </summary>
    public string Provider { get; set; } = string.Empty;
    
    /// <summary>
    /// Finish reason
    /// </summary>
    public string? FinishReason { get; set; }
    
    /// <summary>
    /// Usage statistics
    /// </summary>
    public AIUsageStats Usage { get; set; } = new();
    
    /// <summary>
    /// Time taken
    /// </summary>
    public TimeSpan Duration { get; set; }
    
    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Creates a successful result
    /// </summary>
    public static AIVisionResult Ok(string content, string provider, AIUsageStats? usage = null) => new()
    {
        Content = content,
        Success = true,
        Provider = provider,
        Usage = usage ?? new AIUsageStats()
    };
    
    /// <summary>
    /// Creates a failed result
    /// </summary>
    public static AIVisionResult Fail(string error, string provider) => new()
    {
        Success = false,
        ErrorMessage = error,
        Provider = provider
    };
}

/// <summary>
/// Token and cost usage statistics for AI requests
/// </summary>
public class AIUsageStats
{
    /// <summary>
    /// Number of input/prompt tokens
    /// </summary>
    public int InputTokens { get; set; }
    
    /// <summary>
    /// Number of output/completion tokens
    /// </summary>
    public int OutputTokens { get; set; }
    
    /// <summary>
    /// Total tokens used
    /// </summary>
    public int TotalTokens => InputTokens + OutputTokens;
    
    /// <summary>
    /// Estimated cost in USD
    /// </summary>
    public decimal EstimatedCost { get; set; }
    
    /// <summary>
    /// Cost per input token (for reference)
    /// </summary>
    public decimal? CostPerInputToken { get; set; }
    
    /// <summary>
    /// Cost per output token (for reference)
    /// </summary>
    public decimal? CostPerOutputToken { get; set; }
}

/// <summary>
/// Streaming chunk from an AI completion
/// </summary>
public class AIStreamChunk
{
    /// <summary>
    /// The text content of this chunk
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// Whether this is the final chunk
    /// </summary>
    public bool IsComplete { get; set; }
    
    /// <summary>
    /// Finish reason (only set on final chunk)
    /// </summary>
    public string? FinishReason { get; set; }
    
    /// <summary>
    /// Chunk index in the stream
    /// </summary>
    public int Index { get; set; }
}

