using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMCode.Exports.Book;

namespace AMCode.Exports.Common.Exceptions.Util
{
    /// <summary>
    /// A static class designed to help create exception messages.
    /// </summary>
    public static class ExportsExceptionUtil
    {
        /// <summary>
        /// Check which parameters are less than or equal to zero and return their names.
        /// </summary>
        /// <param name="parameters">The <see cref="IntParameterCheck"/> objects.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="string"/> parameter names.</returns>
        public static IEnumerable<string> GetParametersLessThanZero(params IntParameterCheck[] parameters)
            => parameters.Where(parameter => parameter.Value <= 0).Select(parameter => parameter.Name);

        /// <summary>
        /// Create a <see cref="EmptyCollectionException"/> message.
        /// </summary>
        /// <param name="header">The class name and method name header.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>A <see cref="string"/> message.</returns>
        public static string CreateEmptyCollectionExceptionMessage(string header, string parameterName)
            => $"{header} Error: An empty collection as been detected. Parameter \"{parameterName}\" cannot be empty.";

        /// <summary>
        /// Create an <see cref="IndexOutOfRangeException"/> message.
        /// </summary>
        /// <param name="header">The class name and method name header.</param>
        /// <param name="incorrectParameters">The parameter names that were out of bounds.</param>
        /// <returns>A <see cref="string"/> message.</returns>
        public static string CreateIndexOutOfRangeException(string header, string incorrectParameters)
        {
            return new StringBuilder()
                    .Append($"{header} Error: ")
                    .Append($"The following parameter(s) were less than or equal to zero: \"{incorrectParameters}\". ")
                    .Append("Please provide index values greater than or equal to one only.")
                    .ToString();
        }

        /// <summary>
        /// Create a <see cref="MaxColumnCountExceededException"/> message.
        /// </summary>
        /// <param name="header">The class name and method name header.</param>
        /// <param name="maxAllowedColumns">The maximum number of allowed columns in a worksheet.</param>
        /// <returns>A <see cref="string"/> message.</returns>
        public static string CreateMaxColumnCountExceededExceptionMessage(string header, int maxAllowedColumns)
            => $"{header} Error: Column count cannot exceed max allowed columns. Max allowed column count is {maxAllowedColumns}";

        /// <summary>
        /// Create a <see cref="NullReferenceException"/> message.
        /// </summary>
        /// <param name="header">The class name and method name header.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>A <see cref="string"/> message.</returns>
        public static string CreateNullReferenceExceptionMessage(string header, string parameterName)
            => $"{header} Error: The \"{parameterName}\" parameter cannot be null.";

        /// <summary>
        /// Create an <see cref="ArgumentException"/> message for a less than zero value constraint.
        /// </summary>
        /// <param name="header">The class name and method name header.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>A <see cref="string"/> message.</returns>
        public static string CreateLessThanZeroExceptionMessage(string header, string parameterName)
            => $"{header} Error: Parameter \"{parameterName}\" cannot be less than zero.";

        /// <summary>
        /// Create an <see cref="ArgumentException"/> message for a less than or equal to zero value constraint.
        /// </summary>
        /// <param name="header">The class name and method name header.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>A <see cref="string"/> message.</returns>
        public static string CreateLessThanEqualToZeroExceptionMessage(string header, string parameterName)
            => $"{header} Error: Parameter \"{parameterName}\" cannot be less than or equal to zero.";
    }
}