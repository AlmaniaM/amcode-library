# AMCode.Web — Agent Guide

## What This Is

ASP.NET Core web utilities library providing validation behaviors, error handling middleware, rate limiting, and MediatR pipeline behaviors. Used by all AMCode web APIs.

## When to Use

- Adding MediatR pipeline behaviors (validation, logging)
- Need standardized error handling middleware
- Rate limiting configuration
- Shared web service patterns

## Key Types

- `ValidationBehavior<TRequest, TResponse>` — FluentValidation MediatR behavior
- `LoggingBehavior<TRequest, TResponse>` — Request/response logging
- `RateLimitingService` / `IRateLimitingService` — Per-user rate limits
- Error types and exception handling middleware

## Verification

```bash
cd repos/amcode-library
dotnet build weblibrary/AMCode.Web/AMCode.Web.csproj
```
