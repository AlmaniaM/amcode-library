using System;
using AMCode.SyncfusionIo.Xlsx.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AMCode.Common.Util;
using AMCode.Common.Xlsx;
using AMCode.SyncfusionIo.Xlsx.Drawing;
using AMCode.SyncfusionIo.Xlsx.Extensions;
using AMCode.SyncfusionIo.Xlsx.Util;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx.Internal
{
    /// <summary>
    /// A class designed to represent an Excel sheet.
    /// </summary>
    public class Worksheet : IWorksheet, IInternalWorksheet
    {
        private readonly Lib.IMigrantRange migrantRange;

        /// <summary>
        /// Create an instance of the <see cref="Worksheet"/> class.
        /// </summary>
        /// <param name="internalWorksheets">An <see cref="IInternalWorksheets"/> object.</param>
        /// <param name="libWorksheet">A <see cref="Lib.IWorksheet"/> object.</param>
        internal Worksheet(IInternalWorksheets internalWorksheets, Lib.IWorksheet libWorksheet)
        {
            InnerLibWorksheet = libWorksheet;
            migrantRange = libWorksheet.MigrantRange;
            this.internalWorksheets = internalWorksheets;
        }

        /// <inheritdoc/>
        public IRange this[string address] => GetRange(address);

        /// <inheritdoc/>
        public IRange this[int row, int column] => GetRange(row, column);

        /// <inheritdoc/>
        public IRange this[int row, int column, int lastRow, int lastColumn]
            => GetRange(row, column, lastRow, lastColumn);

        /// <inheritdoc/>
        public IRange[] Cells => InnerLibWorksheet.Cells.Select(cell => new Range(cell)).ToArray();

        /// <inheritdoc/>
        public Lib.IWorksheet InnerLibWorksheet { get; }

        /// <inheritdoc/>
        IInternalWorksheets internalWorksheets { get; }

        /// <inheritdoc/>
        public string Name
        {
            get => InnerLibWorksheet.Name;
            set => InnerLibWorksheet.Name = value;
        }

        /// <inheritdoc/>
        public IRange Range => new Range(InnerLibWorksheet.Range);

        /// <inheritdoc/>
        public IStyles Styles => Worksheets.Styles;

        /// <inheritdoc/>
        public IWorksheets Worksheets => internalWorksheets;

        /// <summary>
        /// Find the index of the provided column name.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <returns>Index of >= 1 if found and -1 if not found.</returns>
        public int FindIndexOfColumnName(string columnName)
        {
            for (var columnIndex = 1; columnIndex <= InnerLibWorksheet.Columns.Length; columnIndex++)
            {
                var cellValue = GetRange(1, columnIndex).Text;

                if (cellValue.Equals(columnName))
                {
                    return columnIndex;
                }
            }

            return -1;
        }

        /// <inheritdoc/>
        public double GetColumnWidth(int columnIndex) => InnerLibWorksheet.GetColumnWidth(columnIndex);

        /// <inheritdoc/>
        public double? GetColumnWidth(string columnName)
        {
            var columnIndex = FindIndexOfColumnName(columnName);
            if (columnIndex >= 1)
            {
                return GetColumnWidth(columnIndex);
            }

            return null;
        }

        /// <inheritdoc/>
        public int GetColumnWidthInPixels(int columnIndex) => InnerLibWorksheet.GetColumnWidthInPixels(columnIndex);

        /// <inheritdoc/>
        public int? GetColumnWidthInPixels(string columnName)
        {
            var columnIndex = FindIndexOfColumnName(columnName);
            if (columnIndex >= 1)
            {
                return GetColumnWidthInPixels(columnIndex);
            }

            return null;
        }

        /// <inheritdoc/>
        public IRange GetRange(string address) => new Range(InnerLibWorksheet[address]);

        /// <inheritdoc/>
        public IRange GetRange(int row, int column) => new Range(InnerLibWorksheet[row, column]);

        /// <inheritdoc/>
        public IRange GetRange(int row, int column, int lastRow, int lastColumn) => new Range(InnerLibWorksheet[row, column, lastRow, lastColumn]);

        /// <inheritdoc/>
        public void Remove() => InnerLibWorksheet.Remove();

        /// <inheritdoc/>
        public void SaveAs(Stream stream, string delimiter) => InnerLibWorksheet.SaveAs(stream, delimiter);

        /// <inheritdoc/>
        public void SetBoolean(int rowIndex, int columnIndex, bool value)
        {
            migrantRange.ResetRowColumn(rowIndex, columnIndex);
            migrantRange.SetValue(value);
        }

        /// <inheritdoc/>
        public void SetCell(ICell cell)
        {
            string getHeader() => ExceptionUtil.CreateExceptionHeader<ICell>(SetCell);
            checkForNullParamAndThrowIfNull(cell, getHeader, nameof(cell));
            checkForNullParamAndThrowIfNull(cell.CellValue, getHeader, nameof(cell.CellValue));

            SetValue(cell.RowIndex, cell.ColumnIndex, cell.CellValue.ValueType, cell.CellValue.Value);
        }

        /// <summary>
        /// Sets a <see cref="ICellValue"/> as the value to the specified cell.
        /// </summary>
        /// <param name="row">First row index. One-based.</param>
        /// <param name="column">First column index. One-based.</param>
        /// <param name="cellValue">The <see cref="ICell"/> to set.</param>
        public void SetCellValue(int row, int column, ICellValue cellValue)
        {
            string getHeader() => ExceptionUtil.CreateExceptionHeader<int, int, ICellValue>(SetCellValue);
            checkForIndexLessThanOneAndThrowIfInvalid(row, getHeader, nameof(row));
            checkForIndexLessThanOneAndThrowIfInvalid(column, getHeader, nameof(column));
            checkForNullParamAndThrowIfNull(cellValue, getHeader, nameof(cellValue));

            SetValue(row, column, cellValue.ValueType, cellValue.Value);
        }

        /// <inheritdoc/>
        public void SetColumnNumberFormat(int columnIndex, string numberFormat)
        {
            checkForIndexLessThanOneAndThrowIfInvalid(columnIndex, () => ExceptionUtil.CreateExceptionHeader<int, string>(SetColumnNumberFormat), nameof(columnIndex));

            var style = getColumnStyle(columnIndex);
            style.NumberFormat = numberFormat;
            InnerLibWorksheet.SetDefaultColumnStyle(columnIndex, style.InnerLibStyle);
        }

        /// <inheritdoc/>
        public void SetColumnNumberFormat(string columnName, string numberFormat)
        {
            checkForNullOrEmptyStringAndThrowIfInvalid(columnName, () => ExceptionUtil.CreateExceptionHeader<string, string>(SetColumnNumberFormat), nameof(columnName));

            var columnIndex = FindIndexOfColumnName(columnName);
            if (columnIndex >= 1)
            {
                SetColumnNumberFormat(columnIndex, numberFormat);
            }
        }

        /// <inheritdoc/>
        public void SetColumnTextHAlignment(int columnIndex, ExcelHAlign hAlign)
        {
            string createExceptionHeader() => ExceptionUtil.CreateExceptionHeader<int, ExcelHAlign>(SetColumnTextHAlignment);
            checkForIndexLessThanOneAndThrowIfInvalid(columnIndex, () => createExceptionHeader(), nameof(columnIndex));
            checkIfExcelHAlignCannotBeParsedAndThrowIfInvalid(hAlign, () => createExceptionHeader());

            var style = getColumnStyle(columnIndex);
            style.HorizontalAlignment = hAlign;
            InnerLibWorksheet.SetDefaultColumnStyle(columnIndex, style.InnerLibStyle);
        }

        /// <inheritdoc/>
        public void SetColumnTextHAlignment(string columnName, ExcelHAlign hAlign)
        {
            string createExceptionHeader() => ExceptionUtil.CreateExceptionHeader<string, ExcelHAlign>(SetColumnTextHAlignment);
            checkForNullOrEmptyStringAndThrowIfInvalid(columnName, () => createExceptionHeader(), nameof(columnName));
            checkIfExcelHAlignCannotBeParsedAndThrowIfInvalid(hAlign, () => createExceptionHeader());

            var columnIndex = FindIndexOfColumnName(columnName);
            if (columnIndex >= 1)
            {
                SetColumnTextHAlignment(columnIndex, hAlign);
            }
        }

        /// <inheritdoc/>
        public void SetColumnStyle(int columnIndex, IStyleParam styleParam)
        {
            string getHeader() => ExceptionUtil.CreateExceptionHeader<int, IStyleParam>(SetColumnStyle);
            checkForIndexLessThanOneAndThrowIfInvalid(columnIndex, getHeader, nameof(columnIndex));
            checkForNullParamAndThrowIfNull(styleParam, getHeader, nameof(styleParam));

            var style = getColumnStyle(columnIndex);
            style = UpdateStyle(style, styleParam);

            InnerLibWorksheet.SetDefaultColumnStyle(columnIndex, style.InnerLibStyle);
        }

        /// <inheritdoc/>
        public void SetColumnStyle(string columnName, IStyleParam styleParam)
        {
            string getHeader() => ExceptionUtil.CreateExceptionHeader<string, IStyleParam>(SetColumnStyle);
            checkForNullOrEmptyStringAndThrowIfInvalid(columnName, getHeader, nameof(columnName));
            checkForNullParamAndThrowIfNull(styleParam, getHeader, nameof(styleParam));

            var columnIndex = FindIndexOfColumnName(columnName);
            if (columnIndex >= 1)
            {
                SetColumnStyle(columnIndex, styleParam);
            }
        }

        /// <inheritdoc/>
        public void SetColumnWidth(int columnIndex, double width) => InnerLibWorksheet.SetColumnWidth(columnIndex, width);

        /// <inheritdoc/>
        public void SetColumnWidth(string columnName, double width)
        {
            var columnIndex = FindIndexOfColumnName(columnName);
            if (columnIndex >= 1)
            {
                SetColumnWidth(columnIndex, width);
            }
        }

        /// <inheritdoc/>
        public void SetColumnWidthInPixels(int columnIndex, int width) => InnerLibWorksheet.SetColumnWidthInPixels(columnIndex, width);

        /// <inheritdoc/>
        public void SetColumnWidthInPixels(string columnName, int width)
        {
            var columnIndex = FindIndexOfColumnName(columnName);
            if (columnIndex >= 1)
            {
                SetColumnWidthInPixels(columnIndex, width);
            }
        }

        /// <inheritdoc/>
        public void SetColumnWidthInPixels(int startColumnIndex, int count, int width) => InnerLibWorksheet.SetColumnWidthInPixels(startColumnIndex, count, width);

        /// <inheritdoc/>
        public void SetColumnWidthInPixels(string startColumnName, int count, int width)
        {
            var columnIndex = FindIndexOfColumnName(startColumnName);
            if (columnIndex >= 1)
            {
                SetColumnWidthInPixels(columnIndex, count, width);
            }
        }

        /// <inheritdoc/>
        public void SetNumber(int rowIndex, int columnIndex, double value)
        {
            migrantRange.ResetRowColumn(rowIndex, columnIndex);
            migrantRange.SetValue(value);
        }

        /// <inheritdoc/>
        public void SetRangeStyle(string address, IStyleParam styleParam)
        {
            string getHeader() => ExceptionUtil.CreateExceptionHeader<string, IStyleParam>(SetRangeStyle);
            checkForNullEmptyOrWhiteSpaceAddressAndThrowIfInvalid(address, getHeader);
            checkForNullParamAndThrowIfNull(styleParam, getHeader, nameof(styleParam));

            UpdateStyle(GetRange(address).CellStyle, styleParam);
        }

        /// <inheritdoc/>
        public void SetRangeStyle(int row, int column, IStyleParam styleParam)
        {
            string getHeader() => ExceptionUtil.CreateExceptionHeader<int, int, IStyleParam>(SetRangeStyle);
            checkForIndexLessThanOneAndThrowIfInvalid(row, getHeader, nameof(row));
            checkForIndexLessThanOneAndThrowIfInvalid(column, getHeader, nameof(column));
            checkForNullParamAndThrowIfNull(styleParam, getHeader, nameof(styleParam));

            UpdateStyle(GetRange(row, column).CellStyle, styleParam);
        }

        /// <inheritdoc/>
        public void SetRangeStyle(int row, int column, int lastRow, int lastColumn, IStyleParam styleParam)
        {
            string getHeader() => ExceptionUtil.CreateExceptionHeader<int, int, IStyleParam>(SetRangeStyle);
            checkForIndexLessThanOneAndThrowIfInvalid(row, getHeader, nameof(row));
            checkForIndexLessThanOneAndThrowIfInvalid(column, getHeader, nameof(column));
            checkForIndexLessThanOneAndThrowIfInvalid(lastRow, getHeader, nameof(lastRow));
            checkForMaxIndexOutOfRangeAndThrowIfInvalid(lastRow, ExcelLimitValues.MaxRowCount, getHeader, nameof(lastRow));
            checkForIndexLessThanOneAndThrowIfInvalid(lastColumn, getHeader, nameof(lastColumn));
            checkForMaxIndexOutOfRangeAndThrowIfInvalid(lastColumn, ExcelLimitValues.MaxColumnCount, getHeader, nameof(lastColumn));
            checkForNullParamAndThrowIfNull(styleParam, getHeader, nameof(styleParam));

            UpdateStyle(GetRange(row, column, lastRow, lastColumn).CellStyle, styleParam);
        }

        /// <inheritdoc/>
        public void SetValue(int rowIndex, int columnIndex, Type dataType, object value)
        {
            migrantRange.ResetRowColumn(rowIndex, columnIndex);

            if (dataType == typeof(int))
            {
                migrantRange.SetValue(Convert.ToInt32(value));
            }
            else if (dataType == typeof(long) || dataType == typeof(double))
            {
                migrantRange.SetValue(Convert.ToDouble(value));
            }
            else if (dataType == typeof(string))
            {
                migrantRange.SetValue(Convert.ToString(value));
            }
            else if (dataType == typeof(bool))
            {
                migrantRange.SetValue(Convert.ToBoolean(value));
            }
            else if (dataType == typeof(DateTime))
            {
                migrantRange.SetValue(Convert.ToDateTime(value));
            }
            else
            {
                migrantRange.SetValue(value.ToString());
            }
        }

        /// <inheritdoc/>
        public void SetText(int rowIndex, int columnIndex, string value)
        {
            migrantRange.ResetRowColumn(rowIndex, columnIndex);
            migrantRange.SetValue(value);
        }

        /// <inheritdoc/>
        public void SetValue(int rowIndex, int columnIndex, string value)
        {
            migrantRange.ResetRowColumn(rowIndex, columnIndex);
            migrantRange.SetValue(value);
        }

        /// <summary>
        /// Updates a <see cref="IStyle"/> object with the values of a <see cref="IStyleParam"/> object.
        /// </summary>
        /// <typeparam name="T">An <see cref="IStyle"/> object.</typeparam>
        /// <param name="internalStyle">The <see cref="IInternalStyle"/> object to update.</param>
        /// <param name="styleParam">The <see cref="IStyleParam"/> values to use.</param>
        /// <returns>An updated <see cref="IStyle"/> object.</returns>
        public T UpdateStyle<T>(T internalStyle, IStyleParam styleParam)
            where T : IStyle
        {
            internalStyle.Color = styleParam.Color ?? internalStyle.Color;
            internalStyle.ColorIndex = styleParam.ColorIndex ?? internalStyle.ColorIndex;
            internalStyle.FillPattern = styleParam.FillPattern ?? internalStyle.FillPattern;
            internalStyle.HorizontalAlignment = styleParam.HorizontalAlignment ?? internalStyle.HorizontalAlignment;

            if (styleParam.NumberFormat != null)
            {
                internalStyle.NumberFormat = styleParam.NumberFormat ?? internalStyle.NumberFormat;
            }

            internalStyle.PatternColor = styleParam.PatternColor ?? internalStyle.PatternColor;
            internalStyle.Font.Bold = styleParam.Bold ?? internalStyle.Font.Bold;
            internalStyle.Font.Italic = styleParam.Italic ?? internalStyle.Font.Italic;
            internalStyle.Font.Color = styleParam.FontColor ?? internalStyle.Font.Color;
            internalStyle.Font.Size = styleParam.FontSize ?? internalStyle.Font.Size;

            var borderStyles = getBorderStyles(styleParam.BorderStyles, internalStyle.Borders);
            foreach (var borderStyleKeyValue in borderStyles)
            {
                var borderStyle = borderStyleKeyValue.Value;
                var cellBorderEdge = borderStyleKeyValue.Key;

                var border = internalStyle.Borders[cellBorderEdge];
                border.Color = borderStyle.Color;
                border.LineStyle = borderStyle.LineStyle;
                border.ShowDiagonalLine = borderStyle.ShowDiagonalLine;
            }

            return internalStyle;
        }

        /// <summary>
        /// Check to see if the provided index is less than or equal to zero.
        /// </summary>
        /// <param name="index">The index value to check.</param>
        /// <param name="getHeader">A function to get the exception header.</param>
        /// <param name="nameofParameter">The name of the property.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when the <paramref name="index"/> value is less than or equal to zero.</exception>
        private void checkForIndexLessThanOneAndThrowIfInvalid(int index, Func<string> getHeader, string nameofParameter)
        {
            if (index <= 0)
            {
                throw new IndexOutOfRangeException(XlsxExceptionUtil.CreateLessThanOneIndexMessage(getHeader(), nameofParameter));
            }
        }

        /// <summary>
        /// Check to see if the provided index is greater than the max allowed value. If so, then it'll throw an exception.
        /// </summary>
        /// <param name="index">The index value to check.</param>
        /// <param name="maxIndex">The max allowed value.</param>
        /// <param name="getHeader">A function to get the exception header.</param>
        /// <param name="nameofParameter">The name of the property.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown when the <paramref name="index"/> value is greater than <paramref name="maxIndex"/>.</exception>
        private void checkForMaxIndexOutOfRangeAndThrowIfInvalid(int index, int maxIndex, Func<string> getHeader, string nameofParameter)
        {
            if (index > maxIndex)
            {
                throw new IndexOutOfRangeException(XlsxExceptionUtil.CreateMaxIndexMessage(getHeader(), nameofParameter, index, maxIndex));
            }
        }

        /// <summary>
        /// Check if the provided address is valid. If not, then it'll throw an exception.
        /// </summary>
        /// <param name="address">The address to check.</param>
        /// <param name="getHeader">A function to get the exception header.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="address"/> is null, empty, or whitespace.</exception>
        private void checkForNullEmptyOrWhiteSpaceAddressAndThrowIfInvalid(string address, Func<string> getHeader)
        {
            if (string.IsNullOrEmpty(address) || string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException(XlsxExceptionUtil.CreateAddressNullEmptyWhiteSpaceMessage(getHeader()));
            }
        }

        /// <summary>
        /// Check if the provided string is valid. If not, then it'll throw an exception.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <param name="getHeader">A function to get the exception header.</param>
        /// <param name="nameofParameter">The name of the property.</param>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="str"/> is null or empty.</exception>
        private void checkForNullOrEmptyStringAndThrowIfInvalid(string str, Func<string> getHeader, string nameofParameter)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException(XlsxExceptionUtil.CreateNullOrEmptyStringMessage(getHeader(), nameofParameter));
            }
        }

        /// <summary>
        /// Check to see if a parameter is null. If so, then throw an exception.
        /// </summary>
        /// <param name="param">The parameter to check.</param>
        /// <param name="getHeader">A function to get the exception header.</param>
        /// <param name="nameofParameter">The name of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown when the parameter is null.</exception>
        private void checkForNullParamAndThrowIfNull(object param, Func<string> getHeader, string nameofParameter)
        {
            if (param is null)
            {
                throw new ArgumentException(XlsxExceptionUtil.CreateNullArgumentMessage(getHeader(), nameofParameter));
            }
        }

        /// <summary>
        /// Throws an exception if <paramref name="hAlign"/> cannot be parsed to an <see cref="ExcelHAlign"/> type.
        /// </summary>
        /// <param name="hAlign">The <see cref="ExcelHAlign"/> value.</param>
        /// <param name="getHeader">A function to get the exception header.</param>
        /// <exception cref="ArgumentException">Thrown when the provided <see cref="ExcelHAlign"/> could not be parsed.</exception>
        private void checkIfExcelHAlignCannotBeParsedAndThrowIfInvalid(ExcelHAlign hAlign, Func<string> getHeader)
        {
            var successfullyParsed = Enum.TryParse(Enum.GetName(typeof(ExcelHAlign), hAlign), out Lib.ExcelHAlign parsedHAlign);

            if (!successfullyParsed)
            {
                throw new ArgumentException(XlsxExceptionUtil.CreateExcelHAlignParseErrorMessage(getHeader(), (int)hAlign));
            }
        }

        /// <summary>
        /// Creates a <see cref="Dictionary{TKey, TValue}"/> of <see cref="ExcelBordersIndex"/> keys and <see cref="BorderStyle"/> values.
        /// It will combine the provided styles with existing ones. If a <see cref="BorderStyle"/> is provided then that style will be returned. If
        /// a <see cref="BorderStyle"/> is missing then a default style will be created. The default style has a light gray edge for each side of the cell. Diagonal
        /// lines are set to invisible if not explicitly provided.
        /// </summary>
        /// <param name="borderStyles">A <see cref="Dictionary{TKey, TValue}"/> of <see cref="ExcelBordersIndex"/> keys and <see cref="BorderStyle"/> values.</param>
        /// <param name="borders">The existing <see cref="IBorders"/>.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> of all the borders for a cell.</returns>
        private Dictionary<ExcelBordersIndex, BorderStyle> getBorderStyles(Dictionary<ExcelBordersIndex, BorderStyle> borderStyles, IBorders borders)
        {
            var excelBorders = Enum.GetValues(typeof(ExcelBordersIndex)).Cast<ExcelBordersIndex>().ToList();

            BorderStyle createBorderStyle(ExcelBordersIndex borderIndex, IBorder border)
            {
                return new BorderStyle
                {
                    Color = border.Color == Color.FromArgb(255, 0, 0, 0) ? Color.DefaultCellBorder : border.Color,
                    LineStyle = border.LineStyle == ExcelLineStyle.None && borderIndex.IsOuterEdge() ? ExcelLineStyle.Thin : border.LineStyle,
                    ShowDiagonalLine = border.ShowDiagonalLine
                };
            }

            // If no border style has been provided then create a default from the existing border style.
            if (borderStyles is null)
            {
                return excelBorders.ToDictionary(borderIndex => borderIndex, borderIndex =>
                {
                    var border = borders[borderIndex];
                    if (border is null)
                    {
                        return new BorderStyle();
                    }

                    return createBorderStyle(borderIndex, border);
                });
            }

            // Return the provided border styles and fill in any missing ones as defaults.
            return excelBorders.ToDictionary(borderIndex => borderIndex, borderIndex =>
            {
                borderStyles.TryGetValue(borderIndex, out var providedBorderStyle);

                if (providedBorderStyle != null)
                {
                    return providedBorderStyle;
                }

                return createBorderStyle(borderIndex, borders[borderIndex]);
            });
        }

        /// <summary>
        /// Get an existing column <see cref="IStyle"/> or create a new one based on the index.
        /// </summary>
        /// <param name="columnIndex">The column index.</param>
        /// <returns>A <see cref="IStyle"/> object.</returns>
        private IInternalStyle getColumnStyle(int columnIndex)
        {
            var styleName = getColumnStyleName(columnIndex);
            var existingStyle = internalWorksheets.InternalStyles.Contains(styleName);
            var style = existingStyle ? internalWorksheets.InternalStyles.GetStyle(styleName) : internalWorksheets.InternalStyles.Add(styleName);
            return (IInternalStyle)style;
        }

        /// <summary>
        /// Get a column style name for the column index.
        /// </summary>
        /// <param name="columnIndex">The index of the column.</param>
        /// <returns>A <see cref="string"/> name that's always the same for a given index.</returns>
        private string getColumnStyleName(int columnIndex) => $"Column_{columnIndex}_Style";
    }
}