using AMCode.AI.Models;
using Microsoft.Extensions.Logging;

namespace AMCode.AI.Services;

/// <summary>
/// Service for building optimized prompts for different AI providers
/// </summary>
public class PromptBuilderService
{
    private readonly ILogger<PromptBuilderService> _logger;

    public PromptBuilderService(ILogger<PromptBuilderService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Build a recipe parsing prompt with options
    /// </summary>
    /// <param name="text">The recipe text to parse</param>
    /// <param name="options">Parsing options</param>
    /// <returns>Formatted prompt</returns>
    public string BuildRecipeParsingPrompt(string text, RecipeParsingOptions? options = null)
    {
        try
        {
            _logger.LogDebug("Building recipe parsing prompt for text length: {Length}", text.Length);

            var optionsText = options != null ? BuildOptionsText(options) : string.Empty;

            var prompt = $@"
You are a recipe parser. Parse the following recipe text and return ONLY valid JSON. Do not include any explanations, code, or other text. Return ONLY the JSON object.

Required JSON format:

{{
  ""title"": ""Recipe Title"",
  ""description"": ""Optional description"",
  ""ingredients"": [
    {{
      ""name"": ""ingredient name"",
      ""amount"": ""amount"",
      ""unit"": ""unit"",
      ""text"": ""full original ingredient line exactly as written"",
      ""preparation"": ""preparation from ingredient text (e.g., diced, chopped)"",
      ""instructions"": ""preparation from directions (e.g., chopped from 'chop the onions' in directions)"",
      ""notes"": ""optional notes""
    }}
  ],
  ""instructions"": [""step 1"", ""step 2""],
  ""prepTimeMinutes"": 15,
  ""cookTimeMinutes"": 30,
  ""totalTimeMinutes"": 45,
  ""servings"": 4,
  ""category"": ""Main Course"",
  ""tags"": [""tag1"", ""tag2""],
  ""difficulty"": 3,
  ""confidence"": 0.95,
  ""notes"": ""Additional notes or tips""
}}

**CRITICAL INGREDIENT EXAMPLES** - The ""text"" field MUST preserve the EXACT original line:

If recipe text says: ""1 cup Sugar""
Then ingredient should be: {{""name"": ""Sugar"", ""amount"": ""1"", ""unit"": ""cup"", ""text"": ""1 cup Sugar""}}

If recipe text says: ""2 tablespoons olive oil""
Then ingredient should be: {{""name"": ""olive oil"", ""amount"": ""2"", ""unit"": ""tablespoons"", ""text"": ""2 tablespoons olive oil""}}

If recipe text says: ""Salt, to taste""
Then ingredient should be: {{""name"": ""Salt"", ""amount"": """", ""unit"": """", ""text"": ""Salt, to taste""}}

If recipe text says: ""1/2 cup of flour""
Then ingredient should be: {{""name"": ""flour"", ""amount"": ""1/2"", ""unit"": ""cup"", ""text"": ""1/2 cup of flour""}}

**The ""text"" field MUST contain the COMPLETE, ORIGINAL ingredient line exactly as written in the recipe.**

Rules:
- Return ONLY valid JSON - no code, no explanations, no markdown blocks
- **CRITICAL**: The ""text"" field for each ingredient MUST be the complete original line from the recipe text
- Extract ingredients as structured objects with name, amount, unit, and full text (preserve original)
- Extract instructions as step-by-step strings - preserve the original instruction text
- For each ingredient, extract ""preparation"" from ingredient text itself (e.g., ""diced"" from ""2 cups onions, diced"")
- For each ingredient, extract ""instructions"" from recipe directions by analyzing how ingredients are mentioned (e.g., ""chopped"" from ""chop the onions"" in directions)
- Convert times to minutes (e.g., ""30 minutes"" becomes 30)
- Estimate confidence based on text clarity (0.0 to 1.0)
- If information is missing, use null or empty string
- Be conservative with confidence scores
- Extract category and tags when possible
- Estimate difficulty on a 1-5 scale
{optionsText}

Recipe text:
{text}

Return ONLY the JSON object:";

            return prompt.Trim();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to build recipe parsing prompt");
            throw new InvalidOperationException("Failed to build recipe parsing prompt", ex);
        }
    }

    /// <summary>
    /// Build a simple recipe parsing prompt without complex structure
    /// </summary>
    /// <param name="text">The recipe text to parse</param>
    /// <param name="options">Parsing options</param>
    /// <returns>Simplified prompt</returns>
    public string BuildSimpleRecipeParsingPrompt(string text, RecipeParsingOptions? options = null)
    {
        try
        {
            _logger.LogDebug("Building simple recipe parsing prompt for text length: {Length}", text.Length);

            var prompt = $@"
Parse this recipe and return JSON with title, ingredients (as array of strings), instructions (as array of strings), prepTimeMinutes, cookTimeMinutes, servings, and confidence (0.0-1.0):

{text}

JSON:";

            return prompt.Trim();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to build simple recipe parsing prompt");
            throw new InvalidOperationException("Failed to build simple recipe parsing prompt", ex);
        }
    }

    /// <summary>
    /// Build a function calling prompt for structured output
    /// </summary>
    /// <param name="text">The recipe text to parse</param>
    /// <param name="options">Parsing options</param>
    /// <returns>Function calling prompt</returns>
    public string BuildFunctionCallingPrompt(string text, RecipeParsingOptions? options = null)
    {
        try
        {
            _logger.LogDebug("Building function calling prompt for text length: {Length}", text.Length);

            var prompt = $@"
Parse the following recipe text and extract structured information. Use the provided function to return the parsed data.

Recipe text:
{text}

Please extract:
- Title
- Description (if present)
- Ingredients (with amounts, units, and names)
- Instructions (step by step)
- Timing information (prep, cook, total time)
- Servings
- Category and tags
- Difficulty level
- Any additional notes

Use the function call to return this information in a structured format.";

            return prompt.Trim();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to build function calling prompt");
            throw new InvalidOperationException("Failed to build function calling prompt", ex);
        }
    }

    private string BuildOptionsText(RecipeParsingOptions options)
    {
        var optionsList = new List<string>();

        if (options.RequiresFunctionCalling)
            optionsList.Add("- Use function calling if available for structured output");
        if (options.RequiresVision)
            optionsList.Add("- Analyze any images if present");
        if (options.MaxTokens.HasValue)
            optionsList.Add($"- Limit response to {options.MaxTokens} tokens");
        if (options.Temperature.HasValue)
            optionsList.Add($"- Use temperature {options.Temperature} for creativity");
        if (!string.IsNullOrEmpty(options.Language) && options.Language != "en")
            optionsList.Add($"- Parse in {options.Language} language");
        if (options.ExtractNutritionalInfo)
            optionsList.Add("- Extract nutritional information if available");
        if (options.ExtractCookingTechniques)
            optionsList.Add("- Extract cooking techniques and methods");
        if (options.ExtractEquipment)
            optionsList.Add("- Extract required equipment and tools");
        if (options.MaxRecipes > 1)
            optionsList.Add($"- Extract up to {options.MaxRecipes} recipes if multiple are present");
        if (!string.IsNullOrEmpty(options.CustomInstructions))
            optionsList.Add($"- Additional instructions: {options.CustomInstructions}");

        return optionsList.Any() ? "\n\nAdditional Requirements:\n" + string.Join("\n", optionsList) : string.Empty;
    }
}
