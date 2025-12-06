# Extensions

**Location:** `AMCode.Common/Components/Extensions/`  
**Last Updated:** 2025-01-27  
**Purpose:** Extension methods for common .NET types including strings, collections, objects, types, and more

---

## Overview

The Extensions component provides a comprehensive set of extension methods that enhance common .NET types with additional functionality. These extensions simplify common operations, improve code readability, and provide consistent patterns across the AMCode ecosystem. The extensions are organized by the type they extend, making them easy to discover and use.

## Responsibilities

- **String Operations**: Enhanced string manipulation, comparison, and validation methods
- **Collection Operations**: Extended functionality for IEnumerable, IList, and arrays
- **Dictionary Operations**: Enhanced dictionary access and value retrieval
- **Object Operations**: Deep copying, type checking, and serialization utilities
- **Type Operations**: Type checking, primitive detection, and reflection utilities
- **Stream Operations**: Enhanced stream reading and manipulation
- **Filter Operations**: Extensions for filter-related types (Filter, FilterItem, FilterName)
- **ExpandoObject Operations**: Utilities for working with dynamic ExpandoObjects
- **MethodInfo Operations**: Reflection utilities for method information

## Class Catalog

### Classes

#### StringExtensions

**File:** `StringExtensions.cs`

**Purpose:** Provides extension methods for string manipulation, validation, and comparison operations.

**Key Responsibilities:**
- Check for null, empty, or whitespace strings
- Case-insensitive string comparison
- Split strings with custom delimiters and options
- Quote handling and CSV-related operations

**Key Members:**
```csharp
public static class StringExtensions
{
    public static bool IsNullEmptyOrWhiteSpace(this string s);
    public static bool EqualsIgnoreCase(this string s, string s2);
    public static string[] SplitIgnoreComma(this string str, string delimiter);
    public static string[] SplitIgnoreComma(this string str, string delimiter, bool emptyStringAsNull, bool addQuotes);
}
```

**Usage:**
```csharp
string text = "Hello, World";
if (!text.IsNullEmptyOrWhiteSpace())
{
    var parts = text.SplitIgnoreComma(",");
    bool isEqual = text.EqualsIgnoreCase("HELLO, WORLD");
}
```

**Dependencies:**
- `AMCode.Common.IO` - For path utilities

**Related Components:**
- [FilterExtensions](#filterextensions) - For filter-related string operations

---

#### EnumerableExtensions

**File:** `EnumerableExtensions.cs`

**Purpose:** Provides ForEach extension methods for IEnumerable, IList, and arrays with index support.

**Key Responsibilities:**
- Iterate over collections with action delegates
- Provide index information during iteration
- Support IEnumerable, IList, and array types

**Key Members:**
```csharp
public static class EnumerableExtensions
{
    public static void ForEach<T>(this IEnumerable<T> list, Action<T> action);
    public static void ForEach<T>(this IEnumerable<T> list, Action<T, int> action);
    public static void ForEach<T>(this T[] list, Action<T> action);
    public static void ForEach<T>(this T[] list, Action<T, int> action);
    public static void ForEach<T>(this IList<T> list, Action<T> action);
    public static void ForEach<T>(this IList<T> list, Action<T, int> action);
}
```

**Usage:**
```csharp
var items = new List<string> { "a", "b", "c" };
items.ForEach(item => Console.WriteLine(item));
items.ForEach((item, index) => Console.WriteLine($"{index}: {item}"));
```

**Dependencies:**
- `System.Linq` - For LINQ operations

**Related Components:**
- None (standalone utility)

---

#### DictionaryExtensions

**File:** `DictionaryExtensions.cs`

**Purpose:** Provides safe dictionary value retrieval with default value handling.

**Key Responsibilities:**
- Get dictionary values with null safety
- Return default values when keys are not found
- Type-safe value retrieval

**Key Members:**
```csharp
public static class DictionaryExtensions
{
    public static T GetValue<K, T>(this IDictionary<K, T> dictionary, K key);
}
```

**Usage:**
```csharp
var dict = new Dictionary<string, int> { { "key1", 100 } };
int value = dict.GetValue("key1"); // Returns 100
int missing = dict.GetValue("missing"); // Returns 0 (default)
```

**Dependencies:**
- None (standalone utility)

**Related Components:**
- None (standalone utility)

---

#### ObjectExtensions

**File:** `ObjectExtensions.cs`

**Purpose:** Provides object manipulation utilities including deep copying, type checking, and serialization.

**Key Responsibilities:**
- Deep copy objects using JSON serialization
- Check if objects are assignable to specific types
- Convert objects to JSON strings
- Provide type-safe conversions

**Key Members:**
```csharp
public static class ObjectExtensions
{
    public static T DeepCopy<T>(this T obj);
    public static TInterface DeepCopy<TInterface, TClass>(this TInterface obj) 
        where TInterface : class 
        where TClass : class, TInterface;
    public static bool Is<T>(this object source, out T destination);
    public static string ToJson(this object obj);
}
```

**Usage:**
```csharp
var original = new MyClass { Name = "Test" };
var copy = original.DeepCopy();

if (obj.Is<MyClass>(out var myClass))
{
    // Use myClass
}

string json = obj.ToJson();
```

**Dependencies:**
- `Newtonsoft.Json` - For JSON serialization
- `AMCode.Common.Extensions.Strings` - For string utilities

**Related Components:**
- [TypeExtensions](#typeextensions) - For type checking utilities

---

#### TypeExtensions

**File:** `TypeExtensions.cs`

**Purpose:** Provides type checking and reflection utilities for Type objects.

**Key Responsibilities:**
- Check if types are primitive or simple types
- Detect DateTime types
- Determine if types are nullable
- Check for generic type definitions

**Key Members:**
```csharp
public static class TypeExtensions
{
    public static bool IsDate(this Type type);
    public static bool IsSimple(this Type type);
    public static bool IsPrimitive(this Type type);
}
```

**Usage:**
```csharp
Type stringType = typeof(string);
bool isSimple = stringType.IsSimple(); // Returns true

Type dateType = typeof(DateTime);
bool isDate = dateType.IsDate(); // Returns true
```

**Dependencies:**
- `System.Reflection` - For reflection operations

**Related Components:**
- [ObjectExtensions](#objectextensions) - For object type checking

---

#### StreamExtensions

**File:** `StreamExtensions.cs`

**Purpose:** Provides enhanced stream reading and manipulation operations.

**Key Responsibilities:**
- Read streams to strings
- Read streams to byte arrays
- Provide async stream operations
- Handle stream encoding

**Key Members:**
```csharp
public static class StreamExtensions
{
    public static string ReadToString(this Stream stream);
    public static byte[] ReadToByteArray(this Stream stream);
    public static async Task<string> ReadToStringAsync(this Stream stream);
}
```

**Usage:**
```csharp
using (var stream = File.OpenRead("file.txt"))
{
    string content = stream.ReadToString();
    byte[] data = stream.ReadToByteArray();
}
```

**Dependencies:**
- `System.IO` - For stream operations

**Related Components:**
- [IO Component](../IO/README.md) - For file I/O operations

---

#### FilterExtensions

**File:** `FilterExtensions.cs`

**Purpose:** Provides extension methods for Filter objects.

**Key Responsibilities:**
- Manipulate filter objects
- Validate filter structures
- Convert filters to different formats

**Key Members:**
```csharp
public static class FilterExtensions
{
    // Filter manipulation methods
}
```

**Usage:**
```csharp
var filter = new Filter();
// Use filter extensions
```

**Dependencies:**
- `AMCode.Common.Components.Filter` - For filter types

**Related Components:**
- [Filter Component](../Filter/README.md) - For filter structures
- [FilterItemExtensions](#filteritemextensions) - For filter item operations
- [FilterNameExtensions](#filternameextensions) - For filter name operations

---

#### FilterItemExtensions

**File:** `FilterItemExtensions.cs`

**Purpose:** Provides extension methods for FilterItem objects.

**Key Responsibilities:**
- Manipulate filter items
- Validate filter item values
- Convert filter items

**Key Members:**
```csharp
public static class FilterItemExtensions
{
    // FilterItem manipulation methods
}
```

**Usage:**
```csharp
var filterItem = new FilterItem();
// Use filter item extensions
```

**Dependencies:**
- `AMCode.Common.Components.Filter` - For filter types

**Related Components:**
- [Filter Component](../Filter/README.md) - For filter structures
- [FilterExtensions](#filterextensions) - For filter operations

---

#### FilterNameExtensions

**File:** `FilterNameExtensions.cs`

**Purpose:** Provides extension methods for FilterName objects.

**Key Responsibilities:**
- Manipulate filter names
- Validate filter name formats
- Convert filter names

**Key Members:**
```csharp
public static class FilterNameExtensions
{
    // FilterName manipulation methods
}
```

**Usage:**
```csharp
var filterName = new FilterName();
// Use filter name extensions
```

**Dependencies:**
- `AMCode.Common.Components.Filter` - For filter types

**Related Components:**
- [Filter Component](../Filter/README.md) - For filter structures
- [FilterExtensions](#filterextensions) - For filter operations

---

#### ExpandoObjectExtensions

**File:** `ExpandoObjectExtensions.cs`

**Purpose:** Provides extension methods for working with ExpandoObject and dynamic types.

**Key Responsibilities:**
- Manipulate ExpandoObject properties
- Convert ExpandoObjects to dictionaries
- Access dynamic properties safely

**Key Members:**
```csharp
public static class ExpandoObjectExtensions
{
    // ExpandoObject manipulation methods
}
```

**Usage:**
```csharp
dynamic expando = new ExpandoObject();
// Use ExpandoObject extensions
```

**Dependencies:**
- `System.Dynamic` - For ExpandoObject support

**Related Components:**
- [IO Component](../IO/README.md) - CSV operations use ExpandoObjects

---

#### MethodInfoExtensions

**File:** `MethodInfoExtensions.cs`

**Purpose:** Provides extension methods for MethodInfo reflection operations.

**Key Responsibilities:**
- Extract method information
- Check method signatures
- Validate method parameters

**Key Members:**
```csharp
public static class MethodInfoExtensions
{
    // MethodInfo manipulation methods
}
```

**Usage:**
```csharp
MethodInfo method = typeof(MyClass).GetMethod("MyMethod");
// Use MethodInfo extensions
```

**Dependencies:**
- `System.Reflection` - For reflection operations

**Related Components:**
- [Util Component](../Util/README.md) - For reflection utilities

---

## Architecture Patterns

- **Extension Method Pattern**: All functionality provided through extension methods for seamless integration
- **Fluent Interface**: Some extensions support method chaining for improved readability
- **Null Safety**: Extensions handle null values gracefully where appropriate
- **Type Safety**: Generic extensions provide type-safe operations
- **Performance**: Extensions are optimized for common use cases

## Usage Patterns

### Pattern 1: String Validation

```csharp
string input = GetUserInput();
if (input.IsNullEmptyOrWhiteSpace())
{
    // Handle empty input
}
```

### Pattern 2: Case-Insensitive Comparison

```csharp
string userInput = "Hello";
if (userInput.EqualsIgnoreCase("HELLO"))
{
    // Match found
}
```

### Pattern 3: Collection Iteration

```csharp
var items = new List<string> { "a", "b", "c" };
items.ForEach(item => ProcessItem(item));
items.ForEach((item, index) => ProcessItemWithIndex(item, index));
```

### Pattern 4: Dictionary Safe Access

```csharp
var config = new Dictionary<string, string>();
string value = config.GetValue("key"); // Returns null if not found, no exception
```

### Pattern 5: Deep Copying Objects

```csharp
var original = new MyClass { Name = "Test", Value = 100 };
var copy = original.DeepCopy();
// copy is a deep copy, modifications won't affect original
```

### Pattern 6: Type Checking

```csharp
if (obj.Is<MyClass>(out var myClass))
{
    // Use myClass safely
}

Type myType = typeof(MyClass);
bool isSimple = myType.IsSimple();
bool isDate = myType.IsDate();
```

### Pattern 7: Stream Reading

```csharp
using (var stream = File.OpenRead("file.txt"))
{
    string content = stream.ReadToString();
    byte[] data = stream.ReadToByteArray();
}
```

### Pattern 8: Object Serialization

```csharp
var obj = new MyClass { Name = "Test" };
string json = obj.ToJson();
```

---

## Dependencies

### Internal Dependencies

- `AMCode.Common.Components.Filter` - For filter-related extensions
- `AMCode.Common.IO` - For path utilities used in string extensions

### External Dependencies

- `Newtonsoft.Json` - For JSON serialization in ObjectExtensions
- `System.Linq` - For LINQ operations in EnumerableExtensions
- `System.Reflection` - For reflection operations in TypeExtensions and MethodInfoExtensions
- `System.Dynamic` - For ExpandoObject support

---

## Related Components

### Within Same Library

- [Filter](../Filter/README.md) - Filter extensions work with filter types
- [IO](../IO/README.md) - Stream extensions complement IO operations
- [Common](../Common/README.md) - Type extensions work with common types

### In Other Libraries

- None (Extensions are foundational utilities)

---

## Testing

### Test Coverage

- Unit tests: `AMCode.Common.UnitTests/Components/Extensions/Tests/`
- Integration tests: Not applicable (extension methods)

### Example Test

```csharp
[Test]
public void StringExtensions_IsNullEmptyOrWhiteSpace_ReturnsTrueForEmpty()
{
    string empty = "";
    Assert.IsTrue(empty.IsNullEmptyOrWhiteSpace());
}

[Test]
public void EnumerableExtensions_ForEach_ExecutesAction()
{
    var items = new List<int> { 1, 2, 3 };
    int sum = 0;
    items.ForEach(item => sum += item);
    Assert.AreEqual(6, sum);
}
```

---

## Notes

- **Namespace Organization**: Extensions are organized by the type they extend (Strings, Enumerables, Dictionary, etc.)
- **Null Safety**: Most extensions handle null values gracefully, but some may throw exceptions for null inputs
- **Performance**: Extensions are designed for performance, but deep copying uses JSON serialization which can be slow for large objects
- **Type Safety**: Generic extensions provide compile-time type safety
- **Method Chaining**: Some extensions support method chaining for fluent APIs
- **LINQ Compatibility**: Extensions work alongside LINQ methods without conflicts

---

**See Also:**
- [Library README](../../README.md) - AMCode.Common library overview
- [Root README](../../../../README.md) - AMCode library overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

