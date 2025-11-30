using AMCode.AI.Providers;

namespace AMCode.AI.Configurations;

/// <summary>
/// Centralized registry for AI provider names and configuration key mappings.
/// </summary>
public static class AIProviderRegistry
{
    /// <summary>
    /// Provider name constants matching actual ProviderName property values.
    /// </summary>
    public static class Names
    {
        public const string OpenAI = "OpenAI GPT";
        public const string AzureOpenAISdk = "Azure OpenAI SDK";
        public const string AzureOpenAI = "Azure OpenAI";
        public const string AWSBedrock = "AWS Bedrock";
        public const string AnthropicClaude = "Anthropic Claude";
        public const string OllamaLocal = "Ollama Local";
        public const string LMStudioLocal = "LM Studio Local";
        public const string Grok = "Grok (X.AI)";
        public const string HuggingFace = "Hugging Face";
        public const string Perplexity = "Perplexity";
        public const string AzureComputerVision = "Azure Computer Vision";
    }

    /// <summary>
    /// Maps configuration keys to provider types for factory instantiation.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, Type> ProviderTypeMap = 
        new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            { "openai", typeof(OpenAIGPTProvider) },
            { "azure-openai", typeof(AzureOpenAIProvider) },
            { "azure-openai-sdk", typeof(AzureOpenAISdkProvider) },
            { "aws-bedrock", typeof(AWSBedrockProvider) },
            { "anthropic", typeof(AnthropicClaudeProvider) },
            { "ollama", typeof(OllamaAIProvider) },
            { "lmstudio", typeof(LMStudioAIProvider) },
            { "grok", typeof(GrokProvider) },
            { "huggingface", typeof(HuggingFaceAIProvider) },
            { "perplexity", typeof(PerplexityProvider) },
            { "azure-computer-vision", typeof(AzureComputerVisionProvider) }
        };

    /// <summary>
    /// Maps configuration keys to provider name aliases for backward compatibility.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, string[]> ProviderNameAliases = 
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            { "openai", new[] { Names.OpenAI, "OpenAI", "OpenAI GPT-4o Mini" } },
            { "azure-openai", new[] { Names.AzureOpenAI, "AzureOpenAI" } },
            { "azure-openai-sdk", new[] { Names.AzureOpenAISdk } },
            { "aws-bedrock", new[] { Names.AWSBedrock, "AWSBedrock" } },
            { "anthropic", new[] { Names.AnthropicClaude, "Anthropic", "Claude" } },
            { "ollama", new[] { Names.OllamaLocal, "Ollama" } },
            { "lmstudio", new[] { Names.LMStudioLocal, "LMStudio", "LM Studio" } },
            { "grok", new[] { Names.Grok, "X.AI Grok" } },
            { "huggingface", new[] { Names.HuggingFace, "HuggingFace" } },
            { "perplexity", new[] { Names.Perplexity, "Perplexity AI" } },
            { "azure-computer-vision", new[] { Names.AzureComputerVision, "AzureComputerVision" } }
        };
}

