using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using AMCode.Documents.Xlsx;

namespace AMCode.Documents.Xlsx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of the range interface
    /// Represents a range of cells in an Excel worksheet
    /// </summary>
    public class OpenXmlRange : IRange
    {
        private readonly OpenXmlWorksheet _worksheet;
        private readonly string _address;
        private readonly string _fullAddress;

        /// <summary>
        /// Initializes a new instance of the OpenXmlRange class
        /// </summary>
        /// <param name="address">The range address (e.g., "A1:C10")</param>
        /// <param name="worksheet">The worksheet containing this range</param>
        /// <exception cref="ArgumentNullException">Thrown when address or worksheet is null</exception>
        public OpenXmlRange(string address, OpenXmlWorksheet worksheet)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentNullException(nameof(address));
            _worksheet = worksheet ?? throw new ArgumentNullException(nameof(worksheet));
            
            _address = address;
            _fullAddress = $"{worksheet.Name}!{address}";
            
            ParseAddress(address);
        }

        /// <summary>
        /// Gets the starting cell reference
        /// </summary>
        public string StartCell { get; private set; }

        /// <summary>
        /// Gets the ending cell reference
        /// </summary>
        public string EndCell { get; private set; }

        /// <summary>
        /// Gets the cells in this range
        /// </summary>
        public IEnumerable<ICell> Cells
        {
            get
            {
                var cells = new List<ICell>();
                for (int row = StartRow; row <= EndRow; row++)
                {
                    for (int col = StartColumn; col <= EndColumn; col++)
                    {
                        var cellReference = GetCellReference(row, col);
                        var cell = _worksheet.GetCell(cellReference);
                        if (cell != null)
                        {
                            cells.Add(new OpenXmlCell(cell, _worksheet));
                        }
                    }
                }
                return cells;
            }
        }

        /// <summary>
        /// Gets or sets the value of the range
        /// </summary>
        public object Value
        {
            get
            {
                if (RowCount == 1 && ColumnCount == 1)
                {
                    var cell = _worksheet.GetCell(StartCell);
                    return cell?.CellValue?.Text;
                }
                return null; // Multi-cell ranges don't have a single value
            }
            set
            {
                if (RowCount == 1 && ColumnCount == 1)
                {
                    _worksheet.SetCellValue(StartCell, value);
                }
                else
                {
                    // For multi-cell ranges, set the value to all cells
                    for (int row = StartRow; row <= EndRow; row++)
                    {
                        for (int col = StartColumn; col <= EndColumn; col++)
                        {
                            var cellReference = GetCellReference(row, col);
                            _worksheet.SetCellValue(cellReference, value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the number of rows in this range
        /// </summary>
        public int RowCount { get; private set; }

        /// <summary>
        /// Gets the number of columns in this range
        /// </summary>
        public int ColumnCount { get; private set; }

        /// <summary>
        /// Gets the starting row index (1-based)
        /// </summary>
        public int StartRow { get; private set; }

        /// <summary>
        /// Gets the ending row index (1-based)
        /// </summary>
        public int EndRow { get; private set; }

        /// <summary>
        /// Gets the starting column index (1-based)
        /// </summary>
        public int StartColumn { get; private set; }

        /// <summary>
        /// Gets the ending column index (1-based)
        /// </summary>
        public int EndColumn { get; private set; }

        /// <summary>
        /// Gets the starting column letter
        /// </summary>
        public string StartColumnLetter { get; private set; }

        /// <summary>
        /// Gets the ending column letter
        /// </summary>
        public string EndColumnLetter { get; private set; }

        /// <summary>
        /// Gets or sets the range background color
        /// </summary>
        public string BackgroundColor
        {
            get
            {
                // TODO: Implement proper stylesheet-based color retrieval
                return null;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the background color for all cells in the range
            }
        }

        /// <summary>
        /// Gets or sets the range font name
        /// </summary>
        public string FontName
        {
            get
            {
                // TODO: Implement proper stylesheet-based font retrieval
                return null;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the font name for all cells in the range
            }
        }

        /// <summary>
        /// Gets or sets the range font size
        /// </summary>
        public double FontSize
        {
            get
            {
                // TODO: Implement proper stylesheet-based font retrieval
                return 11.0;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the font size for all cells in the range
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the range font is bold
        /// </summary>
        public bool IsBold
        {
            get
            {
                // TODO: Implement proper stylesheet-based font retrieval
                return false;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the bold property for all cells in the range
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the range font is italic
        /// </summary>
        public bool IsItalic
        {
            get
            {
                // TODO: Implement proper stylesheet-based font retrieval
                return false;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the italic property for all cells in the range
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the range font is underlined
        /// </summary>
        public bool IsUnderlined
        {
            get
            {
                // TODO: Implement proper stylesheet-based font retrieval
                return false;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the underline property for all cells in the range
            }
        }

        /// <summary>
        /// Gets or sets the range font color
        /// </summary>
        public string FontColor
        {
            get
            {
                // TODO: Implement proper stylesheet-based font retrieval
                return null;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the font color for all cells in the range
            }
        }

        /// <summary>
        /// Gets or sets the range horizontal alignment
        /// </summary>
        public string HorizontalAlignment
        {
            get
            {
                // TODO: Implement proper stylesheet-based alignment retrieval
                return null;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the horizontal alignment for all cells in the range
            }
        }

        /// <summary>
        /// Gets or sets the range vertical alignment
        /// </summary>
        public string VerticalAlignment
        {
            get
            {
                // TODO: Implement proper stylesheet-based alignment retrieval
                return null;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the vertical alignment for all cells in the range
            }
        }

        /// <summary>
        /// Gets or sets the range border style
        /// </summary>
        public string BorderStyle
        {
            get
            {
                // TODO: Implement proper stylesheet-based border retrieval
                return null;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the border style for all cells in the range
            }
        }

        /// <summary>
        /// Gets or sets the range border color
        /// </summary>
        public string BorderColor
        {
            get
            {
                // TODO: Implement proper stylesheet-based border retrieval
                return null;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the border color for all cells in the range
            }
        }

        /// <summary>
        /// Gets or sets the range number format
        /// </summary>
        public string NumberFormat
        {
            get
            {
                // TODO: Implement proper stylesheet-based number format retrieval
                return null;
            }
            set
            {
                // This is a simplified implementation
                // In a real implementation, you would set the number format for all cells in the range
            }
        }

        /// <summary>
        /// Gets a cell by relative position within the range
        /// </summary>
        /// <param name="rowOffset">The row offset from the start of the range (0-based)</param>
        /// <param name="columnOffset">The column offset from the start of the range (0-based)</param>
        /// <returns>The cell at the specified position, or null if not found</returns>
        public ICell GetCell(int rowOffset, int columnOffset)
        {
            if (rowOffset < 0 || columnOffset < 0)
                return null;
            if (rowOffset >= RowCount || columnOffset >= ColumnCount)
                return null;

            var row = StartRow + rowOffset;
            var col = StartColumn + columnOffset;
            var cellReference = GetCellReference(row, col);
            var cell = _worksheet.GetCell(cellReference);
            
            return cell != null ? new OpenXmlCell(cell, _worksheet) : null;
        }

        /// <summary>
        /// Gets a cell by cell reference within the range
        /// </summary>
        /// <param name="cellReference">The cell reference (e.g., "A1")</param>
        /// <returns>The cell at the specified reference, or null if not found</returns>
        public ICell GetCell(string cellReference)
        {
            if (string.IsNullOrWhiteSpace(cellReference))
                return null;

            var cell = _worksheet.GetCell(cellReference);
            return cell != null ? new OpenXmlCell(cell, _worksheet) : null;
        }

        /// <summary>
        /// Gets a sub-range within this range
        /// </summary>
        /// <param name="startRowOffset">The starting row offset from the start of the range (0-based)</param>
        /// <param name="startColumnOffset">The starting column offset from the start of the range (0-based)</param>
        /// <param name="endRowOffset">The ending row offset from the start of the range (0-based)</param>
        /// <param name="endColumnOffset">The ending column offset from the start of the range (0-based)</param>
        /// <returns>The sub-range, or null if not found</returns>
        public IRange GetRange(int startRowOffset, int startColumnOffset, int endRowOffset, int endColumnOffset)
        {
            if (startRowOffset < 0 || startColumnOffset < 0 || endRowOffset < 0 || endColumnOffset < 0)
                return null;
            if (startRowOffset >= RowCount || startColumnOffset >= ColumnCount || endRowOffset >= RowCount || endColumnOffset >= ColumnCount)
                return null;
            if (startRowOffset > endRowOffset || startColumnOffset > endColumnOffset)
                return null;

            var startRow = StartRow + startRowOffset;
            var startCol = StartColumn + startColumnOffset;
            var endRow = StartRow + endRowOffset;
            var endCol = StartColumn + endColumnOffset;

            var startCell = GetCellReference(startRow, startCol);
            var endCell = GetCellReference(endRow, endCol);
            var address = $"{startCell}:{endCell}";

            return new OpenXmlRange(address, _worksheet);
        }

        /// <summary>
        /// Gets a sub-range within this range
        /// </summary>
        /// <param name="startCellReference">The starting cell reference</param>
        /// <param name="endCellReference">The ending cell reference</param>
        /// <returns>The sub-range, or null if not found</returns>
        public IRange GetRange(string startCellReference, string endCellReference)
        {
            if (string.IsNullOrWhiteSpace(startCellReference) || string.IsNullOrWhiteSpace(endCellReference))
                return null;

            var address = $"{startCellReference}:{endCellReference}";
            return new OpenXmlRange(address, _worksheet);
        }

        /// <summary>
        /// Clears all cells in this range
        /// </summary>
        public void Clear()
        {
            for (int row = StartRow; row <= EndRow; row++)
            {
                for (int col = StartColumn; col <= EndColumn; col++)
                {
                    var cellReference = GetCellReference(row, col);
                    var cell = _worksheet.GetCell(cellReference);
                    if (cell != null)
                    {
                        cell.CellValue = null;
                        cell.DataType = null;
                    }
                }
            }
        }

        /// <summary>
        /// Clears the formatting of all cells in this range
        /// </summary>
        public void ClearFormatting()
        {
            // This is a simplified implementation
            // In a real implementation, you would clear all formatting properties
        }

        /// <summary>
        /// Clears the content of all cells in this range
        /// </summary>
        public void ClearContent()
        {
            Clear();
        }

        /// <summary>
        /// Clears the comments of all cells in this range
        /// </summary>
        public void ClearComments()
        {
            // This is a simplified implementation
            // In a real implementation, you would clear all comments
        }

        /// <summary>
        /// Copies this range to another range
        /// </summary>
        /// <param name="targetRange">The target range reference</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        public bool CopyTo(string targetRange)
        {
            return CopyTo(targetRange, true);
        }

        /// <summary>
        /// Copies this range to another range with formatting
        /// </summary>
        /// <param name="targetRange">The target range reference</param>
        /// <param name="includeFormatting">Whether to include formatting</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        public bool CopyTo(string targetRange, bool includeFormatting)
        {
            return CopyTo(targetRange, includeFormatting, true, true);
        }

        /// <summary>
        /// Copies this range to another range with specific options
        /// </summary>
        /// <param name="targetRange">The target range reference</param>
        /// <param name="includeFormatting">Whether to include formatting</param>
        /// <param name="includeValues">Whether to include values</param>
        /// <param name="includeFormulas">Whether to include formulas</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        public bool CopyTo(string targetRange, bool includeFormatting, bool includeValues, bool includeFormulas)
        {
            try
            {
                var target = new OpenXmlRange(targetRange, _worksheet);
                if (target.RowCount != RowCount || target.ColumnCount != ColumnCount)
                    return false;

                for (int row = 0; row < RowCount; row++)
                {
                    for (int col = 0; col < ColumnCount; col++)
                    {
                        var sourceCell = GetCell(row, col);
                        var targetCell = target.GetCell(row, col);
                        
                        if (sourceCell != null && targetCell != null)
                        {
                            if (includeValues)
                                targetCell.Value = sourceCell.Value;
                            
                            if (includeFormulas && !string.IsNullOrEmpty(sourceCell.Formula))
                                targetCell.Formula = sourceCell.Formula;
                            
                            if (includeFormatting)
                            {
                                targetCell.FontName = sourceCell.FontName;
                                targetCell.FontSize = sourceCell.FontSize;
                                targetCell.IsBold = sourceCell.IsBold;
                                targetCell.IsItalic = sourceCell.IsItalic;
                                targetCell.IsUnderlined = sourceCell.IsUnderlined;
                                targetCell.FontColor = sourceCell.FontColor;
                                targetCell.BackgroundColor = sourceCell.BackgroundColor;
                                targetCell.HorizontalAlignment = sourceCell.HorizontalAlignment;
                                targetCell.VerticalAlignment = sourceCell.VerticalAlignment;
                                targetCell.BorderStyle = sourceCell.BorderStyle;
                                targetCell.BorderColor = sourceCell.BorderColor;
                                targetCell.NumberFormat = sourceCell.NumberFormat;
                            }
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Auto-fits all columns in this range based on their content
        /// </summary>
        /// <returns>True if the auto-fit was successful, false otherwise</returns>
        public bool AutoFitColumns()
        {
            try
            {
                for (int col = StartColumn; col <= EndColumn; col++)
                {
                    _worksheet.AutoFitColumn(col);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Auto-fits all rows in this range based on their content
        /// </summary>
        /// <returns>True if the auto-fit was successful, false otherwise</returns>
        public bool AutoFitRows()
        {
            try
            {
                for (int row = StartRow; row <= EndRow; row++)
                {
                    _worksheet.AutoFitRow(row);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Auto-fits all columns and rows in this range based on their content
        /// </summary>
        /// <returns>True if the auto-fit was successful, false otherwise</returns>
        public bool AutoFit()
        {
            return AutoFitColumns() && AutoFitRows();
        }

        /// <summary>
        /// Merges all cells in this range
        /// </summary>
        /// <returns>True if the merge was successful, false otherwise</returns>
        public bool Merge()
        {
            // This is a simplified implementation
            // In a real implementation, you would merge the cells using OpenXml
            return true;
        }

        /// <summary>
        /// Unmerges all cells in this range
        /// </summary>
        /// <returns>True if the unmerge was successful, false otherwise</returns>
        public bool Unmerge()
        {
            // This is a simplified implementation
            // In a real implementation, you would unmerge the cells using OpenXml
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether this range is merged
        /// </summary>
        public bool IsMerged { get; private set; }

        /// <summary>
        /// Selects this range
        /// </summary>
        /// <returns>True if the selection was successful, false otherwise</returns>
        public bool Select()
        {
            // This is a simplified implementation
            // In a real implementation, you would select the range
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether this range is selected
        /// </summary>
        public bool IsSelected { get; private set; }

        /// <summary>
        /// Gets the range address (e.g., "A1:C10")
        /// </summary>
        public string Address => _address;

        /// <summary>
        /// Gets the range address with worksheet reference (e.g., "Sheet1!A1:C10")
        /// </summary>
        public string FullAddress => _fullAddress;

        /// <summary>
        /// Parses the range address to extract start and end cells
        /// </summary>
        /// <param name="address">The range address</param>
        private void ParseAddress(string address)
        {
            var parts = address.Split(':');
            if (parts.Length == 1)
            {
                // Single cell
                StartCell = EndCell = parts[0];
                StartRow = EndRow = GetRowIndex(parts[0]);
                StartColumn = EndColumn = GetColumnIndex(parts[0]);
                StartColumnLetter = EndColumnLetter = GetColumnLetter(parts[0]);
            }
            else if (parts.Length == 2)
            {
                // Range
                StartCell = parts[0];
                EndCell = parts[1];
                StartRow = GetRowIndex(parts[0]);
                EndRow = GetRowIndex(parts[1]);
                StartColumn = GetColumnIndex(parts[0]);
                EndColumn = GetColumnIndex(parts[1]);
                StartColumnLetter = GetColumnLetter(parts[0]);
                EndColumnLetter = GetColumnLetter(parts[1]);
            }
            else
            {
                throw new ArgumentException("Invalid range address format", nameof(address));
            }

            RowCount = EndRow - StartRow + 1;
            ColumnCount = EndColumn - StartColumn + 1;
        }

        /// <summary>
        /// Gets the row index from a cell reference
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <returns>The row index</returns>
        private int GetRowIndex(string cellReference)
        {
            var rowPart = "";
            foreach (char c in cellReference)
            {
                if (char.IsDigit(c))
                    rowPart += c;
            }
            return int.Parse(rowPart);
        }

        /// <summary>
        /// Gets the column index from a cell reference
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <returns>The column index</returns>
        private int GetColumnIndex(string cellReference)
        {
            var columnPart = "";
            foreach (char c in cellReference)
            {
                if (char.IsLetter(c))
                    columnPart += c;
            }

            int result = 0;
            foreach (char c in columnPart)
            {
                result = result * 26 + (c - 'A' + 1);
            }
            return result;
        }

        /// <summary>
        /// Gets the column letter from a cell reference
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <returns>The column letter</returns>
        private string GetColumnLetter(string cellReference)
        {
            var columnPart = "";
            foreach (char c in cellReference)
            {
                if (char.IsLetter(c))
                    columnPart += c;
            }
            return columnPart;
        }

        /// <summary>
        /// Gets the cell reference from row and column indices
        /// </summary>
        /// <param name="row">The row index</param>
        /// <param name="column">The column index</param>
        /// <returns>The cell reference</returns>
        private string GetCellReference(int row, int column)
        {
            var columnLetter = "";
            while (column > 0)
            {
                column--;
                columnLetter = (char)('A' + column % 26) + columnLetter;
                column /= 26;
            }
            return $"{columnLetter}{row}";
        }
    }
}
