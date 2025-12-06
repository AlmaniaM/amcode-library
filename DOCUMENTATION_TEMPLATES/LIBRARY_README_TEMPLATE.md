# {Library Name}

**Version:** {VERSION}
**Target Framework:** {.NET VERSION}
**Last Updated:** {DATE}
**Purpose:** {Brief description of library purpose}

---

## Overview

{Detailed description of what this library does, its primary use cases, and key features.}

## Architecture

{Description of the library's architecture pattern, design principles, and key architectural decisions.}

### Key Components

- **{Component 1}**: {Description}
- **{Component 2}**: {Description}
- **{Component 3}**: {Description}

## Features

- {Feature 1}
- {Feature 2}
- {Feature 3}

## Dependencies

### Internal Dependencies

- {Dependency 1} - {Purpose}
- {Dependency 2} - {Purpose}

### External Dependencies

- {Package 1} ({Version}) - {Purpose}
- {Package 2} ({Version}) - {Purpose}

## Project Structure

```
{Library Name}/
├── {Folder 1}/              # {Description}
│   └── README.md           # [Link to subfolder README]
├── {Folder 2}/              # {Description}
│   └── README.md           # [Link to subfolder README]
├── {File 1}.cs              # {Description}
├── {File 2}.cs              # {Description}
└── {Library Name}.csproj    # Project file
```

## Key Interfaces

### {Interface Name}

**Location:** `{Path}/{Interface Name}.cs`

**Purpose:** {Description of interface purpose}

**Key Methods:**

- `{Method 1}()` - {Description}
- `{Method 2}()` - {Description}

**See Also:** [Subfolder README]({Path}/README.md)

## Key Classes

### {Class Name}

**Location:** `{Path}/{Class Name}.cs`

**Purpose:** {Description of class purpose}

**Key Responsibilities:**

- {Responsibility 1}
- {Responsibility 2}

**See Also:** [Subfolder README]({Path}/README.md)

## Usage Examples

### Basic Usage

```csharp
// Example code showing basic usage
using {Library Name};

var service = new {Service Class}();
var result = await service.{Method}();
```

### Advanced Usage

```csharp
// Example code showing advanced usage
using {Library Name};

var options = new {Options Class}
{
    {Property} = {Value}
};

var service = new {Service Class}(options);
var result = await service.{Method}();
```

## Configuration

### appsettings.json Example

```json
{
  "{Section}": {
    "{Key}": "{Value}"
  }
}
```

### Dependency Injection Setup

```csharp
services.Add{Library Name}(options =>
{
    options.{Property} = {Value};
});
```

## Testing

### Test Projects

- **{Test Project Name}**: {Description}
  - Unit tests
  - Integration tests
  - [Test Project README]({Path}/README.md)

### Running Tests

```bash
dotnet test {Library Name}.Tests
```

## Subfolder Documentation

For detailed documentation on specific components:

- [{Folder 1}]({Folder 1}/README.md) - {Description}
- [{Folder 2}]({Folder 2}/README.md) - {Description}
- [{Folder 3}]({Folder 3}/README.md) - {Description}

## Related Libraries

- [{Related Library 1}](../{Path}/README.md) - {Relationship}
- [{Related Library 2}](../{Path}/README.md) - {Relationship}

## Migration Notes

{Any migration notes, breaking changes, or upgrade instructions}

## Known Issues

{List of known issues or limitations}

## Future Considerations

{Planned features or improvements}

---

**See Also:**

- [Root README](../README.md) - Project overview
- [Documentation Plan](../DOCUMENTATION_PLAN.md) - Documentation strategy

**Last Updated:** {DATE}
**Maintained By:** Development Team
