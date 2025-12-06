# Filter

**Location:** `AMCode.Common/Components/Filter/`  
**Last Updated:** 2025-01-27  
**Purpose:** Filter structures for data filtering operations with support for filter names, items, and validation

---

## Overview

The Filter component provides a flexible filtering system for data operations. It consists of three main parts: Filter (the container), FilterName (naming information), and FilterItem (individual filter values). The component supports both display names for user interfaces and field names for data access, making it suitable for building dynamic filtering UIs that map to database queries or data operations.

## Responsibilities

- **Filter Definition**: Define filters with names, items, and requirements
- **Filter Naming**: Provide both display names (for UI) and field names (for data access)
- **Filter Items**: Manage individual filter values with selection and state
- **Filter Validation**: Support required filters and disabled states
- **JSON Serialization**: Enable filter persistence and API communication

## Class Catalog

### Interfaces

#### IFilter

**File:** `Models/IFilter.cs`

**Purpose:** Interface representing a filter object that contains multiple filter items and naming information.

**Key Members:**
```csharp
public interface IFilter
{
    IFilterName FilterName { get; set; }
    IFilterName FilterIdName { get; set; }
    IList<IFilterItem> FilterItems { get; set; }
    bool Required { get; set; }
}
```

**Usage:**
```csharp
IFilter filter = new Filter
{
    FilterName = new FilterName { DisplayName = "Category", FieldName = "category" },
    FilterItems = new List<IFilterItem> { /* ... */ },
    Required = false
};
```

**Implementations:**
- [Filter](#filter) - Main filter implementation

---

#### IFilterItem

**File:** `Models/IFilterItem.cs`

**Purpose:** Interface representing a single filter option/value within a filter.

**Key Members:**
```csharp
public interface IFilterItem
{
    string FilterVal { get; set; }
    string FilterId { get; set; }
    bool Selected { get; set; }
    bool Disabled { get; set; }
}
```

**Usage:**
```csharp
IFilterItem item = new FilterItem
{
    FilterVal = "Electronics",
    FilterId = "1",
    Selected = true,
    Disabled = false
};
```

**Implementations:**
- [FilterItem](#filteritem) - Main filter item implementation

---

#### IFilterName

**File:** `Models/IFilterName.cs`

**Purpose:** Interface representing naming information for a filter, supporting both display and field names.

**Key Members:**
```csharp
public interface IFilterName
{
    string DisplayName { get; set; }
    string FieldName { get; set; }
}
```

**Usage:**
```csharp
IFilterName filterName = new FilterName
{
    DisplayName = "Product Category",
    FieldName = "category"
};
```

**Implementations:**
- [FilterName](#filtername) - Main filter name implementation

---

### Classes

#### Filter

**File:** `Filter.cs`

**Purpose:** Represents a filter with a name, multiple filter items, and optional requirements. Supports JSON serialization for API communication and persistence.

**Key Responsibilities:**
- Contain filter items and naming information
- Support required/optional filter validation
- Provide filter ID name for related filter operations
- Enable JSON serialization/deserialization

**Key Members:**
```csharp
public class Filter : IFilter
{
    public Filter();
    public Filter(FilterName filterName, FilterName filterIdname, List<FilterItem> filterItems);
    public IFilterName FilterName { get; set; }
    public IFilterName FilterIdName { get; set; }
    public IList<IFilterItem> FilterItems { get; set; }
    public bool Required { get; set; }
}
```

**Usage:**
```csharp
var filter = new Filter
{
    FilterName = new FilterName 
    { 
        DisplayName = "Category", 
        FieldName = "category" 
    },
    FilterIdName = new FilterName 
    { 
        DisplayName = "Category ID", 
        FieldName = "categoryId" 
    },
    FilterItems = new List<IFilterItem>
    {
        new FilterItem { FilterVal = "Electronics", FilterId = "1", Selected = false },
        new FilterItem { FilterVal = "Clothing", FilterId = "2", Selected = true },
        new FilterItem { FilterVal = "Books", FilterId = "3", Selected = false, Disabled = true }
    },
    Required = false
};
```

**Dependencies:**
- `Newtonsoft.Json` - For JSON serialization
- `System.Linq` - For collection operations

**Related Components:**
- [FilterItem](#filteritem) - Individual filter items
- [FilterName](#filtername) - Filter naming information
- [IFilter](#ifilter) - Interface definition

---

#### FilterItem

**File:** `FilterItem.cs`

**Purpose:** Represents a single filter option/value with selection state and optional ID mapping.

**Key Responsibilities:**
- Store filter value and optional ID
- Track selection state
- Support disabled state for unavailable options

**Key Members:**
```csharp
public class FilterItem : IFilterItem
{
    public string FilterVal { get; set; }
    public string FilterId { get; set; }
    public bool Selected { get; set; }
    public bool Disabled { get; set; }
}
```

**Usage:**
```csharp
var item = new FilterItem
{
    FilterVal = "Electronics",
    FilterId = "1",
    Selected = true,
    Disabled = false
};

// Check if item is selected and enabled
if (item.Selected && !item.Disabled)
{
    // Apply filter
}
```

**Dependencies:**
- None (standalone class)

**Related Components:**
- [Filter](#filter) - Container for filter items
- [IFilterItem](#ifilteritem) - Interface definition

---

#### FilterName

**File:** `FilterName.cs`

**Purpose:** Represents naming information for a filter, supporting both user-facing display names and programmatic field names.

**Key Responsibilities:**
- Provide display name for user interfaces
- Provide field name for data access operations
- Support mapping between UI and data layers

**Key Members:**
```csharp
public class FilterName : IFilterName
{
    public string DisplayName { get; set; }
    public string FieldName { get; set; }
}
```

**Usage:**
```csharp
var filterName = new FilterName
{
    DisplayName = "Product Category",  // Shown to users
    FieldName = "category"             // Used in queries
};

// Use DisplayName in UI
label.Text = filterName.DisplayName;

// Use FieldName in data access
var query = $"SELECT * FROM Products WHERE {filterName.FieldName} = @value";
```

**Dependencies:**
- None (standalone class)

**Related Components:**
- [Filter](#filter) - Uses FilterName for naming
- [IFilterName](#ifiltername) - Interface definition

---

## Architecture Patterns

- **Interface-Based Design**: All components expose interfaces (IFilter, IFilterItem, IFilterName) for testability and flexibility
- **Separation of Concerns**: Display names (UI) separated from field names (data access)
- **JSON Serialization**: Filters support JSON serialization for API communication and persistence
- **Composition Pattern**: Filter contains FilterName and FilterItem objects
- **State Management**: FilterItem tracks selection and disabled states

## Usage Patterns

### Pattern 1: Creating a Simple Filter

```csharp
var filter = new Filter
{
    FilterName = new FilterName 
    { 
        DisplayName = "Status", 
        FieldName = "status" 
    },
    FilterItems = new List<IFilterItem>
    {
        new FilterItem { FilterVal = "Active", Selected = true },
        new FilterItem { FilterVal = "Inactive", Selected = false }
    },
    Required = false
};
```

### Pattern 2: Filter with IDs

```csharp
var filter = new Filter
{
    FilterName = new FilterName 
    { 
        DisplayName = "Category", 
        FieldName = "categoryId" 
    },
    FilterIdName = new FilterName 
    { 
        DisplayName = "Category ID", 
        FieldName = "categoryId" 
    },
    FilterItems = new List<IFilterItem>
    {
        new FilterItem { FilterVal = "Electronics", FilterId = "1", Selected = false },
        new FilterItem { FilterVal = "Clothing", FilterId = "2", Selected = true }
    }
};
```

### Pattern 3: Required Filter

```csharp
var filter = new Filter
{
    FilterName = new FilterName 
    { 
        DisplayName = "Region", 
        FieldName = "region" 
    },
    FilterItems = new List<IFilterItem>
    {
        new FilterItem { FilterVal = "North", Selected = true },
        new FilterItem { FilterVal = "South", Selected = false }
    },
    Required = true  // Filter must have a selection
};
```

### Pattern 4: Disabled Filter Items

```csharp
var filter = new Filter
{
    FilterName = new FilterName 
    { 
        DisplayName = "Product Type", 
        FieldName = "type" 
    },
    FilterItems = new List<IFilterItem>
    {
        new FilterItem { FilterVal = "Available", Selected = false, Disabled = false },
        new FilterItem { FilterVal = "Unavailable", Selected = false, Disabled = true }  // Disabled option
    }
};
```

### Pattern 5: JSON Serialization

```csharp
// Serialize filter to JSON
var filter = new Filter { /* ... */ };
string json = JsonConvert.SerializeObject(filter);

// Deserialize from JSON
var deserializedFilter = JsonConvert.DeserializeObject<Filter>(json);
```

### Pattern 6: Getting Selected Filter Items

```csharp
var selectedItems = filter.FilterItems
    .Where(item => item.Selected && !item.Disabled)
    .ToList();

foreach (var item in selectedItems)
{
    // Apply filter using item.FilterVal or item.FilterId
    Console.WriteLine($"Selected: {item.FilterVal}");
}
```

### Pattern 7: Building Query from Filter

```csharp
var selectedItems = filter.FilterItems
    .Where(item => item.Selected && !item.Disabled)
    .ToList();

if (selectedItems.Any())
{
    var values = selectedItems.Select(item => item.FilterId ?? item.FilterVal).ToList();
    var query = $"SELECT * FROM Products WHERE {filter.FilterName.FieldName} IN ({string.Join(",", values)})";
}
```

### Pattern 8: Filter Validation

```csharp
bool IsFilterValid(IFilter filter)
{
    if (filter.Required)
    {
        return filter.FilterItems?.Any(item => item.Selected && !item.Disabled) ?? false;
    }
    return true;  // Optional filters are always valid
}
```

---

## Dependencies

### Internal Dependencies

- None (Filter is a standalone component)

### External Dependencies

- `Newtonsoft.Json` - For JSON serialization in Filter class
- `System.Linq` - For collection operations

---

## Related Components

### Within Same Library

- [Extensions](../Extensions/README.md) - FilterExtensions, FilterItemExtensions, FilterNameExtensions provide extension methods for filter types
- [Common](../Common/README.md) - Common models and interfaces

### In Other Libraries

- None (Filter is a foundational component)

---

## Testing

### Test Coverage

- Unit tests: `AMCode.Common.UnitTests/Components/Filter/Tests/`
- Integration tests: Not applicable (data structures)

### Example Test

```csharp
[Test]
public void Filter_Required_ValidatesSelection()
{
    var filter = new Filter
    {
        FilterName = new FilterName { DisplayName = "Test", FieldName = "test" },
        FilterItems = new List<IFilterItem>
        {
            new FilterItem { FilterVal = "Option1", Selected = true }
        },
        Required = true
    };
    
    Assert.IsTrue(filter.Required);
    Assert.IsTrue(filter.FilterItems.Any(item => item.Selected));
}

[Test]
public void FilterItem_Disabled_ExcludesFromSelection()
{
    var item = new FilterItem
    {
        FilterVal = "Test",
        Selected = true,
        Disabled = true
    };
    
    // Disabled items should not be considered selected
    Assert.IsTrue(item.Disabled);
}
```

---

## Notes

- **Display vs Field Names**: DisplayName is for UI presentation, FieldName is for data access operations
- **FilterId vs FilterVal**: FilterId is used when you need to map to database IDs, FilterVal is the display value
- **Required Filters**: When Required is true, at least one FilterItem must be Selected and not Disabled
- **JSON Serialization**: Filters support full JSON serialization for API communication
- **Null Safety**: FilterItems can be null, always check before iterating
- **Selection State**: Selected items should typically be checked along with Disabled state
- **FilterIdName**: Used when you need a separate filter for IDs (e.g., category filter and category ID filter)

---

**See Also:**
- [Library README](../../README.md) - AMCode.Common library overview
- [Extensions README](../Extensions/README.md) - Filter extension methods
- [Root README](../../../../README.md) - AMCode library overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

