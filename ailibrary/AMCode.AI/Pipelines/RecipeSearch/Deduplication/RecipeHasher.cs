using System.Security.Cryptography;
using System.Text;

namespace AMCode.AI.Pipelines.RecipeSearch.Deduplication;

/// <summary>
/// Tier 1 deduplication — fast hash for DB ingest.
/// Hash = SHA256(lowercase(title) + ":" + sorted(first 5 ingredient names))
/// </summary>
public static class RecipeHasher
{
    /// <summary>
    /// Compute a source hash for a recipe.
    /// Used to quickly detect exact/near-exact duplicates on DB ingest.
    /// </summary>
    /// <param name="title">Recipe title</param>
    /// <param name="ingredientNames">All ingredient names for this recipe</param>
    /// <returns>Hex-encoded SHA256 hash</returns>
    public static string ComputeHash(string title, IEnumerable<string> ingredientNames)
    {
        var normalizedTitle = title.Trim().ToLowerInvariant();

        var sortedIngredients = ingredientNames
            .Select(n => n.Trim().ToLowerInvariant())
            .Where(n => n.Length > 0)
            .OrderBy(n => n)
            .Take(5);

        var input = normalizedTitle + ":" + string.Join(",", sortedIngredients);

        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }

    /// <summary>
    /// Compute a source hash for a SearchedRecipe
    /// </summary>
    public static string ComputeHash(SearchedRecipe recipe)
    {
        var ingredientNames = recipe.Ingredients.Select(i => i.Name);
        return ComputeHash(recipe.Title, ingredientNames);
    }
}
