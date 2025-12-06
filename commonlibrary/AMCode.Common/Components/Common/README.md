# Common

**Location:** `AMCode.Common/Components/Common/`  
**Last Updated:** 2025-01-27  
**Purpose:** Common models and interfaces including Result pattern for type-safe error handling and ICloneable interface for object cloning

---

## Overview

The Common component provides foundational models and interfaces used throughout the AMCode ecosystem. It includes the Result pattern implementation for type-safe error handling without exceptions, and the ICloneable interface for object cloning operations. These components serve as building blocks for other AMCode libraries and applications.

## Responsibilities

- **Result Pattern**: Provide type-safe result handling for operations that can succeed or fail
- **Object Cloning**: Define interface for cloneable objects
- **Error Handling**: Enable functional error handling without exceptions
- **Type Safety**: Provide generic type-safe result operations

## Class Catalog

### Interfaces

#### ICloneable<TResult>

**File:** `Models/ICloneable.cs`

**Purpose:** Generic interface for cloning objects, providing type-safe cloning operations.

**Key Members:**
```csharp
public interface ICloneable<out TResult>
{
    TResult Clone();
}
```

**Usage:**
```csharp
public class MyClass : ICloneable<MyClass>
{
    public string Name { get; set; }
    
    public MyClass Clone()
    {
        return new MyClass { Name = this.Name };
    }
}

var original = new MyClass { Name = "Test" };
var clone = original.Clone();
```

**Implementations:**
- Classes that need cloning functionality implement this interface

---

### Classes

#### Result<T>

**File:** `Models/Result.cs`

**Purpose:** Represents the result of an operation that can either succeed with a value or fail with an error message. Provides type-safe error handling without exceptions.

**Key Responsibilities:**
- Represent operation success or failure
- Hold value on success or error message on failure
- Provide factory methods for creating results
- Enable functional error handling patterns

**Key Members:**
```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T Value { get; }
    public string Error { get; }
    
    public static Result<T> Success(T value);
    public static Result<T> Failure(string error);
}
```

**Usage:**
```csharp
// Success case
var result = Result<string>.Success("Operation completed");
if (result.IsSuccess)
{
    Console.WriteLine(result.Value); // "Operation completed"
}

// Failure case
var errorResult = Result<int>.Failure("Invalid input");
if (errorResult.IsFailure)
{
    Console.WriteLine(errorResult.Error); // "Invalid input"
}

// In a method
public Result<User> GetUser(int userId)
{
    if (userId <= 0)
    {
        return Result<User>.Failure("Invalid user ID");
    }
    
    var user = userRepository.Find(userId);
    if (user == null)
    {
        return Result<User>.Failure("User not found");
    }
    
    return Result<User>.Success(user);
}
```

**Dependencies:**
- None (standalone class)

**Related Components:**
- [Result](#result) - Non-generic version for operations without return values

---

#### Result

**File:** `Models/Result.cs`

**Purpose:** Represents the result of an operation that can either succeed or fail, without a return value. Used for operations that don't return data but may fail.

**Key Responsibilities:**
- Represent operation success or failure
- Hold error message on failure
- Provide factory methods for creating results
- Enable functional error handling for void operations

**Key Members:**
```csharp
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }
    
    public static Result Success();
    public static Result Failure(string error);
}
```

**Usage:**
```csharp
// Success case
var result = Result.Success();
if (result.IsSuccess)
{
    // Operation succeeded
}

// Failure case
var errorResult = Result.Failure("Operation failed");
if (errorResult.IsFailure)
{
    Console.WriteLine(errorResult.Error); // "Operation failed"
}

// In a method
public Result SaveUser(User user)
{
    if (user == null)
    {
        return Result.Failure("User cannot be null");
    }
    
    try
    {
        userRepository.Save(user);
        return Result.Success();
    }
    catch (Exception ex)
    {
        return Result.Failure($"Failed to save user: {ex.Message}");
    }
}
```

**Dependencies:**
- None (standalone class)

**Related Components:**
- [Result<T>](#resultt) - Generic version for operations with return values

---

## Architecture Patterns

- **Result Pattern**: Functional error handling without exceptions
- **Factory Pattern**: Static factory methods for creating results
- **Immutable Pattern**: Result objects are immutable once created
- **Type Safety**: Generic Result<T> provides compile-time type safety
- **Interface Segregation**: ICloneable<TResult> provides focused cloning interface

## Usage Patterns

### Pattern 1: Basic Result Usage

```csharp
public Result<int> Divide(int a, int b)
{
    if (b == 0)
    {
        return Result<int>.Failure("Cannot divide by zero");
    }
    
    return Result<int>.Success(a / b);
}

var result = Divide(10, 2);
if (result.IsSuccess)
{
    Console.WriteLine($"Result: {result.Value}");
}
else
{
    Console.WriteLine($"Error: {result.Error}");
}
```

### Pattern 2: Chaining Results

```csharp
public Result<User> GetUserWithValidation(int userId)
{
    var validationResult = ValidateUserId(userId);
    if (validationResult.IsFailure)
    {
        return Result<User>.Failure(validationResult.Error);
    }
    
    var user = userRepository.Find(userId);
    if (user == null)
    {
        return Result<User>.Failure("User not found");
    }
    
    return Result<User>.Success(user);
}
```

### Pattern 3: Void Operations

```csharp
public Result DeleteUser(int userId)
{
    var user = userRepository.Find(userId);
    if (user == null)
    {
        return Result.Failure("User not found");
    }
    
    userRepository.Delete(user);
    return Result.Success();
}
```

### Pattern 4: Result with Exception Handling

```csharp
public Result<string> ReadFile(string path)
{
    try
    {
        var content = File.ReadAllText(path);
        return Result<string>.Success(content);
    }
    catch (FileNotFoundException)
    {
        return Result<string>.Failure("File not found");
    }
    catch (Exception ex)
    {
        return Result<string>.Failure($"Error reading file: {ex.Message}");
    }
}
```

### Pattern 5: Implementing ICloneable

```csharp
public class Person : ICloneable<Person>
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public Person Clone()
    {
        return new Person
        {
            Name = this.Name,
            Age = this.Age
        };
    }
}

var original = new Person { Name = "John", Age = 30 };
var clone = original.Clone();
clone.Name = "Jane"; // Original is not affected
```

### Pattern 6: Deep Cloning with Result

```csharp
public Result<Person> ClonePerson(Person person)
{
    if (person == null)
    {
        return Result<Person>.Failure("Person cannot be null");
    }
    
    try
    {
        var clone = person.Clone();
        return Result<Person>.Success(clone);
    }
    catch (Exception ex)
    {
        return Result<Person>.Failure($"Failed to clone person: {ex.Message}");
    }
}
```

### Pattern 7: Result in Async Operations

```csharp
public async Task<Result<string>> FetchDataAsync(string url)
{
    try
    {
        var response = await httpClient.GetStringAsync(url);
        return Result<string>.Success(response);
    }
    catch (HttpRequestException ex)
    {
        return Result<string>.Failure($"HTTP error: {ex.Message}");
    }
    catch (Exception ex)
    {
        return Result<string>.Failure($"Unexpected error: {ex.Message}");
    }
}
```

### Pattern 8: Result Validation

```csharp
public Result<User> CreateUser(string email, string password)
{
    var emailResult = ValidateEmail(email);
    if (emailResult.IsFailure)
    {
        return Result<User>.Failure(emailResult.Error);
    }
    
    var passwordResult = ValidatePassword(password);
    if (passwordResult.IsFailure)
    {
        return Result<User>.Failure(passwordResult.Error);
    }
    
    var user = new User { Email = email, Password = password };
    userRepository.Save(user);
    return Result<User>.Success(user);
}
```

---

## Dependencies

### Internal Dependencies

- None (Common is a foundational component)

### External Dependencies

- None (uses only .NET standard library)

---

## Related Components

### Within Same Library

- [Extensions](../Extensions/README.md) - ObjectExtensions provides DeepCopy method that can work with ICloneable
- [IO](../IO/README.md) - May use Result pattern for file operations

### In Other Libraries

- None (Common is a foundational component used by other libraries)

---

## Testing

### Test Coverage

- Unit tests: `AMCode.Common.UnitTests/Components/Common/Tests/`
- Integration tests: Not applicable (data structures)

### Example Test

```csharp
[Test]
public void Result_Success_ReturnsSuccessResult()
{
    var result = Result<string>.Success("test");
    Assert.IsTrue(result.IsSuccess);
    Assert.IsFalse(result.IsFailure);
    Assert.AreEqual("test", result.Value);
    Assert.IsNull(result.Error);
}

[Test]
public void Result_Failure_ReturnsFailureResult()
{
    var result = Result<int>.Failure("error message");
    Assert.IsFalse(result.IsSuccess);
    Assert.IsTrue(result.IsFailure);
    Assert.AreEqual(default(int), result.Value);
    Assert.AreEqual("error message", result.Error);
}

[Test]
public void ICloneable_Clone_CreatesNewInstance()
{
    var original = new Person { Name = "John", Age = 30 };
    var clone = original.Clone();
    
    Assert.AreNotSame(original, clone);
    Assert.AreEqual(original.Name, clone.Name);
    Assert.AreEqual(original.Age, clone.Age);
}
```

---

## Notes

- **Result Pattern**: Provides functional error handling without exceptions
- **Type Safety**: Result<T> provides compile-time type safety for return values
- **Immutable**: Result objects are immutable once created
- **No Exceptions**: Result pattern avoids exception overhead for expected failures
- **ICloneable**: Generic interface provides type-safe cloning
- **Shallow vs Deep**: ICloneable implementation determines shallow or deep cloning
- **Null Handling**: Result pattern helps avoid null reference exceptions
- **Error Messages**: Error messages should be descriptive and user-friendly
- **Success/Failure**: Use IsSuccess or IsFailure properties, not direct boolean checks on Error

---

**See Also:**
- [Library README](../../README.md) - AMCode.Common library overview
- [Extensions README](../Extensions/README.md) - ObjectExtensions for deep copying
- [Root README](../../../../README.md) - AMCode library overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

