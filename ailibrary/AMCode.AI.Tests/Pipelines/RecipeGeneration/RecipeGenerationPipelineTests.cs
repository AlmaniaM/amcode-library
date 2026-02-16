using AMCode.AI.Factories;
using AMCode.AI.Models;
using AMCode.AI.Pipelines;
using AMCode.AI.Pipelines.RecipeGeneration;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AMCode.AI.Tests.Pipelines.RecipeGeneration;

public class RecipeGenerationPipelineTests
{
    private readonly Mock<IAIProviderFactory> _mockProviderFactory;
    private readonly Mock<IAIProvider> _mockProvider;
    private readonly Mock<ILogger<RecipeGenerationPipeline>> _mockLogger;
    private readonly PipelineConfiguration _config;

    public RecipeGenerationPipelineTests()
    {
        _mockProviderFactory = new Mock<IAIProviderFactory>();
        _mockProvider = new Mock<IAIProvider>();
        _mockLogger = new Mock<ILogger<RecipeGenerationPipeline>>();
        _config = new PipelineConfiguration
        {
            Provider = "TestProvider",
            MaxTokens = 4000,
            Temperature = 0.7f,
            MaxRetries = 0, // No retries for unit tests
        };

        _mockProvider.Setup(p => p.ProviderName).Returns("TestProvider");
        _mockProviderFactory
            .Setup(f => f.CreateProviderByName("TestProvider", null))
            .Returns(_mockProvider.Object);
    }

    [Fact]
    public void PipelineName_ShouldBeRecipeGeneration()
    {
        // Arrange
        var pipeline = CreatePipeline();

        // Assert
        pipeline.PipelineName.Should().Be("RecipeGeneration");
    }

    [Fact]
    public async Task ExecuteAsync_WithSuccessfulResponse_ShouldReturnGeneratedRecipes()
    {
        // Arrange
        var pipeline = CreatePipeline();
        var input = CreateInput();
        var expectedOutput = new RecipeGenerationOutput
        {
            Recipes = new List<GeneratedRecipeItem>
            {
                new()
                {
                    Title = "Test Recipe",
                    Description = "A test recipe",
                    Ingredients = new List<GeneratedIngredient>
                    {
                        new() { Name = "chicken", Amount = "1", Unit = "lb", Text = "1 lb chicken" },
                    },
                    Directions = new List<string> { "Cook the chicken at 375°F (190°C) for 25 minutes." },
                    PrepTimeMinutes = 10,
                    CookTimeMinutes = 25,
                    Servings = 4,
                    Difficulty = "Easy",
                    EstimatedCostPerServing = 5.0m,
                    Cuisine = "American",
                    Tags = new List<string> { "quick", "easy" },
                    AllergenWarnings = new List<string>(),
                    SafetyNotes = new List<string>(),
                    Confidence = 0.9,
                }
            }
        };

        _mockProvider
            .Setup(p => p.CompleteJsonAsync<RecipeGenerationOutput>(
                It.IsAny<AIJsonRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AIJsonResult<RecipeGenerationOutput>.Ok(expectedOutput, "TestProvider"));

        // Act
        var result = await pipeline.ExecuteAsync(input);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Recipes.Should().HaveCount(1);
        result.Value.Recipes[0].Title.Should().Be("Test Recipe");
    }

    [Fact]
    public async Task ExecuteAsync_WithProviderFailure_ShouldReturnFailure()
    {
        // Arrange
        var pipeline = CreatePipeline();
        var input = CreateInput();

        _mockProvider
            .Setup(p => p.CompleteJsonAsync<RecipeGenerationOutput>(
                It.IsAny<AIJsonRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AIJsonResult<RecipeGenerationOutput>.Fail("Provider error", "TestProvider"));

        // Act
        var result = await pipeline.ExecuteAsync(input);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Provider error");
    }

    [Fact]
    public async Task ExecuteAsync_WithEmptyRecipeList_ShouldReturnFailure()
    {
        // Arrange
        var pipeline = CreatePipeline();
        var input = CreateInput();
        var emptyOutput = new RecipeGenerationOutput { Recipes = new List<GeneratedRecipeItem>() };

        _mockProvider
            .Setup(p => p.CompleteJsonAsync<RecipeGenerationOutput>(
                It.IsAny<AIJsonRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AIJsonResult<RecipeGenerationOutput>.Ok(emptyOutput, "TestProvider"));

        // Act
        var result = await pipeline.ExecuteAsync(input);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("empty recipe list");
    }

    [Fact]
    public async Task ExecuteAsync_ShouldRunSafetyValidation()
    {
        // Arrange
        var pipeline = CreatePipeline();
        var input = CreateInput(allergies: "peanuts");
        var outputWithAllergen = new RecipeGenerationOutput
        {
            Recipes = new List<GeneratedRecipeItem>
            {
                new()
                {
                    Title = "Bad Recipe",
                    Ingredients = new List<GeneratedIngredient>
                    {
                        new() { Name = "peanuts", Text = "1 cup peanuts" },
                    },
                    Directions = new List<string> { "Mix" },
                    AllergenWarnings = new List<string>(),
                    SafetyNotes = new List<string>(),
                }
            }
        };

        _mockProvider
            .Setup(p => p.CompleteJsonAsync<RecipeGenerationOutput>(
                It.IsAny<AIJsonRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AIJsonResult<RecipeGenerationOutput>.Ok(outputWithAllergen, "TestProvider"));

        // Act
        var result = await pipeline.ExecuteAsync(input);

        // Assert — safety validator should have added warnings
        result.IsSuccess.Should().BeTrue();
        result.Value!.Recipes[0].AllergenWarnings.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldPassCorrectTemperatureAndTokenSettings()
    {
        // Arrange
        var pipeline = CreatePipeline();
        var input = CreateInput();

        AIJsonRequest? capturedRequest = null;
        _mockProvider
            .Setup(p => p.CompleteJsonAsync<RecipeGenerationOutput>(
                It.IsAny<AIJsonRequest>(), It.IsAny<CancellationToken>()))
            .Callback<AIJsonRequest, CancellationToken>((req, _) => capturedRequest = req)
            .ReturnsAsync(AIJsonResult<RecipeGenerationOutput>.Fail("capture only", "TestProvider"));

        // Act
        await pipeline.ExecuteAsync(input);

        // Assert
        capturedRequest.Should().NotBeNull();
        capturedRequest!.MaxTokens.Should().Be(4000);
        capturedRequest.Temperature.Should().Be(0.7f);
        capturedRequest.StrictJsonMode.Should().BeTrue();
        capturedRequest.SystemMessage.Should().NotBeNullOrEmpty();
        capturedRequest.Prompt.Should().Contain("chicken");
    }

    #region Helpers

    private RecipeGenerationPipeline CreatePipeline()
    {
        return new RecipeGenerationPipeline(
            _mockProviderFactory.Object,
            _config,
            _mockLogger.Object);
    }

    private static RecipeGenerationInput CreateInput(string? allergies = null)
    {
        return new RecipeGenerationInput
        {
            Ingredients = new List<string> { "chicken", "rice" },
            Cuisine = "Italian",
            Servings = 4,
            RecipeCount = 1,
            Allergies = allergies,
        };
    }

    #endregion
}
