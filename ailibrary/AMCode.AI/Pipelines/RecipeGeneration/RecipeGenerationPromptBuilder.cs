using System.Text;

namespace AMCode.AI.Pipelines.RecipeGeneration;

/// <summary>
/// Builds system and user prompts for recipe generation.
/// Safety-first: allergen avoidance, food safety temps, practical recipes.
/// </summary>
public static class RecipeGenerationPromptBuilder
{
    public static string BuildSystemPrompt()
    {
        return """
            You are a professional chef and recipe developer. Generate practical, delicious recipes that are SAFE for human consumption.

            CRITICAL SAFETY RULES:
            - All cooking temperatures MUST follow food safety guidelines: poultry 165°F/74°C minimum internal temp, ground meat 160°F/71°C, pork 145°F/63°C, beef steaks 145°F/63°C for medium-rare.
            - NEVER suggest consuming raw or undercooked meat, poultry, seafood, or eggs unless it is a known safe preparation (e.g., sashimi-grade tuna, beef tartare with fresh high-quality beef, pasteurized eggs).
            - If the user has declared allergies, NEVER include those allergens in ANY ingredient. Cross-reference every single ingredient against the declared allergies — including hidden sources (e.g., soy in Worcestershire sauce, dairy in bread, nuts in pesto, gluten in soy sauce).
            - Include explicit allergen warnings for common hidden allergens present in the recipe.
            - Include food safety notes where relevant (e.g., "wash produce thoroughly", "refrigerate leftovers within 2 hours", proper meat thawing methods).

            QUALITY RULES:
            - Generate recipes based on REAL culinary traditions and well-known techniques.
            - Use realistic ingredient quantities and cooking times — no approximations like "cook until done" without specifics.
            - Recipes MUST be practical for home cooks with standard kitchen equipment.
            - Include specific temperatures (in °F and °C) and specific times for every cooking step.
            - Prefer well-known, tested recipe patterns. Reference classic dishes or traditional preparations when relevant.
            - Each recipe should be distinct — different flavor profiles, cooking techniques, or cuisine styles.
            - Ingredient amounts must be in standard US measurements with metric equivalents where helpful.

            OUTPUT FORMAT:
            - Return ONLY valid JSON matching the schema provided. No markdown, no commentary outside the JSON.
            - Every ingredient must have a complete "text" field (e.g., "2 cups (240g) diced chicken breast").
            - Directions should be numbered steps with specific temps, times, and visual/texture cues.
            - Confidence should be 0.0-1.0 reflecting how well the recipe matches the user's constraints.
            """;
    }

    public static string BuildUserPrompt(RecipeGenerationInput input)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Generate {input.RecipeCount} recipe(s) with the following constraints:");
        sb.AppendLine();

        // Ingredients
        if (input.Ingredients.Count > 0)
        {
            sb.AppendLine($"AVAILABLE INGREDIENTS: {string.Join(", ", input.Ingredients)}");
            sb.AppendLine("Use these ingredients as the primary base. You may add common pantry staples (salt, pepper, oil, butter, garlic, onion) but keep additional ingredients minimal.");
        }

        // Cuisine
        if (!string.IsNullOrWhiteSpace(input.Cuisine))
        {
            sb.AppendLine($"CUISINE: {input.Cuisine}");
        }

        // Meal types
        if (input.MealTypes.Count > 0)
        {
            sb.AppendLine($"MEAL TYPE: {string.Join(", ", input.MealTypes)}");
        }

        // Budget
        if (input.MaxBudgetPerServing.HasValue)
        {
            sb.AppendLine($"MAX BUDGET: ${input.MaxBudgetPerServing.Value} per serving");
        }

        // Dietary
        if (input.DietaryPreferences.Count > 0)
        {
            sb.AppendLine($"DIETARY REQUIREMENTS: {string.Join(", ", input.DietaryPreferences)}");
            sb.AppendLine("These are STRICT requirements — every recipe must fully comply.");
        }

        // Allergies — CRITICAL SAFETY
        if (!string.IsNullOrWhiteSpace(input.Allergies))
        {
            sb.AppendLine($"ALLERGIES (CRITICAL — DO NOT INCLUDE THESE): {input.Allergies}");
            sb.AppendLine("Cross-check EVERY ingredient (including sauces, marinades, hidden sources) against these allergies. Include allergenWarnings for any potential cross-contamination risks.");
        }

        // Cooking time
        if (!string.IsNullOrWhiteSpace(input.CookingTimeRange))
        {
            sb.AppendLine($"COOKING TIME CONSTRAINT: {input.CookingTimeRange}");
        }

        // Difficulty
        if (!string.IsNullOrWhiteSpace(input.Difficulty))
        {
            sb.AppendLine($"DIFFICULTY LEVEL: {input.Difficulty}");
        }

        // Servings
        sb.AppendLine($"SERVINGS: {input.Servings}");

        // Additional notes
        if (!string.IsNullOrWhiteSpace(input.AdditionalNotes))
        {
            sb.AppendLine($"ADDITIONAL NOTES: {input.AdditionalNotes}");
        }

        sb.AppendLine();
        sb.AppendLine("Respond with a JSON object matching this schema:");
        sb.AppendLine(GetJsonSchema());

        return sb.ToString();
    }

    private static string GetJsonSchema()
    {
        return """
            {
              "recipes": [
                {
                  "title": "string",
                  "description": "string (1-2 sentences describing the dish)",
                  "ingredients": [
                    {
                      "name": "string (ingredient name)",
                      "amount": "string (numeric amount, e.g. '2', '1/2')",
                      "unit": "string (e.g. 'cups', 'tbsp', 'lbs')",
                      "preparation": "string or null (e.g. 'diced', 'minced')",
                      "text": "string (full ingredient line, e.g. '2 cups diced chicken breast')"
                    }
                  ],
                  "directions": ["string (step 1)", "string (step 2)"],
                  "prepTimeMinutes": 0,
                  "cookTimeMinutes": 0,
                  "servings": 0,
                  "difficulty": "Easy | Medium | Hard",
                  "estimatedCostPerServing": 0.00,
                  "cuisine": "string",
                  "tags": ["string"],
                  "nutrition": {
                    "caloriesPerServing": 0,
                    "proteinGrams": 0,
                    "carbsGrams": 0,
                    "fatGrams": 0
                  },
                  "allergenWarnings": ["string (any allergen risks)"],
                  "safetyNotes": ["string (food safety reminders)"],
                  "sourceInspiration": "string or null (e.g. 'Inspired by classic Italian carbonara')",
                  "confidence": 0.0
                }
              ]
            }
            """;
    }
}
