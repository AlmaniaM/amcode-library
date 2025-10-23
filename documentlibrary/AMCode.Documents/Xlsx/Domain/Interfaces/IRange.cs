using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;
using AMCode.Xlsx;

namespace AMCode.Documents.Xlsx.Domain.Interfaces
{
    /// <summary>
    /// Domain interface for range operations
    /// Represents a range of cells within a worksheet
    /// </summary>
    public interface IRange
    {
        /// <summary>
        /// Gets the address of the range (e.g., "A1:B5")
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Gets the worksheet that contains this range
        /// </summary>
        IWorksheet Worksheet { get; }

        /// <summary>
        /// Gets the number of rows in this range
        /// </summary>
        int RowCount { get; }

        /// <summary>
        /// Gets the number of columns in this range
        /// </summary>
        int ColumnCount { get; }

        /// <summary>
        /// Gets the first row index (1-based)
        /// </summary>
        int FirstRow { get; }

        /// <summary>
        /// Gets the last row index (1-based)
        /// </summary>
        int LastRow { get; }

        /// <summary>
        /// Gets the first column index (1-based)
        /// </summary>
        int FirstColumn { get; }

        /// <summary>
        /// Gets the last column index (1-based)
        /// </summary>
        int LastColumn { get; }

        /// <summary>
        /// Gets a collection of all cells in this range
        /// </summary>
        IEnumerable<ICell> Cells { get; }

        /// <summary>
        /// Gets a collection of all rows in this range
        /// </summary>
        IEnumerable<IRow> Rows { get; }

        /// <summary>
        /// Gets a collection of all columns in this range
        /// </summary>
        IEnumerable<IColumn> Columns { get; }

        /// <summary>
        /// Gets a specific cell within this range
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <returns>Result containing the cell or error information</returns>
        Result<ICell> GetCell(int rowIndex, int columnIndex);

        /// <summary>
        /// Gets a specific cell within this range by address
        /// </summary>
        /// <param name="cellAddress">The cell address (e.g., "A1")</param>
        /// <returns>Result containing the cell or error information</returns>
        Result<ICell> GetCell(string cellAddress);

        /// <summary>
        /// Gets a specific row within this range
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <returns>Result containing the row or error information</returns>
        Result<IRow> GetRow(int rowIndex);

        /// <summary>
        /// Gets a specific column within this range
        /// </summary>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <returns>Result containing the column or error information</returns>
        Result<IColumn> GetColumn(int columnIndex);

        /// <summary>
        /// Gets a sub-range within this range
        /// </summary>
        /// <param name="startRow">The start row index (1-based)</param>
        /// <param name="startColumn">The start column index (1-based)</param>
        /// <param name="endRow">The end row index (1-based)</param>
        /// <param name="endColumn">The end column index (1-based)</param>
        /// <returns>Result containing the sub-range or error information</returns>
        Result<IRange> GetSubRange(int startRow, int startColumn, int endRow, int endColumn);

        /// <summary>
        /// Gets a sub-range within this range by address
        /// </summary>
        /// <param name="address">The range address (e.g., "A1:C5")</param>
        /// <returns>Result containing the sub-range or error information</returns>
        Result<IRange> GetSubRange(string address);

        /// <summary>
        /// Sets the value of all cells in this range
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetValue(object value);

        /// <summary>
        /// Sets the value of all cells in this range with a specific data type
        /// </summary>
        /// <typeparam name="T">The data type of the value</typeparam>
        /// <param name="value">The value to set</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetValue<T>(T value);

        /// <summary>
        /// Clears all values in this range
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result Clear();

        /// <summary>
        /// Clears all formatting in this range
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result ClearFormatting();

        /// <summary>
        /// Clears all content and formatting in this range
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result ClearAll();

        /// <summary>
        /// Copies this range to another range
        /// </summary>
        /// <param name="destinationRange">The destination range</param>
        /// <returns>Result indicating success or failure</returns>
        Result CopyTo(IRange destinationRange);

        /// <summary>
        /// Copies this range to another range with options
        /// </summary>
        /// <param name="destinationRange">The destination range</param>
        /// <param name="copyOptions">The copy options</param>
        /// <returns>Result indicating success or failure</returns>
        Result CopyTo(IRange destinationRange, CopyOptions copyOptions);

        /// <summary>
        /// Checks if this range intersects with another range
        /// </summary>
        /// <param name="otherRange">The other range to check</param>
        /// <returns>True if the ranges intersect, false otherwise</returns>
        bool IntersectsWith(IRange otherRange);

        /// <summary>
        /// Checks if this range contains another range
        /// </summary>
        /// <param name="otherRange">The other range to check</param>
        /// <returns>True if this range contains the other range, false otherwise</returns>
        bool Contains(IRange otherRange);

        /// <summary>
        /// Checks if this range contains a specific cell
        /// </summary>
        /// <param name="cellAddress">The cell address to check</param>
        /// <returns>True if this range contains the cell, false otherwise</returns>
        bool Contains(string cellAddress);

        /// <summary>
        /// Checks if this range contains a specific cell
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <returns>True if this range contains the cell, false otherwise</returns>
        bool Contains(int rowIndex, int columnIndex);

        /// <summary>
        /// Gets the intersection of this range with another range
        /// </summary>
        /// <param name="otherRange">The other range</param>
        /// <returns>Result containing the intersection range or error information</returns>
        Result<IRange> Intersect(IRange otherRange);

        /// <summary>
        /// Gets the union of this range with another range
        /// </summary>
        /// <param name="otherRange">The other range</param>
        /// <returns>Result containing the union range or error information</returns>
        Result<IRange> Union(IRange otherRange);

        /// <summary>
        /// Gets the offset range from this range
        /// </summary>
        /// <param name="rowOffset">The row offset</param>
        /// <param name="columnOffset">The column offset</param>
        /// <returns>Result containing the offset range or error information</returns>
        Result<IRange> Offset(int rowOffset, int columnOffset);

        /// <summary>
        /// Gets the resized range from this range
        /// </summary>
        /// <param name="rowCount">The new row count</param>
        /// <param name="columnCount">The new column count</param>
        /// <returns>Result containing the resized range or error information</returns>
        Result<IRange> Resize(int rowCount, int columnCount);

        /// <summary>
        /// Gets the used range within this range
        /// </summary>
        /// <returns>Result containing the used range or error information</returns>
        Result<IRange> GetUsedRange();

        /// <summary>
        /// Gets the entire row range for a specific row
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <returns>Result containing the entire row range or error information</returns>
        Result<IRange> GetEntireRow(int rowIndex);

        /// <summary>
        /// Gets the entire column range for a specific column
        /// </summary>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <returns>Result containing the entire column range or error information</returns>
        Result<IRange> GetEntireColumn(int columnIndex);

        /// <summary>
        /// Gets the entire row range for a specific row by address
        /// </summary>
        /// <param name="rowAddress">The row address (e.g., "1:1")</param>
        /// <returns>Result containing the entire row range or error information</returns>
        Result<IRange> GetEntireRow(string rowAddress);

        /// <summary>
        /// Gets the entire column range for a specific column by address
        /// </summary>
        /// <param name="columnAddress">The column address (e.g., "A:A")</param>
        /// <returns>Result containing the entire column range or error information</returns>
        Result<IRange> GetEntireColumn(string columnAddress);

        /// <summary>
        /// Gets the range address in A1 notation
        /// </summary>
        /// <returns>The range address in A1 notation</returns>
        string GetAddress();

        /// <summary>
        /// Gets the range address in R1C1 notation
        /// </summary>
        /// <returns>The range address in R1C1 notation</returns>
        string GetAddressR1C1();

        /// <summary>
        /// Gets the range address in A1 notation with options
        /// </summary>
        /// <param name="options">The address options</param>
        /// <returns>The range address in A1 notation with options</returns>
        string GetAddress(AddressOptions options);

        /// <summary>
        /// Gets the range address in R1C1 notation with options
        /// </summary>
        /// <param name="options">The address options</param>
        /// <returns>The range address in R1C1 notation with options</returns>
        string GetAddressR1C1(AddressOptions options);
    }

    /// <summary>
    /// Structure representing copy options for range operations
    /// </summary>
    public struct CopyOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether to copy values
        /// </summary>
        public bool CopyValues { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy formatting
        /// </summary>
        public bool CopyFormatting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy formulas
        /// </summary>
        public bool CopyFormulas { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy comments
        /// </summary>
        public bool CopyComments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy validation
        /// </summary>
        public bool CopyValidation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy borders
        /// </summary>
        public bool CopyBorders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy fill
        /// </summary>
        public bool CopyFill { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy font
        /// </summary>
        public bool CopyFont { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy number format
        /// </summary>
        public bool CopyNumberFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy alignment
        /// </summary>
        public bool CopyAlignment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy protection
        /// </summary>
        public bool CopyProtection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy column width
        /// </summary>
        public bool CopyColumnWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy row height
        /// </summary>
        public bool CopyRowHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy merged cells
        /// </summary>
        public bool CopyMergedCells { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy conditional formatting
        /// </summary>
        public bool CopyConditionalFormatting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy data validation
        /// </summary>
        public bool CopyDataValidation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy hyperlinks
        /// </summary>
        public bool CopyHyperlinks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy charts
        /// </summary>
        public bool CopyCharts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy pictures
        /// </summary>
        public bool CopyPictures { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy shapes
        /// </summary>
        public bool CopyShapes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy controls
        /// </summary>
        public bool CopyControls { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to copy all
        /// </summary>
        public bool CopyAll { get; set; }
    }

    /// <summary>
    /// Structure representing address options for range operations
    /// </summary>
    public struct AddressOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use absolute references
        /// </summary>
        public bool UseAbsoluteReferences { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use relative references
        /// </summary>
        public bool UseRelativeReferences { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use mixed references
        /// </summary>
        public bool UseMixedReferences { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include sheet name
        /// </summary>
        public bool IncludeSheetName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use external references
        /// </summary>
        public bool UseExternalReferences { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use R1C1 notation
        /// </summary>
        public bool UseR1C1Notation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use A1 notation
        /// </summary>
        public bool UseA1Notation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use row absolute references
        /// </summary>
        public bool UseRowAbsoluteReferences { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use column absolute references
        /// </summary>
        public bool UseColumnAbsoluteReferences { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use row relative references
        /// </summary>
        public bool UseRowRelativeReferences { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use column relative references
        /// </summary>
        public bool UseColumnRelativeReferences { get; set; }
    }
}
