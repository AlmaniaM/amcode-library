using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;
using AMCode.Xlsx;

namespace AMCode.Documents.Xlsx.Domain.Interfaces
{
    /// <summary>
    /// Domain interface for column operations
    /// Represents a column within a worksheet
    /// </summary>
    public interface IColumn
    {
        /// <summary>
        /// Gets the column index (1-based)
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets the column letter (e.g., "A", "B", "AA")
        /// </summary>
        string Letter { get; }

        /// <summary>
        /// Gets the worksheet that contains this column
        /// </summary>
        IWorksheet Worksheet { get; }

        /// <summary>
        /// Gets a collection of all cells in this column
        /// </summary>
        IEnumerable<ICell> Cells { get; }

        /// <summary>
        /// Gets a specific cell in this column
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <returns>Result containing the cell or error information</returns>
        Result<ICell> GetCell(int rowIndex);

        /// <summary>
        /// Gets a specific cell in this column by address
        /// </summary>
        /// <param name="cellAddress">The cell address (e.g., "A1")</param>
        /// <returns>Result containing the cell or error information</returns>
        Result<ICell> GetCell(string cellAddress);

        /// <summary>
        /// Gets the width of this column
        /// </summary>
        /// <returns>Result containing the column width or error information</returns>
        Result<double> GetWidth();

        /// <summary>
        /// Sets the width of this column
        /// </summary>
        /// <param name="width">The column width</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetWidth(double width);

        /// <summary>
        /// Gets a value indicating whether this column is hidden
        /// </summary>
        bool IsHidden { get; }

        /// <summary>
        /// Sets the visibility of this column
        /// </summary>
        /// <param name="hidden">True to hide the column, false to show it</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetHidden(bool hidden);

        /// <summary>
        /// Clears all cells in this column
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result Clear();

        /// <summary>
        /// Clears all formatting in this column
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result ClearFormatting();

        /// <summary>
        /// Clears all content and formatting in this column
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result ClearAll();
    }
}
