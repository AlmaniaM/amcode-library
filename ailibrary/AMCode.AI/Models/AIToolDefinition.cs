namespace AMCode.AI.Models;

/// <summary>
/// Defines a tool/function that the AI can call during a conversation.
/// Used for function calling (OpenAI) / tool use (Anthropic).
/// </summary>
public class AIToolDefinition
{
    /// <summary>
    /// Tool name (must match what the AI will call)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Human-readable description of what the tool does
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// JSON Schema describing the tool's parameters
    /// </summary>
    public string ParametersJsonSchema { get; set; } = "{}";
}

/// <summary>
/// Represents a tool call made by the AI in its response.
/// The caller should execute the tool and return the result via AIChatMessage.ToolResult().
/// </summary>
public class AIToolCall
{
    /// <summary>
    /// Unique identifier for this tool call (used to match results)
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Name of the tool being called
    /// </summary>
    public string ToolName { get; set; } = string.Empty;

    /// <summary>
    /// JSON string of the arguments passed to the tool
    /// </summary>
    public string ArgumentsJson { get; set; } = "{}";
}

/// <summary>
/// Result of executing a tool call, to be sent back to the AI.
/// </summary>
public class AIToolResult
{
    /// <summary>
    /// The tool call ID this result corresponds to
    /// </summary>
    public string ToolCallId { get; set; } = string.Empty;

    /// <summary>
    /// JSON string of the tool's result
    /// </summary>
    public string ResultJson { get; set; } = string.Empty;

    /// <summary>
    /// Whether the tool execution resulted in an error
    /// </summary>
    public bool IsError { get; set; }
}
