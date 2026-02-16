using AMCode.AI.Factories;
using AMCode.AI.Models;
using Microsoft.Extensions.Logging;

namespace AMCode.AI.Pipelines.RecipeGeneration;

/// <summary>
/// AI pipeline for generating recipes from user constraints.
/// Uses structured JSON completion with safety validation.
/// </summary>
public class RecipeGenerationPipeline : AIPipelineBase<RecipeGenerationInput, RecipeGenerationOutput>
{
    public override string PipelineName => "RecipeGeneration";

    public RecipeGenerationPipeline(
        IAIProviderFactory providerFactory,
        PipelineConfiguration config,
        ILogger<RecipeGenerationPipeline> logger)
        : base(providerFactory, config, logger)
    {
    }

    protected override async Task<Result<RecipeGenerationOutput>> ExecuteWithProviderAsync(
        IAIProvider provider, RecipeGenerationInput input, CancellationToken cancellationToken)
    {
        var systemPrompt = RecipeGenerationPromptBuilder.BuildSystemPrompt();
        var userPrompt = RecipeGenerationPromptBuilder.BuildUserPrompt(input);

        _logger.LogInformation(
            "RecipeGeneration: generating {Count} recipe(s) with {IngredientCount} ingredients, cuisine={Cuisine}",
            input.RecipeCount, input.Ingredients.Count, input.Cuisine ?? "any");

        var request = new AIJsonRequest
        {
            SystemMessage = systemPrompt,
            Prompt = userPrompt,
            MaxTokens = _config.MaxTokens ?? 4000,
            Temperature = _config.Temperature ?? 0.7f,
            StrictJsonMode = true,
        };

        var jsonResult = await provider.CompleteJsonAsync<RecipeGenerationOutput>(request, cancellationToken);

        if (!jsonResult.Success || jsonResult.Data == null)
        {
            _logger.LogWarning(
                "RecipeGeneration: provider '{Provider}' failed: {Error}",
                provider.ProviderName, jsonResult.ErrorMessage);

            return Result<RecipeGenerationOutput>.Failure(
                jsonResult.ErrorMessage ?? "Recipe generation failed — no response from AI provider");
        }

        var output = jsonResult.Data;

        if (output.Recipes.Count == 0)
        {
            return Result<RecipeGenerationOutput>.Failure(
                "AI returned empty recipe list. Please try different ingredients or fewer constraints.");
        }

        // Post-generation safety validation
        RecipeSafetyValidator.ValidateAndEnrich(output, input);

        _logger.LogInformation(
            "RecipeGeneration: successfully generated {Count} recipe(s) via {Provider} ({InputTokens} in, {OutputTokens} out)",
            output.Recipes.Count, provider.ProviderName, jsonResult.Usage.InputTokens, jsonResult.Usage.OutputTokens);

        return Result<RecipeGenerationOutput>.Success(output);
    }
}
