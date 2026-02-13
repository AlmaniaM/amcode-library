# AMCode.Vertica.Client — Agent Guide

## What This Is

Vertica database client library. Provides connectivity and query execution for HP Vertica analytical databases.

## When to Use

- Connecting to Vertica databases
- Running analytical queries against Vertica

## When NOT to Use

- MongoDB → `AMCode.Data`
- General SQL → `AMCode.Sql.Builder`
- Most RecipeOCR features don't use Vertica

## Verification

```bash
cd repos/amcode-library
dotnet build verticalibrary/AMCode.Vertica.Client/AMCode.Vertica.Client.csproj
```
