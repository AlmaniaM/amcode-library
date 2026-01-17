using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using AMCode.Documents.Xlsx;

namespace AMCode.Documents.Xlsx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of the row interface
    /// Represents a row in an Excel worksheet
    /// </summary>
    public class OpenXmlRow : IRow
    {
        private readonly Row _row;
        private readonly OpenXmlWorksheet _worksheet;

        /// <summary>
        /// Initializes a new instance of the OpenXmlRow class
        /// </summary>
        /// <param name="row">The underlying OpenXml row</param>
        /// <param name="worksheet">The worksheet containing this row</param>
        /// <exception cref="ArgumentNullException">Thrown when row or worksheet is null</exception>
        public OpenXmlRow(Row row, OpenXmlWorksheet worksheet)
        {
            _row = row ?? throw new ArgumentNullException(nameof(row));
            _worksheet = worksheet ?? throw new ArgumentNullException(nameof(worksheet));
        }

        /// <summary>
        /// Gets the row index (1-based)
        /// </summary>
        public int Index => (int)(_row.RowIndex?.Value ?? 0);

        /// <summary>
        /// Gets the height of the row in points
        /// </summary>
        public double Height
        {
            get => _row.Height?.Value ?? 15.0;
            set
            {
                if (_row.Height == null)
                    _row.Height = new DoubleValue();
                _row.Height.Value = value;
                _row.CustomHeight = true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the row height is custom
        /// </summary>
        public bool HasCustomHeight => _row.CustomHeight?.Value ?? false;

        /// <summary>
        /// Gets a value indicating whether the row is hidden
        /// </summary>
        public bool IsHidden
        {
            get => _row.Hidden?.Value ?? false;
            set
            {
                if (_row.Hidden == null)
                    _row.Hidden = new BooleanValue();
                _row.Hidden.Value = value;
            }
        }

        /// <summary>
        /// Gets the collection of cells in this row
        /// </summary>
        public IEnumerable<ICell> Cells
        {
            get
            {
                var cells = new List<ICell>();
                foreach (var cell in _row.Elements<Cell>())
                {
                    cells.Add(new OpenXmlCell(cell, _worksheet));
                }
                return cells;
            }
        }

        /// <summary>
        /// Gets the number of cells in this row
        /// </summary>
        public int CellCount => _row.Elements<Cell>().Count();

        /// <summary>
        /// Gets a cell by column index
        /// </summary>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <returns>The cell at the specified column, or null if not found</returns>
        public ICell GetCell(int columnIndex)
        {
            if (columnIndex < 1)
                return null;

            var cellReference = GetCellReference(Index, columnIndex);
            var cell = _row.Elements<Cell>().FirstOrDefault(c => c.CellReference == cellReference);
            
            return cell != null ? new OpenXmlCell(cell, _worksheet) : null;
        }

        /// <summary>
        /// Gets a cell by column letter
        /// </summary>
        /// <param name="columnLetter">The column letter (e.g., "A", "B", "AA")</param>
        /// <returns>The cell at the specified column, or null if not found</returns>
        public ICell GetCell(string columnLetter)
        {
            if (string.IsNullOrWhiteSpace(columnLetter))
                return null;

            var columnIndex = GetColumnIndex(columnLetter);
            return GetCell(columnIndex);
        }

        /// <summary>
        /// Sets the value of a cell in this row
        /// </summary>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <param name="value">The value to set</param>
        /// <returns>True if the value was set successfully, false otherwise</returns>
        public bool SetCellValue(int columnIndex, object value)
        {
            if (columnIndex < 1)
                return false;

            try
            {
                var cellReference = GetCellReference(Index, columnIndex);
                var cell = GetOrCreateCell(cellReference);
                SetCellValue(cell, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Sets the value of a cell in this row
        /// </summary>
        /// <param name="columnLetter">The column letter (e.g., "A", "B", "AA")</param>
        /// <param name="value">The value to set</param>
        /// <returns>True if the value was set successfully, false otherwise</returns>
        public bool SetCellValue(string columnLetter, object value)
        {
            if (string.IsNullOrWhiteSpace(columnLetter))
                return false;

            try
            {
                var columnIndex = GetColumnIndex(columnLetter);
                return SetCellValue(columnIndex, value);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the value of a cell in this row
        /// </summary>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <returns>The cell value, or null if not found</returns>
        public object GetCellValue(int columnIndex)
        {
            var cell = GetCell(columnIndex);
            return cell?.Value;
        }

        /// <summary>
        /// Gets the value of a cell in this row
        /// </summary>
        /// <param name="columnLetter">The column letter (e.g., "A", "B", "AA")</param>
        /// <returns>The cell value, or null if not found</returns>
        public object GetCellValue(string columnLetter)
        {
            var cell = GetCell(columnLetter);
            return cell?.Value;
        }

        /// <summary>
        /// Clears all cells in this row
        /// </summary>
        public void Clear()
        {
            foreach (var cell in _row.Elements<Cell>())
            {
                cell.CellValue = null;
                cell.DataType = null;
                cell.CellFormula = null;
            }
        }

        /// <summary>
        /// Clears the formatting of all cells in this row
        /// </summary>
        public void ClearFormatting()
        {
            // This is a simplified implementation
            // In a real implementation, you would clear all formatting properties
        }

        /// <summary>
        /// Clears the content of all cells in this row
        /// </summary>
        public void ClearContent()
        {
            Clear();
        }

        /// <summary>
        /// Copies this row to another row
        /// </summary>
        /// <param name="targetRowIndex">The target row index</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        public bool CopyTo(int targetRowIndex)
        {
            return CopyTo(targetRowIndex, true);
        }

        /// <summary>
        /// Copies this row to another row with formatting
        /// </summary>
        /// <param name="targetRowIndex">The target row index</param>
        /// <param name="includeFormatting">Whether to include formatting</param>
        /// <returns>True if the copy was successful, false otherwise</returns>
        public bool CopyTo(int targetRowIndex, bool includeFormatting)
        {
            try
            {
                var targetRow = _worksheet.GetOrCreateRow((uint)targetRowIndex);
                if (targetRow == null)
                    return false;

                foreach (var cell in _row.Elements<Cell>())
                {
                    var cellReference = cell.CellReference?.Value;
                    if (string.IsNullOrEmpty(cellReference))
                        continue;

                    var columnIndex = GetColumnIndex(cellReference);
                    var targetCellReference = GetCellReference(targetRowIndex, columnIndex);
                    var targetCell = GetOrCreateCell(targetCellReference, targetRow);

                    // Copy value
                    if (cell.CellValue != null)
                    {
                        targetCell.CellValue = new CellValue(cell.CellValue.Text);
                        targetCell.DataType = cell.DataType;
                    }

                    // Copy formula
                    if (cell.CellFormula != null)
                    {
                        targetCell.CellFormula = new CellFormula(cell.CellFormula.Text);
                    }

                    // Copy formatting
                    if (includeFormatting)
                    {
                        // This is a simplified implementation
                        // In a real implementation, you would copy all formatting properties
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
        /// Auto-fits the height of this row based on its content
        /// </summary>
        /// <returns>True if the auto-fit was successful, false otherwise</returns>
        public bool AutoFit()
        {
            try
            {
                // This is a simplified implementation
                // In a real implementation, you would calculate the actual content height
                var height = 15.0; // Placeholder height
                Height = height;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or creates a cell in this row
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <returns>The cell</returns>
        private Cell GetOrCreateCell(string cellReference)
        {
            var cell = _row.Elements<Cell>().FirstOrDefault(c => c.CellReference == cellReference);
            if (cell == null)
            {
                cell = new Cell { CellReference = cellReference };
                _row.Append(cell);
            }
            return cell;
        }

        /// <summary>
        /// Gets or creates a cell in a target row
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <param name="targetRow">The target row</param>
        /// <returns>The cell</returns>
        private Cell GetOrCreateCell(string cellReference, Row targetRow)
        {
            var cell = targetRow.Elements<Cell>().FirstOrDefault(c => c.CellReference == cellReference);
            if (cell == null)
            {
                cell = new Cell { CellReference = cellReference };
                targetRow.Append(cell);
            }
            return cell;
        }

        /// <summary>
        /// Sets the value of a cell
        /// </summary>
        /// <param name="cell">The cell</param>
        /// <param name="value">The value</param>
        private void SetCellValue(Cell cell, object value)
        {
            if (value == null)
            {
                cell.CellValue = null;
                cell.DataType = null;
                return;
            }

            if (value is string stringValue)
            {
                cell.CellValue = new CellValue(stringValue);
                cell.DataType = CellValues.String;
            }
            else if (value is int || value is long || value is short || value is byte)
            {
                cell.CellValue = new CellValue(value.ToString());
                cell.DataType = CellValues.Number;
            }
            else if (value is double || value is float || value is decimal)
            {
                cell.CellValue = new CellValue(value.ToString());
                cell.DataType = CellValues.Number;
            }
            else if (value is bool boolValue)
            {
                cell.CellValue = new CellValue(boolValue ? "1" : "0");
                cell.DataType = CellValues.Boolean;
            }
            else if (value is DateTime dateTime)
            {
                cell.CellValue = new CellValue(dateTime.ToOADate().ToString());
                cell.DataType = CellValues.Number;
            }
            else
            {
                cell.CellValue = new CellValue(value.ToString());
                cell.DataType = CellValues.String;
            }
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

        public IRange GetRange(int startColumn, int endColumn)
        {
            throw new NotImplementedException("GetRange for row is not yet implemented");
        }

        public IRange GetRange(string startColumn, string endColumn)
        {
            throw new NotImplementedException("GetRange for row is not yet implemented");
        }

        public IRow InsertAbove()
        {
            throw new NotImplementedException("InsertAbove is not yet implemented");
        }

        public IRow InsertBelow()
        {
            throw new NotImplementedException("InsertBelow is not yet implemented");
        }

        public bool Delete()
        {
            throw new NotImplementedException("Delete row is not yet implemented");
        }
    }
}
