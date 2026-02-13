# AMCode.Common.Testing — Agent Guide

## What This Is

Testing infrastructure library (.NET 9) for integration tests. Provides Docker container management, CSV data loading, and database container setup for MongoDB, SQL Server, and PostgreSQL.

## When to Use

- Writing integration tests that need database containers
- Need Docker container lifecycle management in tests
- Loading CSV test fixtures
- Need standardized test infrastructure across projects

## When NOT to Use

- Unit tests without external dependencies → use xUnit/NUnit directly
- Mocking → use Moq/NSubstitute directly
- Application code → this is test-only

## Key Interfaces

| Interface | Purpose |
|-----------|---------|
| `IDockerContainer` | Docker lifecycle (GetContainerAsync, RunAsync, StopAsync) |
| `IDbContainer` | Database-specific container wrapper |
| `CSVDataReader` | CSV test data loading |
| `IDockerImage`, `IDockerVolume` | Container resource abstractions |

## Dependencies

- Docker.DotNet 3.125.5 — Docker API client
- AMCode.Common — Shared utilities

## Verification

```bash
cd repos/amcode-library
dotnet build commontestinglibrary/AMCode.Common.Testing/AMCode.Common.Testing.csproj
```
