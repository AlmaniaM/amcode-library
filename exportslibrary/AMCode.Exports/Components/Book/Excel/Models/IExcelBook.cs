using System;
using System.Collections.Generic;
using AMCode.Exports.Common.Exceptions;
using AMCode.SyncfusionIo.Xlsx;
using AMCode.SyncfusionIo.Xlsx.Common;

namespace AMCode.Exports.Book
{
    /// <summary>
    /// An interface designed to represent a constructible Excel book.
    /// </summary>
    public interface IExcelBook : IBook<IExcelDataColumn>
    {
        /// <summary>
        /// Get or set the maximum number or rows per sheet.
        /// </summary>
        int MaxRowsPerSheet { get; set; }

        /// <summary>
        /// The sheet name prefix to use when the max row count is larger than the allowed count.
        /// </summary>
        string SheetNamePrefix { get; set; }

        /// <summary>
        /// Set a collection of <see cref="string"/> names to be the row/cell columns for all sheets.
        /// </summary>
        /// <param name="columns">Provide a <see cref="IEnumerable{T}"/> of <see cref="string"/> column names.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="columns"/> is null.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="columns"/> is an empty collection.</exception>
        void SetColumnsAllSheets(IEnumerable<string> columns);

        /// <summary>
        /// Set a collection of <see cref="IColumnStyleParam"/>s as the column styles.
        /// </summary>
        /// <param name="styleParams">A <see cref="IList{T}"/> of <see cref="IColumnStyleParam"/>s with then <see cref="IColumnStyleParam.ColumnIndex"/>
        /// set to the column one-based index you want the styles applied to.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="styleParams"/> is null.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="styleParams"/> is an empty collection.</exception>
        void SetColumnStylesByColumnIndex(IList<IColumnStyleParam> styleParams);

        /// <summary>
        /// Set a collection of <see cref="IColumnStyleParam"/>s as the column styles for all worksheets.
        /// This operation is slower because it will always apply styles to all sheets.
        /// </summary>
        /// <param name="styleParams">A <see cref="IList{T}"/> of <see cref="IColumnStyleParam"/>s with then <see cref="IColumnStyleParam.ColumnIndex"/>
        /// set to the column one-based index you want the styles applied to.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="styleParams"/> is null.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="styleParams"/> is an empty collection.</exception>
        void SetColumnStylesByColumnIndexAllSheets(IList<IColumnStyleParam> styleParams);

        /// <summary>
        /// Set a collection of <see cref="IColumnStyleParam"/>s as the column styles.
        /// </summary>
        /// <param name="styleParams">A <see cref="IList{T}"/> of <see cref="IColumnStyleParam"/>s with then <see cref="IColumnStyleParam.ColumnName"/>
        /// set to the column name value you want the styles applied to.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="styleParams"/> is null.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="styleParams"/> is an empty collection.</exception>
        void SetColumnStylesByColumnName(IList<IColumnStyleParam> styleParams);

        /// <summary>
        /// Set a collection of <see cref="IColumnStyleParam"/>s as the column styles for all worksheets.
        /// This operation is slower because it will always apply styles to all sheets.
        /// </summary>
        /// <param name="styleParams">A <see cref="IList{T}"/> of <see cref="IColumnStyleParam"/>s with then <see cref="IColumnStyleParam.ColumnName"/>
        /// set to the column name value you want the styles applied to.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="styleParams"/> is null.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="styleParams"/> is an empty collection.</exception>
        void SetColumnStylesByColumnNameAllSheets(IList<IColumnStyleParam> styleParams);

        /// <summary>
        /// Set the width of a column.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <param name="width">The width number.</param>
        void SetColumnWidthInPixels(string name, double width);

        /// <summary>
        /// Set the width of a column for all worksheets. This operation is slower because it will always apply styles to all sheets.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <param name="width">The width number.</param>
        void SetColumnWidthInPixelsAllSheets(string name, double width);

        /// <summary>
        /// Set a style for a range of cells.
        /// </summary>
        /// <param name="row">One-based starting row index.</param>
        /// <param name="column">One-based starting column index.</param>
        /// <param name="lastRow">One-based last row index.</param>
        /// <param name="lastColumn">One-based last column index.</param>
        /// <param name="styleParam">The <see cref="IStyleParam"/> to apply as the styles.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="styleParam"/> is null.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown when <paramref name="row"/>, <paramref name="column"/>,
        /// <paramref name="lastRow"/>, or <paramref name="lastColumn"/> are less than or equal to zero.</exception>
        void SetRangeStyle(int row, int column, int lastRow, int lastColumn, IStyleParam styleParam);

        /// <summary>
        /// Set a style for a range of cells for all available worksheets. This operation is slower because it will always apply styles to all sheets.
        /// </summary>
        /// <param name="row">One-based starting row index.</param>
        /// <param name="column">One-based starting column index.</param>
        /// <param name="lastRow">One-based last row index.</param>
        /// <param name="lastColumn">One-based last column index.</param>
        /// <param name="styleParam">The <see cref="IStyleParam"/> to apply as the styles.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="styleParam"/> is null.</exception>
        /// <exception cref="IndexOutOfRangeException">Thrown when <paramref name="row"/>, <paramref name="column"/>,
        /// <paramref name="lastRow"/>, or <paramref name="lastColumn"/> are less than or equal to zero.</exception>
        void SetRangeStyleAllSheets(int row, int column, int lastRow, int lastColumn, IStyleParam styleParam);

        /// <summary>
        /// Set a collection of <see cref="ICellValue"/>s as the totals row.
        /// </summary>
        /// <param name="totals">An <see cref="IEnumerable{T}"/> of <see cref="ICellValue"/></param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="totals"/> is null.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="totals"/> is an empty collection.</exception>
        void SetTotals(IList<ICellValue> totals);
    }
}