namespace AMCode.AI.Enums;

/// <summary>
/// Provides centralized access to AI provider names as defined in appsettings.json.
/// This class prevents mismatches from using static strings throughout the application.
/// </summary>
public static class AIProviderNames
{
    /// <summary>
    /// OpenAI provider name (AI:OpenAI)
    /// </summary>
    public static string OpenAI => "OpenAI";

    /// <summary>
    /// OpenAI GPT-4o Mini provider name (AI:OCRTextParserAI)
    /// </summary>
    public static string OCRTextParserAI => "OCRTextParserAI";

    /// <summary>
    /// Anthropic Claude provider name (AI:Anthropic)
    /// </summary>
    public static string Anthropic => "Anthropic";

    /// <summary>
    /// Azure OpenAI provider name (AI:AzureOpenAIGPT5Nano)
    /// </summary>
    public static string AzureOpenAIGPT5Nano => "AzureOpenAIGPT5Nano";

    /// <summary>
    /// AWS Bedrock provider name (AI:AWSBedrock)
    /// </summary>
    public static string AWSBedrock => "AWSBedrock";

    /// <summary>
    /// Ollama local provider name (AI:Ollama)
    /// </summary>
    public static string Ollama => "Ollama";

    /// <summary>
    /// LM Studio local provider name (AI:LMStudio)
    /// </summary>
    public static string LMStudio => "LMStudio";

    /// <summary>
    /// Perplexity provider name (AI:Perplexity)
    /// </summary>
    public static string Perplexity => "Perplexity";

    /// <summary>
    /// Grok (X.AI) provider name (AI:Grok)
    /// </summary>
    public static string Grok => "Grok";

    /// <summary>
    /// Hugging Face provider name (AI:HuggingFace)
    /// </summary>
    public static string HuggingFace => "HuggingFace";
}

