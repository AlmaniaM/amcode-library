# AMCode.Columns

**Version:** 1.0  
**Target Framework:** .NET 8.0  
**Last Updated:** 2025-01-27  
**Purpose:** Column management and data transformation library with builder pattern support

---

## Overview

AMCode.Columns provides a flexible and extensible framework for defining, managing, and transforming column data. It supports the builder pattern for fluent column definition creation and includes comprehensive data transformation capabilities. The library is designed to work seamlessly with data export and processing operations.

## Architecture

The library follows a clean architecture pattern with clear separation between core interfaces, builder implementations, and data transformation logic. It uses the builder pattern to provide a fluent API for creating column definitions.

### Key Components

- **Core Interfaces**: Fundamental column definition contracts (IColumnDefinition, IColumnName, IValueFormatter)
- **Builder Pattern**: Fluent API for constructing column definitions (ColumnFactoryBuilder)
- **Data Transformation**: Support for transforming column data during processing (IDataTransformColumnDefinition)

## Features

- Fluent builder pattern for column definition creation
- Column name management with display and field name support
- Value formatting for different data types
- Data transformation functions for column processing
- Type-safe column definitions with nullable support
- Extensible architecture for custom formatters and transformers

## Dependencies

### Internal Dependencies

- **AMCode.Common** - Common utilities and components

### External Dependencies

- None (only .NET 8.0 framework)

## Project Structure

```
AMCode.Columns/
├── Core/                        # Core interfaces and classes
│   ├── IColumnDefinition.cs    # Basic column definition interface
│   ├── IColumnName.cs          # Column name interface
│   ├── ColumnName.cs           # Column name implementation
│   └── IValueFormatter.cs      # Value formatter interface
├── Builder/                     # Builder pattern implementation
│   └── ColumnFactoryBuilder.cs # Fluent builder for column definitions
├── DataTransform/               # Data transformation components
│   ├── IDataTransformColumnDefinition.cs # Extended column definition for transformations
│   ├── DataTransformColumnDefinition.cs   # Implementation
│   └── DataTransformFunction.cs           # Transformation function delegate
└── AMCode.Columns.csproj       # Project file
```

## Key Interfaces

### IColumnDefinition

**Location:** `Core/IColumnDefinition.cs`

**Purpose:** Represents the basic contract for a column definition, including name, type, and nullability.

**Key Members:**
- `Name` - Column name
- `Type` - Column data type
- `IsNullable` - Whether the column allows null values

**See Also:** Core components documentation above

### IColumnName

**Location:** `Core/IColumnName.cs`

**Purpose:** Represents column name information with separate display and field names.

**Key Members:**
- `DisplayName` - Human-readable column name
- `FieldName` - Technical field name for data access

**See Also:** Core components documentation above

### IValueFormatter

**Location:** `Core/IValueFormatter.cs`

**Purpose:** Provides formatting capabilities for column values to string and object representations.

**Key Methods:**
- `Format(object value)` - Formats value to string
- `FormatToObject(object value)` - Formats value to object

**See Also:** Core components documentation above

### IDataTransformColumnDefinition

**Location:** `DataTransform/IDataTransformColumnDefinition.cs`

**Purpose:** Extends IColumnDefinition with data transformation capabilities, including transformation functions and value formatters.

**Key Members:**
- `ColumnName` - Column name information
- `PropertyName` - Property name for data binding
- `DataTransformFunction` - Delegate for data transformation
- `ValueFormatter` - Formatter for value output
- `FieldName` - Computed field name
- `GetFormatter()` - Gets the formatter instance

**See Also:** DataTransform documentation above

## Key Classes

### ColumnName

**Location:** `Core/ColumnName.cs`

**Purpose:** Concrete implementation of IColumnName providing display and field name management.

**Key Responsibilities:**
- Store display name for UI presentation
- Store field name for data access
- Implement IColumnName contract

**See Also:** Core components documentation above

### ColumnFactoryBuilder

**Location:** `Builder/ColumnFactoryBuilder.cs`

**Purpose:** Fluent builder for creating column definitions with method chaining support.

**Key Responsibilities:**
- Build column definitions using fluent API
- Configure column names, properties, transformations, and formatters
- Create IDataTransformColumnDefinition instances

**Key Methods:**
- `ColumnName(IColumnName)` - Sets column name
- `PropertyName(string)` - Sets property name
- `DataTransformation(Delegate)` - Sets transformation function
- `ValueFormatter(IValueFormatter)` - Sets value formatter
- `Build()` - Creates the column definition

**See Also:** Builder documentation above

### DataTransformColumnDefinition

**Location:** `DataTransform/DataTransformColumnDefinition.cs`

**Purpose:** Implementation of IDataTransformColumnDefinition providing complete column definition with transformation support.

**Key Responsibilities:**
- Implement column definition with transformation capabilities
- Manage column name, property name, and transformation function
- Provide value formatting through IValueFormatter
- Compute field name from column name

**See Also:** DataTransform documentation above

## Usage Examples

### Basic Usage - Creating a Column Definition

```csharp
using AMCode.Columns;
using AMCode.Columns.Core;
using AMCode.Columns.Builder;

// Create a column name
var columnName = new ColumnName
{
    DisplayName = "Product Name",
    FieldName = "ProductName"
};

// Build a column definition
var column = new ColumnFactoryBuilder()
    .ColumnName(columnName)
    .PropertyName("ProductName")
    .Build();
```

### Advanced Usage - With Data Transformation

```csharp
using AMCode.Columns;
using AMCode.Columns.Core;
using AMCode.Columns.Builder;
using AMCode.Columns.DataTransform;

// Create column with transformation function
var column = new ColumnFactoryBuilder()
    .ColumnName(new ColumnName 
    { 
        DisplayName = "Price", 
        FieldName = "Price" 
    })
    .PropertyName("Price")
    .DataTransformation(new DataTransformFunction<decimal>(price => price * 1.2m)) // Add 20% markup
    .ValueFormatter(new CustomPriceFormatter())
    .Build();

// Use the column definition
var transformedValue = column.DataTransformFunction?.DynamicInvoke(100.0m);
var formattedValue = column.GetFormatter()?.Format(transformedValue);
```

### Using with Value Formatters

```csharp
using AMCode.Columns.Core;

// Implement custom formatter
public class CurrencyFormatter : IValueFormatter
{
    public string Format(object value)
    {
        if (value is decimal d)
            return d.ToString("C2");
        return value?.ToString() ?? string.Empty;
    }

    public object FormatToObject(object value)
    {
        return Format(value);
    }
}

// Use in column definition
var column = new ColumnFactoryBuilder()
    .ColumnName(new ColumnName { DisplayName = "Amount", FieldName = "Amount" })
    .ValueFormatter(new CurrencyFormatter())
    .Build();
```

## Configuration

This library does not require external configuration. It is designed to be used programmatically through its fluent API.

### Dependency Injection Setup

```csharp
// Register column builders if needed
services.AddTransient<ColumnFactoryBuilder>();

// Or use directly without DI
var builder = new ColumnFactoryBuilder();
```

## Testing

### Test Projects

This library does not have a dedicated test project in the solution. Testing should be done through integration with libraries that consume AMCode.Columns (e.g., AMCode.Exports).

### Running Tests

Tests for this library are typically included in consuming library test projects:

```bash
dotnet test AMCode.Exports.UnitTests
```

## Component Details

### Core Components

The `Core/` folder contains fundamental interfaces and classes:

- **IColumnDefinition**: Basic column definition contract with name, type, and nullability
- **IColumnName**: Column name interface with display and field name support
- **ColumnName**: Concrete implementation of IColumnName
- **IValueFormatter**: Interface for formatting column values to string and object representations

### Builder Component

The `Builder/` folder contains the fluent builder implementation:

- **ColumnFactoryBuilder**: Fluent builder for creating column definitions with method chaining support

### DataTransform Components

The `DataTransform/` folder contains data transformation capabilities:

- **IDataTransformColumnDefinition**: Extends IColumnDefinition with transformation support
- **DataTransformColumnDefinition**: Implementation with transformation function and value formatter
- **DataTransformFunction<T>**: Generic delegate for data transformation functions

## Related Libraries

- [AMCode.Common](../commonlibrary/AMCode.Common/README.md) - Common utilities used by this library
- [AMCode.Exports](../../exportslibrary/AMCode.Exports/README.md) - Export functionality that uses column definitions

## Migration Notes

No migration notes at this time. This is a stable library with a well-defined API.

## Known Issues

None currently documented.

## Future Considerations

- Additional built-in value formatters
- More transformation function types
- Column validation support
- Column metadata extensions

---

**See Also:**

- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

