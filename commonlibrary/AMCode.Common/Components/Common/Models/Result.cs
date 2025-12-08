using System;
using System.Collections.Generic;
using System.Linq;

namespace AMCode.Common.Models
{
    /// <summary>
    /// Represents the result of an operation that can either succeed or fail.
    /// Provides a functional approach to error handling without exceptions.
    /// </summary>
    /// <typeparam name="T">The type of the value returned on success</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets a value indicating whether the operation failed
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Gets the value if the operation was successful
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets the error message if the operation failed
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Gets the list of error messages if the operation failed
        /// </summary>
        public List<string> Errors { get; }

        private Result(bool isSuccess, T value, string error, List<string> errors = null)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error ?? string.Empty;
            Errors = errors ?? new List<string>();
        }

        /// <summary>
        /// Creates a successful result with a value
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>A successful result</returns>
        public static Result<T> Success(T value)
        {
            return new Result<T>(true, value, string.Empty);
        }

        /// <summary>
        /// Creates a failed result with an error message
        /// </summary>
        /// <param name="error">The error message</param>
        /// <returns>A failed result</returns>
        public static Result<T> Failure(string error)
        {
            return new Result<T>(false, default(T)!, error, new List<string> { error });
        }

        /// <summary>
        /// Creates a failed result with multiple error messages
        /// </summary>
        /// <param name="errors">The list of error messages</param>
        /// <returns>A failed result</returns>
        public static Result<T> Failure(List<string> errors)
        {
            return new Result<T>(false, default(T)!, errors.FirstOrDefault() ?? "Unknown error", errors);
        }

        /// <summary>
        /// Creates a failed result with a single error message and additional errors
        /// </summary>
        /// <param name="error">The primary error message</param>
        /// <param name="additionalErrors">Additional error messages</param>
        /// <returns>A failed result</returns>
        public static Result<T> Failure(string error, params string[] additionalErrors)
        {
            var allErrors = new List<string> { error };
            allErrors.AddRange(additionalErrors);
            return new Result<T>(false, default(T)!, error, allErrors);
        }

        /// <summary>
        /// Implicitly converts a value to a successful result
        /// </summary>
        /// <param name="value">The value to convert</param>
        public static implicit operator Result<T>(T value) => Success(value);

        /// <summary>
        /// Implicitly converts an error string to a failed result
        /// </summary>
        /// <param name="error">The error message</param>
        public static implicit operator Result<T>(string error) => Failure(error);

        /// <summary>
        /// Maps the value to another type if the result is successful
        /// </summary>
        /// <typeparam name="TOut">The output type</typeparam>
        /// <param name="mapper">The mapping function</param>
        /// <returns>A new result with the mapped value, or a failure result if the current result failed</returns>
        public Result<TOut> Map<TOut>(Func<T, TOut> mapper)
        {
            return IsSuccess ? Result<TOut>.Success(mapper(Value)) : Result<TOut>.Failure(Error, Errors.ToArray());
        }

        /// <summary>
        /// Maps the value to another result if the current result is successful
        /// </summary>
        /// <typeparam name="TOut">The output type</typeparam>
        /// <param name="mapper">The mapping function that returns a result</param>
        /// <returns>The mapped result, or a failure result if the current result failed</returns>
        public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> mapper)
        {
            return IsSuccess ? mapper(Value) : Result<TOut>.Failure(Error, Errors.ToArray());
        }

        /// <summary>
        /// Executes an action if the result is successful
        /// </summary>
        /// <param name="action">The action to execute</param>
        /// <returns>The same result for method chaining</returns>
        public Result<T> OnSuccess(Action<T> action)
        {
            if (IsSuccess)
                action(Value);
            return this;
        }

        /// <summary>
        /// Executes an action if the result is a failure
        /// </summary>
        /// <param name="action">The action to execute with the error message</param>
        /// <returns>The same result for method chaining</returns>
        public Result<T> OnFailure(Action<string> action)
        {
            if (IsFailure)
                action(Error);
            return this;
        }

        /// <summary>
        /// Returns the value if successful, otherwise returns the default value
        /// </summary>
        /// <param name="defaultValue">The default value to return on failure</param>
        /// <returns>The value if successful, otherwise the default value</returns>
        public T GetValueOrDefault(T defaultValue = default(T)!)
        {
            return IsSuccess ? Value : defaultValue;
        }

        /// <summary>
        /// Returns the value if successful, otherwise throws an exception
        /// </summary>
        /// <returns>The value if successful</returns>
        /// <exception cref="InvalidOperationException">Thrown when the result is a failure</exception>
        public T GetValueOrThrow()
        {
            if (IsFailure)
                throw new InvalidOperationException($"Cannot get value from failed result: {Error}");
            return Value;
        }

        /// <summary>
        /// Returns a string representation of the result
        /// </summary>
        /// <returns>A string representation</returns>
        public override string ToString()
        {
            return IsSuccess ? $"Success: {Value}" : $"Failure: {Error}";
        }
    }

    /// <summary>
    /// Represents the result of an operation that can either succeed or fail.
    /// Non-generic version for operations that don't return a value.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets a value indicating whether the operation failed
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Gets the error message if the operation failed
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Gets the list of error messages if the operation failed
        /// </summary>
        public List<string> Errors { get; }

        private Result(bool isSuccess, string error, List<string> errors = null)
        {
            IsSuccess = isSuccess;
            Error = error ?? string.Empty;
            Errors = errors ?? new List<string>();
        }

        /// <summary>
        /// Creates a successful result
        /// </summary>
        /// <returns>A successful result</returns>
        public static Result Success()
        {
            return new Result(true, string.Empty);
        }

        /// <summary>
        /// Creates a failed result with an error message
        /// </summary>
        /// <param name="error">The error message</param>
        /// <returns>A failed result</returns>
        public static Result Failure(string error)
        {
            return new Result(false, error, new List<string> { error });
        }

        /// <summary>
        /// Creates a failed result with multiple error messages
        /// </summary>
        /// <param name="errors">The list of error messages</param>
        /// <returns>A failed result</returns>
        public static Result Failure(List<string> errors)
        {
            return new Result(false, errors.FirstOrDefault() ?? "Unknown error", errors);
        }

        /// <summary>
        /// Creates a failed result with a single error message and additional errors
        /// </summary>
        /// <param name="error">The primary error message</param>
        /// <param name="additionalErrors">Additional error messages</param>
        /// <returns>A failed result</returns>
        public static Result Failure(string error, params string[] additionalErrors)
        {
            var allErrors = new List<string> { error };
            allErrors.AddRange(additionalErrors);
            return new Result(false, error, allErrors);
        }

        /// <summary>
        /// Implicitly converts an error string to a failed result
        /// </summary>
        /// <param name="error">The error message</param>
        public static implicit operator Result(string error) => Failure(error);

        /// <summary>
        /// Maps to a generic result with a value
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="mapper">The mapping function</param>
        /// <returns>A new result with the mapped value, or a failure result if the current result failed</returns>
        public Result<T> Map<T>(Func<T> mapper)
        {
            return IsSuccess ? Result<T>.Success(mapper()) : Result<T>.Failure(Error, Errors.ToArray());
        }

        /// <summary>
        /// Executes an action if the result is successful
        /// </summary>
        /// <param name="action">The action to execute</param>
        /// <returns>The same result for method chaining</returns>
        public Result OnSuccess(Action action)
        {
            if (IsSuccess)
                action();
            return this;
        }

        /// <summary>
        /// Executes an action if the result is a failure
        /// </summary>
        /// <param name="action">The action to execute with the error message</param>
        /// <returns>The same result for method chaining</returns>
        public Result OnFailure(Action<string> action)
        {
            if (IsFailure)
                action(Error);
            return this;
        }

        /// <summary>
        /// Returns a string representation of the result
        /// </summary>
        /// <returns>A string representation</returns>
        public override string ToString()
        {
            return IsSuccess ? "Success" : $"Failure: {Error}";
        }
    }
}
