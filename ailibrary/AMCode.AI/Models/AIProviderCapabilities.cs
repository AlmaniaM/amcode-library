namespace AMCode.AI.Models;

/// <summary>
/// Capabilities and features of an AI provider
/// </summary>
public class AIProviderCapabilities
{
    /// <summary>
    /// Whether the provider supports streaming responses
    /// </summary>
    public bool SupportsStreaming { get; set; }
    
    /// <summary>
    /// Whether the provider supports function calling
    /// </summary>
    public bool SupportsFunctionCalling { get; set; }
    
    /// <summary>
    /// Whether the provider supports vision/image analysis
    /// </summary>
    public bool SupportsVision { get; set; }
    
    /// <summary>
    /// Whether the provider supports long context windows
    /// </summary>
    public bool SupportsLongContext { get; set; }
    
    /// <summary>
    /// Maximum number of tokens per request
    /// </summary>
    public int MaxTokens { get; set; }
    
    /// <summary>
    /// Maximum context length in tokens
    /// </summary>
    public int MaxContextLength { get; set; }
    
    /// <summary>
    /// Supported languages
    /// </summary>
    public string[] SupportedLanguages { get; set; } = Array.Empty<string>();
    
    /// <summary>
    /// Cost per token in USD
    /// </summary>
    public decimal CostPerToken { get; set; }
    
    /// <summary>
    /// Base cost per request in USD
    /// </summary>
    public decimal CostPerRequest { get; set; }
    
    /// <summary>
    /// Average response time
    /// </summary>
    public TimeSpan AverageResponseTime { get; set; }
    
    /// <summary>
    /// Whether the provider supports custom models
    /// </summary>
    public bool SupportsCustomModels { get; set; }
    
    /// <summary>
    /// Whether the provider supports fine-tuning
    /// </summary>
    public bool SupportsFineTuning { get; set; }
    
    /// <summary>
    /// Whether the provider supports embeddings
    /// </summary>
    public bool SupportsEmbeddings { get; set; }
    
    /// <summary>
    /// Whether the provider supports moderation
    /// </summary>
    public bool SupportsModeration { get; set; }
    
    /// <summary>
    /// Maximum requests per minute
    /// </summary>
    public int MaxRequestsPerMinute { get; set; }
    
    /// <summary>
    /// Maximum requests per day
    /// </summary>
    public int MaxRequestsPerDay { get; set; }
}
