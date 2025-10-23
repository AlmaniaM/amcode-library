using AMCode.Common.Xlsx;

namespace AMCode.SyncfusionIo.Xlsx.Util
{
    /// <summary>
    /// A class designed to help create common exception messages.
    /// </summary>
    public class XlsxExceptionUtil
    {
        /// <summary>
        /// Creates an exception message for when attempting to use invalid range addresses.
        /// </summary>
        /// <param name="header">The <see cref="string"/> exception header which indicates where the exception occurred.</param>
        /// <returns>A <see cref="string"/> exception message.</returns>
        public static string CreateAddressNullEmptyWhiteSpaceMessage(string header)
            => $"{header} Error: The address cannot be a null, empty string, or white space. You must provide a valid Excel address range.";

        /// <summary>
        /// Creates an exception message for when an incorrect <see cref="ExcelHAlign"/> value is attempted to be parsed.
        /// </summary>
        /// <param name="header">The <see cref="string"/> exception header which indicates where the exception occurred.</param>
        /// <param name="hAlign">The value that was attempted to be parsed.</param>
        /// <returns>A <see cref="string"/> exception message.</returns>
        public static string CreateExcelHAlignParseErrorMessage(string header, int hAlign)
            => $"{header} Error: The value \"{hAlign}\" does not exist in the {nameof(ExcelHAlign)} enum type.";

        /// <summary>
        /// Creates an exception message for when an index parameter is less than or equal to zero.
        /// </summary>
        /// <param name="header">The <see cref="string"/> exception header which indicates where the exception occurred.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>A <see cref="string"/> exception message.</returns>
        public static string CreateLessThanOneIndexMessage(string header, string parameterName)
            => $"{header} Error: The \"{parameterName}\" index is one-based. You must provide an index starting from 1 and above.";

        /// <summary>
        /// Creates an exception message for when an index parameter is greater than allowed.
        /// </summary>
        /// <param name="header">The <see cref="string"/> exception header which indicates where the exception occurred.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value that was passed as the index.</param>
        /// <param name="maxValue">The max allowed index value.</param>
        /// <returns>A <see cref="string"/> exception message.</returns>
        public static string CreateMaxIndexMessage(string header, string parameterName, int value, int maxValue)
            => $"{header} Error: The \"{parameterName}\" index of \"{value}\" is greater than the allowed \"{maxValue}\".";

        /// <summary>
        /// Creates an exception message for a null parameter.
        /// </summary>
        /// <param name="header">The <see cref="string"/> exception header which indicates where the exception occurred.</param>
        /// <param name="parameterName">The name of the null parameter.</param>
        /// <returns>A <see cref="string"/> exception message.</returns>
        public static string CreateNullArgumentMessage(string header, string parameterName)
            => $"{header} Error: The value for \"{parameterName}\" cannot be null.";

        /// <summary>
        /// Creates an exception message for when a string is null or empty.
        /// </summary>
        /// <param name="header">The <see cref="string"/> exception header which indicates where the exception occurred.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>A <see cref="string"/> exception message.</returns>
        public static string CreateNullOrEmptyStringMessage(string header, string parameterName)
            => $"{header} Error: The value for \"{parameterName}\" cannot be null or empty.";
    }
}