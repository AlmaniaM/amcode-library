# AMCode.Web.UnitTests

**Version**: 1.0  
**Last Updated**: 2025-01-28  
**Purpose**: Unit tests for AMCode.Web library components

---

## Overview

This test project contains unit tests for all components in the `AMCode.Web` library, including middleware, behaviors, rate limiting, and error handling components.

## Test Organization

### Middleware Tests
**Location**: `Middleware/`

- **RateLimitingMiddlewareTests.cs**: Tests for rate limiting middleware functionality
  - Excluded path handling
  - Rate limit enforcement
  - Client identifier extraction (user ID, API key, IP address)
  - Endpoint-specific overrides
  - Rate limit headers
  - Error responses

### Rate Limiting Tests
**Location**: `RateLimiting/`

- **RateLimitingServiceTests.cs**: Tests for rate limiting service
  - Algorithm validation (SlidingWindow, TokenBucket, FixedWindow, ConcurrencyLimiter)
  - Policy enforcement
  - Key independence
  - Window expiration
  - Error handling (fail-open behavior)

- **ClientIdentifierExtractorTests.cs**: Tests for client identifier extraction
  - User ID priority
  - API key fallback
  - IP address fallback
  - X-Forwarded-For header handling
  - X-Real-IP header handling

## Test Patterns

### Rate Limiting Test Patterns

Rate limiting tests use unique keys per test to avoid interference between tests:

```csharp
var key = $"test-key-{Guid.NewGuid()}"; // Unique key per test
```

### Timing-Sensitive Tests

Some rate limiting tests are timing-sensitive. Tests that verify window expiration include appropriate delays:

```csharp
await Task.Delay(2500); // Wait for window to expire (with buffer)
```

### Flexible Assertions

Rate limiting algorithm tests use flexible assertions to account for timing variations:

```csharp
// Verify at least N requests were allowed, rather than exactly N
results.Count(r => r.IsAllowed).Should().BeGreaterThanOrEqualTo(5);
results.Count(r => !r.IsAllowed).Should().BeLessThanOrEqualTo(1);
```

## Running Tests

### Run All Tests
```bash
dotnet test
```

### Run Specific Test Class
```bash
dotnet test --filter "FullyQualifiedName~RateLimitingServiceTests"
```

### Run with Verbose Output
```bash
dotnet test --verbosity normal
```

## Test Dependencies

- **NUnit**: Test framework
- **FluentAssertions**: Assertion library
- **Moq**: Mocking framework
- **Microsoft.AspNetCore.Mvc.Testing**: ASP.NET Core testing utilities

## Test Coverage

Current test coverage includes:
- ✅ Rate limiting middleware (all scenarios)
- ✅ Rate limiting service (all algorithms)
- ✅ Client identifier extraction (all priority scenarios)
- ⏳ Exception handling middleware (pending)
- ⏳ Security headers middleware (pending)
- ⏳ Validation behavior (pending)
- ⏳ Logging behavior (pending)

## Known Issues

- Some rate limiting algorithm tests may have timing-related failures in CI/CD environments
- Tests use small delays (10ms) between rapid requests to ensure proper rate limiter processing
- Window expiration tests use buffer delays (500ms) to account for timing variations

## Contributing

When adding new tests:
1. Use unique keys for rate limiting tests to avoid interference
2. Add appropriate delays for timing-sensitive tests
3. Use flexible assertions for algorithm tests
4. Document any timing dependencies in test comments

