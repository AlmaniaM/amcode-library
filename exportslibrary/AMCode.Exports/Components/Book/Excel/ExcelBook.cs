using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading;
using AMCode.Common.Extensions.Dictionary;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.Util;
using AMCode.Exports.Adapters;
using AMCode.Exports.Common;
using AMCode.Exports.Common.Exceptions;
using AMCode.Exports.Common.Exceptions.Util;
using AMCode.Xlsx;
using AMCode.Xlsx.Common;

namespace AMCode.Exports.Book
{
    /// <summary>
    /// A class designed to represent a constructible Excel book.
    /// </summary>
    public class ExcelBook : IExcelBook
    {
        private IList<string> cachedColumns = new List<string>();
        private readonly IExcelApplication excelApplication;
        private readonly IWorkbook workbook;
        private readonly IList<IWorksheet> worksheets = new List<IWorksheet>();
        private readonly IDictionary<int, int> worksheetLastRow = new Dictionary<int, int>();

        /// <summary>
        /// Create an instance of the <see cref="ExcelBook"/> class.
        /// </summary>
        /// <param name="excelApplication"></param>
        public ExcelBook(IExcelApplication excelApplication)
        {
            this.excelApplication = excelApplication;
            workbook = this.excelApplication.Workbooks.Create();
            AddSheet();
        }

        /// <inheritdoc/>
        public int MaxRowsPerSheet { get; set; } = ExcelLimitValues.MaxRowCount;

        /// <inheritdoc/>
        public string SheetNamePrefix { get; set; } = "Sheet";

        /// <summary>
        /// Get the starting row index.
        /// </summary>
        /// <returns>An <see cref="int"/> value.</returns>
        public int StartingRowIndex => cachedColumns is null || cachedColumns.Count == 0 ? 1 : 2;

        /// <summary>
        /// Add a collection of data <see cref="ExpandoObject"/>s. If your data exceeds the max allowed rows per sheet then
        /// a new sheet will be created and the rest of the data will be added there.
        /// </summary>
        /// <param name="dataList">An <see cref="IList{T}"/> of <see cref="ExpandoObject"/>s.</param>
        /// <param name="columns">An <see cref="IEnumerable{T}"/> collection of <see cref="IExcelDataColumn"/>s.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling requests.</param>
        /// <inheritdoc/>
        public void AddData(IList<ExpandoObject> dataList, IEnumerable<IBookDataColumn> columns, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var columnsList = columns?.ToList();

            validateAddDataParameters(dataList, columnsList);

            cancellationToken.ThrowIfCancellationRequested();

            var lastWorksheetIndex = workbook.Worksheets.Count - 1;
            var startingRowExisted = worksheetLastRow.TryGetValue(lastWorksheetIndex, out var startingRow);

            var currentRowDataInsertIndex = getCurrentRowDataInsertIndex(startingRow, columnsList.Count);
            var rowCount = dataList.Count;
            for (var currentRowIndex = 0; currentRowIndex < rowCount; currentRowIndex++, currentRowDataInsertIndex++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // If we've reached the end of a sheet row limit then we create a new sheet
                if (currentRowDataInsertIndex > MaxRowsPerSheet)
                {
                    AddSheet();

                    if (cachedColumns.Count > 0)
                    {
                        SetColumns(cachedColumns);
                    }

                    currentRowDataInsertIndex = StartingRowIndex;
                    startingRow = StartingRowIndex;
                }

                var currentRow = dataList[currentRowIndex];

                columns.ForEach((c, columnIndex) =>
                {
                    var column = c as IExcelDataColumn;
                    var value = currentRow.GetValue(column.DataFieldName);
                    worksheet.SetCellValue(
                        currentRowDataInsertIndex,
                        columnIndex + 1,
                        new CellValue
                        {
                            Value = value,
                            ValueType = column.DataType
                        }
                    );
                });
            }

            cancellationToken.ThrowIfCancellationRequested();

            worksheetLastRow[lastWorksheetIndex] = currentRowDataInsertIndex;
        }

        /// <summary>
        /// Set the global <see cref="worksheet"/> to a new <see cref="IWorksheet"/> object.
        /// </summary>
        public void AddSheet()
        {
            var worksheetsCount = workbook.Worksheets.Count;
            var sheetPrefix = SheetNamePrefix ?? "Sheet";
            var nextSheetName = $"{sheetPrefix} {worksheetsCount + 1}";
            worksheets.Add(workbook.Worksheets.GetWorksheet(nextSheetName) ?? workbook.Worksheets.Create(nextSheetName));
        }

        /// <inheritdoc/>
        public void Dispose() => excelApplication.Dispose();

        /// <inheritdoc/>
        public Stream Save()
        {
            var stream = new MemoryStream();
            SaveAs(stream);
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        /// <inheritdoc/>
        public void SaveAs(Stream stream) => workbook.SaveAs(stream);

        /// <inheritdoc/>
        public void SetColumns(IEnumerable<string> columns)
            => setColumns(worksheet, columns);

        /// <inheritdoc/>
        public void SetColumnsAllSheets(IEnumerable<string> columns)
            => worksheets.ForEach(sheet => setColumns(sheet, columns));

        /// <inheritdoc/>
        public void SetColumnStylesByColumnIndex(IList<IColumnStyleParam> styleParams)
            => setColumnStylesByColumnIndex(worksheet, styleParams);

        /// <inheritdoc/>
        public void SetColumnStylesByColumnIndexAllSheets(IList<IColumnStyleParam> styleParams)
            => worksheets.ForEach(sheet => setColumnStylesByColumnIndex(sheet, styleParams));

        /// <inheritdoc/>
        public void SetColumnStylesByColumnName(IList<IColumnStyleParam> styleParams)
            => setColumnStylesByColumnName(worksheet, styleParams);

        /// <inheritdoc/>
        public void SetColumnStylesByColumnNameAllSheets(IList<IColumnStyleParam> styleParams)
            => worksheets.ForEach(sheet => setColumnStylesByColumnName(sheet, styleParams));

        /// <inheritdoc/>
        public void SetColumnWidthInPixelsAllSheets(string name, double width)
            => worksheets.ForEach(sheet => setColumnWidthInPixels(sheet, name, width));

        /// <inheritdoc/>
        public void SetColumnWidthInPixels(string name, double width)
            => setColumnWidthInPixels(worksheet, name, width);

        /// <inheritdoc/>
        public void SetRangeStyle(int row, int column, int lastRow, int lastColumn, IStyleParam styleParam)
            => setRangeStyle(worksheet, row, column, lastRow, lastColumn, styleParam);

        /// <inheritdoc/>
        public void SetRangeStyleAllSheets(int row, int column, int lastRow, int lastColumn, IStyleParam styleParam)
            => worksheets.ForEach(sheet => setRangeStyle(sheet, row, column, lastRow, lastColumn, styleParam));

        /// <inheritdoc/>
        public void SetTotals(IList<ICellValue> totals)
        {
            if (totals is null || totals.Count == 0)
            {
                var header = ExceptionUtil.CreateExceptionHeader<IList<ICellValue>>(SetTotals);

                if (totals is null)
                {
                    throw new NullReferenceException(ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, nameof(totals)));
                }

                if (totals.Count == 0)
                {
                    throw new EmptyCollectionException(header, nameof(totals));
                }
            }

            totals.ForEach((totalsCell, index) => worksheet.SetCellValue(StartingRowIndex, index + 1, totalsCell));
        }

        /// <summary>
        /// The current worksheet;
        /// </summary>
        private IWorksheet worksheet => worksheets.Count > 0 ? worksheets.ElementAt(worksheets.Count - 1) : default;

        /// <summary>
        /// Check if the range has a totals row at the top of the page.
        /// </summary>
        /// <param name="worksheet">The <see cref="IWorksheet"/> to check.</param>
        /// <param name="columnCount">The number of columns in the sheet.</param>
        /// <returns>True if any values exist in the second row. False otherwise.</returns>
        private bool checkIfAnyValuesExistInTotalsRow(IWorksheet worksheet, int columnCount)
        {
            var range = worksheet[2, 1, 2, columnCount];
            var values = range.Cells.Select(cell => cell.ObjectValue?.ToString());

            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Create an exception header for a specific function in this
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        private string getCurrentExceptionHeader(string functionName, params string[] propertyNames)
        {
            var propertyNamesString = string.Join(", ", propertyNames);
            return $"[{nameof(ExcelBook)}][{functionName}]({propertyNamesString})";
        }

        /// <summary>
        /// Get the row index where the data should start inserting from.
        /// </summary>
        /// <param name="startRow">The current starting row.</param>
        /// <param name="columnCount">The number of columns in the book.</param>
        /// <returns>An <see cref="int"/> starting row where the data should starting inserting from.</returns>
        private int getCurrentRowDataInsertIndex(int startRow, int columnCount)
        {
            if (startRow != 0)
            {
                return startRow;
            }

            var hasTotalsRow = checkIfAnyValuesExistInTotalsRow(worksheet, columnCount);
            return StartingRowIndex + (hasTotalsRow ? 1 : 0);
        }

        /// <summary>
        /// Set a collection of <see cref="string"/> names to be the row/cell columns.
        /// </summary>
        /// <param name="worksheet">The <see cref="IWorksheet"/> to apply the column names to.</param>
        /// <param name="columns">Provide a <see cref="IEnumerable{T}"/> of <see cref="string"/> column names.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="columns"/> is null.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="columns"/> is an empty collection.</exception>
        private void setColumns(IWorksheet worksheet, IEnumerable<string> columns)
        {
            var columnList = columns?.ToList();

            if (columnList is null || columnList.Count == 0 || columnList.Count > ExcelLimitValues.MaxColumnCount)
            {
                var header = ExceptionUtil.CreateExceptionHeader<IList<string>>(SetColumns);

                if (columnList is null || columnList.Count == 0)
                {
                    if (columnList is null)
                    {
                        throw new NullReferenceException(ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, nameof(columns)));
                    }

                    throw new EmptyCollectionException(header, nameof(columns));
                }

                if (columnList.Count > ExcelLimitValues.MaxColumnCount)
                {
                    throw new MaxColumnCountExceededException(header, ExcelLimitValues.MaxColumnCount);
                }
            }

            cachedColumns = columnList;
            cachedColumns.ForEach((columnName, i) => worksheet.SetText(1, i + 1, columnName));
        }

        /// <summary>
        /// Set a collection of <see cref="IColumnStyleParam"/>s as the column styles.
        /// </summary>
        /// <param name="worksheet">The <see cref="IWorksheet"/> to apply the style to.</param>
        /// <param name="styleParams">A <see cref="IList{T}"/> of <see cref="IColumnStyleParam"/>s with then <see cref="IColumnStyleParam.ColumnIndex"/>
        /// set to the column one-based index you want the styles applied to.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="styleParams"/> is null.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="styleParams"/> is an empty collection.</exception>
        private void setColumnStylesByColumnIndex(IWorksheet worksheet, IList<IColumnStyleParam> styleParams)
        {
            if (styleParams is null || styleParams.Count == 0)
            {
                var header = ExceptionUtil.CreateExceptionHeader<IWorksheet, IList<IColumnStyleParam>>(setColumnStylesByColumnIndex);

                if (styleParams is null)
                {
                    throw new NullReferenceException(ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, nameof(styleParams)));
                }

                throw new EmptyCollectionException(header, nameof(styleParams));
            }

            styleParams
                .Where(styleParam => styleParam.ColumnIndex != null)
                .ForEach(styleParam => worksheet.SetColumnStyle(styleParam.ColumnIndex.Value, XlsxAdapter.ConvertToXlsxStyleParam(styleParam)));
        }

        /// <summary>
        /// Set a collection of <see cref="IColumnStyleParam"/>s as the column styles.
        /// </summary>
        /// <param name="worksheet">The <see cref="IWorksheet"/> to apply the style to.</param>
        /// <param name="styleParams">A <see cref="IList{T}"/> of <see cref="IColumnStyleParam"/>s with then <see cref="IColumnStyleParam.ColumnName"/>
        /// set to the column name value you want the styles applied to.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="styleParams"/> is null.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="styleParams"/> is an empty collection.</exception>
        private void setColumnStylesByColumnName(IWorksheet worksheet, IList<IColumnStyleParam> styleParams)
        {
            if (styleParams is null || styleParams.Count == 0)
            {
                var header = ExceptionUtil.CreateExceptionHeader<IWorksheet, IList<IColumnStyleParam>>(setColumnStylesByColumnName);

                if (styleParams is null)
                {
                    throw new NullReferenceException(ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, nameof(styleParams)));
                }

                throw new EmptyCollectionException(header, nameof(styleParams));
            }

            styleParams.ForEach(styleParam =>
            {
                var columnName = styleParam.ColumnName;
                worksheet.SetColumnStyle(columnName, XlsxAdapter.ConvertToXlsxStyleParam(styleParam));
            });
        }

        /// <summary>
        /// Set the width of a column.
        /// </summary>
        /// <param name="worksheet">The <see cref="IWorksheet"/> to apply the column widths to.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="width">The width number.</param>
        private void setColumnWidthInPixels(IWorksheet worksheet, string name, double width)
        {
            if (name is null)
            {
                var header = ExceptionUtil.CreateExceptionHeader<IWorksheet, string, double>(setColumnWidthInPixels);
                throw new NullReferenceException(ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, nameof(name)));
            }

            worksheet.SetColumnWidthInPixels(name, Convert.ToInt32(width));
        }

        /// <summary>
        /// Set a style for a range of cells.
        /// </summary>
        /// <param name="worksheet">The <see cref="IWorksheet"/> to apply the style to.</param>
        /// <param name="row">One-based starting row index.</param>
        /// <param name="column">One-based starting column index.</param>
        /// <param name="lastRow">One-based last row index.</param>
        /// <param name="lastColumn">One-based last column index.</param>
        /// <param name="styleParam">The <see cref="IStyleParam"/> to apply as the styles.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="styleParam"/> is null.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown when <paramref name="row"/>, <paramref name="column"/>,
        /// <paramref name="lastRow"/>, or <paramref name="lastColumn"/> are less than or equal to zero.</exception>
        private void setRangeStyle(IWorksheet worksheet, int row, int column, int lastRow, int lastColumn, IStyleParam styleParam)
        {
            if (row <= 0 || column <= 0 || lastRow <= 0 || lastColumn <= 0 || styleParam is null)
            {
                var header = ExceptionUtil.CreateExceptionHeader<IWorksheet, int, int, int, int, IStyleParam>(setRangeStyle);

                if (styleParam != null)
                {
                    var incorrectParameters = ExportsExceptionUtil.GetParametersLessThanZero(
                        new IntParameterCheck() { Name = nameof(row), Value = row },
                        new IntParameterCheck() { Name = nameof(column), Value = column },
                        new IntParameterCheck() { Name = nameof(lastRow), Value = lastRow },
                        new IntParameterCheck() { Name = nameof(lastColumn), Value = lastColumn }
                    );

                    throw new IndexOutOfRangeException(
                        ExportsExceptionUtil.CreateIndexOutOfRangeException(header, string.Join(", ", incorrectParameters))
                    );
                }

                throw new NullReferenceException(ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, nameof(styleParam)));
            }

            worksheet.SetRangeStyle(row, column, lastRow, lastColumn, XlsxAdapter.ConvertToXlsxStyleParam(styleParam));
        }

        /// <summary>
        /// Validate the parameters and throw any necessary exceptions.
        /// </summary>
        /// <param name="dataList">An <see cref="IList{T}"/> of <see cref="ExpandoObject"/>s.</param>
        /// <param name="columns">A <see cref="IList{T}"/> of <see cref="IBookDataColumn"/>s.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="dataList"/> or <paramref name="columns"/> is null.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="dataList"/> or <paramref name="columns"/> is an empty collection.</exception>
        /// <exception cref="MaxColumnCountExceededException">Thrown when <paramref name="columns"/> count exceeds max allowed columns.</exception>
        /// <exception cref="OperationCanceledException">Thrown when the current operation is canceled by the provided <see cref="CancellationToken"/>.</exception>
        private void validateAddDataParameters(IList<ExpandoObject> dataList, IList<IBookDataColumn> columns)
        {
            if (dataList is null || dataList.Count == 0 || columns is null || columns.Count == 0 || columns.Count > ExcelLimitValues.MaxColumnCount)
            {
                var header = ExceptionUtil.CreateExceptionHeader<IList<ExpandoObject>, IList<IBookDataColumn>, CancellationToken>(AddData);

                if (dataList is null || dataList.Count == 0)
                {
                    if (dataList is null)
                    {
                        throw new NullReferenceException(ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, nameof(dataList)));
                    }

                    if (dataList.Count == 0)
                    {
                        throw new EmptyCollectionException(header, nameof(dataList));
                    }
                }

                if (columns is null || columns.Count == 0)
                {
                    if (columns is null)
                    {
                        throw new NullReferenceException(ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, nameof(columns)));
                    }

                    throw new EmptyCollectionException(header, nameof(columns));
                }

                if (columns.Count > ExcelLimitValues.MaxColumnCount)
                {
                    throw new MaxColumnCountExceededException(header, ExcelLimitValues.MaxColumnCount);
                }
            }
        }
    }
}