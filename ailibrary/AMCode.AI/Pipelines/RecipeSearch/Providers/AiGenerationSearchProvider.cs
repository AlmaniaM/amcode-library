using System.Diagnostics;
using AMCode.AI.Models;
using AMCode.AI.Pipelines.RecipeGeneration;
using AMCode.AI.Pipelines.RecipeSearch.Deduplication;
using Microsoft.Extensions.Logging;

namespace AMCode.AI.Pipelines.RecipeSearch.Providers;

/// <summary>
/// Search provider that generates recipes using the AI pipeline.
/// Wraps existing RecipeGenerationPipeline. Always available (no rate limit).
/// </summary>
public class AiGenerationSearchProvider : IRecipeSearchProvider
{
    private readonly IAIPipeline<RecipeGenerationInput, RecipeGenerationOutput> _pipeline;
    private readonly ILogger<AiGenerationSearchProvider> _logger;

    public string ProviderName => "AiGeneration";
    public int Priority => 10;
    public bool IsAvailable => true;

    public AiGenerationSearchProvider(
        IAIPipeline<RecipeGenerationInput, RecipeGenerationOutput> pipeline,
        ILogger<AiGenerationSearchProvider> logger)
    {
        _pipeline = pipeline;
        _logger = logger;
    }

    public async Task<Result<ProviderSearchResult>> SearchAsync(
        RecipeSearchInput input, int maxResults, CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();

        var pipelineInput = new RecipeGenerationInput
        {
            Ingredients = input.Ingredients,
            Cuisine = input.Cuisine,
            MealTypes = input.MealTypes,
            MaxBudgetPerServing = input.MaxBudgetPerServing,
            DietaryPreferences = input.DietaryPreferences,
            Allergies = input.Allergies,
            CookingTimeRange = input.CookingTimeRange,
            Difficulty = input.Difficulty,
            Servings = input.Servings,
            AdditionalNotes = input.AdditionalNotes,
            RecipeCount = maxResults,
        };

        var result = await _pipeline.ExecuteAsync(pipelineInput, cancellationToken);
        sw.Stop();

        if (!result.IsSuccess)
        {
            _logger.LogWarning("AI generation failed: {Error}", result.Error);
            return Result<ProviderSearchResult>.Failure(result.Error ?? "AI generation failed");
        }

        var recipes = result.Value!.Recipes.Select(item => new SearchedRecipe
        {
            Title = item.Title,
            Description = item.Description,
            Ingredients = item.Ingredients.Select(i => new SearchedIngredient
            {
                Name = i.Name,
                Amount = i.Amount,
                Unit = i.Unit,
                Preparation = i.Preparation,
                Text = i.Text,
            }).ToList(),
            Directions = item.Directions,
            PrepTimeMinutes = item.PrepTimeMinutes,
            CookTimeMinutes = item.CookTimeMinutes,
            Servings = item.Servings,
            Difficulty = item.Difficulty,
            EstimatedCostPerServing = item.EstimatedCostPerServing,
            Cuisine = item.Cuisine,
            Tags = item.Tags,
            Nutrition = item.Nutrition != null ? new SearchedNutrition
            {
                CaloriesPerServing = item.Nutrition.CaloriesPerServing,
                ProteinGrams = item.Nutrition.ProteinGrams,
                CarbsGrams = item.Nutrition.CarbsGrams,
                FatGrams = item.Nutrition.FatGrams,
            } : null,
            AllergenWarnings = item.AllergenWarnings,
            SafetyNotes = item.SafetyNotes,
            Confidence = item.Confidence,
            Source = RecipeSource.AiGenerated,
            SourceHash = RecipeHasher.ComputeHash(item.Title, item.Ingredients.Select(i => i.Name)),
        }).ToList();

        _logger.LogInformation(
            "AI generation returned {Count} recipe(s) in {Duration}ms",
            recipes.Count, sw.ElapsedMilliseconds);

        return Result<ProviderSearchResult>.Success(new ProviderSearchResult
        {
            ProviderName = ProviderName,
            Recipes = recipes,
            SearchDuration = sw.Elapsed,
        });
    }
}
