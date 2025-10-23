using System;

namespace AMCode.Exports.Book
{
    /// <summary>
    /// An interface designed to provide information about a column for setting cell data.
    /// </summary>
    public interface IExcelDataColumn : IBookDataColumn
    {
        /// <summary>
        /// The column data type.
        /// </summary>
        Type DataType { get; set; }
    }
}