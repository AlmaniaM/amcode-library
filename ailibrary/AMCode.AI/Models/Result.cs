namespace AMCode.AI.Models;

/// <summary>
/// Represents the result of an operation that can either succeed or fail
/// </summary>
/// <typeparam name="T">The type of the successful result value</typeparam>
public class Result<T>
{
    /// <summary>
    /// Whether the operation was successful
    /// </summary>
    public bool IsSuccess { get; }
    
    /// <summary>
    /// The successful result value (only available when IsSuccess is true)
    /// </summary>
    public T? Value { get; }
    
    /// <summary>
    /// The error message (only available when IsSuccess is false)
    /// </summary>
    public string? Error { get; }
    
    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }
    
    /// <summary>
    /// Create a successful result
    /// </summary>
    /// <param name="value">The successful result value</param>
    /// <returns>A successful result</returns>
    public static Result<T> Success(T value) => new(true, value, null);
    
    /// <summary>
    /// Create a failed result
    /// </summary>
    /// <param name="error">The error message</param>
    /// <returns>A failed result</returns>
    public static Result<T> Failure(string error) => new(false, default, error);
    
    /// <summary>
    /// Implicit conversion from value to successful result
    /// </summary>
    /// <param name="value">The value</param>
    /// <returns>A successful result</returns>
    public static implicit operator Result<T>(T value) => Success(value);
}
