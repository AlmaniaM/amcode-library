namespace AMCode.AI.Pipelines.RecipeSearch.Deduplication;

/// <summary>
/// Tier 2 deduplication — fuzzy matching on aggregated results.
/// Compares titles via normalized Levenshtein distance and ingredient overlap.
/// AI-generated recipes are never deduped against other sources.
/// </summary>
public class RecipeDeduplicator
{
    private readonly double _titleThreshold;
    private readonly double _ingredientOverlap;

    public RecipeDeduplicator(double titleThreshold = 0.85, double ingredientOverlap = 0.60)
    {
        _titleThreshold = titleThreshold;
        _ingredientOverlap = ingredientOverlap;
    }

    public RecipeDeduplicator(RecipeSearchConfig config)
        : this(config.DeduplicationTitleThreshold, config.DeduplicationIngredientOverlap)
    {
    }

    /// <summary>
    /// Remove duplicate recipes from a merged list.
    /// AI-generated recipes are never removed as duplicates.
    /// When duplicates are found, keeps the version from the higher-priority source
    /// (Spoonacular > FatSecret > RecipeNLG > LocalDb).
    /// </summary>
    /// <returns>Deduplicated list and count of duplicates removed</returns>
    public (List<SearchedRecipe> Recipes, int DuplicatesRemoved) Deduplicate(List<SearchedRecipe> recipes)
    {
        if (recipes.Count <= 1)
            return (recipes, 0);

        var kept = new List<SearchedRecipe>();
        var removed = 0;

        foreach (var candidate in recipes)
        {
            // AI recipes are never deduped
            if (candidate.Source == RecipeSource.AiGenerated)
            {
                kept.Add(candidate);
                continue;
            }

            var isDuplicate = false;

            foreach (var existing in kept)
            {
                // Don't compare against AI recipes
                if (existing.Source == RecipeSource.AiGenerated)
                    continue;

                // Tier 1: exact hash match
                if (!string.IsNullOrEmpty(candidate.SourceHash) &&
                    !string.IsNullOrEmpty(existing.SourceHash) &&
                    candidate.SourceHash == existing.SourceHash)
                {
                    isDuplicate = true;
                    break;
                }

                // Tier 2: fuzzy title + ingredient overlap
                var titleSimilarity = NormalizedLevenshteinSimilarity(
                    candidate.Title.ToLowerInvariant(),
                    existing.Title.ToLowerInvariant());

                if (titleSimilarity >= _titleThreshold)
                {
                    var ingredientOverlap = ComputeIngredientOverlap(
                        candidate.Ingredients, existing.Ingredients);

                    if (ingredientOverlap >= _ingredientOverlap)
                    {
                        isDuplicate = true;
                        break;
                    }
                }
            }

            if (isDuplicate)
                removed++;
            else
                kept.Add(candidate);
        }

        return (kept, removed);
    }

    /// <summary>
    /// Compute normalized Levenshtein similarity (0.0 = completely different, 1.0 = identical)
    /// </summary>
    internal static double NormalizedLevenshteinSimilarity(string a, string b)
    {
        if (a == b) return 1.0;
        if (a.Length == 0 || b.Length == 0) return 0.0;

        var maxLen = Math.Max(a.Length, b.Length);
        var distance = LevenshteinDistance(a, b);
        return 1.0 - (double)distance / maxLen;
    }

    /// <summary>
    /// Compute ingredient name overlap as a fraction (0.0–1.0).
    /// Uses the smaller set as denominator.
    /// </summary>
    internal static double ComputeIngredientOverlap(
        List<SearchedIngredient> a, List<SearchedIngredient> b)
    {
        if (a.Count == 0 || b.Count == 0) return 0.0;

        var namesA = a.Select(i => i.Name.Trim().ToLowerInvariant()).ToHashSet();
        var namesB = b.Select(i => i.Name.Trim().ToLowerInvariant()).ToHashSet();

        var intersection = namesA.Intersect(namesB).Count();
        var minSize = Math.Min(namesA.Count, namesB.Count);

        return (double)intersection / minSize;
    }

    private static int LevenshteinDistance(string a, string b)
    {
        var n = a.Length;
        var m = b.Length;
        var dp = new int[n + 1, m + 1];

        for (var i = 0; i <= n; i++) dp[i, 0] = i;
        for (var j = 0; j <= m; j++) dp[0, j] = j;

        for (var i = 1; i <= n; i++)
        {
            for (var j = 1; j <= m; j++)
            {
                var cost = a[i - 1] == b[j - 1] ? 0 : 1;
                dp[i, j] = Math.Min(
                    Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                    dp[i - 1, j - 1] + cost);
            }
        }

        return dp[n, m];
    }
}
