# AMCode.Data — Agent Guide

## What This Is

MongoDB data access library (.NET 9) providing generic repository patterns with document transformation, session management, and support for both strongly-typed and dynamic queries.

## When to Use

- MongoDB repository pattern implementations
- Document schema mapping and field transformation
- Session and connection lifecycle management

## When NOT to Use

- Application-specific repositories → implement in your app's Infrastructure layer
- SQL databases → `AMCode.Sql.Builder` or direct EF Core
- File storage → `AMCode.Storage`

## Key Interfaces

| Interface | Purpose |
|-----------|---------|
| `IMongoDataProvider` | Main interface: execute, query, and generic operations |
| `IMongoConnectionFactory` | Connection pooling and session lifecycle |
| `IDocumentTransformDefinition` | Schema mapping and field transformation |

## Critical Rules

- **CRITICAL**: MongoDB `MapField()` CANNOT set `private readonly` fields in .NET 9 — remove `readonly`
- **CRITICAL**: `UnmapMember()` only works for members of the mapped class, not base class members
- Uses MongoDB.Driver 3.5.0

## Verification

```bash
cd repos/amcode-library
dotnet build datalibrary/AMCode.Data/AMCode.Data.csproj
```
