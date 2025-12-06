# {Subfolder Name}

**Location:** `{Library Name}/{Subfolder Name}/`  
**Last Updated:** {DATE}  
**Purpose:** {Brief description of subfolder purpose}

---

## Overview

{Detailed description of what this subfolder contains, its role in the library, and how it fits into the overall architecture.}

## Responsibilities

- {Responsibility 1}
- {Responsibility 2}
- {Responsibility 3}

## Class Catalog

### Interfaces

#### {Interface Name}

**File:** `{Interface Name}.cs`

**Purpose:** {Description of interface purpose}

**Key Members:**
```csharp
public interface {Interface Name}
{
    {ReturnType} {Method}({Parameters});
    {PropertyType} {Property} { get; set; }
}
```

**Usage:**
```csharp
// Example usage
I{Interface Name} service = new {Implementation}();
```

**Implementations:**
- [{Implementation 1}](#{implementation-1}) - {Description}

---

#### {Interface Name 2}

{Repeat structure for each interface}

---

### Classes

#### {Class Name}

**File:** `{Class Name}.cs`

**Purpose:** {Description of class purpose}

**Key Responsibilities:**
- {Responsibility 1}
- {Responsibility 2}

**Key Members:**
```csharp
public class {Class Name} : {BaseClass/Interface}
{
    public {ReturnType} {Method}({Parameters}) { }
    public {PropertyType} {Property} { get; set; }
}
```

**Usage:**
```csharp
// Example usage
var instance = new {Class Name}();
var result = instance.{Method}();
```

**Dependencies:**
- {Dependency 1}
- {Dependency 2}

**Related Components:**
- [{Related Class}](#{related-class}) - {Relationship}

---

#### {Class Name 2}

{Repeat structure for each class}

---

### Enums

#### {Enum Name}

**File:** `{Enum Name}.cs`

**Purpose:** {Description of enum purpose}

**Values:**
- `{Value1}` - {Description}
- `{Value2}` - {Description}
- `{Value3}` - {Description}

**Usage:**
```csharp
var value = {Enum Name}.{Value1};
```

---

### Models

#### {Model Name}

**File:** `{Model Name}.cs`

**Purpose:** {Description of model purpose}

**Properties:**
- `{Property1}` ({Type}) - {Description}
- `{Property2}` ({Type}) - {Description}

**Usage:**
```csharp
var model = new {Model Name}
{
    {Property1} = {Value},
    {Property2} = {Value}
};
```

---

## Architecture Patterns

{Description of design patterns used in this subfolder}

- **{Pattern 1}**: {Description and where it's used}
- **{Pattern 2}**: {Description and where it's used}

## Usage Patterns

### Pattern 1: {Pattern Name}

```csharp
// Example code showing the pattern
```

### Pattern 2: {Pattern Name}

```csharp
// Example code showing the pattern
```

## Dependencies

### Internal Dependencies

- `{Library Name}.{Other Subfolder}` - {Purpose}
- `{Library Name}.{Another Subfolder}` - {Purpose}

### External Dependencies

- `{Package Name}` ({Version}) - {Purpose}

## Related Components

### Within Same Library

- [{Related Subfolder}](../{Related Subfolder}/README.md) - {Relationship}

### In Other Libraries

- [{Related Library}](../../{Path}/README.md) - {Relationship}

## Testing

### Test Coverage

- Unit tests: `{Test Project}/{Subfolder}/Tests/`
- Integration tests: `{Test Project}/{Subfolder}/IntegrationTests/`

### Example Test

```csharp
[Test]
public void {TestName}()
{
    // Test implementation
}
```

## Notes

{Any additional notes, warnings, or important information}

---

**See Also:**
- [Library README](../README.md) - Library overview
- [Root README](../../README.md) - Project overview

**Last Updated:** {DATE}  
**Maintained By:** Development Team

