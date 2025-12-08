# AMCode.Web

**Version:** 1.0  
**Target Framework:** .NET 9.0  
**Purpose:** Reusable ASP.NET Core utilities, middleware, and behaviors for web applications

---

## Overview

AMCode.Web is a library containing reusable ASP.NET Core components extracted from production applications. It provides middleware, MediatR behaviors, serialization utilities, error handling models, and rate limiting components that can be used across multiple .NET web applications.

## Features

### Middleware
- **ExceptionHandlingMiddleware**: Global exception handling with standardized error responses
- **SecurityHeadersMiddleware**: Adds security headers (CSP, X-Frame-Options, etc.) to HTTP responses

### MediatR Behaviors
- **ValidationBehavior**: Automatic request validation using FluentValidation
- **LoggingBehavior**: Request/response logging with timing information

### Serialization
- **FlexibleCamelCaseNamingPolicy**: JSON naming policy that serializes to camelCase while accepting both camelCase and PascalCase during deserialization

### Error Handling
- **ErrorResponse**: Standardized error response format
- **ErrorDetail**: Error detail information with code, message, and details

### Rate Limiting
- **IRateLimitingService**: Service interface for checking and enforcing rate limits
- **RateLimitingService**: Implementation of IRateLimitingService using .NET's built-in rate limiting
- **RateLimitingMiddleware**: ASP.NET Core middleware for enforcing rate limits on API requests
- **RateLimitPolicy**: Configuration for rate limit policies
- **RateLimitResult**: Result of a rate limit check
- **RateLimitingSettings**: Configuration settings for rate limiting (default policies, endpoint overrides, excluded paths)
- **EndpointRateLimitOverride**: Endpoint-specific rate limit override configuration
- **ClientIdentifierExtractor**: Extracts client identifiers (user ID, API key, IP address) for rate limiting

## Installation

Add the project reference to your `.csproj`:

```xml
<ItemGroup>
  <ProjectReference Include="path/to/AMCode.Web/AMCode.Web.csproj" />
</ItemGroup>
```

## Dependencies

- **AMCode.Common**: Common utilities and components
- **Microsoft.AspNetCore.Http.Abstractions**: ASP.NET Core HTTP abstractions
- **Microsoft.Extensions.Logging.Abstractions**: Logging abstractions
- **MediatR.Contracts**: MediatR contracts for behaviors
- **FluentValidation**: Validation framework
- **System.Text.Json**: JSON serialization

## Usage

### Registering Services

```csharp
using AMCode.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add AMCode.Web behaviors
builder.Services.AddAMCodeWeb();

// Configure JSON serialization (when using MVC controllers)
builder.Services.AddControllers()
    .AddAMCodeWebJsonOptions();
```

### Using Middleware

```csharp
using AMCode.Web.Extensions;
using AMCode.Web.Middleware;

var app = builder.Build();

// Add AMCode.Web middleware (adds security headers and exception handling)
app.UseAMCodeWeb();

// Add rate limiting middleware (optional)
app.UseMiddleware<RateLimitingMiddleware>();

// Your other middleware...
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
```

### Error Handling

The `ExceptionHandlingMiddleware` automatically handles exceptions and returns standardized error responses:

```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Validation failed",
    "details": [
      {
        "propertyName": "Email",
        "errorMessage": "Email is required",
        "attemptedValue": null
      }
    ]
  }
}
```

### Rate Limiting

#### Registering Rate Limiting Service

```csharp
using AMCode.Web.RateLimiting;
using AMCode.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure rate limiting settings
builder.Services.Configure<RateLimitingSettings>(builder.Configuration.GetSection("RateLimiting"));

// Register rate limiting service
builder.Services.AddSingleton<IRateLimitingService, RateLimitingService>();
```

#### Using IRateLimitingService

```csharp
using AMCode.Web.RateLimiting;

public class MyService
{
    private readonly IRateLimitingService _rateLimitingService;

    public MyService(IRateLimitingService rateLimitingService)
    {
        _rateLimitingService = rateLimitingService;
    }

    public async Task<bool> CheckRateLimitAsync(string clientKey)
    {
        var policy = new RateLimitPolicy
        {
            PermitLimit = 100,
            Window = TimeSpan.FromMinutes(15),
            Algorithm = RateLimitAlgorithm.SlidingWindow
        };

        var result = await _rateLimitingService.CheckRateLimitAsync(clientKey, policy);
        return result.IsAllowed;
    }
}
```

#### Using ClientIdentifierExtractor

```csharp
using AMCode.Web.RateLimiting;

var clientId = ClientIdentifierExtractor.ExtractClientIdentifier(context);
// Returns: "user:123", "apikey:abc123", or "ip:192.168.1.1"
```

#### Rate Limiting Configuration

Add to `appsettings.json`:

```json
{
  "RateLimiting": {
    "DefaultPolicy": {
      "PermitLimit": 100,
      "Window": "00:15:00",
      "Algorithm": "SlidingWindow",
      "QueueLimit": 10
    },
    "Policies": {
      "Strict": {
        "PermitLimit": 10,
        "Window": "00:01:00",
        "Algorithm": "FixedWindow"
      }
    },
    "EndpointOverrides": {
      "/api/auth/login": {
        "PolicyName": "Strict"
      }
    },
    "ExcludedPaths": [
      "/health",
      "/metrics"
    ]
  }
}
```

### JSON Serialization

The `FlexibleCamelCaseNamingPolicy` serializes responses in camelCase while accepting both camelCase and PascalCase in requests:

```csharp
// Response: { "firstName": "John", "lastName": "Doe" }
// Request: Accepts both { "firstName": "John" } and { "FirstName": "John" }
```

## Component Details

### ExceptionHandlingMiddleware

Handles exceptions globally and returns standardized error responses:
- **ValidationException**: Returns 400 Bad Request with validation errors
- **UnauthorizedAccessException**: Returns 401 Unauthorized
- **Other exceptions**: Returns 500 Internal Server Error

### SecurityHeadersMiddleware

Adds security headers to HTTP responses:
- X-Content-Type-Options: nosniff
- X-Frame-Options: DENY
- X-XSS-Protection: 1; mode=block
- Referrer-Policy: strict-origin-when-cross-origin
- Permissions-Policy: geolocation=(), microphone=(), camera=()
- Content-Security-Policy: Configured based on environment (development vs production)

### ValidationBehavior

Automatically validates MediatR requests using FluentValidation:
- Runs before the request handler
- Throws `ValidationException` if validation fails
- Works with any request type that has validators registered

### LoggingBehavior

Logs MediatR requests and responses:
- Logs request start with unique GUID
- Logs request completion with elapsed time
- Logs errors with full exception details

## Component Details

### RateLimitingService

Implements `IRateLimitingService` using .NET's built-in `System.Threading.RateLimiting`:
- Supports multiple algorithms: SlidingWindow, TokenBucket, FixedWindow, ConcurrencyLimiter
- Thread-safe with concurrent dictionary for rate limiter instances
- Fail-open behavior: allows requests if rate limit check fails
- Located in `RateLimiting/RateLimitingService.cs`

### RateLimitingMiddleware

ASP.NET Core middleware for enforcing rate limits:
- Automatically extracts client identifiers (user ID, API key, IP address)
- Supports endpoint-specific overrides and excluded paths
- Returns 429 Too Many Requests with proper headers
- Located in `Middleware/RateLimitingMiddleware.cs`

## Testing

Unit tests are located in `AMCode.Web.UnitTests`. Tests will be migrated in Phase 5 of the library migration process.

## Migration Notes

This library was created by extracting reusable components from RecipeOCR.Backend. All components have been verified to have zero RecipeOCR-specific dependencies and are fully reusable across .NET applications.

## License

Part of the AMCode Library ecosystem.
