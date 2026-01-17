using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using AMCode.Documents.Xlsx;

namespace AMCode.Documents.Xlsx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of the column interface
    /// Represents a column in an Excel worksheet
    /// </summary>
    public class OpenXmlColumn : IColumn
    {
        private readonly Column _column;
        private readonly OpenXmlWorksheet _worksheet;

        /// <summary>
        /// Initializes a new instance of the OpenXmlColumn class
        /// </summary>
        /// <param name="column">The underlying OpenXml column</param>
        /// <param name="worksheet">The worksheet containing this column</param>
        /// <exception cref="ArgumentNullException">Thrown when column or worksheet is null</exception>
        public OpenXmlColumn(Column column, OpenXmlWorksheet worksheet)
        {
            _column = column ?? throw new ArgumentNullException(nameof(column));
            _worksheet = worksheet ?? throw new ArgumentNullException(nameof(worksheet));
        }

        /// <summary>
        /// Gets the column index (1-based)
        /// </summary>
        public int Index => (int)(_column.Min?.Value ?? 0);

        /// <summary>
        /// Gets the column letter (e.g., "A", "B", "AA")
        /// </summary>
        public string Letter
        {
            get
            {
                var index = Index;
                var columnLetter = "";
                while (index > 0)
                {
                    index--;
                    columnLetter = (char)('A' + index % 26) + columnLetter;
                    index /= 26;
                }
                return columnLetter;
            }
        }

        /// <summary>
        /// Gets the width of the column in points
        /// </summary>
        public double Width
        {
            get => _column.Width?.Value ?? 8.43;
            set
            {
                if (_column.Width == null)
                    _column.Width = new DoubleValue();
                _column.Width.Value = value;
                _column.CustomWidth = true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the column width is custom
        /// </summary>
        public bool HasCustomWidth => _column.CustomWidth?.Value ?? false;

        /// <summary>
        /// Gets a value indicating whether the column is hidden
        /// </summary>
        public bool IsHidden
        {
            get => _column.Hidden?.Value ?? false;
            set
            {
                if (_column.Hidden == null)
                    _column.Hidden = new BooleanValue();
                _column.Hidden.Value = value;
            }
        }

        /// <summary>
        /// Gets the collection of cells in this column
        /// </summary>
        public IEnumerable<ICell> Cells
        {
            get
            {
                var cells = new List<ICell>();
                var columnIndex = Index;
                
                // Get all rows in the worksheet
                var rows = _worksheet.WorksheetPart.Worksheet.GetFirstChild<SheetData>()?.Elements<Row>();
                if (rows != null)
                {
                    foreach (var row in rows)
                    {
                        var cell = row.Elements<Cell>().FirstOrDefault(c => 
                        {
                            var cellColumnIndex = GetColumnIndex(c.CellReference?.Value ?? "");
                            return cellColumnIndex == columnIndex;
                        });
                        
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
        /// Gets the number of cells in this column
        /// </summary>
        public int CellCount => Cells.Count();

        /// <summary>
        /// Gets a cell by row index
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <returns>The cell at the specified row, or null if not found</returns>
        public ICell GetCell(int rowIndex)
        {
            if (rowIndex < 1)
                return null;

            var cellReference = GetCellReference(rowIndex, Index);
            var row = _worksheet.WorksheetPart.Worksheet.GetFirstChild<SheetData>()?.Elements<Row>()
                .FirstOrDefault(r => r.RowIndex == rowIndex);
            
            if (row == null)
                return null;

            var cell = row.Elements<Cell>().FirstOrDefault(c => c.CellReference == cellReference);
            return cell != null ? new OpenXmlCell(cell, _worksheet) : null;
        }

        /// <summary>
        /// Sets the value of a cell in this column
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <param name="value">The value to set</param>
        /// <returns>True if the value was set successfully, false otherwise</returns>
        public bool SetCellValue(int rowIndex, object value)
        {
            if (rowIndex < 1)
                return false;

            try
            {
                var cellReference = GetCellReference(rowIndex, Index);
                var result = _worksheet.SetCellValue(cellReference, value);
                return result.IsSuccess;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the value of a cell in this column
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <returns>The cell value, or null if not found</returns>
        public object GetCellValue(int rowIndex)
        {
            var cell = GetCell(rowIndex);
            return cell?.Value;
        }

        /// <summary>
        /// Clears all cells in this column
        /// </summary>
        public void Clear()
        {
            var columnIndex = Index;
            var rows = _worksheet.WorksheetPart.Worksheet.GetFirstChild<SheetData>()?.Elements<Row>();
            if (rows != null)
            {
                foreach (var row in rows)
                {
                    var cell = row.Elements<Cell>().FirstOrDefault(c => 
                    {
                        var cellColumnIndex = GetColumnIndex(c.CellReference?.Value ?? "");
                        return cellColumnIndex == columnIndex;
                    });
                    
                    if (cell != null)
                    {
                        cell.CellValue = null;
                        cell.DataType = null;
                        cell.CellFormula = null;
                    }
                }
            }
        }

        /// <summary>
        /// Clears the formatting of all cells in this column
        /// </summary>
        public void ClearFormatting()
        {
            // This is a simplified implementation
            // In a real implementation, you would clear all formatting properties
        }

        /// <summary>
        /// Clears the content of all cells in this column
        /// </summary>
        public void ClearContent()
        {
            Clear();
        }

        /// <summary>
        /// Copies this column to another column
        /// </summary>
        /// <param name="targetColumnIndex">The target column index</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        public bool CopyTo(int targetColumnIndex)
        {
            return CopyTo(targetColumnIndex, true);
        }

        /// <summary>
        /// Copies this column to another column with formatting
        /// </summary>
        /// <param name="targetColumnIndex">The target column index</param>
        /// <param name="includeFormatting">Whether to include formatting</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        public bool CopyTo(int targetColumnIndex, bool includeFormatting)
        {
            try
            {
                var sourceColumnIndex = Index;
                var rows = _worksheet.WorksheetPart.Worksheet.GetFirstChild<SheetData>()?.Elements<Row>();
                if (rows == null)
                    return false;

                foreach (var row in rows)
                {
                    var sourceCell = row.Elements<Cell>().FirstOrDefault(c => 
                    {
                        var cellColumnIndex = GetColumnIndex(c.CellReference?.Value ?? "");
                        return cellColumnIndex == sourceColumnIndex;
                    });
                    
                    if (sourceCell != null)
                    {
                        var targetCellReference = GetCellReference((int)(row.RowIndex?.Value ?? 0), targetColumnIndex);
                        var targetCell = GetOrCreateCell(targetCellReference, row);

                        // Copy value
                        if (sourceCell.CellValue != null)
                        {
                            targetCell.CellValue = new CellValue(sourceCell.CellValue.Text);
                            targetCell.DataType = sourceCell.DataType;
                        }

                        // Copy formula
                        if (sourceCell.CellFormula != null)
                        {
                            targetCell.CellFormula = new CellFormula(sourceCell.CellFormula.Text);
                        }

                        // Copy formatting
                        if (includeFormatting)
                        {
                            // This is a simplified implementation
                            // In a real implementation, you would copy all formatting properties
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
        /// Auto-fits the width of this column based on its content
        /// </summary>
        /// <returns>True if the auto-fit was successful, false otherwise</returns>
        public bool AutoFit()
        {
            try
            {
                // This is a simplified implementation
                // In a real implementation, you would calculate the actual content width
                var width = 10.0; // Placeholder width
                Width = width;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or creates a cell in a row
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="row">The row</param>
        /// <returns>The cell</returns>
        private Cell GetOrCreateCell(string cellReference, Row row)
        {
            var cell = row.Elements<Cell>().FirstOrDefault(c => c.CellReference == cellReference);
            if (cell == null)
            {
                cell = new Cell { CellReference = cellReference };
                row.Append(cell);
            }
            return cell;
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

        // Stub implementations for interface members
        public string FontName { get; set; }
        public double FontSize { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderlined { get; set; }
        public string FontColor { get; set; }
        public string HorizontalAlignment { get; set; }
        public string VerticalAlignment { get; set; }
        public string BorderStyle { get; set; }
        public string BorderColor { get; set; }
        public string BackgroundColor { get; set; }
        public bool IsLocked { get; set; }

        public IRange GetRange(int startRow, int endRow)
        {
            throw new NotImplementedException("GetRange for column is not yet implemented");
        }

        public IColumn InsertLeft()
        {
            throw new NotImplementedException("InsertLeft is not yet implemented");
        }

        public IColumn InsertRight()
        {
            throw new NotImplementedException("InsertRight is not yet implemented");
        }

        public bool Delete()
        {
            throw new NotImplementedException("Delete column is not yet implemented");
        }
    }
}
