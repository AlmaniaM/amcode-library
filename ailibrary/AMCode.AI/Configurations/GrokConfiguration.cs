namespace AMCode.AI.Configurations;

/// <summary>
/// Configuration for Grok (X.AI) provider
/// </summary>
public class GrokConfiguration
{
    /// <summary>
    /// Grok API key
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Model to use for requests
    /// Note: grok-beta was deprecated on 2025-09-15, use grok-3 instead
    /// </summary>
    public string Model { get; set; } = "grok-3";

    /// <summary>
    /// Base URL for Grok API
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.x.ai/v1";

    /// <summary>
    /// Maximum tokens for this provider
    /// </summary>
    public int MaxTokens { get; set; } = 8192;

    /// <summary>
    /// Default temperature
    /// </summary>
    public float Temperature { get; set; } = 0.1f;

    /// <summary>
    /// Request timeout
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Whether to enable function calling
    /// </summary>
    public bool EnableFunctionCalling { get; set; } = false;

    /// <summary>
    /// Whether to enable vision capabilities
    /// </summary>
    public bool EnableVision { get; set; } = false;

    /// <summary>
    /// Cost per token for input
    /// </summary>
    public decimal CostPerInputToken { get; set; } = 0.00002m;

    /// <summary>
    /// Cost per token for output
    /// </summary>
    public decimal CostPerOutputToken { get; set; } = 0.00008m;
}
