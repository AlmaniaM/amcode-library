using System;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF table interface
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// Number of rows
        /// </summary>
        int Rows { get; }
        
        /// <summary>
        /// Number of columns
        /// </summary>
        int Columns { get; }
        
        /// <summary>
        /// Set cell value
        /// </summary>
        void SetCellValue(int row, int column, string value);
        
        /// <summary>
        /// Set cell value with formatting
        /// </summary>
        void SetCellValue(int row, int column, string value, FontStyle fontStyle);
        
        /// <summary>
        /// Set cell background color
        /// </summary>
        void SetCellBackground(int row, int column, Color color);
        
        /// <summary>
        /// Set cell border
        /// </summary>
        void SetCellBorder(int row, int column, Color color, double thickness);
        
        /// <summary>
        /// Set row height
        /// </summary>
        void SetRowHeight(int row, double height);
        
        /// <summary>
        /// Set column width
        /// </summary>
        void SetColumnWidth(int column, double width);
    }
}
