using System;

namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Represents a cell in an Excel worksheet
    /// </summary>
    public interface ICell
    {
        /// <summary>
        /// Gets the cell reference (e.g., "A1")
        /// </summary>
        string Reference { get; }

        /// <summary>
        /// Gets or sets the cell value
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// Gets the row index (1-based)
        /// </summary>
        int Row { get; }

        /// <summary>
        /// Gets the column index (1-based)
        /// </summary>
        int Column { get; }

        /// <summary>
        /// Gets the column letter (e.g., "A", "B", "AA")
        /// </summary>
        string ColumnLetter { get; }

        /// <summary>
        /// Gets or sets the cell formula
        /// </summary>
        string Formula { get; set; }

        /// <summary>
        /// Gets or sets the cell comment
        /// </summary>
        string Comment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cell is locked
        /// </summary>
        bool IsLocked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cell is hidden
        /// </summary>
        bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the cell number format
        /// </summary>
        string NumberFormat { get; set; }

        /// <summary>
        /// Gets or sets the cell font name
        /// </summary>
        string FontName { get; set; }

        /// <summary>
        /// Gets or sets the cell font size
        /// </summary>
        double FontSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cell font is bold
        /// </summary>
        bool IsBold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cell font is italic
        /// </summary>
        bool IsItalic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cell font is underlined
        /// </summary>
        bool IsUnderlined { get; set; }

        /// <summary>
        /// Gets or sets the cell font color
        /// </summary>
        string FontColor { get; set; }

        /// <summary>
        /// Gets or sets the cell background color
        /// </summary>
        string BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the cell horizontal alignment
        /// </summary>
        string HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the cell vertical alignment
        /// </summary>
        string VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the cell border style
        /// </summary>
        string BorderStyle { get; set; }

        /// <summary>
        /// Gets or sets the cell border color
        /// </summary>
        string BorderColor { get; set; }

        /// <summary>
        /// Clears the cell content
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears the cell formatting
        /// </summary>
        void ClearFormatting();

        /// <summary>
        /// Clears the cell comment
        /// </summary>
        void ClearComment();

        /// <summary>
        /// Copies the cell to another cell
        /// </summary>
        /// <param name="targetCell">The target cell reference</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        bool CopyTo(string targetCell);

        /// <summary>
        /// Copies the cell to another cell with formatting
        /// </summary>
        /// <param name="targetCell">The target cell reference</param>
        /// <param name="includeFormatting">Whether to include formatting</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        bool CopyTo(string targetCell, bool includeFormatting);
    }
}
