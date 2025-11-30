using AMCode.AI.Models;

namespace AMCode.AI;

/// <summary>
/// AI provider interface for different AI services.
/// Supports general text completion, structured JSON responses, chat, embeddings, and vision.
/// </summary>
public interface IAIProvider
{
    #region Provider Information
    
    /// <summary>
    /// Provider name for identification
    /// </summary>
    string ProviderName { get; }
    
    /// <summary>
    /// Whether this provider requires internet connection
    /// </summary>
    bool RequiresInternet { get; }
    
    /// <summary>
    /// Whether this provider is currently available
    /// </summary>
    bool IsAvailable { get; }
    
    /// <summary>
    /// Provider capabilities and features
    /// </summary>
    AIProviderCapabilities Capabilities { get; }
    
    /// <summary>
    /// Check provider health status
    /// </summary>
    /// <returns>Provider health information</returns>
    Task<AIProviderHealth> CheckHealthAsync();
    
    #endregion
    
    #region General Text Completion
    
    /// <summary>
    /// Send a prompt and get a text completion response
    /// </summary>
    /// <param name="prompt">The prompt to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Completion result</returns>
    Task<AICompletionResult> CompleteAsync(string prompt, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Send a completion request with full options
    /// </summary>
    /// <param name="request">The completion request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Completion result</returns>
    Task<AICompletionResult> CompleteAsync(AICompletionRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Stream a text completion response chunk by chunk
    /// </summary>
    /// <param name="request">The completion request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async enumerable of streaming chunks</returns>
    IAsyncEnumerable<AIStreamChunk> CompleteStreamingAsync(AICompletionRequest request, CancellationToken cancellationToken = default);
    
    #endregion
    
    #region Structured JSON Responses
    
    /// <summary>
    /// Send a prompt and get a structured JSON response
    /// </summary>
    /// <typeparam name="T">The expected response type</typeparam>
    /// <param name="prompt">The prompt to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Parsed JSON result</returns>
    Task<AIJsonResult<T>> CompleteJsonAsync<T>(string prompt, CancellationToken cancellationToken = default) where T : class;
    
    /// <summary>
    /// Send a JSON request with full options
    /// </summary>
    /// <typeparam name="T">The expected response type</typeparam>
    /// <param name="request">The JSON request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Parsed JSON result</returns>
    Task<AIJsonResult<T>> CompleteJsonAsync<T>(AIJsonRequest request, CancellationToken cancellationToken = default) where T : class;
    
    #endregion
    
    #region Chat Conversation
    
    /// <summary>
    /// Send a single message and get a response
    /// </summary>
    /// <param name="message">The user message</param>
    /// <param name="systemMessage">Optional system instructions</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Chat result</returns>
    Task<AIChatResult> ChatAsync(string message, string? systemMessage = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Continue a multi-turn conversation
    /// </summary>
    /// <param name="request">The chat request with message history</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Chat result</returns>
    Task<AIChatResult> ChatAsync(AIChatRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Stream a chat response chunk by chunk
    /// </summary>
    /// <param name="request">The chat request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Async enumerable of streaming chunks</returns>
    IAsyncEnumerable<AIStreamChunk> ChatStreamingAsync(AIChatRequest request, CancellationToken cancellationToken = default);
    
    #endregion
    
    #region Embeddings
    
    /// <summary>
    /// Generate embeddings for a single text
    /// </summary>
    /// <param name="text">The text to embed</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Embedding result</returns>
    Task<AIEmbeddingResult> GetEmbeddingAsync(string text, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Generate embeddings for multiple texts
    /// </summary>
    /// <param name="request">The embedding request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Embedding result</returns>
    Task<AIEmbeddingResult> GetEmbeddingsAsync(AIEmbeddingRequest request, CancellationToken cancellationToken = default);
    
    #endregion
    
    #region Vision/Image Analysis
    
    /// <summary>
    /// Analyze an image with a prompt
    /// </summary>
    /// <param name="prompt">The prompt/question about the image</param>
    /// <param name="imageUrl">URL of the image to analyze</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Vision analysis result</returns>
    Task<AIVisionResult> AnalyzeImageAsync(string prompt, string imageUrl, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Analyze images with full options
    /// </summary>
    /// <param name="request">The vision request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Vision analysis result</returns>
    Task<AIVisionResult> AnalyzeImageAsync(AIVisionRequest request, CancellationToken cancellationToken = default);
    
    #endregion
    
    #region Recipe Parsing (Domain-Specific)
    
    /// <summary>
    /// Parse recipe text using default options
    /// </summary>
    /// <param name="text">The text to parse</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Parsed recipe result</returns>
    Task<ParsedRecipeResult> ParseTextAsync(string text, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Parse text with custom options
    /// </summary>
    /// <param name="text">The text to parse</param>
    /// <param name="options">Parsing options and preferences</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Parsed recipe result</returns>
    Task<ParsedRecipeResult> ParseTextAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get cost estimate for a recipe parsing request
    /// </summary>
    /// <param name="text">Text to be processed</param>
    /// <param name="options">Parsing options</param>
    /// <returns>Estimated cost</returns>
    Task<decimal> GetCostEstimateAsync(string text, RecipeParsingOptions options);
    
    #endregion
}
