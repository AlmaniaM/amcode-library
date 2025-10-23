using System;
using AMCode.SyncfusionIo.Xlsx.Common;using AMCode.SyncfusionIo.Xlsx.Drawing;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx
{
    /// <summary>
    /// An interface representing an Excel range object.
    /// </summary>
    public interface IRange
    {
        /// <summary>
        /// Gets cell Range.
        /// </summary>
        /// <param name="name">The range address such as "A2:D20"</param>
        /// <returns>An <see cref="IRange"/> representing the provided range address.</returns>
        IRange this[string name] { get; }

        /// <summary>
        /// Gets or sets cell range by row and column index. Row and column indexes are one-based.
        /// </summary>
        /// <param name="row">The cell row. One-based index.</param>
        /// <param name="column">The cell column. One-based index.</param>
        /// <returns>An <see cref="IRange"/> representing a single cell.</returns>
        IRange this[int row, int column] { get; }

        /// <summary>
        /// Get cell Range. Row and column indexes are one-based.
        /// </summary>
        /// <param name="row">The starting row index. One-based index.</param>
        /// <param name="column">The starting column index. One-based index.</param>
        /// <param name="lastRow">The last row index. One-based index.</param>
        /// <param name="lastColumn">The last column index. One-based index.</param>
        /// <returns>An <see cref="IRange"/> object representing the provided row/column staring point and row/column ending point.</returns>
        IRange this[int row, int column, int lastRow, int lastColumn] { get; }

        /// <summary>
        /// Get the range address within the Workbook scope.
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Get the range address within the current Worksheet scope.
        /// </summary>
        string AddressLocal { get; }

        /// <summary>
        /// Gets or sets boolean value in the Range.
        /// </summary>
        bool Boolean { get; set; }

        /// <summary>
        /// Gets a <see cref="IBorders"/> collection that represents the borders of a style in the Range.
        /// </summary>
        /// <remarks>
        /// Borders including a Range defined as part of a conditional format will be returned.
        /// </remarks>
        IBorders Borders { get; }

        /// <summary>
        /// Gets an <see cref="IRange"/> object that represents the cells in the Range.
        /// </summary>
        IRange[] Cells { get; }

        /// <summary>
        /// Gets an <see cref="IStyle"/> object that represents the style of the Range.
        /// </summary>
        IStyle CellStyle { get; set; }

        /// <summary>
        /// An actual <see cref="string"/> representation of what the user sees in the cell.
        /// </summary>
        string DisplayText { get; }

        /// <summary>
        /// Gets the column index of the first column in the Range which is a one based index.
        /// </summary>
        int FirstColumnIndex { get; }

        /// <summary>
        /// Gets the row index of the first row in the Range which is a one based index.
        /// </summary>
        int FirstRowIndex { get; }

        /// <summary>
        /// Gets the column index of the last column in the Range which is a one based index.
        /// </summary>
        int LastColumnIndex { get; }

        /// <summary>
        /// Gets the row index of the last row in the Range which is a one based index.
        /// </summary>
        int LastRowIndex { get; }

        /// <summary>
        /// Gets or sets number value that is contained by Range.
        /// </summary>
        /// <exception cref="FormatException">When Range value is not a number.</exception>
        double Number { get; set; }

        /// <summary>
        /// Gets or sets format of cell which is similar to Style.NumberFormat property.
        /// Gets NULL if all cells in the specified Range don't have the same number format.
        /// </summary>
        string NumberFormat { get; set; }

        /// <summary>
        /// Gets or sets string value of the Range.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the value of the Range. Does not support FormulaArray value.
        /// </summary>
        /// <remarks>Sets different data types values as a string, Value property parses the input
        /// string to determine its type which leads performance delay.
        /// The only difference between the Value2 property and the Value property is that
        /// the Value2 property does not use the Currency and Date data types. Also, it does
        /// not support FormulaArray value.
        /// </remarks>
        string Value { get; set; }

        /// <summary>
        /// Gets or sets the cell value.
        /// </summary>
        /// <remarks>Sets different data types values as a object. Value2 first checks whether the
        /// specified object has the type known for it (DateTime, TimeSpan, Double, Int).
        /// If yes, then it uses the corresponding typed properties (DateTime, TimeSpan,
        /// and Number). Otherwise, it calls Value property with String data type.
        /// The only difference between the Value2 property and the Value property is that
        /// the Value2 property does not use the Currency and Date data types. Also, it does
        /// not support FormulaArray value.
        /// </remarks>
        object ObjectValue { get; set; }

        /// <summary>
        /// Gets cell Range.
        /// </summary>
        /// <param name="name">The range address such as "A2:D20"</param>
        /// <returns>An <see cref="IRange"/> representing the provided range address.</returns>
        IRange GetRange(string name);

        /// <summary>
        /// Gets or sets cell range by row and column index. Row and column indexes are one-based.
        /// </summary>
        /// <param name="row">The cell row. One-based index.</param>
        /// <param name="column">The cell column. One-based index.</param>
        /// <returns>An <see cref="IRange"/> representing a single cell.</returns>
        IRange GetRange(int row, int column);

        /// <summary>
        /// Get cell Range. Row and column indexes are one-based.
        /// </summary>
        /// <param name="row">The starting row index. One-based index.</param>
        /// <param name="column">The starting column index. One-based index.</param>
        /// <param name="lastRow">The last row index. One-based index.</param>
        /// <param name="lastColumn">The last column index. One-based index.</param>
        /// <returns>An <see cref="IRange"/> object representing the provided row/column staring point and row/column ending point.</returns>
        IRange GetRange(int row, int column, int lastRow, int lastColumn);

        /// <summary>
        /// Applies border around the Range with the specified <see cref="ExcelLineStyle"/> and <see cref="Color"/>.
        /// </summary>
        /// <param name="borderLine">Represents border line style.</param>
        /// <param name="borderColor">Represents border color.</param>
        void SetBorderAround(ExcelLineStyle borderLine, Color borderColor);

        /// <summary>
        /// Applies border around the Range with the specified Syncfusion.XlsIO.ExcelLineStyle.
        /// </summary>
        /// <param name="borderLine">Represents border line style.</param>
        void SetBorderAround(ExcelLineStyle borderLine);

        /// <summary>
        /// Applies border around the Range with the specified <see cref="ExcelLineStyle"/> and <see cref="ExcelKnownColors"/>.
        /// </summary>
        /// <param name="borderLine">Represents border line style.</param>
        /// <param name="borderColor">Represents border color.</param>
        void SetBorderAround(ExcelLineStyle borderLine, ExcelKnownColors borderColor);
    }

    internal interface IInternalRange : IRange
    {
        /// <summary>
        /// Get the underlying <see cref="Lib.IRange"/> object.
        /// </summary>
        Lib.IRange InnerLibRange { get; }
    }
}