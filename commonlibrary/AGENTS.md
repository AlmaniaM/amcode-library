# AMCode.Common — Agent Guide

## What This Is

Foundational utility library (.NET 8) providing error handling patterns, I/O operations, and reflection helpers shared across all AMCode libraries.

## When to Use

- Need `Result<T>` / `Result` for type-safe error handling (Success/Failure)
- Need archive compression/decompression (`IZipArchive`)
- Need shared extension methods across libraries

## When NOT to Use

- AI operations → `AMCode.AI`
- Database access → `AMCode.Data`
- File storage → `AMCode.Storage`

## Key Types

| Type | Purpose |
|------|---------|
| `Result<T>` / `Result` | Functional error handling with `Map`, `Bind`, `OnSuccess`, `OnFailure` |
| `IZipArchive` | Archive compression/decompression |
| Extension methods | Result chaining (`Combine`, `Tap`, `ToNullable`) |

## Verification

```bash
cd repos/amcode-library
dotnet build commonlibrary/AMCode.Common/AMCode.Common.csproj
```
