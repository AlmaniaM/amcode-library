using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Xlsx.Domain.Interfaces
{
    /// <summary>
    /// Interface for worksheet content operations
    /// Provides methods for reading and writing cell data, formulas, and content validation
    /// </summary>
    public interface IWorksheetContent
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
        /// Gets the number of rows in the worksheet
        /// </summary>
        int RowCount { get; }

        /// <summary>
        /// Gets the number of columns in the worksheet
        /// </summary>
        int ColumnCount { get; }

        /// <summary>
        /// Gets the used range of the worksheet
        /// </summary>
        IRange UsedRange { get; }

        /// <summary>
        /// Gets a cell by its reference (e.g., "A1", "B10")
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <returns>Result containing the cell or error information</returns>
        Result<ICell> GetCell(string cellReference);

        /// <summary>
        /// Gets a cell by its row and column indices
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <returns>Result containing the cell or error information</returns>
        Result<ICell> GetCell(int row, int column);

        /// <summary>
        /// Sets the value of a cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="value">The value to set</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellValue(string cellReference, object value);

        /// <summary>
        /// Sets the value of a cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <param name="value">The value to set</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellValue(int row, int column, object value);

        /// <summary>
        /// Sets a formula in a cell
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="formula">The formula to set</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellFormula(string cellReference, string formula);

        /// <summary>
        /// Sets a formula in a cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <param name="formula">The formula to set</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCellFormula(int row, int column, string formula);

        /// <summary>
        /// Gets a range by its reference (e.g., "A1:C10")
        /// </summary>
        /// <param name="rangeReference">The range reference</param>
        /// <returns>Result containing the range or error information</returns>
        Result<IRange> GetRange(string rangeReference);

        /// <summary>
        /// Gets a range by start and end cell references
        /// </summary>
        /// <param name="startCell">The start cell reference</param>
        /// <param name="endCell">The end cell reference</param>
        /// <returns>Result containing the range or error information</returns>
        Result<IRange> GetRange(string startCell, string endCell);

        /// <summary>
        /// Gets a range by start and end cell positions
        /// </summary>
        /// <param name="startRow">The start row index (0-based)</param>
        /// <param name="startColumn">The start column index (0-based)</param>
        /// <param name="endRow">The end row index (0-based)</param>
        /// <param name="endColumn">The end column index (0-based)</param>
        /// <returns>Result containing the range or error information</returns>
        Result<IRange> GetRange(int startRow, int startColumn, int endRow, int endColumn);

        /// <summary>
        /// Gets a row by its index
        /// </summary>
        /// <param name="rowIndex">The row index (0-based)</param>
        /// <returns>Result containing the row or error information</returns>
        Result<IRow> GetRow(int rowIndex);

        /// <summary>
        /// Gets a column by its index
        /// </summary>
        /// <param name="columnIndex">The column index (0-based)</param>
        /// <returns>Result containing the column or error information</returns>
        Result<IColumn> GetColumn(int columnIndex);

        /// <summary>
        /// Gets all rows in the worksheet
        /// </summary>
        /// <returns>Collection of rows</returns>
        IEnumerable<IRow> GetRows();

        /// <summary>
        /// Gets all columns in the worksheet
        /// </summary>
        /// <returns>Collection of columns</returns>
        IEnumerable<IColumn> GetColumns();

        /// <summary>
        /// Clears all content from the worksheet
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result ClearContent();

        /// <summary>
        /// Clears content from a specific range
        /// </summary>
        /// <param name="rangeReference">The range reference to clear</param>
        /// <returns>Result indicating success or failure</returns>
        Result ClearContent(string rangeReference);

        /// <summary>
        /// Clears content from a specific range
        /// </summary>
        /// <param name="startRow">The start row index (0-based)</param>
        /// <param name="startColumn">The start column index (0-based)</param>
        /// <param name="endRow">The end row index (0-based)</param>
        /// <param name="endColumn">The end column index (0-based)</param>
        /// <returns>Result indicating success or failure</returns>
        Result ClearContent(int startRow, int startColumn, int endRow, int endColumn);

        /// <summary>
        /// Copies content from one range to another
        /// </summary>
        /// <param name="sourceRange">The source range reference</param>
        /// <param name="destinationRange">The destination range reference</param>
        /// <returns>Result indicating success or failure</returns>
        Result CopyContent(string sourceRange, string destinationRange);

        /// <summary>
        /// Copies content from one range to another
        /// </summary>
        /// <param name="sourceStartRow">The source start row index (0-based)</param>
        /// <param name="sourceStartColumn">The source start column index (0-based)</param>
        /// <param name="sourceEndRow">The source end row index (0-based)</param>
        /// <param name="sourceEndColumn">The source end column index (0-based)</param>
        /// <param name="destinationStartRow">The destination start row index (0-based)</param>
        /// <param name="destinationStartColumn">The destination start column index (0-based)</param>
        /// <returns>Result indicating success or failure</returns>
        Result CopyContent(int sourceStartRow, int sourceStartColumn, int sourceEndRow, int sourceEndColumn,
                          int destinationStartRow, int destinationStartColumn);

        /// <summary>
        /// Finds the first cell containing the specified value
        /// </summary>
        /// <param name="value">The value to find</param>
        /// <returns>Result containing the cell or error information</returns>
        Result<ICell> FindCell(object value);

        /// <summary>
        /// Finds all cells containing the specified value
        /// </summary>
        /// <param name="value">The value to find</param>
        /// <returns>Collection of cells containing the value</returns>
        IEnumerable<ICell> FindCells(object value);

        /// <summary>
        /// Finds the first cell matching the specified pattern
        /// </summary>
        /// <param name="pattern">The pattern to match</param>
        /// <returns>Result containing the cell or error information</returns>
        Result<ICell> FindCellByPattern(string pattern);

        /// <summary>
        /// Finds all cells matching the specified pattern
        /// </summary>
        /// <param name="pattern">The pattern to match</param>
        /// <returns>Collection of cells matching the pattern</returns>
        IEnumerable<ICell> FindCellsByPattern(string pattern);

        /// <summary>
        /// Gets the maximum used row index
        /// </summary>
        /// <returns>The maximum used row index</returns>
        int GetMaxUsedRow();

        /// <summary>
        /// Gets the maximum used column index
        /// </summary>
        /// <returns>The maximum used column index</returns>
        int GetMaxUsedColumn();

        /// <summary>
        /// Gets the minimum used row index
        /// </summary>
        /// <returns>The minimum used row index</returns>
        int GetMinUsedRow();

        /// <summary>
        /// Gets the minimum used column index
        /// </summary>
        /// <returns>The minimum used column index</returns>
        int GetMinUsedColumn();

        /// <summary>
        /// Checks if a cell is empty
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <returns>True if the cell is empty, false otherwise</returns>
        bool IsCellEmpty(string cellReference);

        /// <summary>
        /// Checks if a cell is empty
        /// </summary>
        /// <param name="row">The row index (0-based)</param>
        /// <param name="column">The column index (0-based)</param>
        /// <returns>True if the cell is empty, false otherwise</returns>
        bool IsCellEmpty(int row, int column);

        /// <summary>
        /// Checks if a range is empty
        /// </summary>
        /// <param name="rangeReference">The range reference</param>
        /// <returns>True if the range is empty, false otherwise</returns>
        bool IsRangeEmpty(string rangeReference);

        /// <summary>
        /// Checks if a range is empty
        /// </summary>
        /// <param name="startRow">The start row index (0-based)</param>
        /// <param name="startColumn">The start column index (0-based)</param>
        /// <param name="endRow">The end row index (0-based)</param>
        /// <param name="endColumn">The end column index (0-based)</param>
        /// <returns>True if the range is empty, false otherwise</returns>
        bool IsRangeEmpty(int startRow, int startColumn, int endRow, int endColumn);
    }
}