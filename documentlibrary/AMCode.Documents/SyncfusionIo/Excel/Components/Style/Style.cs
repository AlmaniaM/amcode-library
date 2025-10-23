using AMCode.Common.Xlsx;
using AMCode.SyncfusionIo.Xlsx.Common;using AMCode.SyncfusionIo.Xlsx.Drawing;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx.Internal
{
    /// <summary>
    /// A class designed to style an Excel cell.
    /// </summary>
    public class Style : IStyle, IInternalStyle
    {
        /// <summary>
        /// Create an instance of the <see cref="Style"/> class.
        /// </summary>
        /// <param name="libStyle">Provide an instance of the <see cref="Lib.IStyle"/> object.</param>
        internal Style(Lib.IStyle libStyle)
        {
            InnerLibStyle = libStyle;
        }

        /// <inheritdoc/>
        public IBorders Borders => new Borders(InnerLibStyle.Borders);

        /// <inheritdoc/>
        public Color Color
        {
            get => new Color(InnerLibStyle.Color);
            set => InnerLibStyle.Color = value.InnerLibColor;
        }

        /// <inheritdoc/>
        public ExcelKnownColors ColorIndex
        {
            get => (ExcelKnownColors)InnerLibStyle.ColorIndex;
            set => InnerLibStyle.ColorIndex = (Lib.ExcelKnownColors)value;
        }

        /// <inheritdoc/>
        public ExcelPattern FillPattern
        {
            get => (ExcelPattern)InnerLibStyle.FillPattern;
            set => InnerLibStyle.FillPattern = (Lib.ExcelPattern)value;
        }

        /// <inheritdoc/>
        public IFont Font => new Font(InnerLibStyle.Font);

        /// <inheritdoc/>
        public ExcelHAlign HorizontalAlignment
        {
            get => (ExcelHAlign)InnerLibStyle.HorizontalAlignment;
            set => InnerLibStyle.HorizontalAlignment = (Lib.ExcelHAlign)value;
        }

        /// <summary>
        /// Get the underlying <see cref="Lib.IStyle"/> object.
        /// </summary>
        public Lib.IStyle InnerLibStyle { get; }

        /// <inheritdoc/>
        public string Name => InnerLibStyle.Name;

        /// <inheritdoc/>
        public string NumberFormat
        {
            get => InnerLibStyle.NumberFormat;
            set => InnerLibStyle.NumberFormat = value;
        }

        /// <inheritdoc/>
        public Color PatternColor
        {
            get => new Color(InnerLibStyle.PatternColor);
            set => InnerLibStyle.PatternColor = value.InnerLibColor;
        }
    }
}