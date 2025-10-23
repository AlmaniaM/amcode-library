using AMCode.AI.Models;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace AMCode.AI.Services;

/// <summary>
/// Service for validating parsed recipes
/// </summary>
public class RecipeValidationService : IRecipeValidationService
{
    private readonly ILogger<RecipeValidationService> _logger;
    private readonly Dictionary<string, string> _validationRules;

    public RecipeValidationService(ILogger<RecipeValidationService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _validationRules = InitializeValidationRules();
    }

    public async Task<RecipeValidationResult> ValidateRecipeAsync(ParsedRecipe recipe, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Validating recipe: {Title}", recipe.Title);

        var result = new RecipeValidationResult
        {
            RecipeTitle = recipe.Title ?? "Untitled Recipe"
        };

        try
        {
            // Validate title
            ValidateTitle(recipe, result);

            // Validate ingredients
            ValidateIngredients(recipe, result);

            // Validate instructions
            ValidateInstructions(recipe, result);

            // Validate timing information
            ValidateTiming(recipe, result);

            // Validate servings
            ValidateServings(recipe, result);

            // Calculate overall score
            CalculateScore(result);

            _logger.LogInformation("Recipe validation completed. Score: {Score}, Valid: {IsValid}", 
                result.Score, result.IsValid);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during recipe validation");
            result.Issues.Add(new ValidationIssue
            {
                Type = "ValidationError",
                Severity = ValidationSeverity.Critical,
                Message = "An error occurred during validation",
                Field = "General",
                SuggestedFix = "Please try again or contact support"
            });
            result.Score = 0.0;
            result.IsValid = false;
            return result;
        }
    }

    public async Task<IEnumerable<RecipeValidationResult>> ValidateRecipesAsync(IEnumerable<ParsedRecipe> recipes, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Validating {Count} recipes in batch", recipes.Count());

        var results = new List<RecipeValidationResult>();
        
        foreach (var recipe in recipes)
        {
            var result = await ValidateRecipeAsync(recipe, cancellationToken);
            results.Add(result);
        }

        return results;
    }

    public Dictionary<string, string> GetValidationRules()
    {
        return new Dictionary<string, string>(_validationRules);
    }

    private void ValidateTitle(ParsedRecipe recipe, RecipeValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(recipe.Title))
        {
            result.Issues.Add(new ValidationIssue
            {
                Type = "MissingTitle",
                Severity = ValidationSeverity.Critical,
                Message = "Recipe title is required",
                Field = "Title",
                SuggestedFix = "Add a descriptive title for the recipe"
            });
        }
        else if (recipe.Title.Length < 3)
        {
            result.Issues.Add(new ValidationIssue
            {
                Type = "TitleTooShort",
                Severity = ValidationSeverity.Major,
                Message = "Recipe title is too short",
                Field = "Title",
                SuggestedFix = "Use a more descriptive title (at least 3 characters)"
            });
        }
        else if (recipe.Title.Length > 100)
        {
            result.Warnings.Add(new ValidationWarning
            {
                Type = "TitleTooLong",
                Message = "Recipe title is very long",
                Field = "Title",
                SuggestedImprovement = "Consider shortening the title for better readability"
            });
        }
    }

    private void ValidateIngredients(ParsedRecipe recipe, RecipeValidationResult result)
    {
        if (recipe.Ingredients == null || !recipe.Ingredients.Any())
        {
            result.Issues.Add(new ValidationIssue
            {
                Type = "MissingIngredients",
                Severity = ValidationSeverity.Critical,
                Message = "Recipe must have at least one ingredient",
                Field = "Ingredients",
                SuggestedFix = "Add ingredients to the recipe"
            });
        }
        else
        {
            var validIngredients = recipe.Ingredients.Where(i => !string.IsNullOrWhiteSpace(i.Text)).ToList();
            
            if (validIngredients.Count == 0)
            {
                result.Issues.Add(new ValidationIssue
                {
                    Type = "EmptyIngredients",
                    Severity = ValidationSeverity.Critical,
                    Message = "All ingredients are empty",
                    Field = "Ingredients",
                    SuggestedFix = "Add valid ingredient descriptions"
                });
            }
            else if (validIngredients.Count < 2)
            {
                result.Warnings.Add(new ValidationWarning
                {
                    Type = "FewIngredients",
                    Message = "Recipe has very few ingredients",
                    Field = "Ingredients",
                    SuggestedImprovement = "Consider adding more ingredients for a complete recipe"
                });
            }

            // Check for ingredient quality
            foreach (var ingredient in validIngredients)
            {
                if (string.IsNullOrWhiteSpace(ingredient.Text) || ingredient.Text.Length < 3)
                {
                    result.Warnings.Add(new ValidationWarning
                    {
                        Type = "IngredientTooShort",
                        Message = $"Ingredient '{ingredient.Text}' is very short",
                        Field = "Ingredients",
                        SuggestedImprovement = "Provide more detailed ingredient descriptions"
                    });
                }
            }
        }
    }

    private void ValidateInstructions(ParsedRecipe recipe, RecipeValidationResult result)
    {
        if (recipe.Instructions == null || !recipe.Instructions.Any())
        {
            result.Issues.Add(new ValidationIssue
            {
                Type = "MissingInstructions",
                Severity = ValidationSeverity.Critical,
                Message = "Recipe must have at least one instruction",
                Field = "Instructions",
                SuggestedFix = "Add cooking instructions to the recipe"
            });
        }
        else
        {
            var validInstructions = recipe.Instructions.Where(i => !string.IsNullOrWhiteSpace(i)).ToList();
            
            if (validInstructions.Count == 0)
            {
                result.Issues.Add(new ValidationIssue
                {
                    Type = "EmptyInstructions",
                    Severity = ValidationSeverity.Critical,
                    Message = "All instructions are empty",
                    Field = "Instructions",
                    SuggestedFix = "Add valid cooking instructions"
                });
            }
            else if (validInstructions.Count < 2)
            {
                result.Warnings.Add(new ValidationWarning
                {
                    Type = "FewInstructions",
                    Message = "Recipe has very few instructions",
                    Field = "Instructions",
                    SuggestedImprovement = "Consider adding more detailed cooking steps"
                });
            }

            // Check instruction quality
            foreach (var instruction in validInstructions)
            {
                if (instruction.Length < 10)
                {
                    result.Warnings.Add(new ValidationWarning
                    {
                        Type = "InstructionTooShort",
                        Message = "Instruction is very brief",
                        Field = "Instructions",
                        SuggestedImprovement = "Provide more detailed cooking instructions"
                    });
                }
            }
        }
    }

    private void ValidateTiming(ParsedRecipe recipe, RecipeValidationResult result)
    {
        var hasPrepTime = recipe.PrepTimeMinutes.HasValue && recipe.PrepTimeMinutes > 0;
        var hasCookTime = recipe.CookTimeMinutes.HasValue && recipe.CookTimeMinutes > 0;

        if (!hasPrepTime && !hasCookTime)
        {
            result.Warnings.Add(new ValidationWarning
            {
                Type = "MissingTiming",
                Message = "No timing information provided",
                Field = "Timing",
                SuggestedImprovement = "Add prep time and/or cook time for better user experience"
            });
        }

        if (hasPrepTime && recipe.PrepTimeMinutes > 480) // 8 hours
        {
            result.Warnings.Add(new ValidationWarning
            {
                Type = "LongPrepTime",
                Message = "Prep time seems unusually long",
                Field = "PrepTimeMinutes",
                SuggestedImprovement = "Verify prep time is correct"
            });
        }

        if (hasCookTime && recipe.CookTimeMinutes > 1440) // 24 hours
        {
            result.Warnings.Add(new ValidationWarning
            {
                Type = "LongCookTime",
                Message = "Cook time seems unusually long",
                Field = "CookTimeMinutes",
                SuggestedImprovement = "Verify cook time is correct"
            });
        }
    }

    private void ValidateServings(ParsedRecipe recipe, RecipeValidationResult result)
    {
        if (!recipe.Servings.HasValue)
        {
            result.Warnings.Add(new ValidationWarning
            {
                Type = "MissingServings",
                Message = "No serving information provided",
                Field = "Servings",
                SuggestedImprovement = "Add serving information for better user experience"
            });
        }
        else if (recipe.Servings <= 0)
        {
            result.Issues.Add(new ValidationIssue
            {
                Type = "InvalidServings",
                Severity = ValidationSeverity.Major,
                Message = "Servings must be greater than 0",
                Field = "Servings",
                SuggestedFix = "Enter a valid number of servings"
            });
        }
        else if (recipe.Servings > 50)
        {
            result.Warnings.Add(new ValidationWarning
            {
                Type = "HighServings",
                Message = "Servings count seems unusually high",
                Field = "Servings",
                SuggestedImprovement = "Verify serving count is correct"
            });
        }
    }

    private void CalculateScore(RecipeValidationResult result)
    {
        var totalWeight = 100.0;
        var score = totalWeight;

        // Deduct points for issues
        foreach (var issue in result.Issues)
        {
            score -= issue.Severity switch
            {
                ValidationSeverity.Critical => 30,
                ValidationSeverity.Major => 15,
                ValidationSeverity.Minor => 5,
                _ => 0
            };
        }

        // Deduct points for warnings (smaller penalty)
        foreach (var warning in result.Warnings)
        {
            score -= 2;
        }

        // Ensure score is between 0 and 100
        result.Score = Math.Max(0, Math.Min(100, score)) / 100.0;
        
        // Recipe is valid if score is above 0.7 and no critical issues
        result.IsValid = result.Score >= 0.7 && 
                        !result.Issues.Any(i => i.Severity == ValidationSeverity.Critical);
    }

    private Dictionary<string, string> InitializeValidationRules()
    {
        return new Dictionary<string, string>
        {
            { "MissingTitle", "Recipe must have a title" },
            { "TitleTooShort", "Recipe title should be at least 3 characters" },
            { "TitleTooLong", "Recipe title should be under 100 characters" },
            { "MissingIngredients", "Recipe must have at least one ingredient" },
            { "EmptyIngredients", "All ingredients must have content" },
            { "FewIngredients", "Recipe should have multiple ingredients" },
            { "IngredientTooShort", "Ingredients should be descriptive" },
            { "MissingInstructions", "Recipe must have at least one instruction" },
            { "EmptyInstructions", "All instructions must have content" },
            { "FewInstructions", "Recipe should have multiple instructions" },
            { "InstructionTooShort", "Instructions should be detailed" },
            { "MissingTiming", "Timing information improves user experience" },
            { "LongPrepTime", "Prep time seems unusually long" },
            { "LongCookTime", "Cook time seems unusually long" },
            { "MissingServings", "Serving information improves user experience" },
            { "InvalidServings", "Servings must be greater than 0" },
            { "HighServings", "Servings count seems unusually high" }
        };
    }
}
