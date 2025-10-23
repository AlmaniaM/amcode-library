using System;

namespace AMCode.Documents.Common.Models
{
    /// <summary>
    /// Result pattern for error handling without exceptions
    /// </summary>
    /// <typeparam name="T">The type of the result value</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets the result value if the operation was successful
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets the error message if the operation failed
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Gets the exception if the operation failed with an exception
        /// </summary>
        public Exception Exception { get; }

        private Result(bool isSuccess, T value, string error, Exception exception = null)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
            Exception = exception;
        }

        /// <summary>
        /// Create a successful result
        /// </summary>
        public static Result<T> Success(T value) => new Result<T>(true, value, null);

        /// <summary>
        /// Create a failed result with error message
        /// </summary>
        public static Result<T> Failure(string error) => new Result<T>(false, default, error);

        /// <summary>
        /// Create a failed result with exception
        /// </summary>
        public static Result<T> Failure(Exception exception) => new Result<T>(false, default, exception.Message, exception);

        /// <summary>
        /// Create a failed result with error message and exception
        /// </summary>
        public static Result<T> Failure(string error, Exception exception) => new Result<T>(false, default, error, exception);

        /// <summary>
        /// Implicit conversion from value to successful result
        /// </summary>
        public static implicit operator Result<T>(T value) => Success(value);

        /// <summary>
        /// Implicit conversion from error message to failed result
        /// </summary>
        public static implicit operator Result<T>(string error) => Failure(error);
    }

    /// <summary>
    /// Non-generic result for operations that don't return a value
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets the error message if the operation failed
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Gets the exception if the operation failed with an exception
        /// </summary>
        public Exception Exception { get; }

        private Result(bool isSuccess, string error, Exception exception = null)
        {
            IsSuccess = isSuccess;
            Error = error;
            Exception = exception;
        }

        /// <summary>
        /// Create a successful result
        /// </summary>
        public static Result Success() => new Result(true, null);

        /// <summary>
        /// Create a failed result with error message
        /// </summary>
        public static Result Failure(string error) => new Result(false, error);

        /// <summary>
        /// Create a failed result with exception
        /// </summary>
        public static Result Failure(Exception exception) => new Result(false, exception.Message, exception);

        /// <summary>
        /// Create a failed result with error message and exception
        /// </summary>
        public static Result Failure(string error, Exception exception) => new Result(false, error, exception);

        /// <summary>
        /// Implicit conversion from error message to failed result
        /// </summary>
        public static implicit operator Result(string error) => Failure(error);
    }
}
