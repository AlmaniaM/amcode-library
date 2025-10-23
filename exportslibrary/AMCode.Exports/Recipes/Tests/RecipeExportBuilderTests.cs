using AMCode.Common.Models;
using AMCode.Exports.Components.ExportBuilder;
using AMCode.Exports.Components.Results.Models;
using AMCode.Exports.Recipes.Interfaces;
using AMCode.Exports.Recipes.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AMCode.Exports.Recipes.Tests
{
    [TestFixture]
    public class RecipeExportBuilderTests
    {
        private Mock<IExcelExportBuilder> _mockExcelBuilder;
        private Mock<ICsvExportBuilder> _mockCsvBuilder;
        private Mock<ILogger<RecipeExportBuilder>> _mockLogger;
        private RecipeExportBuilder _builder;
        private List<Recipe> _testRecipes;
        
        [SetUp]
        public void Setup()
        {
            _mockExcelBuilder = new Mock<IExcelExportBuilder>();
            _mockCsvBuilder = new Mock<ICsvExportBuilder>();
            _mockLogger = new Mock<ILogger<RecipeExportBuilder>>();
            _builder = new RecipeExportBuilder(_mockExcelBuilder.Object, _mockCsvBuilder.Object, _mockLogger.Object);
            
            _testRecipes = new List<Recipe>
            {
                new Recipe
                {
                    Title = "Test Recipe 1",
                    Category = "Breakfast",
                    PrepTimeMinutes = 15,
                    CookTimeMinutes = 30,
                    Servings = 4,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient { Name = "Flour", Amount = "2", Unit = "cups", Text = "2 cups flour" },
                        new RecipeIngredient { Name = "Sugar", Amount = "1", Unit = "cup", Text = "1 cup sugar" }
                    },
                    Instructions = new List<string> { "Mix ingredients", "Bake at 350°F" },
                    Tags = new List<string> { "test", "breakfast" },
                    CreatedAt = DateTime.UtcNow
                },
                new Recipe
                {
                    Title = "Test Recipe 2",
                    Category = "Lunch",
                    PrepTimeMinutes = 10,
                    CookTimeMinutes = 20,
                    Servings = 2,
                    Ingredients = new List<RecipeIngredient>
                    {
                        new RecipeIngredient { Name = "Bread", Amount = "2", Unit = "slices", Text = "2 slices bread" },
                        new RecipeIngredient { Name = "Cheese", Amount = "1", Unit = "slice", Text = "1 slice cheese" }
                    },
                    Instructions = new List<string> { "Assemble sandwich", "Toast" },
                    Tags = new List<string> { "test", "lunch" },
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
        
        [Test]
        public async Task ExportRecipesAsync_WithExcelFormat_ReturnsSuccess()
        {
            // Arrange
            var mockStream = new MemoryStream();
            var mockResult = new Mock<IExportResult>();
            mockResult.Setup(x => x.Stream).Returns(mockStream);
            
            _mockExcelBuilder.Setup(x => x.CreateExportAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<IEnumerable<IExcelDataColumn>>(), It.IsAny<ExcelBookBuilderConfig>()))
                .ReturnsAsync(mockResult.Object);
            
            var options = new RecipeExportOptions { Format = "excel", Title = "Test Export" };
            
            // Act
            var result = await _builder.ExportRecipesAsync(_testRecipes, options);
            
            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }
        
        [Test]
        public async Task ExportRecipesAsync_WithCsvFormat_ReturnsSuccess()
        {
            // Arrange
            var mockStream = new MemoryStream();
            var mockResult = new Mock<IExportResult>();
            mockResult.Setup(x => x.Stream).Returns(mockStream);
            
            _mockCsvBuilder.Setup(x => x.CreateExportAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<IEnumerable<ICsvDataColumn>>(), It.IsAny<CsvBookBuilderConfig>()))
                .ReturnsAsync(mockResult.Object);
            
            var options = new RecipeExportOptions { Format = "csv", Title = "Test Export" };
            
            // Act
            var result = await _builder.ExportRecipesAsync(_testRecipes, options);
            
            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }
        
        [Test]
        public async Task ExportRecipesAsync_WithUnsupportedFormat_ReturnsFailure()
        {
            // Arrange
            var options = new RecipeExportOptions { Format = "unsupported", Title = "Test Export" };
            
            // Act
            var result = await _builder.ExportRecipesAsync(_testRecipes, options);
            
            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Error.Contains("Unsupported format"));
        }
        
        [Test]
        public async Task ExportRecipesAsync_WithCustomColumns_ReturnsSuccess()
        {
            // Arrange
            var mockStream = new MemoryStream();
            var mockResult = new Mock<IExportResult>();
            mockResult.Setup(x => x.Stream).Returns(mockStream);
            
            _mockExcelBuilder.Setup(x => x.CreateExportAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<IEnumerable<IExcelDataColumn>>(), It.IsAny<ExcelBookBuilderConfig>()))
                .ReturnsAsync(mockResult.Object);
            
            var options = new RecipeExportOptions { Format = "excel", Title = "Test Export" };
            var columnConfig = new RecipeColumnConfiguration
            {
                IncludeTitle = true,
                IncludeCategory = true,
                IncludeTiming = false,
                IncludeServings = false
            };
            
            // Act
            var result = await _builder.ExportRecipesAsync(_testRecipes, options, columnConfig);
            
            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }
        
        [Test]
        public async Task ExportShoppingListAsync_WithValidRecipes_ReturnsSuccess()
        {
            // Arrange
            var mockStream = new MemoryStream();
            var mockResult = new Mock<IExportResult>();
            mockResult.Setup(x => x.Stream).Returns(mockStream);
            
            _mockExcelBuilder.Setup(x => x.CreateExportAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<IEnumerable<IExcelDataColumn>>(), It.IsAny<ExcelBookBuilderConfig>()))
                .ReturnsAsync(mockResult.Object);
            
            var options = new RecipeExportOptions { Format = "excel", Title = "Shopping List" };
            
            // Act
            var result = await _builder.ExportShoppingListAsync(_testRecipes, options);
            
            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }
        
        [Test]
        public async Task ExportShoppingListAsync_ConsolidatesIngredients()
        {
            // Arrange
            var mockStream = new MemoryStream();
            var mockResult = new Mock<IExportResult>();
            mockResult.Setup(x => x.Stream).Returns(mockStream);
            
            _mockExcelBuilder.Setup(x => x.CreateExportAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<IEnumerable<IExcelDataColumn>>(), It.IsAny<ExcelBookBuilderConfig>()))
                .ReturnsAsync(mockResult.Object);
            
            var options = new RecipeExportOptions { Format = "excel", Title = "Shopping List" };
            
            // Act
            await _builder.ExportShoppingListAsync(_testRecipes, options);
            
            // Assert
            // Verify that the export was called with consolidated ingredients
            _mockExcelBuilder.Verify(x => x.CreateExportAsync(
                "Shopping List", 
                It.IsAny<int>(), 
                It.IsAny<IEnumerable<IExcelDataColumn>>(), 
                It.IsAny<ExcelBookBuilderConfig>()), 
                Times.Once);
        }
        
        [Test]
        public async Task ExportRecipesAsync_WithException_ReturnsFailure()
        {
            // Arrange
            _mockExcelBuilder.Setup(x => x.CreateExportAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<IEnumerable<IExcelDataColumn>>(), It.IsAny<ExcelBookBuilderConfig>()))
                .Throws(new Exception("Test exception"));
            
            var options = new RecipeExportOptions { Format = "excel", Title = "Test Export" };
            
            // Act
            var result = await _builder.ExportRecipesAsync(_testRecipes, options);
            
            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Error.Contains("Export failed"));
        }
    }
}
