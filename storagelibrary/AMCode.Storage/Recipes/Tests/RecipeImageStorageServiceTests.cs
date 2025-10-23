using AMCode.Common.Models;
using AMCode.Storage.Components.Storage;
using AMCode.Storage.Interfaces;
using AMCode.Storage.Recipes.Interfaces;
using AMCode.Storage.Recipes.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AMCode.Storage.Recipes.Tests
{
    [TestFixture]
    public class RecipeImageStorageServiceTests
    {
        private Mock<ISimpleFileStorage> _mockFileStorage;
        private Mock<IStorageLogger> _mockLogger;
        private RecipeImageStorageOptions _options;
        private RecipeImageStorageService _service;
        
        [SetUp]
        public void Setup()
        {
            _mockFileStorage = new Mock<ISimpleFileStorage>();
            _mockLogger = new Mock<IStorageLogger>();
            _options = new RecipeImageStorageOptions
            {
                BasePath = "test-storage",
                SupportedImageExtensions = new List<string> { ".jpg", ".png", ".gif" }
            };
            _service = new RecipeImageStorageService(_mockFileStorage.Object, _mockLogger.Object, _options);
        }
        
        [Test]
        public async Task StoreRecipeImageAsync_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var recipeId = "test-recipe-1";
            var imageStream = new MemoryStream();
            var fileName = "test-image.jpg";
            var imageType = RecipeImageType.Main;
            
            _mockFileStorage.Setup(x => x.StoreFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(Result.Success());
            
            // Act
            var result = await _service.StoreRecipeImageAsync(recipeId, imageStream, fileName, imageType);
            
            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.IsTrue(result.Value.Contains(recipeId));
            Assert.IsTrue(result.Value.Contains("main"));
        }
        
        [Test]
        public async Task StoreRecipeImageAsync_WithStorageFailure_ReturnsFailure()
        {
            // Arrange
            var recipeId = "test-recipe-1";
            var imageStream = new MemoryStream();
            var fileName = "test-image.jpg";
            var imageType = RecipeImageType.Main;
            
            _mockFileStorage.Setup(x => x.StoreFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(Result.Failure("Storage failed"));
            
            // Act
            var result = await _service.StoreRecipeImageAsync(recipeId, imageStream, fileName, imageType);
            
            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Storage failed", result.Error);
        }
        
        [Test]
        public async Task GetRecipeImageAsync_WithValidRecipeId_ReturnsSuccess()
        {
            // Arrange
            var recipeId = "test-recipe-1";
            var imageType = RecipeImageType.Main;
            var mockStream = new MemoryStream();
            var mockResponse = new Mock<IFileDownloadResponse>();
            mockResponse.Setup(x => x.Stream).Returns(mockStream);
            
            _mockFileStorage.Setup(x => x.GetFileAsync(It.IsAny<string>()))
                .ReturnsAsync(Result.Success(mockResponse.Object));
            
            // Act
            var result = await _service.GetRecipeImageAsync(recipeId, imageType);
            
            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }
        
        [Test]
        public async Task GetRecipeImageAsync_WithFileNotFound_ReturnsFailure()
        {
            // Arrange
            var recipeId = "test-recipe-1";
            var imageType = RecipeImageType.Main;
            
            _mockFileStorage.Setup(x => x.GetFileAsync(It.IsAny<string>()))
                .ReturnsAsync(Result.Failure<IFileDownloadResponse>("File not found"));
            
            // Act
            var result = await _service.GetRecipeImageAsync(recipeId, imageType);
            
            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Error.Contains("File not found"));
        }
        
        [Test]
        public async Task DeleteRecipeImageAsync_WithValidRecipeId_ReturnsSuccess()
        {
            // Arrange
            var recipeId = "test-recipe-1";
            var imageType = RecipeImageType.Main;
            
            _mockFileStorage.Setup(x => x.DeleteFileAsync(It.IsAny<string>()))
                .ReturnsAsync(Result.Success());
            
            // Act
            var result = await _service.DeleteRecipeImageAsync(recipeId, imageType);
            
            // Assert
            Assert.IsTrue(result.IsSuccess);
        }
        
        [Test]
        public async Task ListRecipeImagesAsync_WithValidRecipeId_ReturnsSuccess()
        {
            // Arrange
            var recipeId = "test-recipe-1";
            var mockFiles = new List<string>
            {
                "test-storage/recipes/test-recipe-1/main/image1.jpg",
                "test-storage/recipes/test-recipe-1/thumbnails/thumb1.jpg",
                "test-storage/recipes/test-recipe-1/steps/step1.jpg"
            };
            
            _mockFileStorage.Setup(x => x.ListFilesAsync(It.IsAny<string>()))
                .ReturnsAsync(Result.Success(mockFiles));
            
            // Act
            var result = await _service.ListRecipeImagesAsync(recipeId);
            
            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(3, result.Value.Count);
        }
        
        [Test]
        public async Task StoreRecipeDocumentAsync_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var recipeId = "test-recipe-1";
            var documentStream = new MemoryStream();
            var fileName = "test-recipe.pdf";
            var documentType = RecipeDocumentType.PDF;
            
            _mockFileStorage.Setup(x => x.StoreFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(Result.Success());
            
            // Act
            var result = await _service.StoreRecipeDocumentAsync(recipeId, documentStream, fileName, documentType);
            
            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.IsTrue(result.Value.Contains(recipeId));
            Assert.IsTrue(result.Value.Contains("documents"));
            Assert.IsTrue(result.Value.EndsWith(".pdf"));
        }
        
        [Test]
        public async Task StoreRecipeExportAsync_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var recipeId = "test-recipe-1";
            var exportStream = new MemoryStream();
            var fileName = "recipe-export.xlsx";
            var exportType = RecipeExportType.Excel;
            
            _mockFileStorage.Setup(x => x.StoreFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync(Result.Success());
            
            // Act
            var result = await _service.StoreRecipeExportAsync(recipeId, exportStream, fileName, exportType);
            
            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.IsTrue(result.Value.Contains(recipeId));
            Assert.IsTrue(result.Value.Contains("exports"));
            Assert.IsTrue(result.Value.EndsWith(".xlsx"));
        }
        
        [Test]
        public async Task StoreRecipeImageAsync_WithException_ReturnsFailure()
        {
            // Arrange
            var recipeId = "test-recipe-1";
            var imageStream = new MemoryStream();
            var fileName = "test-image.jpg";
            var imageType = RecipeImageType.Main;
            
            _mockFileStorage.Setup(x => x.StoreFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .Throws(new Exception("Test exception"));
            
            // Act
            var result = await _service.StoreRecipeImageAsync(recipeId, imageStream, fileName, imageType);
            
            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Error.Contains("Failed to store recipe image"));
        }
    }
}
