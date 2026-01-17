using System;
using System.Collections.Generic;

namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Represents a range of cells in an Excel worksheet
    /// </summary>
    public interface IRange
    {
        /// <summary>
        /// Gets the starting cell reference
        /// </summary>
        string StartCell { get; }

        /// <summary>
        /// Gets the ending cell reference
        /// </summary>
        string EndCell { get; }

        /// <summary>
        /// Gets the cells in this range
        /// </summary>
        IEnumerable<ICell> Cells { get; }

        /// <summary>
        /// Gets or sets the value of the range
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// Gets the number of rows in this range
        /// </summary>
        int RowCount { get; }

        /// <summary>
        /// Gets the number of columns in this range
        /// </summary>
        int ColumnCount { get; }

        /// <summary>
        /// Gets the starting row index (1-based)
        /// </summary>
        int StartRow { get; }

        /// <summary>
        /// Gets the ending row index (1-based)
        /// </summary>
        int EndRow { get; }

        /// <summary>
        /// Gets the starting column index (1-based)
        /// </summary>
        int StartColumn { get; }

        /// <summary>
        /// Gets the ending column index (1-based)
        /// </summary>
        int EndColumn { get; }

        /// <summary>
        /// Gets the starting column letter
        /// </summary>
        string StartColumnLetter { get; }

        /// <summary>
        /// Gets the ending column letter
        /// </summary>
        string EndColumnLetter { get; }

        /// <summary>
        /// Gets or sets the range background color
        /// </summary>
        string BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the range font name
        /// </summary>
        string FontName { get; set; }

        /// <summary>
        /// Gets or sets the range font size
        /// </summary>
        double FontSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the range font is bold
        /// </summary>
        bool IsBold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the range font is italic
        /// </summary>
        bool IsItalic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the range font is underlined
        /// </summary>
        bool IsUnderlined { get; set; }

        /// <summary>
        /// Gets or sets the range font color
        /// </summary>
        string FontColor { get; set; }

        /// <summary>
        /// Gets or sets the range horizontal alignment
        /// </summary>
        string HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the range vertical alignment
        /// </summary>
        string VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the range border style
        /// </summary>
        string BorderStyle { get; set; }

        /// <summary>
        /// Gets or sets the range border color
        /// </summary>
        string BorderColor { get; set; }

        /// <summary>
        /// Gets or sets the range number format
        /// </summary>
        string NumberFormat { get; set; }

        /// <summary>
        /// Gets a cell by relative position within the range
        /// </summary>
        /// <param name="rowOffset">The row offset from the start of the range (0-based)</param>
        /// <param name="columnOffset">The column offset from the start of the range (0-based)</param>
        /// <returns>The cell at the specified position, or null if not found</returns>
        ICell GetCell(int rowOffset, int columnOffset);

        /// <summary>
        /// Gets a cell by cell reference within the range
        /// </summary>
        /// <param name="cellReference">The cell reference (e.g., "A1")</param>
        /// <returns>The cell at the specified reference, or null if not found</returns>
        ICell GetCell(string cellReference);

        /// <summary>
        /// Gets a sub-range within this range
        /// </summary>
        /// <param name="startRowOffset">The starting row offset from the start of the range (0-based)</param>
        /// <param name="startColumnOffset">The starting column offset from the start of the range (0-based)</param>
        /// <param name="endRowOffset">The ending row offset from the start of the range (0-based)</param>
        /// <param name="endColumnOffset">The ending column offset from the start of the range (0-based)</param>
        /// <returns>The sub-range, or null if not found</returns>
        IRange GetRange(int startRowOffset, int startColumnOffset, int endRowOffset, int endColumnOffset);

        /// <summary>
        /// Gets a sub-range within this range
        /// </summary>
        /// <param name="startCellReference">The starting cell reference</param>
        /// <param name="endCellReference">The ending cell reference</param>
        /// <returns>The sub-range, or null if not found</returns>
        IRange GetRange(string startCellReference, string endCellReference);

        /// <summary>
        /// Clears all cells in this range
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears the formatting of all cells in this range
        /// </summary>
        void ClearFormatting();

        /// <summary>
        /// Clears the content of all cells in this range
        /// </summary>
        void ClearContent();

        /// <summary>
        /// Clears the comments of all cells in this range
        /// </summary>
        void ClearComments();

        /// <summary>
        /// Copies this range to another range
        /// </summary>
        /// <param name="targetRange">The target range reference</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        bool CopyTo(string targetRange);

        /// <summary>
        /// Copies this range to another range with formatting
        /// </summary>
        /// <param name="targetRange">The target range reference</param>
        /// <param name="includeFormatting">Whether to include formatting</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        bool CopyTo(string targetRange, bool includeFormatting);

        /// <summary>
        /// Copies this range to another range with specific options
        /// </summary>
        /// <param name="targetRange">The target range reference</param>
        /// <param name="includeFormatting">Whether to include formatting</param>
        /// <param name="includeValues">Whether to include values</param>
        /// <param name="includeFormulas">Whether to include formulas</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        bool CopyTo(string targetRange, bool includeFormatting, bool includeValues, bool includeFormulas);

        /// <summary>
        /// Auto-fits all columns in this range based on their content
        /// </summary>
        /// <returns>True if the auto-fit was successful, false otherwise</returns>
        bool AutoFitColumns();

        /// <summary>
        /// Auto-fits all rows in this range based on their content
        /// </summary>
        /// <returns>True if the auto-fit was successful, false otherwise</returns>
        bool AutoFitRows();

        /// <summary>
        /// Auto-fits all columns and rows in this range based on their content
        /// </summary>
        /// <returns>True if the auto-fit was successful, false otherwise</returns>
        bool AutoFit();

        /// <summary>
        /// Merges all cells in this range
        /// </summary>
        /// <returns>True if the merge was successful, false otherwise</returns>
        bool Merge();

        /// <summary>
        /// Unmerges all cells in this range
        /// </summary>
        /// <returns>True if the unmerge was successful, false otherwise</returns>
        bool Unmerge();

        /// <summary>
        /// Gets a value indicating whether this range is merged
        /// </summary>
        bool IsMerged { get; }

        /// <summary>
        /// Selects this range
        /// </summary>
        /// <returns>True if the selection was successful, false otherwise</returns>
        bool Select();

        /// <summary>
        /// Gets a value indicating whether this range is selected
        /// </summary>
        bool IsSelected { get; }

        /// <summary>
        /// Gets the range address (e.g., "A1:C10")
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Gets the range address with worksheet reference (e.g., "Sheet1!A1:C10")
        /// </summary>
        string FullAddress { get; }
    }
}
