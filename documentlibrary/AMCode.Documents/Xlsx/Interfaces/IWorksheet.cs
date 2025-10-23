using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;

namespace AMCode.Xlsx
{
    /// <summary>
    /// Represents an Excel worksheet
    /// </summary>
    public interface IWorksheet
    {
        /// <summary>
        /// Gets the name of the worksheet
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the index of the worksheet in the workbook
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets a value indicating whether the worksheet is visible
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Gets the collection of cells in this worksheet
        /// </summary>
        IEnumerable<ICell> Cells { get; }

        /// <summary>
        /// Gets the collection of rows in this worksheet
        /// </summary>
        IEnumerable<IRow> Rows { get; }

        /// <summary>
        /// Gets the collection of columns in this worksheet
        /// </summary>
        IEnumerable<IColumn> Columns { get; }

        /// <summary>
        /// Gets the used range of this worksheet
        /// </summary>
        IRange UsedRange { get; }

        /// <summary>
        /// Gets a range by cell references
        /// </summary>
        /// <param name="startCell">The starting cell reference (e.g., "A1")</param>
        /// <param name="endCell">The ending cell reference (e.g., "C10")</param>
        /// <returns>Result containing the range or error information</returns>
        Result<IRange> GetRange(string startCell, string endCell);

        /// <summary>
        /// Gets a range by row and column indices
        /// </summary>
        /// <param name="startRow">The starting row index (1-based)</param>
        /// <param name="startColumn">The starting column index (1-based)</param>
        /// <param name="endRow">The ending row index (1-based)</param>
        /// <param name="endColumn">The ending column index (1-based)</param>
        /// <returns>Result containing the range or error information</returns>
        Result<IRange> GetRange(int startRow, int startColumn, int endRow, int endColumn);

        /// <summary>
        /// Sets the value of a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference (e.g., "A1")</param>
        /// <param name="value">The value to set</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellValue(string cellReference, object value);

        /// <summary>
        /// Sets the value of a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (1-based)</param>
        /// <param name="column">The column index (1-based)</param>
        /// <param name="value">The value to set</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellValue(int row, int column, object value);

        /// <summary>
        /// Gets the value of a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference (e.g., "A1")</param>
        /// <returns>Result containing the cell value or error information</returns>
        Result<object> GetCellValue(string cellReference);

        /// <summary>
        /// Gets the value of a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (1-based)</param>
        /// <param name="column">The column index (1-based)</param>
        /// <returns>Result containing the cell value or error information</returns>
        Result<object> GetCellValue(int row, int column);

        /// <summary>
        /// Clears all content from the worksheet
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result Clear();

        /// <summary>
        /// Clears a specific range of cells
        /// </summary>
        /// <param name="startCell">The starting cell reference</param>
        /// <param name="endCell">The ending cell reference</param>
        /// <returns>Result indicating success or failure</returns>
        Result ClearRange(string startCell, string endCell);

        /// <summary>
        /// Clears a specific range of cells by row and column indices
        /// </summary>
        /// <param name="startRow">The starting row index (1-based)</param>
        /// <param name="startColumn">The starting column index (1-based)</param>
        /// <param name="endRow">The ending row index (1-based)</param>
        /// <param name="endColumn">The ending column index (1-based)</param>
        /// <returns>Result indicating success or failure</returns>
        Result ClearRange(int startRow, int startColumn, int endRow, int endColumn);

        /// <summary>
        /// Sets the width of a specific column
        /// </summary>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <param name="width">The width in points</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetColumnWidth(int columnIndex, double width);

        /// <summary>
        /// Sets the width of a specific column by letter
        /// </summary>
        /// <param name="columnLetter">The column letter (e.g., "A", "B", "AA")</param>
        /// <param name="width">The width in points</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetColumnWidth(string columnLetter, double width);

        /// <summary>
        /// Sets the height of a specific row
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <param name="height">The height in points</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetRowHeight(int rowIndex, double height);

        /// <summary>
        /// Gets the width of a specific column
        /// </summary>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <returns>Result containing the column width or error information</returns>
        Result<double> GetColumnWidth(int columnIndex);

        /// <summary>
        /// Gets the width of a specific column by letter
        /// </summary>
        /// <param name="columnLetter">The column letter (e.g., "A", "B", "AA")</param>
        /// <returns>Result containing the column width or error information</returns>
        Result<double> GetColumnWidth(string columnLetter);

        /// <summary>
        /// Gets the height of a specific row
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <returns>Result containing the row height or error information</returns>
        Result<double> GetRowHeight(int rowIndex);

        /// <summary>
        /// Automatically fits the width of a specific column based on its content
        /// </summary>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <returns>Result indicating success or failure</returns>
        Result AutoFitColumn(int columnIndex);

        /// <summary>
        /// Automatically fits the width of a specific column by letter based on its content
        /// </summary>
        /// <param name="columnLetter">The column letter (e.g., "A", "B", "AA")</param>
        /// <returns>Result indicating success or failure</returns>
        Result AutoFitColumn(string columnLetter);

        /// <summary>
        /// Automatically fits the height of a specific row based on its content
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <returns>Result indicating success or failure</returns>
        Result AutoFitRow(int rowIndex);

        /// <summary>
        /// Automatically fits all columns in the worksheet based on their content
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result AutoFitAllColumns();

        /// <summary>
        /// Automatically fits all rows in the worksheet based on their content
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result AutoFitAllRows();

        /// <summary>
        /// Automatically fits all columns and rows in the worksheet based on their content
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result AutoFitAll();

        /// <summary>
        /// Activates this worksheet (makes it the active worksheet)
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result Activate();

        /// <summary>
        /// Deactivates this worksheet
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result Deactivate();
    }
}
