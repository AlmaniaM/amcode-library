using AMCode.AI.Models;
using AMCode.AI.Services;
using AMCode.AI.Factories;
using AMCode.AI.Providers;
using AMCode.AI.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AMCode.AI.Tests.Integration;

/// <summary>
/// Integration tests for AI services
/// </summary>
public class AIIntegrationTests
{
    private readonly ServiceProvider _serviceProvider;

    public AIIntegrationTests()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AI:Provider"] = "OpenAI",
                ["AI:ConfidenceThreshold"] = "0.7",
                ["AI:FallbackEnabled"] = "true",
                ["AI:MaxRetries"] = "3",
                ["AI:RetryDelaySeconds"] = "1",
                ["AI:OpenAI:ApiKey"] = "test-key",
                ["AI:OpenAI:Model"] = "gpt-3.5-turbo",
                ["AI:OpenAI:Temperature"] = "0.1",
                ["AI:OpenAI:MaxTokens"] = "1000"
            })
            .Build();

        services.AddLogging();
        services.AddHttpClient();
        services.AddMultiCloudAI(configuration);

        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public void ServiceRegistration_ShouldRegisterAllServices()
    {
        // Act & Assert
        _serviceProvider.GetService<IRecipeParserService>().Should().NotBeNull();
        _serviceProvider.GetService<ICostAnalyzer>().Should().NotBeNull();
        _serviceProvider.GetService<IRecipeValidationService>().Should().NotBeNull();
        _serviceProvider.GetService<IAIProviderFactory>().Should().NotBeNull();
    }

    [Fact]
    public void RecipeValidationService_ShouldValidateRecipe()
    {
        // Arrange
        var validationService = _serviceProvider.GetRequiredService<IRecipeValidationService>();
        var recipe = new ParsedRecipe
        {
            Title = "Test Recipe",
            Ingredients = new List<RecipeIngredient>
            {
                new RecipeIngredient { Text = "1 cup flour" },
                new RecipeIngredient { Text = "2 eggs" }
            },
            Instructions = new List<string>
            {
                "Mix ingredients together",
                "Bake for 30 minutes"
            },
            PrepTimeMinutes = 15,
            CookTimeMinutes = 30,
            Servings = 4
        };

        // Act
        var result = validationService.ValidateRecipeAsync(recipe).Result;

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Score.Should().BeGreaterThan(0.7);
    }

    [Fact]
    public void RecipeValidationService_ShouldDetectInvalidRecipe()
    {
        // Arrange
        var validationService = _serviceProvider.GetRequiredService<IRecipeValidationService>();
        var recipe = new ParsedRecipe
        {
            Title = "", // Missing title
            Ingredients = new List<RecipeIngredient>(), // No ingredients
            Instructions = new List<string>() // No instructions
        };

        // Act
        var result = validationService.ValidateRecipeAsync(recipe).Result;

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Issues.Should().NotBeEmpty();
        result.Issues.Should().Contain(i => i.Type == "MissingTitle");
        result.Issues.Should().Contain(i => i.Type == "MissingIngredients");
        result.Issues.Should().Contain(i => i.Type == "MissingInstructions");
    }

    [Fact]
    public void AIProviderFactory_ShouldCreateProvider()
    {
        // Arrange
        var factory = _serviceProvider.GetRequiredService<IAIProviderFactory>();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["AI:TestProvider:ApiKey"] = "test-key",
                ["AI:TestProvider:Model"] = "gpt-3.5-turbo"
            })
            .Build();

        // Act
        var provider = factory.CreateProvider<OpenAIGPTProvider>("TestProvider", configuration);

        // Assert
        provider.Should().NotBeNull();
        provider.ProviderName.Should().Be("OpenAI GPT");
    }

    [Fact]
    public void CostAnalyzer_ShouldTrackCosts()
    {
        // Arrange
        var costAnalyzer = _serviceProvider.GetRequiredService<ICostAnalyzer>();

        // Act
        costAnalyzer.RecordCost("OpenAI", 0.001m);
        var totalCost = costAnalyzer.GetTotalCost("OpenAI");

        // Assert
        totalCost.Should().Be(0.001m);
    }

    [Fact]
    public void PromptBuilderService_ShouldBuildPrompt()
    {
        // Arrange
        var promptBuilder = _serviceProvider.GetRequiredService<PromptBuilderService>();
        var text = "Mix flour and eggs";
        var options = new RecipeParsingOptions
        {
            Temperature = 0.1f,
            MaxTokens = 500
        };

        // Act
        var prompt = promptBuilder.BuildRecipeParsingPrompt(text, options);

        // Assert
        prompt.Should().NotBeNullOrEmpty();
        prompt.Should().Contain(text);
        prompt.Should().Contain("JSON");
    }

    public void Dispose()
    {
        _serviceProvider?.Dispose();
    }
}
