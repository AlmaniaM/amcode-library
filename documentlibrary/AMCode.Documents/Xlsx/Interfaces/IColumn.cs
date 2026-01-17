using System;
using System.Collections.Generic;

namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Represents a column in an Excel worksheet
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
        /// Gets the cells in this column
        /// </summary>
        IEnumerable<ICell> Cells { get; }

        /// <summary>
        /// Gets or sets the column width
        /// </summary>
        double Width { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is hidden
        /// </summary>
        bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is locked
        /// </summary>
        bool IsLocked { get; set; }

        /// <summary>
        /// Gets or sets the column background color
        /// </summary>
        string BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the column font name
        /// </summary>
        string FontName { get; set; }

        /// <summary>
        /// Gets or sets the column font size
        /// </summary>
        double FontSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column font is bold
        /// </summary>
        bool IsBold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column font is italic
        /// </summary>
        bool IsItalic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column font is underlined
        /// </summary>
        bool IsUnderlined { get; set; }

        /// <summary>
        /// Gets or sets the column font color
        /// </summary>
        string FontColor { get; set; }

        /// <summary>
        /// Gets or sets the column horizontal alignment
        /// </summary>
        string HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the column vertical alignment
        /// </summary>
        string VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the column border style
        /// </summary>
        string BorderStyle { get; set; }

        /// <summary>
        /// Gets or sets the column border color
        /// </summary>
        string BorderColor { get; set; }

        /// <summary>
        /// Gets a cell by row index
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <returns>The cell at the specified row, or null if not found</returns>
        ICell GetCell(int rowIndex);

        /// <summary>
        /// Gets a range of cells in this column
        /// </summary>
        /// <param name="startRow">The starting row index (1-based)</param>
        /// <param name="endRow">The ending row index (1-based)</param>
        /// <returns>The range of cells, or null if not found</returns>
        IRange GetRange(int startRow, int endRow);

        /// <summary>
        /// Clears all cells in this column
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears the formatting of all cells in this column
        /// </summary>
        void ClearFormatting();

        /// <summary>
        /// Inserts a new column to the left of this column
        /// </summary>
        /// <returns>The new column, or null if insertion failed</returns>
        IColumn InsertLeft();

        /// <summary>
        /// Inserts a new column to the right of this column
        /// </summary>
        /// <returns>The new column, or null if insertion failed</returns>
        IColumn InsertRight();

        /// <summary>
        /// Deletes this column
        /// </summary>
        /// <returns>True if the column was deleted successfully, false otherwise</returns>
        bool Delete();

        /// <summary>
        /// Copies this column to another column
        /// </summary>
        /// <param name="targetColumnIndex">The target column index (1-based)</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        bool CopyTo(int targetColumnIndex);

        /// <summary>
        /// Copies this column to another column with formatting
        /// </summary>
        /// <param name="targetColumnIndex">The target column index (1-based)</param>
        /// <param name="includeFormatting">Whether to include formatting</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        bool CopyTo(int targetColumnIndex, bool includeFormatting);

        /// <summary>
        /// Auto-fits the width of this column based on its content
        /// </summary>
        /// <returns>True if the auto-fit was successful, false otherwise</returns>
        bool AutoFit();

        /// <summary>
        /// Gets the column letter from a column index
        /// </summary>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <returns>The column letter</returns>
        static string GetColumnLetter(int columnIndex)
        {
            if (columnIndex < 1)
                throw new ArgumentException("Column index must be greater than 0", nameof(columnIndex));

            string columnLetter = "";
            while (columnIndex > 0)
            {
                columnIndex--;
                columnLetter = (char)('A' + columnIndex % 26) + columnLetter;
                columnIndex /= 26;
            }
            return columnLetter;
        }

        /// <summary>
        /// Gets the column index from a column letter
        /// </summary>
        /// <param name="columnLetter">The column letter (e.g., "A", "B", "AA")</param>
        /// <returns>The column index (1-based)</returns>
        static int GetColumnIndex(string columnLetter)
        {
            if (string.IsNullOrEmpty(columnLetter))
                throw new ArgumentException("Column letter cannot be null or empty", nameof(columnLetter));

            int columnIndex = 0;
            for (int i = 0; i < columnLetter.Length; i++)
            {
                columnIndex = columnIndex * 26 + (char.ToUpper(columnLetter[i]) - 'A' + 1);
            }
            return columnIndex;
        }
    }
}
