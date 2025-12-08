using System;
using System.Collections.Generic;
using System.Linq;
using AMCode.Common.Models;

namespace AMCode.Common.Extensions
{
    /// <summary>
    /// Extension methods for the Result pattern
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Combines multiple results into a single result. All results must be successful for the combined result to be successful.
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="results">The results to combine</param>
        /// <returns>A combined result</returns>
        public static Result<IEnumerable<T>> Combine<T>(this IEnumerable<Result<T>> results)
        {
            var resultsList = results.ToList();
            var failures = resultsList.Where(r => r.IsFailure).ToList();

            if (failures.Any())
            {
                var allErrors = new List<string>();
                foreach (var failure in failures)
                {
                    if (failure.Errors.Any())
                        allErrors.AddRange(failure.Errors);
                    else
                        allErrors.Add(failure.Error);
                }
                return Result<IEnumerable<T>>.Failure(allErrors);
            }

            return Result<IEnumerable<T>>.Success(resultsList.Select(r => r.Value));
        }

        /// <summary>
        /// Combines multiple non-generic results into a single result. All results must be successful for the combined result to be successful.
        /// </summary>
        /// <param name="results">The results to combine</param>
        /// <returns>A combined result</returns>
        public static Result Combine(this IEnumerable<Result> results)
        {
            var resultsList = results.ToList();
            var failures = resultsList.Where(r => r.IsFailure).ToList();

            if (failures.Any())
            {
                var allErrors = new List<string>();
                foreach (var failure in failures)
                {
                    if (failure.Errors.Any())
                        allErrors.AddRange(failure.Errors);
                    else
                        allErrors.Add(failure.Error);
                }
                return Result.Failure(allErrors);
            }

            return Result.Success();
        }

        /// <summary>
        /// Executes an action if the result is successful, otherwise does nothing
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="result">The result</param>
        /// <param name="action">The action to execute</param>
        /// <returns>The same result for method chaining</returns>
        public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess)
                action(result.Value);
            return result;
        }

        /// <summary>
        /// Executes an action if the result is successful, otherwise does nothing
        /// </summary>
        /// <param name="result">The result</param>
        /// <param name="action">The action to execute</param>
        /// <returns>The same result for method chaining</returns>
        public static Result Tap(this Result result, Action action)
        {
            if (result.IsSuccess)
                action();
            return result;
        }

        /// <summary>
        /// Converts a result to an optional value. Returns Some if successful, None if failed.
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="result">The result</param>
        /// <returns>An optional value</returns>
        public static T ToNullable<T>(this Result<T> result) where T : class
        {
            return result.IsSuccess ? result.Value : null;
        }

        /// <summary>
        /// Converts a result to an optional value. Returns Some if successful, None if failed.
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="result">The result</param>
        /// <returns>An optional value</returns>
        public static T? ToNullableValueType<T>(this Result<T> result) where T : struct
        {
            return result.IsSuccess ? result.Value : (T?)null;
        }

        /// <summary>
        /// Converts a result to a tuple for pattern matching
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="result">The result</param>
        /// <returns>A tuple (IsSuccess, Value, Error)</returns>
        public static (bool IsSuccess, T Value, string Error) ToTuple<T>(this Result<T> result)
        {
            return (result.IsSuccess, result.IsSuccess ? result.Value : default(T), result.Error);
        }

        /// <summary>
        /// Converts a result to a tuple for pattern matching
        /// </summary>
        /// <param name="result">The result</param>
        /// <returns>A tuple (IsSuccess, Error)</returns>
        public static (bool IsSuccess, string Error) ToTuple(this Result result)
        {
            return (result.IsSuccess, result.Error);
        }
    }
}

