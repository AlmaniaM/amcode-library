using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx;

namespace AMCode.Documents.Xlsx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of the worksheet interface
    /// Wraps DocumentFormat.OpenXml.Spreadsheet.Worksheet for Excel operations
    /// </summary>
    public class OpenXmlWorksheet : IWorksheet
    {
        private readonly WorksheetPart _worksheetPart;
        private readonly Worksheet _worksheet;
        private readonly SheetData _sheetData;
        private string _name;
        private int _index;
        private bool _isVisible = true;

        /// <summary>
        /// Initializes a new instance of the OpenXmlWorksheet class
        /// </summary>
        /// <param name="worksheetPart">The worksheet part</param>
        /// <param name="name">The worksheet name</param>
        /// <param name="index">The worksheet index</param>
        /// <exception cref="ArgumentNullException">Thrown when worksheetPart is null</exception>
        public OpenXmlWorksheet(WorksheetPart worksheetPart, string name, int index)
        {
            _worksheetPart = worksheetPart ?? throw new ArgumentNullException(nameof(worksheetPart));
            _worksheet = _worksheetPart.Worksheet ?? throw new InvalidOperationException("Worksheet is null");
            _sheetData = _worksheet.GetFirstChild<SheetData>() ?? new SheetData();
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _index = index;
        }

        /// <summary>
        /// Gets the name of the worksheet
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the index of the worksheet in the workbook
        /// </summary>
        public int Index => _index;

        /// <summary>
        /// Gets a value indicating whether the worksheet is visible
        /// </summary>
        public bool IsVisible => _isVisible;

        /// <summary>
        /// Gets the collection of cells in this worksheet
        /// </summary>
        public IEnumerable<ICell> Cells
        {
            get
            {
                var cells = new List<ICell>();
                foreach (var row in _sheetData.Elements<Row>())
                {
                    foreach (var cell in row.Elements<Cell>())
                    {
                        cells.Add(new OpenXmlCell(cell, this));
                    }
                }
                return cells;
            }
        }

        /// <summary>
        /// Gets the collection of rows in this worksheet
        /// </summary>
        public IEnumerable<IRow> Rows
        {
            get
            {
                var rows = new List<IRow>();
                foreach (var row in _sheetData.Elements<Row>())
                {
                    rows.Add(new OpenXmlRow(row, this));
                }
                return rows;
            }
        }

        /// <summary>
        /// Gets the collection of columns in this worksheet
        /// </summary>
        public IEnumerable<IColumn> Columns
        {
            get
            {
                var columns = new List<IColumn>();
                var columnElements = _worksheet.Elements<Columns>().FirstOrDefault()?.Elements<Column>();
                if (columnElements != null)
                {
                    foreach (var column in columnElements)
                    {
                        columns.Add(new OpenXmlColumn(column, this));
                    }
                }
                return columns;
            }
        }

        /// <summary>
        /// Gets the used range of this worksheet
        /// </summary>
        public IRange UsedRange
        {
            get
            {
                var rows = _sheetData.Elements<Row>().ToList();
                if (!rows.Any())
                    return new OpenXmlRange("A1:A1", this);

                var minRow = (int)(rows.Min(r => r.RowIndex?.Value ?? 1));
                var maxRow = (int)(rows.Max(r => r.RowIndex?.Value ?? 1));
                var minColumn = 1;
                var maxColumn = 1;

                foreach (var row in rows)
                {
                    var cells = row.Elements<Cell>().ToList();
                    if (cells.Any())
                    {
                        var rowMinColumn = cells.Min(c => GetColumnIndex(c.CellReference?.Value ?? "A1"));
                        var rowMaxColumn = cells.Max(c => GetColumnIndex(c.CellReference?.Value ?? "A1"));
                        minColumn = Math.Min(minColumn, rowMinColumn);
                        maxColumn = Math.Max(maxColumn, rowMaxColumn);
                    }
                }

                var startCell = GetCellReference(minRow, minColumn);
                var endCell = GetCellReference(maxRow, maxColumn);
                return new OpenXmlRange($"{startCell}:{endCell}", this);
            }
        }

        /// <summary>
        /// Gets a range by cell references
        /// </summary>
        /// <param name="startCell">The starting cell reference (e.g., "A1")</param>
        /// <param name="endCell">The ending cell reference (e.g., "C10")</param>
        /// <returns>Result containing the range or error information</returns>
        public Result<IRange> GetRange(string startCell, string endCell)
        {
            if (string.IsNullOrWhiteSpace(startCell))
                return Result<IRange>.Failure("Start cell reference cannot be null or empty");
            if (string.IsNullOrWhiteSpace(endCell))
                return Result<IRange>.Failure("End cell reference cannot be null or empty");

            try
            {
                var range = new OpenXmlRange($"{startCell}:{endCell}", this);
                return Result<IRange>.Success(range);
            }
            catch (Exception ex)
            {
                return Result<IRange>.Failure($"Error creating range '{startCell}:{endCell}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a range by row and column indices
        /// </summary>
        /// <param name="startRow">The starting row index (1-based)</param>
        /// <param name="startColumn">The starting column index (1-based)</param>
        /// <param name="endRow">The ending row index (1-based)</param>
        /// <param name="endColumn">The ending column index (1-based)</param>
        /// <returns>Result containing the range or error information</returns>
        public Result<IRange> GetRange(int startRow, int startColumn, int endRow, int endColumn)
        {
            if (startRow < 1 || startColumn < 1 || endRow < 1 || endColumn < 1)
                return Result<IRange>.Failure("Row and column indices must be 1-based and positive");
            if (startRow > endRow || startColumn > endColumn)
                return Result<IRange>.Failure("Start row/column must be less than or equal to end row/column");

            try
            {
                var startCell = GetCellReference(startRow, startColumn);
                var endCell = GetCellReference(endRow, endColumn);
                return GetRange(startCell, endCell);
            }
            catch (Exception ex)
            {
                return Result<IRange>.Failure($"Error creating range from indices: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sets the value of a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference (e.g., "A1")</param>
        /// <param name="value">The value to set</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SetCellValue(string cellReference, object value)
        {
            if (string.IsNullOrWhiteSpace(cellReference))
                return Result.Failure("Cell reference cannot be null or empty");

            try
            {
                var cell = GetOrCreateCell(cellReference);
                SetCellValue(cell, value);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error setting cell value for '{cellReference}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sets the value of a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (1-based)</param>
        /// <param name="column">The column index (1-based)</param>
        /// <param name="value">The value to set</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SetCellValue(int row, int column, object value)
        {
            if (row < 1 || column < 1)
                return Result.Failure("Row and column indices must be 1-based and positive");

            try
            {
                var cellReference = GetCellReference(row, column);
                return SetCellValue(cellReference, value);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error setting cell value for row {row}, column {column}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the value of a specific cell
        /// </summary>
        /// <param name="cellReference">The cell reference (e.g., "A1")</param>
        /// <returns>Result containing the cell value or error information</returns>
        public Result<object> GetCellValue(string cellReference)
        {
            if (string.IsNullOrWhiteSpace(cellReference))
                return Result<object>.Failure("Cell reference cannot be null or empty");

            try
            {
                var cell = GetCell(cellReference);
                if (cell == null)
                    return Result<object>.Success(null);

                return Result<object>.Success(GetCellValue(cell));
            }
            catch (Exception ex)
            {
                return Result<object>.Failure($"Error getting cell value for '{cellReference}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the value of a specific cell by row and column indices
        /// </summary>
        /// <param name="row">The row index (1-based)</param>
        /// <param name="column">The column index (1-based)</param>
        /// <returns>Result containing the cell value or error information</returns>
        public Result<object> GetCellValue(int row, int column)
        {
            if (row < 1 || column < 1)
                return Result<object>.Failure("Row and column indices must be 1-based and positive");

            try
            {
                var cellReference = GetCellReference(row, column);
                return GetCellValue(cellReference);
            }
            catch (Exception ex)
            {
                return Result<object>.Failure($"Error getting cell value for row {row}, column {column}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Clears all content from the worksheet
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result Clear()
        {
            try
            {
                _sheetData.RemoveAllChildren<Row>();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error clearing worksheet: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Clears a specific range of cells
        /// </summary>
        /// <param name="startCell">The starting cell reference</param>
        /// <param name="endCell">The ending cell reference</param>
        /// <returns>Result indicating success or failure</returns>
        public Result ClearRange(string startCell, string endCell)
        {
            if (string.IsNullOrWhiteSpace(startCell))
                return Result.Failure("Start cell reference cannot be null or empty");
            if (string.IsNullOrWhiteSpace(endCell))
                return Result.Failure("End cell reference cannot be null or empty");

            try
            {
                var range = GetRange(startCell, endCell);
                if (!range.IsSuccess)
                    return Result.Failure(range.Error);

                range.Value.Clear();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error clearing range '{startCell}:{endCell}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Clears a specific range of cells by row and column indices
        /// </summary>
        /// <param name="startRow">The starting row index (1-based)</param>
        /// <param name="startColumn">The starting column index (1-based)</param>
        /// <param name="endRow">The ending row index (1-based)</param>
        /// <param name="endColumn">The ending column index (1-based)</param>
        /// <returns>Result indicating success or failure</returns>
        public Result ClearRange(int startRow, int startColumn, int endRow, int endColumn)
        {
            if (startRow < 1 || startColumn < 1 || endRow < 1 || endColumn < 1)
                return Result.Failure("Row and column indices must be 1-based and positive");
            if (startRow > endRow || startColumn > endColumn)
                return Result.Failure("Start row/column must be less than or equal to end row/column");

            try
            {
                var startCell = GetCellReference(startRow, startColumn);
                var endCell = GetCellReference(endRow, endColumn);
                return ClearRange(startCell, endCell);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error clearing range from indices: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sets the width of a specific column
        /// </summary>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <param name="width">The width in points</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SetColumnWidth(int columnIndex, double width)
        {
            if (columnIndex < 1)
                return Result.Failure("Column index must be 1-based and positive");
            if (width < 0)
                return Result.Failure("Column width cannot be negative");

            try
            {
                var columns = _worksheet.GetFirstChild<Columns>();
                if (columns == null)
                {
                    columns = new Columns();
                    _worksheet.InsertBefore(columns, _sheetData);
                }

                var column = columns.Elements<Column>().FirstOrDefault(c => c.Min == (uint)columnIndex && c.Max == (uint)columnIndex);
                if (column == null)
                {
                    column = new Column { Min = (uint)columnIndex, Max = (uint)columnIndex };
                    columns.Append(column);
                }

                column.Width = width;
                column.CustomWidth = true;

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error setting column width for column {columnIndex}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sets the width of a specific column by letter
        /// </summary>
        /// <param name="columnLetter">The column letter (e.g., "A", "B", "AA")</param>
        /// <param name="width">The width in points</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SetColumnWidth(string columnLetter, double width)
        {
            if (string.IsNullOrWhiteSpace(columnLetter))
                return Result.Failure("Column letter cannot be null or empty");

            try
            {
                var columnIndex = GetColumnIndex(columnLetter);
                return SetColumnWidth(columnIndex, width);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error setting column width for column '{columnLetter}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sets the height of a specific row
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <param name="height">The height in points</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SetRowHeight(int rowIndex, double height)
        {
            if (rowIndex < 1)
                return Result.Failure("Row index must be 1-based and positive");
            if (height < 0)
                return Result.Failure("Row height cannot be negative");

            try
            {
                var row = GetOrCreateRow((uint)rowIndex);
                row.Height = height;
                row.CustomHeight = true;

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error setting row height for row {rowIndex}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the width of a specific column
        /// </summary>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <returns>Result containing the column width or error information</returns>
        public Result<double> GetColumnWidth(int columnIndex)
        {
            if (columnIndex < 1)
                return Result<double>.Failure("Column index must be 1-based and positive");

            try
            {
                var columns = _worksheet.GetFirstChild<Columns>();
                var column = columns?.Elements<Column>().FirstOrDefault(c => c.Min <= (uint)columnIndex && c.Max >= (uint)columnIndex);
                
                if (column?.Width != null)
                    return Result<double>.Success(column.Width.Value);
                
                return Result<double>.Success(8.43); // Default column width
            }
            catch (Exception ex)
            {
                return Result<double>.Failure($"Error getting column width for column {columnIndex}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the width of a specific column by letter
        /// </summary>
        /// <param name="columnLetter">The column letter (e.g., "A", "B", "AA")</param>
        /// <returns>Result containing the column width or error information</returns>
        public Result<double> GetColumnWidth(string columnLetter)
        {
            if (string.IsNullOrWhiteSpace(columnLetter))
                return Result<double>.Failure("Column letter cannot be null or empty");

            try
            {
                var columnIndex = GetColumnIndex(columnLetter);
                return GetColumnWidth(columnIndex);
            }
            catch (Exception ex)
            {
                return Result<double>.Failure($"Error getting column width for column '{columnLetter}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the height of a specific row
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <returns>Result containing the row height or error information</returns>
        public Result<double> GetRowHeight(int rowIndex)
        {
            if (rowIndex < 1)
                return Result<double>.Failure("Row index must be 1-based and positive");

            try
            {
                var row = _sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
                
                if (row?.Height != null)
                    return Result<double>.Success(row.Height.Value);
                
                return Result<double>.Success(15); // Default row height
            }
            catch (Exception ex)
            {
                return Result<double>.Failure($"Error getting row height for row {rowIndex}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Automatically fits the width of a specific column based on its content
        /// </summary>
        /// <param name="columnIndex">The column index (1-based)</param>
        /// <returns>Result indicating success or failure</returns>
        public Result AutoFitColumn(int columnIndex)
        {
            if (columnIndex < 1)
                return Result.Failure("Column index must be 1-based and positive");

            try
            {
                // This is a simplified implementation
                // In a real implementation, you would calculate the actual content width
                var width = 10.0; // Placeholder width
                return SetColumnWidth(columnIndex, width);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error auto-fitting column {columnIndex}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Automatically fits the width of a specific column by letter based on its content
        /// </summary>
        /// <param name="columnLetter">The column letter (e.g., "A", "B", "AA")</param>
        /// <returns>Result indicating success or failure</returns>
        public Result AutoFitColumn(string columnLetter)
        {
            if (string.IsNullOrWhiteSpace(columnLetter))
                return Result.Failure("Column letter cannot be null or empty");

            try
            {
                var columnIndex = GetColumnIndex(columnLetter);
                return AutoFitColumn(columnIndex);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error auto-fitting column '{columnLetter}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Automatically fits the height of a specific row based on its content
        /// </summary>
        /// <param name="rowIndex">The row index (1-based)</param>
        /// <returns>Result indicating success or failure</returns>
        public Result AutoFitRow(int rowIndex)
        {
            if (rowIndex < 1)
                return Result.Failure("Row index must be 1-based and positive");

            try
            {
                // This is a simplified implementation
                // In a real implementation, you would calculate the actual content height
                var height = 15.0; // Placeholder height
                return SetRowHeight(rowIndex, height);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error auto-fitting row {rowIndex}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Automatically fits all columns in the worksheet based on their content
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result AutoFitAllColumns()
        {
            try
            {
                var usedRange = UsedRange;
                for (int col = usedRange.StartColumn; col <= usedRange.EndColumn; col++)
                {
                    AutoFitColumn(col);
                }
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error auto-fitting all columns: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Automatically fits all rows in the worksheet based on their content
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result AutoFitAllRows()
        {
            try
            {
                var usedRange = UsedRange;
                for (int row = usedRange.StartRow; row <= usedRange.EndRow; row++)
                {
                    AutoFitRow(row);
                }
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error auto-fitting all rows: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Automatically fits all columns and rows in the worksheet based on their content
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result AutoFitAll()
        {
            try
            {
                var columnsResult = AutoFitAllColumns();
                if (!columnsResult.IsSuccess)
                    return columnsResult;

                var rowsResult = AutoFitAllRows();
                if (!rowsResult.IsSuccess)
                    return rowsResult;

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error auto-fitting all: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Activates this worksheet (makes it the active worksheet)
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result Activate()
        {
            try
            {
                // This is a simplified implementation
                // In a real implementation, you would set the active sheet in the workbook view
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error activating worksheet: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deactivates this worksheet
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result Deactivate()
        {
            try
            {
                // This is a simplified implementation
                // In a real implementation, you would clear the active sheet in the workbook view
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error deactivating worksheet: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates the worksheet name
        /// </summary>
        /// <param name="name">The new name</param>
        internal void UpdateName(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Updates the worksheet index
        /// </summary>
        /// <param name="index">The new index</param>
        internal void UpdateIndex(int index)
        {
            _index = index;
        }

        /// <summary>
        /// Gets the worksheet part
        /// </summary>
        internal WorksheetPart WorksheetPart => _worksheetPart;

        /// <summary>
        /// Gets a cell by reference (internal method for OpenXmlRange)
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <returns>The cell or null if not found</returns>
        internal Cell GetCell(string cellReference)
        {
            var rowIndex = GetRowIndex(cellReference);
            var columnIndex = GetColumnIndex(cellReference);
            
            var row = _sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
            if (row == null)
                return null;

            return row.Elements<Cell>().FirstOrDefault(c => c.CellReference == cellReference);
        }

        /// <summary>
        /// Gets or creates a row by index (internal method for OpenXmlRow)
        /// </summary>
        /// <param name="rowIndex">The row index</param>
        /// <returns>The row</returns>
        internal Row GetOrCreateRow(uint rowIndex)
        {
            var row = _sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
            if (row == null)
            {
                row = new Row { RowIndex = rowIndex };
                _sheetData.Append(row);
            }
            return row;
        }

        /// <summary>
        /// Gets or creates a cell by reference
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <returns>The cell</returns>
        private Cell GetOrCreateCell(string cellReference)
        {
            var rowIndex = GetRowIndex(cellReference);
            var row = GetOrCreateRow(rowIndex);

            var cell = row.Elements<Cell>().FirstOrDefault(c => c.CellReference == cellReference);
            if (cell == null)
            {
                cell = new Cell { CellReference = cellReference };
                row.Append(cell);
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
        /// Gets the value of a cell
        /// </summary>
        /// <param name="cell">The cell</param>
        /// <returns>The cell value</returns>
        private object GetCellValue(Cell cell)
        {
            if (cell.CellValue == null)
                return null;

            var value = cell.CellValue.Text;
            if (string.IsNullOrEmpty(value))
                return null;

            if (cell.DataType?.Value != null)
            {
                if (cell.DataType.Value == CellValues.Boolean)
                {
                    return value == "1" || value.ToLower() == "true";
                }
                else if (cell.DataType.Value == CellValues.Number)
                {
                    if (double.TryParse(value, out var number))
                        return number;
                    return value;
                }
                else if (cell.DataType.Value == CellValues.Date)
                {
                    if (double.TryParse(value, out var oaDate))
                        return DateTime.FromOADate(oaDate);
                    return value;
                }
            }
            return value;
        }

        /// <summary>
        /// Gets the row index from a cell reference
        /// </summary>
        /// <param name="cellReference">The cell reference</param>
        /// <returns>The row index</returns>
        private uint GetRowIndex(string cellReference)
        {
            var rowPart = "";
            foreach (char c in cellReference)
            {
                if (char.IsDigit(c))
                    rowPart += c;
            }
            return uint.Parse(rowPart);
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
