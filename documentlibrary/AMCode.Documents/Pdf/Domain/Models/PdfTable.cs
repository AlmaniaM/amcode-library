using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF table implementation
    /// </summary>
    public class PdfTable : ITable, IPageElement
    {
        private readonly string[,] _cellValues;
        private readonly FontStyle[,] _cellFonts;
        private readonly Color[,] _cellBackgrounds;
        private readonly Color[,] _cellBorders;
        private readonly double[] _rowHeights;
        private readonly double[] _columnWidths;

        /// <summary>
        /// Element type
        /// </summary>
        public string ElementType => "Table";

        /// <summary>
        /// Number of rows
        /// </summary>
        public int Rows { get; }

        /// <summary>
        /// Number of columns
        /// </summary>
        public int Columns { get; }

        /// <summary>
        /// Table X position
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Table Y position
        /// </summary>
        public double Y { get; }

        /// <summary>
        /// Create PDF table
        /// </summary>
        public PdfTable(double x, double y, int rows, int columns)
        {
            if (rows <= 0)
                throw new ArgumentException("Rows must be greater than 0", nameof(rows));
            if (columns <= 0)
                throw new ArgumentException("Columns must be greater than 0", nameof(columns));

            X = x;
            Y = y;
            Rows = rows;
            Columns = columns;

            _cellValues = new string[rows, columns];
            _cellFonts = new FontStyle[rows, columns];
            _cellBackgrounds = new Color[rows, columns];
            _cellBorders = new Color[rows, columns];
            _rowHeights = new double[rows];
            _columnWidths = new double[columns];

            // Initialize with default values
            for (int i = 0; i < rows; i++)
            {
                _rowHeights[i] = 20; // Default row height
                for (int j = 0; j < columns; j++)
                {
                    _cellValues[i, j] = string.Empty;
                    _cellFonts[i, j] = new FontStyle();
                    _cellBackgrounds[i, j] = Color.Transparent;
                    _cellBorders[i, j] = Color.Black;
                    _columnWidths[j] = 100; // Default column width
                }
            }
        }

        /// <summary>
        /// Set cell value
        /// </summary>
        public void SetCellValue(int row, int column, string value)
        {
            ValidateCellIndex(row, column);
            _cellValues[row, column] = value ?? string.Empty;
        }

        /// <summary>
        /// Set cell value with formatting
        /// </summary>
        public void SetCellValue(int row, int column, string value, FontStyle fontStyle)
        {
            ValidateCellIndex(row, column);
            _cellValues[row, column] = value ?? string.Empty;
            _cellFonts[row, column] = fontStyle ?? new FontStyle();
        }

        /// <summary>
        /// Set cell background color
        /// </summary>
        public void SetCellBackground(int row, int column, Color color)
        {
            ValidateCellIndex(row, column);
            _cellBackgrounds[row, column] = color;
        }

        /// <summary>
        /// Set cell border
        /// </summary>
        public void SetCellBorder(int row, int column, Color color, double thickness)
        {
            ValidateCellIndex(row, column);
            _cellBorders[row, column] = color;
        }

        /// <summary>
        /// Set row height
        /// </summary>
        public void SetRowHeight(int row, double height)
        {
            if (row < 0 || row >= Rows)
                throw new ArgumentOutOfRangeException(nameof(row));
            if (height <= 0)
                throw new ArgumentException("Height must be greater than 0", nameof(height));

            _rowHeights[row] = height;
        }

        /// <summary>
        /// Set column width
        /// </summary>
        public void SetColumnWidth(int column, double width)
        {
            if (column < 0 || column >= Columns)
                throw new ArgumentOutOfRangeException(nameof(column));
            if (width <= 0)
                throw new ArgumentException("Width must be greater than 0", nameof(width));

            _columnWidths[column] = width;
        }

        /// <summary>
        /// Get cell value
        /// </summary>
        public string GetCellValue(int row, int column)
        {
            ValidateCellIndex(row, column);
            return _cellValues[row, column];
        }

        /// <summary>
        /// Get cell font style
        /// </summary>
        public FontStyle GetCellFont(int row, int column)
        {
            ValidateCellIndex(row, column);
            return _cellFonts[row, column];
        }

        /// <summary>
        /// Get cell background color
        /// </summary>
        public Color GetCellBackground(int row, int column)
        {
            ValidateCellIndex(row, column);
            return _cellBackgrounds[row, column];
        }

        /// <summary>
        /// Get cell border color
        /// </summary>
        public Color GetCellBorder(int row, int column)
        {
            ValidateCellIndex(row, column);
            return _cellBorders[row, column];
        }

        /// <summary>
        /// Get row height
        /// </summary>
        public double GetRowHeight(int row)
        {
            if (row < 0 || row >= Rows)
                throw new ArgumentOutOfRangeException(nameof(row));
            return _rowHeights[row];
        }

        /// <summary>
        /// Get column width
        /// </summary>
        public double GetColumnWidth(int column)
        {
            if (column < 0 || column >= Columns)
                throw new ArgumentOutOfRangeException(nameof(column));
            return _columnWidths[column];
        }

        private void ValidateCellIndex(int row, int column)
        {
            if (row < 0 || row >= Rows)
                throw new ArgumentOutOfRangeException(nameof(row));
            if (column < 0 || column >= Columns)
                throw new ArgumentOutOfRangeException(nameof(column));
        }
    }
}
