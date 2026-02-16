namespace AMCode.AI.Pipelines.RecipeGeneration;

/// <summary>
/// Post-generation safety validation for AI-generated recipes.
/// Cross-checks allergies, flags unsafe cooking temps, adds warnings.
/// </summary>
public static class RecipeSafetyValidator
{
    // Common allergen aliases for fuzzy matching
    private static readonly Dictionary<string, string[]> AllergenAliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["peanut"] = new[] { "peanut", "peanuts", "groundnut", "arachis" },
        ["tree nut"] = new[] { "almond", "almonds", "walnut", "walnuts", "cashew", "cashews", "pecan", "pecans", "pistachio", "pistachios", "hazelnut", "hazelnuts", "macadamia", "brazil nut", "pine nut", "pine nuts" },
        ["dairy"] = new[] { "milk", "cream", "butter", "cheese", "yogurt", "yoghurt", "whey", "casein", "lactose", "ghee", "sour cream", "cream cheese", "parmesan", "mozzarella", "cheddar", "ricotta", "mascarpone", "brie", "gouda" },
        ["egg"] = new[] { "egg", "eggs", "mayonnaise", "mayo", "meringue", "aioli" },
        ["gluten"] = new[] { "wheat", "flour", "bread", "pasta", "noodle", "noodles", "soy sauce", "barley", "rye", "couscous", "semolina", "breadcrumb", "breadcrumbs", "tortilla", "pita", "crouton", "croutons" },
        ["soy"] = new[] { "soy", "soya", "tofu", "tempeh", "edamame", "soy sauce", "miso", "tamari" },
        ["shellfish"] = new[] { "shrimp", "prawn", "prawns", "crab", "lobster", "crayfish", "crawfish", "scallop", "scallops", "clam", "clams", "mussel", "mussels", "oyster", "oysters" },
        ["fish"] = new[] { "fish", "salmon", "tuna", "cod", "tilapia", "halibut", "anchovy", "anchovies", "sardine", "sardines", "bass", "trout", "mackerel", "swordfish", "mahi", "fish sauce", "worcestershire" },
        ["sesame"] = new[] { "sesame", "tahini", "sesame oil", "sesame seeds" },
        ["mustard"] = new[] { "mustard", "dijon" },
        ["celery"] = new[] { "celery", "celeriac" },
        ["lupin"] = new[] { "lupin", "lupine" },
        ["sulfite"] = new[] { "sulfite", "sulphite", "wine", "dried fruit" },
    };

    // Minimum safe internal temperatures (°F) for meats
    private static readonly Dictionary<string, int> SafeTemperatures = new(StringComparer.OrdinalIgnoreCase)
    {
        ["poultry"] = 165,
        ["chicken"] = 165,
        ["turkey"] = 165,
        ["duck"] = 165,
        ["ground beef"] = 160,
        ["ground pork"] = 160,
        ["ground meat"] = 160,
        ["hamburger"] = 160,
        ["pork"] = 145,
        ["beef"] = 145,
        ["steak"] = 145,
        ["lamb"] = 145,
        ["veal"] = 145,
        ["fish"] = 145,
        ["seafood"] = 145,
    };

    /// <summary>
    /// Validates generated recipes for safety issues.
    /// Adds warnings to each recipe but does not block generation.
    /// </summary>
    public static void ValidateAndEnrich(RecipeGenerationOutput output, RecipeGenerationInput input)
    {
        if (output?.Recipes == null) return;

        var declaredAllergens = ParseAllergens(input.Allergies);

        foreach (var recipe in output.Recipes)
        {
            ValidateAllergens(recipe, declaredAllergens);
            ValidateCookingTemperatures(recipe);
            AddGeneralSafetyNotes(recipe);
        }
    }

    private static List<string> ParseAllergens(string? allergies)
    {
        if (string.IsNullOrWhiteSpace(allergies)) return new List<string>();

        return allergies
            .Split(new[] { ',', ';', '/' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(a => a.Trim().ToLowerInvariant())
            .Where(a => !string.IsNullOrEmpty(a))
            .ToList();
    }

    private static void ValidateAllergens(GeneratedRecipeItem recipe, List<string> declaredAllergens)
    {
        if (declaredAllergens.Count == 0) return;

        foreach (var ingredient in recipe.Ingredients)
        {
            var ingredientLower = $"{ingredient.Name} {ingredient.Text}".ToLowerInvariant();

            foreach (var allergen in declaredAllergens)
            {
                // Direct match
                if (ingredientLower.Contains(allergen))
                {
                    var warning = $"WARNING: Ingredient '{ingredient.Name}' may contain declared allergen '{allergen}'";
                    if (!recipe.AllergenWarnings.Contains(warning))
                        recipe.AllergenWarnings.Add(warning);
                }

                // Check alias matches
                foreach (var (category, aliases) in AllergenAliases)
                {
                    if (!category.Contains(allergen) && !allergen.Contains(category) &&
                        !aliases.Any(a => a.Contains(allergen) || allergen.Contains(a)))
                        continue;

                    foreach (var alias in aliases)
                    {
                        if (ingredientLower.Contains(alias))
                        {
                            var warning = $"WARNING: Ingredient '{ingredient.Name}' contains '{alias}' which is related to declared allergen '{allergen}'";
                            if (!recipe.AllergenWarnings.Contains(warning))
                                recipe.AllergenWarnings.Add(warning);
                        }
                    }
                }
            }
        }
    }

    private static void ValidateCookingTemperatures(GeneratedRecipeItem recipe)
    {
        var allIngredientText = string.Join(" ", recipe.Ingredients.Select(i => i.Name.ToLowerInvariant()));
        var allDirectionText = string.Join(" ", recipe.Directions.Select(d => d.ToLowerInvariant()));

        foreach (var (protein, minTemp) in SafeTemperatures)
        {
            if (!allIngredientText.Contains(protein.ToLowerInvariant()) &&
                !allDirectionText.Contains(protein.ToLowerInvariant()))
                continue;

            var note = $"Ensure {protein} reaches a minimum internal temperature of {minTemp}°F ({(minTemp - 32) * 5 / 9}°C)";
            if (!recipe.SafetyNotes.Any(n => n.Contains(protein, StringComparison.OrdinalIgnoreCase)))
            {
                recipe.SafetyNotes.Add(note);
            }
        }
    }

    private static void AddGeneralSafetyNotes(GeneratedRecipeItem recipe)
    {
        var hasRawProduce = recipe.Ingredients.Any(i =>
        {
            var name = i.Name.ToLowerInvariant();
            return name.Contains("lettuce") || name.Contains("spinach") || name.Contains("arugula") ||
                   name.Contains("tomato") || name.Contains("cucumber") || name.Contains("herb") ||
                   name.Contains("cilantro") || name.Contains("parsley") || name.Contains("basil");
        });

        if (hasRawProduce)
        {
            var note = "Wash all fresh produce thoroughly under running water before use";
            if (!recipe.SafetyNotes.Contains(note))
                recipe.SafetyNotes.Add(note);
        }

        var hasLeftoverPotential = recipe.Servings >= 4 || recipe.CookTimeMinutes >= 30;
        if (hasLeftoverPotential)
        {
            var note = "Refrigerate leftovers within 2 hours. Consume within 3-4 days or freeze for up to 3 months";
            if (!recipe.SafetyNotes.Contains(note))
                recipe.SafetyNotes.Add(note);
        }
    }
}
