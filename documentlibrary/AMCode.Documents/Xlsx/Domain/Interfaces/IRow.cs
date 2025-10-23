using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;
using AMCode.Xlsx;

namespace AMCode.Documents.Xlsx.Domain.Interfaces
{
    /// <summary>
    /// Domain interface for row operations
    /// Represents a row within a worksheet
    /// </summary>
    public interface IRow
    {
        /// <summary>
        /// Gets the row index (1-based)
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets the worksheet that contains this row
        /// </summary>
        IWorksheet Worksheet { get; }

        /// <summary>
        /// Gets a collection of all cells in this row
        /// </summary>
        IEnumerable<ICell> Cells { get; }

        /// <summary>
        /// Gets a specific cell in this row
        /// </summary>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <returns>Result containing the cell or error information</returns>
        Result<ICell> GetCell(int columnIndex);

        /// <summary>
        /// Gets a specific cell in this row by address
        /// </summary>
        /// <param name="cellAddress">The cell address (e.g., "A1")</param>
        /// <returns>Result containing the cell or error information</returns>
        Result<ICell> GetCell(string cellAddress);

        /// <summary>
        /// Gets the height of this row
        /// </summary>
        /// <returns>Result containing the row height or error information</returns>
        Result<double> GetHeight();

        /// <summary>
        /// Sets the height of this row
        /// </summary>
        /// <param name="height">The row height</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetHeight(double height);

        /// <summary>
        /// Gets a value indicating whether this row is hidden
        /// </summary>
        bool IsHidden { get; }

        /// <summary>
        /// Sets the visibility of this row
        /// </summary>
        /// <param name="hidden">True to hide the row, false to show it</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetHidden(bool hidden);

        /// <summary>
        /// Clears all cells in this row
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result Clear();

        /// <summary>
        /// Clears all formatting in this row
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result ClearFormatting();

        /// <summary>
        /// Clears all content and formatting in this row
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result ClearAll();
    }
}
