using AMCode.AI.Models;

namespace AMCode.AI.Services;

/// <summary>
/// Interface for recipe validation service
/// </summary>
[Obsolete("Use pipeline-level validation instead. Add validation to your pipeline's ExecuteWithProviderAsync.")]
public interface IRecipeValidationService
{
    /// <summary>
    /// Validate a parsed recipe for completeness and quality
    /// </summary>
    /// <param name="recipe">The recipe to validate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result with score and issues</returns>
    Task<RecipeValidationResult> ValidateRecipeAsync(ParsedRecipe recipe, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validate multiple recipes in batch
    /// </summary>
    /// <param name="recipes">The recipes to validate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation results for all recipes</returns>
    Task<IEnumerable<RecipeValidationResult>> ValidateRecipesAsync(IEnumerable<ParsedRecipe> recipes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get validation rules and their descriptions
    /// </summary>
    /// <returns>Dictionary of rule names and descriptions</returns>
    Dictionary<string, string> GetValidationRules();
}
