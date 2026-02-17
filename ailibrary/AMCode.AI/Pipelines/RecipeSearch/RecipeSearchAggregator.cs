using System.Diagnostics;
using AMCode.AI.Models;
using AMCode.AI.Pipelines.RecipeSearch.Deduplication;
using AMCode.AI.Pipelines.RecipeSearch.RateLimiting;
using Microsoft.Extensions.Logging;

namespace AMCode.AI.Pipelines.RecipeSearch;

/// <summary>
/// Orchestrates hybrid recipe search across all registered providers.
/// Fan-out to non-rate-limited providers (parallel), deduplicate, rank,
/// and ensure AI always contributes at least MinAiRecipes.
/// </summary>
public class RecipeSearchAggregator
{
    private readonly IEnumerable<IRecipeSearchProvider> _providers;
    private readonly RateLimitTracker _rateLimitTracker;
    private readonly RecipeSearchConfig _config;
    private readonly ILogger<RecipeSearchAggregator> _logger;

    public RecipeSearchAggregator(
        IEnumerable<IRecipeSearchProvider> providers,
        RateLimitTracker rateLimitTracker,
        RecipeSearchConfig config,
        ILogger<RecipeSearchAggregator> logger)
    {
        _providers = providers;
        _rateLimitTracker = rateLimitTracker;
        _config = config;
        _logger = logger;
    }

    public async Task<Result<RecipeSearchOutput>> SearchAsync(
        RecipeSearchInput input, CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();
        var output = new RecipeSearchOutput();

        // Separate AI provider from other providers
        var aiProvider = _providers.FirstOrDefault(p => p.ProviderName == "AiGeneration");
        var otherProviders = _providers
            .Where(p => p.ProviderName != "AiGeneration")
            .OrderBy(p => p.Priority)
            .ToList();

        // Phase 1: Fan-out to non-AI providers in parallel
        var nonAiResults = await SearchNonAiProvidersAsync(otherProviders, input, output, cancellationToken);

        // Phase 2: Deduplicate non-AI results
        var deduplicator = new RecipeDeduplicator(_config);
        var (dedupedRecipes, dupsRemoved) = deduplicator.Deduplicate(nonAiResults);
        output.DuplicatesRemoved = dupsRemoved;

        // Phase 3: Determine how many AI recipes to generate
        var slotsRemaining = _config.MaxResults - dedupedRecipes.Count;
        var aiCount = Math.Max(_config.MinAiRecipes, slotsRemaining);
        aiCount = Math.Min(aiCount, _config.MaxAiRecipes);

        // Phase 4: Generate AI recipes
        if (aiProvider != null && aiCount > 0)
        {
            var aiResult = await SearchProviderWithTimeoutAsync(aiProvider, input, aiCount, cancellationToken);
            if (aiResult != null)
            {
                output.ProvidersSearched.Add(aiProvider.ProviderName);
                dedupedRecipes.AddRange(aiResult.Recipes);
            }
            else
            {
                _logger.LogWarning("AI generation failed; returning only non-AI results");
            }
        }

        // Phase 5: Trim to MaxResults, prioritizing by source order
        output.Recipes = dedupedRecipes.Take(_config.MaxResults).ToList();

        sw.Stop();
        output.TotalSearchDuration = sw.Elapsed;

        _logger.LogInformation(
            "Hybrid search completed: {Count} results from [{Providers}], {Dupes} dupes removed, {Duration}ms",
            output.Recipes.Count,
            string.Join(", ", output.ProvidersSearched),
            output.DuplicatesRemoved,
            sw.ElapsedMilliseconds);

        return Result<RecipeSearchOutput>.Success(output);
    }

    private async Task<List<SearchedRecipe>> SearchNonAiProvidersAsync(
        List<IRecipeSearchProvider> providers,
        RecipeSearchInput input,
        RecipeSearchOutput output,
        CancellationToken cancellationToken)
    {
        var allRecipes = new List<SearchedRecipe>();
        var tasks = new List<Task<(string ProviderName, ProviderSearchResult? Result)>>();

        foreach (var provider in providers)
        {
            if (!provider.IsAvailable)
            {
                output.ProvidersSkipped.Add($"{provider.ProviderName} (unavailable)");
                continue;
            }

            if (_rateLimitTracker.IsRateLimited(provider.ProviderName))
            {
                output.ProvidersSkipped.Add($"{provider.ProviderName} (rate-limited)");
                _logger.LogInformation("Skipping {Provider}: rate-limited", provider.ProviderName);
                continue;
            }

            tasks.Add(SearchProviderAsync(provider, input, _config.MaxResults, cancellationToken));
        }

        if (tasks.Count == 0)
            return allRecipes;

        var results = await Task.WhenAll(tasks);

        foreach (var (providerName, result) in results)
        {
            if (result != null)
            {
                output.ProvidersSearched.Add(providerName);
                allRecipes.AddRange(result.Recipes);
                _rateLimitTracker.RecordCall(providerName);
            }
        }

        return allRecipes;
    }

    private async Task<(string ProviderName, ProviderSearchResult? Result)> SearchProviderAsync(
        IRecipeSearchProvider provider, RecipeSearchInput input, int maxResults,
        CancellationToken cancellationToken)
    {
        var result = await SearchProviderWithTimeoutAsync(provider, input, maxResults, cancellationToken);
        return (provider.ProviderName, result);
    }

    private async Task<ProviderSearchResult?> SearchProviderWithTimeoutAsync(
        IRecipeSearchProvider provider, RecipeSearchInput input, int maxResults,
        CancellationToken cancellationToken)
    {
        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(_config.ProviderTimeout);

            var result = await provider.SearchAsync(input, maxResults, timeoutCts.Token);

            if (result.IsSuccess)
                return result.Value;

            _logger.LogWarning("{Provider} search failed: {Error}", provider.ProviderName, result.Error);
            return null;
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning("{Provider} search timed out after {Timeout}s",
                provider.ProviderName, _config.ProviderTimeout.TotalSeconds);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Provider} search threw exception", provider.ProviderName);
            return null;
        }
    }
}
