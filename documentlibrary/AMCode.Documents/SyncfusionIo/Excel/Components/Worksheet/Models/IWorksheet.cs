using System;
using System.IO;
using AMCode.Common.Xlsx;
using AMCode.SyncfusionIo.Xlsx.Common;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx
{
    /// <summary>
    /// An interface designed to represent an Excel sheet.
    /// </summary>
    public interface IWorksheet
    {
        /// <summary>
        /// Gets cell Range.
        /// </summary>
        /// <param name="address">The range code such as "A1:D20".</param>
        /// <returns>An <see cref="IRange"/> representing the provided range address.</returns>
        IRange this[string address] { get; }

        /// <summary>
        /// Gets or sets cell range by row and column index. Row and column indexes are one-based.
        /// </summary>
        /// <param name="row">One-based row index.</param>
        /// <param name="column">One-based column index.</param>
        /// <returns>An <see cref="IRange"/> representing a single cell from the provided row and column.</returns>
        IRange this[int row, int column] { get; }

        /// <summary>
        /// Get cell Range. Row and column indexes are one-based.
        /// </summary>
        /// <param name="row">First row index. One-based.</param>
        /// <param name="column">First column index. One-based.</param>
        /// <param name="lastRow">Last row index. One-based.</param>
        /// <param name="lastColumn">Last column index. One-based.</param>
        /// <returns>An <see cref="IRange"/> representing the provided row/column start to the row/column end.</returns>
        IRange this[int row, int column, int lastRow, int lastColumn] { get; }

        /// <summary>
        /// Gets the used cells in the worksheet.
        /// </summary>
        IRange[] Cells { get; }

        /// <summary>
        /// The name of the Sheet.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets a Range object that represents a cell or a range of cells in the worksheet.
        /// </summary>
        IRange Range { get; }

        /// <summary>
        /// Get an instance of a <see cref="IStyles"/> object.
        /// </summary>
        IStyles Styles { get; }

        /// <summary>
        /// Get an reference of the parent <see cref="IWorksheets"/> object.
        /// </summary>
        IWorksheets Worksheets { get; }

        /// <summary>
        /// Get the width of the specified column.
        /// </summary>
        /// <param name="columnIndex">One-based column index.</param>
        /// <returns>Width of the specified column.</returns>
        double GetColumnWidth(int columnIndex);

        /// <summary>
        /// Get the width of the specified column.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <returns>Width of the specified column.</returns>
        double? GetColumnWidth(string columnName);

        /// <summary>
        /// Returns the width of the specified column in pixels.
        /// </summary>
        /// <param name="columnIndex">One-based column index.</param>
        /// <returns>Width in pixels of the specified column.</returns>
        int GetColumnWidthInPixels(int columnIndex);

        /// <summary>
        /// Returns the width of the specified column in pixels.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <returns>Width in pixels of the specified column.</returns>
        int? GetColumnWidthInPixels(string columnName);

        /// <summary>
        /// Gets cell Range.
        /// </summary>
        /// <param name="address">The range code such as "A1:D20".</param>
        /// <returns>An <see cref="IRange"/> representing the provided range address.</returns>
        IRange GetRange(string address);

        /// <summary>
        /// Gets or sets cell range (single cell) by row and column index. Row and column indexes are one-based.
        /// </summary>
        /// <param name="row">One-based row index.</param>
        /// <param name="column">One-based column index.</param>
        /// <returns>An <see cref="IRange"/> representing a single cell from the provided row and column.</returns>
        IRange GetRange(int row, int column);

        /// <summary>
        /// Get cell Range. Row and column indexes are one-based.
        /// </summary>
        /// <param name="row">First row index. One-based.</param>
        /// <param name="column">First column index. One-based.</param>
        /// <param name="lastRow">Last row index. One-based.</param>
        /// <param name="lastColumn">Last column index. One-based.</param>
        /// <returns>An <see cref="IRange"/> representing the provided row/column start to the row/column end.</returns>
        IRange GetRange(int row, int column, int lastRow, int lastColumn);

        /// <summary>
        /// Removes worksheet from parent worksheet collection.
        /// </summary>
        void Remove();

        /// <summary>
        /// Saves worksheet as stream using separator. Used only for CSV files.
        /// </summary>
        /// <param name="stream">Stream to save the worksheet to.</param>
        /// <param name="delimiter">The delimiter to use.</param>
        void SaveAs(Stream stream, string delimiter);

        /// <summary>
        /// Sets a <see cref="bool"/> value for the specified cell.
        /// </summary>
        /// <param name="rowIndex">One-based row index.</param>
        /// <param name="columnIndex">One-based column index.</param>
        /// <param name="value">A <see cref="bool"/> value to set.</param>
        void SetBoolean(int rowIndex, int columnIndex, bool value);

        /// <summary>
        /// Sets a <see cref="ICell"/> as the value to the specified cell.
        /// </summary>
        /// <param name="cell">The <see cref="ICell"/> to set.</param>
        /// <exception cref="ArgumentException">Thrown when <see cref="ICell"/> or <see cref="ICell.CellValue"/> are null.</exception>
        void SetCell(ICell cell);

        /// <summary>
        /// Sets a <see cref="ICellValue"/> as the value to the specified cell.
        /// </summary>
        /// <param name="row">First row index. One-based.</param>
        /// <param name="column">First column index. One-based.</param>
        /// <param name="cell">The <see cref="ICellValue"/> to set.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when <paramref name="row"/> or <paramref name="column"/> are less than one.</exception>
        /// <exception cref="ArgumentException">Thrown when <see cref="ICellValue"/> is null.</exception>
        void SetCellValue(int row, int column, ICellValue cell);

        /// <summary>
        /// Set the number format text for a single column.
        /// </summary>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="numberFormat">The number format to set.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when <paramref name="columnIndex"/> is less than one.</exception>
        void SetColumnNumberFormat(int columnIndex, string numberFormat);

        /// <summary>
        /// Set the number format text for a single column.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="numberFormat">The number format to set.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="columnName"/> is null or empty.</exception>
        void SetColumnNumberFormat(string columnName, string numberFormat);

        /// <summary>
        /// Set column text horizontal alignment.
        /// </summary>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="hAlign">The <see cref="ExcelHAlign"/> to set.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when the <paramref name="columnIndex"/> value is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided <see cref="ExcelHAlign"/> could not be parsed.</exception>
        void SetColumnTextHAlignment(int columnIndex, ExcelHAlign hAlign);

        /// <summary>
        /// Set column text horizontal alignment.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="hAlign">The <see cref="ExcelHAlign"/> to set.</param>
        /// <exception cref="ArgumentException">Thrown when the provided <see cref="ExcelHAlign"/> could not be parsed.</exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="columnName"/> is null or empty.</exception>
        void SetColumnTextHAlignment(string columnName, ExcelHAlign hAlign);

        /// <summary>
        /// Set column style.
        /// </summary>
        /// <param name="columnIndex">The column index. One-based index.</param>
        /// <param name="styleParam">The <see cref="IStyleParam"/> object to set.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="styleParam"/> is null.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown when the <paramref name="columnIndex"/> value is less than or equal to zero.</exception>
        void SetColumnStyle(int columnIndex, IStyleParam styleParam);

        /// <summary>
        /// Set column style.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="styleParam">The <see cref="IStyleParam"/> object to set.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="styleParam"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="columnName"/> is null or empty.</exception>
        void SetColumnStyle(string columnName, IStyleParam styleParam);

        /// <summary>
        /// Sets column width for the specified column.
        /// </summary>
        /// <param name="columnIndex">One-based column index.</param>
        /// <param name="width">Width to set.</param>
        void SetColumnWidth(int columnIndex, double width);

        /// <summary>
        /// Sets column width for the specified column.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="width">Width to set.</param>
        void SetColumnWidth(string columnName, double width);

        /// <summary>
        /// Sets column width in pixels to the given column index.
        /// </summary>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="width">Width in pixel to set.</param>
        void SetColumnWidthInPixels(int columnIndex, int width);

        /// <summary>
        /// Sets column width in pixels to the given column name.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="width">Width in pixel to set.</param>
        void SetColumnWidthInPixels(string columnName, int width);

        /// <summary>
        /// Sets column width in pixels to the given number of columns from the specified column index.
        /// </summary>
        /// <param name="startColumnIndex">Start Column index.</param>
        /// <param name="count">Number of Column to be set width.</param>
        /// <param name="width">Width in pixel to set.</param>
        void SetColumnWidthInPixels(int startColumnIndex, int count, int width);

        /// <summary>
        /// Sets column width in pixels to the given number of columns from the specified column name.
        /// </summary>
        /// <param name="startColumnName">Start Column name.</param>
        /// <param name="count">Number of Column to be set width.</param>
        /// <param name="width">Width in pixel to set.</param>
        void SetColumnWidthInPixels(string startColumnName, int count, int width);

        /// <summary>
        /// Sets a <see cref="double"/> value for the specified cell.
        /// </summary>
        /// <param name="rowIndex">One-based row index.</param>
        /// <param name="columnIndex">One-based column index.</param>
        /// <param name="value">A <see cref="double"/> value to set.</param>
        void SetNumber(int rowIndex, int columnIndex, double value);

        /// <summary>
        /// Set the style for a range.
        /// </summary>
        /// <param name="address">The range code such as "A1:D20".</param>
        /// <param name="styleParam">The <see cref="IStyleParam"/> values to apply.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="address"/> is null, empty, or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="styleParam"/> is null.</exception>
        void SetRangeStyle(string address, IStyleParam styleParam);

        /// <summary>
        /// Set the style for a range.
        /// </summary>
        /// <param name="row">First row index. One-based.</param>
        /// <param name="column">First column index. One-based.</param>
        /// <param name="styleParam">The <see cref="IStyleParam"/> values to apply.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="styleParam"/> is null.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown when the <paramref name="row"/> or <paramref name="column"/> values are less than or equal to zero.</exception>
        void SetRangeStyle(int row, int column, IStyleParam styleParam);

        /// <summary>
        /// Set the style for a range.
        /// </summary>
        /// <param name="row">First row index. One-based.</param>
        /// <param name="column">First column index. One-based.</param>
        /// <param name="lastRow">Last row index. One-based.</param>
        /// <param name="lastColumn">Last column index. One-based.</param>
        /// <param name="styleParam">The <see cref="IStyleParam"/> values to apply.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="styleParam"/> is null.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown when the <paramref name="row"/>, <paramref name="column"/>, <paramref name="lastRow"/>, or <paramref name="lastColumn"/>
        /// are invalid index values.</exception>
        void SetRangeStyle(int row, int column, int lastRow, int lastColumn, IStyleParam styleParam);

        /// <summary>
        /// Sets value for the specified cell.
        /// </summary>
        /// <param name="rowIndex">One-based row index.</param>
        /// <param name="columnIndex">One-based column index.</param>
        /// <param name="value">Value to set.</param>
        void SetText(int rowIndex, int columnIndex, string value);

        /// <summary>
        /// Sets value for the specified cell.
        /// </summary>
        /// <param name="rowIndex">One-based row index.</param>
        /// <param name="columnIndex">One-based column index.</param>
        /// <param name="value">Value to set.</param>
        void SetValue(int rowIndex, int columnIndex, string value);

        /// <summary>
        /// Sets value for the specified cell.
        /// </summary>
        /// <param name="rowIndex">One-based row index.</param>
        /// <param name="columnIndex">One-based column index.</param>
        /// <param name="dataType">The <see cref="Type"/> of the <paramref name="value"/>.</param>
        /// <param name="value">Value to set.</param>
        void SetValue(int rowIndex, int columnIndex, Type dataType, object value);
    }

    internal interface IInternalWorksheet : IWorksheet
    {
        /// <summary>
        /// Get the underlying <see cref="Lib.IWorksheet"/> object.
        /// </summary>
        Lib.IWorksheet InnerLibWorksheet { get; }
    }
}