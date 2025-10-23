using System.Linq;
using AMCode.SyncfusionIo.Xlsx.Common;using AMCode.SyncfusionIo.Xlsx.Drawing;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx.Internal
{
    /// <summary>
    /// A class designed to represent a range in an Excel sheet.
    /// </summary>
    public class Range : IRange, IInternalRange
    {
        /// <summary>
        /// Create an instance of the <see cref="Range"/> class.
        /// </summary>
        /// <param name="libRange">Pass in a <see cref="Lib.IRange"/> object as the underlying range object.</param>
        internal Range(Lib.IRange libRange)
        {
            InnerLibRange = libRange;
        }

        /// <inheritdoc/>
        public IRange this[string name] => GetRange(name);

        /// <inheritdoc/>
        public IRange this[int row, int column] => GetRange(row, column);

        /// <inheritdoc/>
        public IRange this[int row, int column, int lastRow, int lastColumn] => GetRange(row, column, lastRow, lastColumn);

        /// <inheritdoc/>
        public string Address => InnerLibRange.Address;

        /// <inheritdoc/>
        public string AddressLocal => InnerLibRange.AddressLocal;

        /// <inheritdoc/>
        public bool Boolean
        {
            get => InnerLibRange.Boolean;
            set => InnerLibRange.Boolean = value;
        }

        /// <inheritdoc/>
        public IBorders Borders => new Borders(InnerLibRange.Borders);

        /// <inheritdoc/>
        public IStyle CellStyle
        {
            get => new Style(InnerLibRange.CellStyle);
            set => new Range(InnerLibRange).CellStyle = value;
        }

        /// <inheritdoc/>
        public string DisplayText => InnerLibRange.DisplayText;

        /// <inheritdoc/>
        public int FirstColumnIndex => InnerLibRange.Column;

        /// <inheritdoc/>
        public int FirstRowIndex => InnerLibRange.Row;

        /// <inheritdoc/>
        public IRange[] Cells => InnerLibRange.Cells.Select(cell => new Range(cell)).ToArray();

        /// <summary>
        /// Gets the underlying <see cref="Lib.IRange"/> object.
        /// </summary>
        public Lib.IRange InnerLibRange { get; }

        /// <inheritdoc/>
        public int LastColumnIndex => InnerLibRange.LastColumn;

        /// <inheritdoc/>
        public int LastRowIndex => InnerLibRange.LastRow;

        /// <inheritdoc/>
        public double Number
        {
            get => InnerLibRange.Number;
            set => InnerLibRange.Number = value;
        }

        /// <inheritdoc/>
        public string NumberFormat
        {
            get => InnerLibRange.NumberFormat;
            set => InnerLibRange.NumberFormat = value;
        }

        /// <inheritdoc/>
        public string Text
        {
            get => InnerLibRange.Text;
            set => InnerLibRange.Text = value;
        }

        /// <inheritdoc/>
        public string Value
        {
            get => InnerLibRange.Value;
            set => InnerLibRange.Value = value;
        }

        /// <inheritdoc/>
        public object ObjectValue
        {
            get => InnerLibRange.Value2;
            set => InnerLibRange.Value2 = value;
        }

        /// <inheritdoc/>
        public void SetBorderAround(ExcelLineStyle borderLine, Color borderColor)
            => InnerLibRange.BorderAround((Lib.ExcelLineStyle)borderLine, borderColor.InnerLibColor);

        /// <inheritdoc/>
        public void SetBorderAround(ExcelLineStyle borderLine)
            => InnerLibRange.BorderAround((Lib.ExcelLineStyle)borderLine);

        /// <inheritdoc/>
        public void SetBorderAround(ExcelLineStyle borderLine, ExcelKnownColors borderColor)
            => InnerLibRange.BorderAround((Lib.ExcelLineStyle)borderLine, (Lib.ExcelKnownColors)borderColor);

        /// <inheritdoc/>
        public IRange GetRange(string name) => new Range(InnerLibRange[name]);

        /// <inheritdoc/>
        public IRange GetRange(int row, int column) => new Range(InnerLibRange[row, column]);

        /// <inheritdoc/>
        public IRange GetRange(int row, int column, int lastRow, int lastColumn) => new Range(InnerLibRange[row, column, lastRow, lastColumn]);
    }
}