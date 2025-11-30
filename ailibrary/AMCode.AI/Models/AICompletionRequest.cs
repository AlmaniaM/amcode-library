namespace AMCode.AI.Models;

/// <summary>
/// Request for general AI text completion
/// </summary>
public class AICompletionRequest
{
    /// <summary>
    /// The prompt or question to send to the AI
    /// </summary>
    public string Prompt { get; set; } = string.Empty;
    
    /// <summary>
    /// Optional system message/instructions for the AI
    /// </summary>
    public string? SystemMessage { get; set; }
    
    /// <summary>
    /// Maximum tokens to generate in the response
    /// </summary>
    public int? MaxTokens { get; set; }
    
    /// <summary>
    /// Temperature for response creativity (0.0 = deterministic, 2.0 = very creative)
    /// </summary>
    public float? Temperature { get; set; }
    
    /// <summary>
    /// Top-p (nucleus) sampling parameter
    /// </summary>
    public float? TopP { get; set; }
    
    /// <summary>
    /// Frequency penalty to reduce repetition (-2.0 to 2.0)
    /// </summary>
    public float? FrequencyPenalty { get; set; }
    
    /// <summary>
    /// Presence penalty to encourage new topics (-2.0 to 2.0)
    /// </summary>
    public float? PresencePenalty { get; set; }
    
    /// <summary>
    /// Stop sequences that will halt generation
    /// </summary>
    public string[]? StopSequences { get; set; }
    
    /// <summary>
    /// Request timeout override
    /// </summary>
    public TimeSpan? Timeout { get; set; }
    
    /// <summary>
    /// Custom metadata for tracking/logging
    /// </summary>
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// Request for structured JSON response from AI
/// </summary>
public class AIJsonRequest : AICompletionRequest
{
    /// <summary>
    /// JSON schema describing the expected response structure (optional, for validation)
    /// </summary>
    public string? JsonSchema { get; set; }
    
    /// <summary>
    /// Whether to enforce strict JSON mode (provider-dependent)
    /// </summary>
    public bool StrictJsonMode { get; set; } = true;
    
    /// <summary>
    /// Example JSON response to guide the AI (optional)
    /// </summary>
    public string? ExampleResponse { get; set; }
}

/// <summary>
/// Request for multi-turn chat conversation
/// </summary>
public class AIChatRequest
{
    /// <summary>
    /// System instruction/prompt that defines the AI's behavior and context.
    /// This is automatically prepended to messages for providers that support it.
    /// For providers that don't support system messages, it may be converted to a user message.
    /// </summary>
    public string? SystemInstruction { get; set; }
    
    /// <summary>
    /// Conversation history as a list of messages.
    /// Note: If SystemInstruction is set, it takes precedence over any System messages in this list.
    /// </summary>
    public List<AIChatMessage> Messages { get; set; } = new();
    
    /// <summary>
    /// Maximum tokens to generate in the response
    /// </summary>
    public int? MaxTokens { get; set; }
    
    /// <summary>
    /// Temperature for response creativity
    /// </summary>
    public float? Temperature { get; set; }
    
    /// <summary>
    /// Top-p sampling parameter
    /// </summary>
    public float? TopP { get; set; }
    
    /// <summary>
    /// Stop sequences
    /// </summary>
    public string[]? StopSequences { get; set; }
    
    /// <summary>
    /// Request timeout override
    /// </summary>
    public TimeSpan? Timeout { get; set; }
    
    /// <summary>
    /// Custom metadata
    /// </summary>
    public Dictionary<string, object>? Metadata { get; set; }
    
    /// <summary>
    /// Gets all messages including the system instruction as the first message (if set).
    /// Use this method to get the complete message list for providers.
    /// </summary>
    /// <returns>Complete list of messages with system instruction prepended</returns>
    public List<AIChatMessage> GetMessagesWithSystemInstruction()
    {
        var result = new List<AIChatMessage>();
        
        // Add system instruction first if specified
        if (!string.IsNullOrEmpty(SystemInstruction))
        {
            result.Add(AIChatMessage.System(SystemInstruction));
        }
        
        // Add remaining messages, excluding any existing system messages if SystemInstruction is set
        foreach (var msg in Messages)
        {
            // Skip system messages if we already have a SystemInstruction
            if (!string.IsNullOrEmpty(SystemInstruction) && msg.Role == AIChatRole.System)
            {
                continue;
            }
            result.Add(msg);
        }
        
        return result;
    }
    
    /// <summary>
    /// Creates a simple chat request with a user message and optional system instruction
    /// </summary>
    public static AIChatRequest Create(string userMessage, string? systemInstruction = null)
    {
        return new AIChatRequest
        {
            SystemInstruction = systemInstruction,
            Messages = new List<AIChatMessage> { AIChatMessage.User(userMessage) }
        };
    }
}

/// <summary>
/// A single message in a chat conversation
/// </summary>
public class AIChatMessage
{
    /// <summary>
    /// Role of the message sender
    /// </summary>
    public AIChatRole Role { get; set; }
    
    /// <summary>
    /// Content of the message
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// Optional name identifier for the message author
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Creates a system message
    /// </summary>
    public static AIChatMessage System(string content) => new() { Role = AIChatRole.System, Content = content };
    
    /// <summary>
    /// Creates a user message
    /// </summary>
    public static AIChatMessage User(string content) => new() { Role = AIChatRole.User, Content = content };
    
    /// <summary>
    /// Creates an assistant message
    /// </summary>
    public static AIChatMessage Assistant(string content) => new() { Role = AIChatRole.Assistant, Content = content };
}

/// <summary>
/// Role of a chat message sender
/// </summary>
public enum AIChatRole
{
    /// <summary>
    /// System instructions/context
    /// </summary>
    System,
    
    /// <summary>
    /// User input
    /// </summary>
    User,
    
    /// <summary>
    /// AI assistant response
    /// </summary>
    Assistant,
    
    /// <summary>
    /// Tool/function result
    /// </summary>
    Tool
}

/// <summary>
/// Request for generating text embeddings
/// </summary>
public class AIEmbeddingRequest
{
    /// <summary>
    /// Text(s) to generate embeddings for
    /// </summary>
    public List<string> Texts { get; set; } = new();
    
    /// <summary>
    /// Specific embedding model to use (provider-dependent)
    /// </summary>
    public string? Model { get; set; }
    
    /// <summary>
    /// Embedding dimensions (if model supports variable dimensions)
    /// </summary>
    public int? Dimensions { get; set; }
    
    /// <summary>
    /// Request timeout override
    /// </summary>
    public TimeSpan? Timeout { get; set; }
}

/// <summary>
/// Request for analyzing images with AI
/// </summary>
public class AIVisionRequest
{
    /// <summary>
    /// Text prompt/question about the image(s)
    /// </summary>
    public string Prompt { get; set; } = string.Empty;
    
    /// <summary>
    /// Image URLs to analyze
    /// </summary>
    public List<string>? ImageUrls { get; set; }
    
    /// <summary>
    /// Base64-encoded image data to analyze
    /// </summary>
    public List<string>? ImageBase64 { get; set; }
    
    /// <summary>
    /// Maximum tokens for the response
    /// </summary>
    public int? MaxTokens { get; set; }
    
    /// <summary>
    /// Level of detail for image analysis (low, high, auto)
    /// </summary>
    public string DetailLevel { get; set; } = "auto";
    
    /// <summary>
    /// Request timeout override
    /// </summary>
    public TimeSpan? Timeout { get; set; }
}

