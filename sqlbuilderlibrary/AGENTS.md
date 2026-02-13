# AMCode.Sql.Builder — Agent Guide

## What This Is

SQL query builder library for constructing parameterized SQL queries programmatically. Provides fluent API for SELECT, INSERT, UPDATE, DELETE operations.

## When to Use

- Building dynamic SQL queries with parameters
- Need fluent query construction for SQL databases

## When NOT to Use

- MongoDB operations → `AMCode.Data`
- ORM operations → use Entity Framework Core directly

## Verification

```bash
cd repos/amcode-library
dotnet build sqlbuilderlibrary/AMCode.Sql.Builder/AMCode.Sql.Builder.csproj
```
