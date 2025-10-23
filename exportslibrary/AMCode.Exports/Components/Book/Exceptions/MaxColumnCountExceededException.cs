using System;
using AMCode.Exports.Common.Exceptions.Util;

namespace AMCode.Exports.Book
{
    /// <summary>
    /// A class designed to represent an exception when the number of columns attempted to be set has exceeded
    /// the allowed number of columns.
    /// </summary>
    public class MaxColumnCountExceededException : Exception
    {
        /// <summary>
        /// Create an instance of the <see cref="MaxColumnCountExceededException"/> class.
        /// </summary>
        /// <param name="header">A string header representing the location of where the exception happened.</param>
        /// <param name="maxAllowedColumns">The max allowed column count.</param>
        public MaxColumnCountExceededException(string header, int maxAllowedColumns)
            : base(ExportsExceptionUtil.CreateMaxColumnCountExceededExceptionMessage(header, maxAllowedColumns))
        { }
    }
}