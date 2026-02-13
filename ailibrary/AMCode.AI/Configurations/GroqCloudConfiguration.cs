namespace AMCode.AI.Configurations;

/// <summary>
/// Configuration for Groq Cloud provider.
/// Uses Groq's OpenAI-compatible API for ultra-fast inference.
/// </summary>
public class GroqCloudConfiguration
{
    /// <summary>
    /// Groq API key
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Default model for text/chat completion
    /// </summary>
    public string Model { get; set; } = "llama-3.3-70b-versatile";

    /// <summary>
    /// Model for vision/image analysis tasks
    /// </summary>
    public string VisionModel { get; set; } = "llama-4-scout-17b-16e-instruct";

    /// <summary>
    /// Base URL for Groq API (OpenAI-compatible)
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.groq.com/openai/v1";

    /// <summary>
    /// Maximum tokens for responses
    /// </summary>
    public int MaxTokens { get; set; } = 4096;

    /// <summary>
    /// Default temperature
    /// </summary>
    public float Temperature { get; set; } = 0.1f;

    /// <summary>
    /// Request timeout
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Whether to enable vision capabilities
    /// </summary>
    public bool EnableVision { get; set; } = true;

    /// <summary>
    /// Cost per input token (varies by model)
    /// </summary>
    public decimal CostPerInputToken { get; set; } = 0.00000059m;

    /// <summary>
    /// Cost per output token (varies by model)
    /// </summary>
    public decimal CostPerOutputToken { get; set; } = 0.00000079m;
}
