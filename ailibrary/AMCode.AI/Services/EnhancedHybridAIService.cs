using AMCode.AI.Models;
using AMCode.AI.Enums;
using AMCode.AI.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace AMCode.AI.Services;

/// <summary>
/// Enhanced hybrid AI service with fallback mechanisms and smart provider selection
/// </summary>
public class EnhancedHybridAIService : IRecipeParserService
{
    private readonly IEnumerable<IAIProvider> _providers;
    private readonly IAIProviderSelector _providerSelector;
    private readonly ILogger<EnhancedHybridAIService> _logger;
    private readonly ICostAnalyzer _costAnalyzer;
    private readonly AIConfiguration _config;
    
    public EnhancedHybridAIService(
        IEnumerable<IAIProvider> providers,
        IAIProviderSelector providerSelector,
        ILogger<EnhancedHybridAIService> logger,
        ICostAnalyzer costAnalyzer,
        IOptions<AIConfiguration> config)
    {
        _providers = providers ?? throw new ArgumentNullException(nameof(providers));
        _providerSelector = providerSelector ?? throw new ArgumentNullException(nameof(providerSelector));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _costAnalyzer = costAnalyzer ?? throw new ArgumentNullException(nameof(costAnalyzer));
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
    }
    
    public async Task<Result<ParsedRecipeResult>> ParseRecipeAsync(string text, CancellationToken cancellationToken = default)
    {
        return await ParseRecipeAsync(text, new RecipeParsingOptions(), cancellationToken);
    }
    
    public async Task<Result<ParsedRecipeResult>> ParseRecipeAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting enhanced hybrid AI processing for text length: {Length}", text.Length);
            
            var request = new AIRequest
            {
                Text = text,
                Options = options,
                EstimatedTokens = EstimateTokenCount(text),
                RequiresFunctionCalling = options.RequiresFunctionCalling,
                RequiresVision = options.RequiresVision,
                MaxRetries = _config.MaxRetries,
                ConfidenceThreshold = _config.ConfidenceThreshold
            };
            
            // Try primary provider first
            try
            {
                var primaryProvider = await _providerSelector.SelectBestProviderAsync(request);
                var result = await ProcessWithProvider(primaryProvider, request, cancellationToken);
                
                if (result.Confidence >= _config.ConfidenceThreshold)
                {
                    _logger.LogInformation("AI parsing successful with primary provider: {Provider}", primaryProvider.ProviderName);
                    return Result<ParsedRecipeResult>.Success(result);
                }
                else
                {
                    _logger.LogWarning("Primary provider {Provider} returned low confidence: {Confidence}", 
                        primaryProvider.ProviderName, result.Confidence);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Primary provider failed, trying fallback providers");
            }
            
            // Try fallback providers if enabled
            if (_config.EnableFallbackProviders)
            {
                var fallbackProviders = await _providerSelector.GetAvailableProvidersAsync();
                var fallbackAttempts = 0;
                
                foreach (var provider in fallbackProviders)
                {
                    if (fallbackAttempts >= _config.MaxFallbackAttempts)
                    {
                        _logger.LogWarning("Maximum fallback attempts reached: {MaxAttempts}", _config.MaxFallbackAttempts);
                        break;
                    }
                    
                    try
                    {
                        var result = await ProcessWithProvider(provider, request, cancellationToken);
                        
                        if (result.Confidence >= _config.ConfidenceThreshold)
                        {
                            _logger.LogInformation("AI parsing successful with fallback provider: {Provider}", provider.ProviderName);
                            return Result<ParsedRecipeResult>.Success(result);
                        }
                        else
                        {
                            _logger.LogWarning("Fallback provider {Provider} returned low confidence: {Confidence}", 
                                provider.ProviderName, result.Confidence);
                        }
                        
                        fallbackAttempts++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Fallback provider {Provider} failed", provider.ProviderName);
                        fallbackAttempts++;
                    }
                }
            }
            
            _logger.LogError("All AI providers failed or returned low confidence");
            return Result<ParsedRecipeResult>.Failure("All AI providers failed or returned low confidence");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Enhanced hybrid AI processing failed");
            return Result<ParsedRecipeResult>.Failure($"AI processing failed: {ex.Message}");
        }
    }
    
    public async Task<bool> IsAvailableAsync()
    {
        try
        {
            var availableProviders = await _providerSelector.GetAvailableProvidersAsync();
            return availableProviders.Any();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check AI provider availability");
            return false;
        }
    }
    
    private async Task<ParsedRecipeResult> ProcessWithProvider(IAIProvider provider, AIRequest request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Processing with provider: {Provider}", provider.ProviderName);
            
            var result = await provider.ParseTextAsync(request.Text, request.Options, cancellationToken);
            
            // Record cost if tracking is enabled
            if (_config.EnableCostTracking)
            {
                _costAnalyzer.RecordCost(provider.ProviderName, result.Cost);
            }
            
            // Post-process result based on provider capabilities
            if (!provider.Capabilities.SupportsFunctionCalling && request.RequiresFunctionCalling)
            {
                _logger.LogWarning("Provider {Provider} doesn't support function calling but it was requested", provider.ProviderName);
            }
            
            if (!provider.Capabilities.SupportsVision && request.RequiresVision)
            {
                _logger.LogWarning("Provider {Provider} doesn't support vision but it was requested", provider.ProviderName);
            }
            
            // Validate result if validation is enabled
            if (request.Options.ValidateData)
            {
                ValidateParsedResult(result);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process with provider {Provider}", provider.ProviderName);
            throw;
        }
    }
    
    private void ValidateParsedResult(ParsedRecipeResult result)
    {
        try
        {
            if (result.Recipes == null || !result.Recipes.Any())
            {
                _logger.LogWarning("No recipes found in parsed result");
                return;
            }
            
            foreach (var recipe in result.Recipes)
            {
                if (string.IsNullOrWhiteSpace(recipe.Title))
                {
                    _logger.LogWarning("Recipe missing title");
                }
                
                if (!recipe.Ingredients.Any())
                {
                    _logger.LogWarning("Recipe missing ingredients");
                }
                
                if (!recipe.Instructions.Any())
                {
                    _logger.LogWarning("Recipe missing instructions");
                }
                
                if (recipe.Confidence < 0.5)
                {
                    _logger.LogWarning("Recipe has low confidence: {Confidence}", recipe.Confidence);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate parsed result");
        }
    }
    
    private int EstimateTokenCount(string text)
    {
        // Simple estimation: ~4 characters per token
        return Math.Max(1, text.Length / 4);
    }
}
