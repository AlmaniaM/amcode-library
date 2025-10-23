using System;
using AMCode.Documents.Common.Models;
using AMCode.Xlsx;

namespace AMCode.Documents.Xlsx.Domain.Interfaces
{
    /// <summary>
    /// Domain interface for cell operations
    /// Represents a single cell within a worksheet
    /// </summary>
    public interface ICell
    {
        /// <summary>
        /// Gets the address of the cell (e.g., "A1")
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Gets the row index of the cell (1-based)
        /// </summary>
        int Row { get; }

        /// <summary>
        /// Gets the column index of the cell (1-based)
        /// </summary>
        int Column { get; }

        /// <summary>
        /// Gets or sets the value of the cell
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// Gets or sets the formula of the cell
        /// </summary>
        string Formula { get; set; }

        /// <summary>
        /// Gets the data type of the cell value
        /// </summary>
        CellDataType DataType { get; }

        /// <summary>
        /// Gets a value indicating whether the cell has a formula
        /// </summary>
        bool HasFormula { get; }

        /// <summary>
        /// Gets a value indicating whether the cell is empty
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Gets a value indicating whether the cell is merged
        /// </summary>
        bool IsMerged { get; }

        /// <summary>
        /// Gets the worksheet that contains this cell
        /// </summary>
        IWorksheet Worksheet { get; }

        /// <summary>
        /// Clears the cell value
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result Clear();

        /// <summary>
        /// Clears the cell formatting
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result ClearFormatting();

        /// <summary>
        /// Clears all cell content and formatting
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result ClearAll();
    }

    /// <summary>
    /// Enumeration of cell data types
    /// </summary>
    public enum CellDataType
    {
        /// <summary>
        /// Empty cell
        /// </summary>
        Empty,

        /// <summary>
        /// String data type
        /// </summary>
        String,

        /// <summary>
        /// Number data type
        /// </summary>
        Number,

        /// <summary>
        /// Date data type
        /// </summary>
        Date,

        /// <summary>
        /// Boolean data type
        /// </summary>
        Boolean,

        /// <summary>
        /// Formula data type
        /// </summary>
        Formula,

        /// <summary>
        /// Error data type
        /// </summary>
        Error
    }
}
