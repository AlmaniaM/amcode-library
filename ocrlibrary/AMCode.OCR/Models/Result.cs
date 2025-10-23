using System.Text.Json.Serialization;

namespace AMCode.OCR.Models;

/// <summary>
/// Represents the result of an operation that can succeed or fail
/// </summary>
/// <typeparam name="T">The type of the result value</typeparam>
public class Result<T>
{
    /// <summary>
    /// Whether the operation was successful
    /// </summary>
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// Whether the operation failed
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// The result value (only available when IsSuccess is true)
    /// </summary>
    public T? Value { get; private set; }

    /// <summary>
    /// The error message (only available when IsFailure is true)
    /// </summary>
    public string Error { get; private set; } = string.Empty;

    /// <summary>
    /// Additional error details
    /// </summary>
    public Dictionary<string, object> ErrorDetails { get; private set; } = new();

    /// <summary>
    /// The exception that caused the failure (if any)
    /// </summary>
    [JsonIgnore]
    public Exception? Exception { get; private set; }

    /// <summary>
    /// The timestamp when the result was created
    /// </summary>
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// The correlation ID for tracking
    /// </summary>
    public string? CorrelationId { get; set; }

    /// <summary>
    /// Additional metadata
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    private Result() { }

    /// <summary>
    /// Creates a successful result
    /// </summary>
    /// <param name="value">The result value</param>
    /// <returns>A successful result</returns>
    public static Result<T> Success(T value)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Value = value
        };
    }

    /// <summary>
    /// Creates a successful result with metadata
    /// </summary>
    /// <param name="value">The result value</param>
    /// <param name="metadata">Additional metadata</param>
    /// <returns>A successful result</returns>
    public static Result<T> Success(T value, Dictionary<string, object> metadata)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Value = value,
            Metadata = metadata
        };
    }

    /// <summary>
    /// Creates a failed result
    /// </summary>
    /// <param name="error">The error message</param>
    /// <returns>A failed result</returns>
    public static Result<T> Failure(string error)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Error = error
        };
    }

    /// <summary>
    /// Creates a failed result with error details
    /// </summary>
    /// <param name="error">The error message</param>
    /// <param name="errorDetails">Additional error details</param>
    /// <returns>A failed result</returns>
    public static Result<T> Failure(string error, Dictionary<string, object> errorDetails)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Error = error,
            ErrorDetails = errorDetails
        };
    }

    /// <summary>
    /// Creates a failed result from an exception
    /// </summary>
    /// <param name="exception">The exception that caused the failure</param>
    /// <returns>A failed result</returns>
    public static Result<T> Failure(Exception exception)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Error = exception.Message,
            Exception = exception
        };
    }

    /// <summary>
    /// Creates a failed result from an exception with additional context
    /// </summary>
    /// <param name="exception">The exception that caused the failure</param>
    /// <param name="context">Additional context about the failure</param>
    /// <returns>A failed result</returns>
    public static Result<T> Failure(Exception exception, string context)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Error = $"{context}: {exception.Message}",
            Exception = exception
        };
    }

    /// <summary>
    /// Creates a failed result with multiple error messages
    /// </summary>
    /// <param name="errors">The error messages</param>
    /// <returns>A failed result</returns>
    public static Result<T> Failure(IEnumerable<string> errors)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Error = string.Join("; ", errors)
        };
    }

    /// <summary>
    /// Implicitly converts a value to a successful result
    /// </summary>
    /// <param name="value">The value to convert</param>
    /// <returns>A successful result</returns>
    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }

    /// <summary>
    /// Implicitly converts a string to a failed result
    /// </summary>
    /// <param name="error">The error message</param>
    /// <returns>A failed result</returns>
    public static implicit operator Result<T>(string error)
    {
        return Failure(error);
    }

    /// <summary>
    /// Returns a string representation of the result
    /// </summary>
    /// <returns>A string representation</returns>
    public override string ToString()
    {
        return IsSuccess 
            ? $"Success: {Value}" 
            : $"Failure: {Error}";
    }
}

/// <summary>
/// Static helper methods for creating results
/// </summary>
public static class Result
{
    /// <summary>
    /// Creates a successful result
    /// </summary>
    /// <typeparam name="T">The type of the result value</typeparam>
    /// <param name="value">The result value</param>
    /// <returns>A successful result</returns>
    public static Result<T> Success<T>(T value)
    {
        return Result<T>.Success(value);
    }

    /// <summary>
    /// Creates a failed result
    /// </summary>
    /// <typeparam name="T">The type of the result value</typeparam>
    /// <param name="error">The error message</param>
    /// <returns>A failed result</returns>
    public static Result<T> Failure<T>(string error)
    {
        return Result<T>.Failure(error);
    }

    /// <summary>
    /// Creates a failed result from an exception
    /// </summary>
    /// <typeparam name="T">The type of the result value</typeparam>
    /// <param name="exception">The exception that caused the failure</param>
    /// <returns>A failed result</returns>
    public static Result<T> Failure<T>(Exception exception)
    {
        return Result<T>.Failure(exception);
    }
}