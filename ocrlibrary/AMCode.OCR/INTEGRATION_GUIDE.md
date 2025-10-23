# AMCode.OCR Integration Guide

## 🎯 Overview

This guide provides comprehensive instructions for integrating AMCode.OCR into your .NET applications, including setup, configuration, and best practices.

## 📦 Installation

### Package Manager Console

```powershell
Install-Package AMCode.OCR -Version 1.0.0
```

### .NET CLI

```bash
dotnet add package AMCode.OCR --version 1.0.0
```

### PackageReference

```xml
<PackageReference Include="AMCode.OCR" Version="1.0.0" />
```

## 🔧 Basic Integration

### 1. Add Required Services

```csharp
using AMCode.OCR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Add AMCode.OCR services
                services.AddAMCodeOCR(context.Configuration);
                
                // Add your application services
                services.AddScoped<MyOCRService>();
            });
}
```

### 2. Configuration Setup

#### appsettings.json

```json
{
  "AMCode": {
    "OCR": {
      "DefaultProvider": "Azure",
      "FallbackProviders": ["AWS", "Google"],
      "ConfidenceThreshold": 0.7,
      "MaxRetries": 3,
      "EnableFallbackProviders": true,
      "MaxFallbackProviders": 2,
      "EnableBatchProcessing": true,
      "MaxBatchSize": 10,
      "Azure": {
        "SubscriptionKey": "your-azure-key",
        "Endpoint": "https://your-endpoint.cognitiveservices.azure.com/",
        "Region": "eastus"
      },
      "AWS": {
        "Region": "us-east-1",
        "AccessKey": "your-access-key",
        "SecretKey": "your-secret-key"
      },
      "Google": {
        "ProjectId": "your-project-id",
        "CredentialsPath": "path/to/credentials.json"
      }
    }
  }
}
```

### 3. Basic Usage

```csharp
public class MyOCRService
{
    private readonly IOCRService _ocrService;
    private readonly ILogger<MyOCRService> _logger;

    public MyOCRService(IOCRService ocrService, ILogger<MyOCRService> logger)
    {
        _ocrService = ocrService;
        _logger = logger;
    }

    public async Task<string> ExtractTextFromImageAsync(string imagePath)
    {
        try
        {
            var result = await _ocrService.ExtractTextAsync(imagePath);
            
            if (result.IsSuccess)
            {
                _logger.LogInformation("OCR successful: {Text}", result.Value.Text);
                return result.Value.Text;
            }
            else
            {
                _logger.LogError("OCR failed: {Error}", result.Error);
                throw new Exception($"OCR failed: {result.Error}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OCR processing failed");
            throw;
        }
    }
}
```

## 🏗️ Advanced Integration

### 1. Custom Configuration

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Configure OCR with custom settings
    services.Configure<OCRConfiguration>(options =>
    {
        options.DefaultProvider = "Azure";
        options.FallbackProviders = new[] { "AWS", "Google" };
        options.ConfidenceThreshold = 0.8;
        options.MaxRetries = 5;
        options.EnableFallbackProviders = true;
        options.MaxFallbackProviders = 3;
        options.EnableBatchProcessing = true;
        options.MaxBatchSize = 20;
    });

    // Configure Azure specifically
    services.Configure<AzureOCRConfiguration>(options =>
    {
        options.SubscriptionKey = "your-key";
        options.Endpoint = "https://your-endpoint.cognitiveservices.azure.com/";
        options.Region = "eastus";
        options.CostPerRequest = 0.001;
    });

    services.AddAMCodeOCR();
}
```

### 2. Custom Provider Registration

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddAMCodeOCR();
    
    // Register custom OCR provider
    services.AddScoped<IOCRProvider, CustomOCRProvider>();
}
```

### 3. Health Checks Integration

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddAMCodeOCR();
    
    // Add health checks
    services.AddHealthChecks()
        .AddCheck<OCRHealthCheck>("ocr");
}

public class OCRHealthCheck : IHealthCheck
{
    private readonly IOCRService _ocrService;

    public OCRHealthCheck(IOCRService ocrService)
    {
        _ocrService = ocrService;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var isAvailable = await _ocrService.IsAvailableAsync();
            if (!isAvailable)
            {
                return HealthCheckResult.Unhealthy("OCR service is not available");
            }

            var health = await _ocrService.GetHealthAsync();
            if (!health.IsHealthy)
            {
                return HealthCheckResult.Degraded($"OCR service is unhealthy: {health.Status}");
            }

            return HealthCheckResult.Healthy("OCR service is healthy");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"OCR health check failed: {ex.Message}");
        }
    }
}
```

## 🔄 ASP.NET Core Integration

### 1. Controller Integration

```csharp
[ApiController]
[Route("api/[controller]")]
public class OCRController : ControllerBase
{
    private readonly IOCRService _ocrService;
    private readonly ILogger<OCRController> _logger;

    public OCRController(IOCRService ocrService, ILogger<OCRController> logger)
    {
        _ocrService = ocrService;
        _logger = logger;
    }

    [HttpPost("extract-text")]
    public async Task<IActionResult> ExtractText(IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            return BadRequest("No image provided");
        }

        try
        {
            using var stream = image.OpenReadStream();
            var result = await _ocrService.ExtractTextAsync(stream);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    text = result.Value.Text,
                    confidence = result.Value.Confidence,
                    language = result.Value.Language,
                    provider = result.Value.Provider,
                    processingTime = result.Value.ProcessingTime
                });
            }
            else
            {
                return BadRequest(new { error = result.Error });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OCR processing failed");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    [HttpPost("batch-extract")]
    public async Task<IActionResult> BatchExtract(IFormFileCollection images)
    {
        if (images == null || !images.Any())
        {
            return BadRequest("No images provided");
        }

        try
        {
            var streams = images.Select(img => img.OpenReadStream()).ToList();
            var result = await _ocrService.ProcessBatchAsync(streams);

            if (result.IsSuccess)
            {
                var results = result.Value.Select(r => new
                {
                    text = r.Text,
                    confidence = r.Confidence,
                    language = r.Language,
                    provider = r.Provider,
                    processingTime = r.ProcessingTime
                }).ToList();

                return Ok(results);
            }
            else
            {
                return BadRequest(new { error = result.Error });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Batch OCR processing failed");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }

    [HttpGet("health")]
    public async Task<IActionResult> GetHealth()
    {
        try
        {
            var health = await _ocrService.GetHealthAsync();
            return Ok(new
            {
                status = health.Status,
                isHealthy = health.IsHealthy,
                isAvailable = health.IsAvailable,
                successRate = health.SuccessRate,
                averageProcessingTime = health.AverageProcessingTime
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            return StatusCode(500, new { error = "Health check failed" });
        }
    }
}
```

### 2. Middleware Integration

```csharp
public class OCRMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IOCRService _ocrService;
    private readonly ILogger<OCRMiddleware> _logger;

    public OCRMiddleware(RequestDelegate next, IOCRService ocrService, ILogger<OCRMiddleware> logger)
    {
        _next = next;
        _ocrService = ocrService;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Add OCR service to context for downstream components
        context.Items["OCRService"] = _ocrService;
        
        await _next(context);
    }
}

// Register middleware
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseMiddleware<OCRMiddleware>();
    // ... other middleware
}
```

## 🧪 Testing Integration

### 1. Unit Testing

```csharp
[TestClass]
public class MyOCRServiceTests
{
    private Mock<IOCRService> _mockOCRService = null!;
    private Mock<ILogger<MyOCRService>> _mockLogger = null!;
    private MyOCRService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockOCRService = new Mock<IOCRService>();
        _mockLogger = new Mock<ILogger<MyOCRService>>();
        _service = new MyOCRService(_mockOCRService.Object, _mockLogger.Object);
    }

    [TestMethod]
    public async Task ExtractTextFromImageAsync_WithValidImage_ShouldReturnText()
    {
        // Arrange
        var imagePath = "test-image.jpg";
        var expectedText = "Sample extracted text";
        var ocrResult = new OCRResult
        {
            Text = expectedText,
            Confidence = 0.9,
            Language = "en",
            Provider = "Azure Computer Vision"
        };

        _mockOCRService
            .Setup(x => x.ExtractTextAsync(imagePath, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(ocrResult));

        // Act
        var result = await _service.ExtractTextFromImageAsync(imagePath);

        // Assert
        result.Should().Be(expectedText);
        _mockOCRService.Verify(x => x.ExtractTextAsync(imagePath, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task ExtractTextFromImageAsync_WithOCRFailure_ShouldThrowException()
    {
        // Arrange
        var imagePath = "test-image.jpg";
        var errorMessage = "OCR processing failed";

        _mockOCRService
            .Setup(x => x.ExtractTextAsync(imagePath, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<OCRResult>(errorMessage));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<Exception>(() => 
            _service.ExtractTextFromImageAsync(imagePath));
    }
}
```

### 2. Integration Testing

```csharp
[TestClass]
public class OCRIntegrationTests
{
    private ServiceProvider _serviceProvider = null!;
    private IOCRService _ocrService = null!;

    [TestInitialize]
    public void Setup()
    {
        var services = new ServiceCollection();
        
        // Configure for testing
        services.AddLogging(builder => builder.AddConsole());
        services.Configure<OCRConfiguration>(options =>
        {
            options.DefaultProvider = "Test";
            options.EnableFallbackProviders = false;
        });
        
        services.AddAMCodeOCR();
        services.AddScoped<IOCRProvider, TestOCRProvider>();
        
        _serviceProvider = services.BuildServiceProvider();
        _ocrService = _serviceProvider.GetRequiredService<IOCRService>();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _serviceProvider?.Dispose();
    }

    [TestMethod]
    public async Task ExtractTextAsync_WithTestProvider_ShouldReturnResult()
    {
        // Arrange
        var imageBytes = Encoding.UTF8.GetBytes("test image data");
        using var imageStream = new MemoryStream(imageBytes);

        // Act
        var result = await _ocrService.ExtractTextAsync(imageStream);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Text.Should().NotBeNullOrEmpty();
        result.Value.Provider.Should().Be("Test OCR Provider");
    }
}
```

## 🔒 Security Considerations

### 1. API Key Management

```csharp
// Use Azure Key Vault
public void ConfigureServices(IServiceCollection services)
{
    services.AddAzureKeyVault();
    
    services.Configure<AzureOCRConfiguration>(options =>
    {
        options.SubscriptionKey = _configuration["AzureOCR:SubscriptionKey"];
        options.Endpoint = _configuration["AzureOCR:Endpoint"];
    });
}
```

### 2. Environment Variables

```bash
# Production environment variables
export AzureOCR__SubscriptionKey="your-production-key"
export AzureOCR__Endpoint="https://your-production-endpoint.cognitiveservices.azure.com/"
export AWS__AccessKey="your-production-access-key"
export AWS__SecretKey="your-production-secret-key"
export GoogleOCR__ProjectId="your-production-project-id"
```

### 3. Input Validation

```csharp
public class OCRRequestValidator : IValidator<OCRRequest>
{
    public ValidationResult Validate(OCRRequest request)
    {
        var result = new ValidationResult();

        if (request.ImageStream == null && string.IsNullOrEmpty(request.ImagePath))
        {
            result.Errors.Add("Either ImageStream or ImagePath must be provided");
        }

        if (request.ConfidenceThreshold < 0 || request.ConfidenceThreshold > 1)
        {
            result.Errors.Add("ConfidenceThreshold must be between 0 and 1");
        }

        if (request.MaxImageSizeMB <= 0)
        {
            result.Errors.Add("MaxImageSizeMB must be greater than 0");
        }

        return result;
    }
}
```

## 📊 Monitoring and Logging

### 1. Structured Logging

```csharp
public class OCRService
{
    private readonly IOCRService _ocrService;
    private readonly ILogger<OCRService> _logger;

    public async Task<OCRResult> ProcessImageAsync(Stream imageStream)
    {
        using var activity = Activity.StartActivity("OCR.ProcessImage");
        activity?.SetTag("image.size", imageStream.Length);

        try
        {
            var result = await _ocrService.ExtractTextAsync(imageStream);
            
            if (result.IsSuccess)
            {
                _logger.LogInformation("OCR processing completed successfully. " +
                    "Provider: {Provider}, Confidence: {Confidence}, ProcessingTime: {ProcessingTime}ms",
                    result.Value.Provider, result.Value.Confidence, result.Value.ProcessingTime.TotalMilliseconds);
                
                activity?.SetTag("ocr.success", true);
                activity?.SetTag("ocr.confidence", result.Value.Confidence);
                activity?.SetTag("ocr.provider", result.Value.Provider);
            }
            else
            {
                _logger.LogWarning("OCR processing failed: {Error}", result.Error);
                activity?.SetTag("ocr.success", false);
                activity?.SetTag("ocr.error", result.Error);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OCR processing threw an exception");
            activity?.SetTag("ocr.success", false);
            activity?.SetTag("ocr.exception", ex.Message);
            throw;
        }
    }
}
```

### 2. Metrics Collection

```csharp
public class OCRMetrics
{
    private readonly Counter _ocrRequestsTotal;
    private readonly Histogram _ocrProcessingTime;
    private readonly Counter _ocrErrorsTotal;

    public OCRMetrics()
    {
        _ocrRequestsTotal = Metrics.CreateCounter("ocr_requests_total", "Total number of OCR requests");
        _ocrProcessingTime = Metrics.CreateHistogram("ocr_processing_time_seconds", "OCR processing time in seconds");
        _ocrErrorsTotal = Metrics.CreateCounter("ocr_errors_total", "Total number of OCR errors");
    }

    public void RecordOCRRequest(string provider)
    {
        _ocrRequestsTotal.WithLabels(provider).Inc();
    }

    public void RecordOCRProcessingTime(TimeSpan processingTime)
    {
        _ocrProcessingTime.Observe(processingTime.TotalSeconds);
    }

    public void RecordOCRError(string provider, string errorType)
    {
        _ocrErrorsTotal.WithLabels(provider, errorType).Inc();
    }
}
```

## 🚀 Performance Optimization

### 1. Caching

```csharp
public class CachedOCRService
{
    private readonly IOCRService _ocrService;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachedOCRService> _logger;

    public async Task<OCRResult> ExtractTextAsync(Stream imageStream, string cacheKey = null)
    {
        if (!string.IsNullOrEmpty(cacheKey) && _cache.TryGetValue(cacheKey, out OCRResult cachedResult))
        {
            _logger.LogInformation("Returning cached OCR result for key: {CacheKey}", cacheKey);
            return cachedResult;
        }

        var result = await _ocrService.ExtractTextAsync(imageStream);
        
        if (result.IsSuccess && !string.IsNullOrEmpty(cacheKey))
        {
            _cache.Set(cacheKey, result.Value, TimeSpan.FromHours(1));
            _logger.LogInformation("Cached OCR result for key: {CacheKey}", cacheKey);
        }

        return result;
    }
}
```

### 2. Connection Pooling

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddHttpClient("OCR", client =>
    {
        client.Timeout = TimeSpan.FromMinutes(5);
    })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        MaxConnectionsPerServer = 10
    });

    services.AddAMCodeOCR();
}
```

## 🔄 Error Handling and Retry

### 1. Retry Policy

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddAMCodeOCR();
    
    // Add retry policy
    services.AddHttpClient("OCR")
        .AddPolicyHandler(GetRetryPolicy());
}

private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => !msg.IsSuccessStatusCode)
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (outcome, timespan, retryCount, context) =>
            {
                Console.WriteLine($"Retry {retryCount} after {timespan} seconds");
            });
}
```

### 2. Circuit Breaker

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddAMCodeOCR();
    
    // Add circuit breaker
    services.AddHttpClient("OCR")
        .AddPolicyHandler(GetCircuitBreakerPolicy());
}

private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30),
            onBreak: (exception, duration) =>
            {
                Console.WriteLine($"Circuit breaker opened for {duration}");
            },
            onReset: () =>
            {
                Console.WriteLine("Circuit breaker reset");
            });
}
```

## 📝 Best Practices

### 1. Resource Management

```csharp
public async Task<OCRResult> ProcessImageAsync(string imagePath)
{
    using var imageStream = File.OpenRead(imagePath);
    return await _ocrService.ExtractTextAsync(imageStream);
}
```

### 2. Async/Await Patterns

```csharp
public async Task<List<OCRResult>> ProcessImagesAsync(IEnumerable<string> imagePaths)
{
    var tasks = imagePaths.Select(async path =>
    {
        using var stream = File.OpenRead(path);
        return await _ocrService.ExtractTextAsync(stream);
    });

    var results = await Task.WhenAll(tasks);
    return results.Where(r => r.IsSuccess).Select(r => r.Value).ToList();
}
```

### 3. Configuration Validation

```csharp
public class OCRConfigurationValidator : IValidateOptions<OCRConfiguration>
{
    public ValidateOptionsResult Validate(string name, OCRConfiguration options)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(options.DefaultProvider))
        {
            errors.Add("DefaultProvider is required");
        }

        if (options.ConfidenceThreshold < 0 || options.ConfidenceThreshold > 1)
        {
            errors.Add("ConfidenceThreshold must be between 0 and 1");
        }

        if (options.MaxRetries < 0)
        {
            errors.Add("MaxRetries must be non-negative");
        }

        return errors.Count > 0 
            ? ValidateOptionsResult.Fail(errors) 
            : ValidateOptionsResult.Success;
    }
}
```

## 🎯 Conclusion

This integration guide provides comprehensive instructions for integrating AMCode.OCR into your .NET applications. Follow these patterns and best practices to ensure reliable, performant, and maintainable OCR functionality in your applications.

For more information, see the [README.md](README.md) and [Examples](Examples/) directory.
