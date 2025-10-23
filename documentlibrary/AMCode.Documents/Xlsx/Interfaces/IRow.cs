using System;
using System.Collections.Generic;

namespace AMCode.Xlsx
{
    /// <summary>
    /// Represents a row in an Excel worksheet
    /// </summary>
    public interface IRow
    {
        /// <summary>
        /// Gets the row index (1-based)
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Gets the cells in this row
        /// </summary>
        IEnumerable<ICell> Cells { get; }

        /// <summary>
        /// Gets or sets the row height
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the row is hidden
        /// </summary>
        bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the row is locked
        /// </summary>
        bool IsLocked { get; set; }

        /// <summary>
        /// Gets or sets the row background color
        /// </summary>
        string BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the row font name
        /// </summary>
        string FontName { get; set; }

        /// <summary>
        /// Gets or sets the row font size
        /// </summary>
        double FontSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the row font is bold
        /// </summary>
        bool IsBold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the row font is italic
        /// </summary>
        bool IsItalic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the row font is underlined
        /// </summary>
        bool IsUnderlined { get; set; }

        /// <summary>
        /// Gets or sets the row font color
        /// </summary>
        string FontColor { get; set; }

        /// <summary>
        /// Gets or sets the row horizontal alignment
        /// </summary>
        string HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the row vertical alignment
        /// </summary>
        string VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the row border style
        /// </summary>
        string BorderStyle { get; set; }

        /// <summary>
        /// Gets or sets the row border color
        /// </summary>
        string BorderColor { get; set; }

        /// <summary>
        /// Gets a cell by column index
        /// </summary>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <returns>The cell at the specified column, or null if not found</returns>
        ICell GetCell(int columnIndex);

        /// <summary>
        /// Gets a cell by column letter
        /// </summary>
        /// <param name="columnLetter">The column letter (e.g., "A", "B", "AA")</param>
        /// <returns>The cell at the specified column, or null if not found</returns>
        ICell GetCell(string columnLetter);

        /// <summary>
        /// Gets a range of cells in this row
        /// </summary>
        /// <param name="startColumn">The starting column index (1-based)</param>
        /// <param name="endColumn">The ending column index (1-based)</param>
        /// <returns>The range of cells, or null if not found</returns>
        IRange GetRange(int startColumn, int endColumn);

        /// <summary>
        /// Gets a range of cells in this row
        /// </summary>
        /// <param name="startColumnLetter">The starting column letter</param>
        /// <param name="endColumnLetter">The ending column letter</param>
        /// <returns>The range of cells, or null if not found</returns>
        IRange GetRange(string startColumnLetter, string endColumnLetter);

        /// <summary>
        /// Clears all cells in this row
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears the formatting of all cells in this row
        /// </summary>
        void ClearFormatting();

        /// <summary>
        /// Inserts a new row above this row
        /// </summary>
        /// <returns>The new row, or null if insertion failed</returns>
        IRow InsertAbove();

        /// <summary>
        /// Inserts a new row below this row
        /// </summary>
        /// <returns>The new row, or null if insertion failed</returns>
        IRow InsertBelow();

        /// <summary>
        /// Deletes this row
        /// </summary>
        /// <returns>True if the row was deleted successfully, false otherwise</returns>
        bool Delete();

        /// <summary>
        /// Copies this row to another row
        /// </summary>
        /// <param name="targetRowIndex">The target row index (1-based)</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        bool CopyTo(int targetRowIndex);

        /// <summary>
        /// Copies this row to another row with formatting
        /// </summary>
        /// <param name="targetRowIndex">The target row index (1-based)</param>
        /// <param name="includeFormatting">Whether to include formatting</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        bool CopyTo(int targetRowIndex, bool includeFormatting);

        /// <summary>
        /// Auto-fits the height of this row based on its content
        /// </summary>
        /// <returns>True if the auto-fit was successful, false otherwise</returns>
        bool AutoFit();
    }
}
