using AMCode.SyncfusionIo.Xlsx.Drawing;
using AMCode.SyncfusionIo.Xlsx.Common;using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx.Internal
{
    /// <summary>
    /// A class designed to represent a cell border.
    /// </summary>
    public class Border : IBorder, IInternalBorder
    {
        /// <summary>
        /// Create an instance of the <see cref="Border"/> class.
        /// </summary>
        /// <param name="libBorder">Provide an <see cref="Lib.IBorder"/> object.</param>
        internal Border(Lib.IBorder libBorder)
        {
            InnerLibBorder = libBorder;
        }

        /// <inheritdoc/>
        public ExcelKnownColors KnownColor
        {
            get => (ExcelKnownColors)InnerLibBorder.Color;
            set => InnerLibBorder.Color = (Lib.ExcelKnownColors)value;
        }

        /// <inheritdoc/>
        public Color Color
        {
            get => new Color(InnerLibBorder.ColorRGB);
            set => InnerLibBorder.ColorRGB = value.InnerLibColor;
        }

        /// <inheritdoc/>
        public ExcelLineStyle LineStyle
        {
            get => (ExcelLineStyle)InnerLibBorder.LineStyle;
            set => InnerLibBorder.LineStyle = (Lib.ExcelLineStyle)value;
        }

        /// <inheritdoc/>
        public bool ShowDiagonalLine
        {
            get => InnerLibBorder.ShowDiagonalLine;
            set => InnerLibBorder.ShowDiagonalLine = value;
        }

        /// <inheritdoc cref="IInternalBorder.InnerLibBorder"/>
        internal Lib.IBorder InnerLibBorder { get; }

        /// <inheritdoc/>
        Lib.IBorder IInternalBorder.InnerLibBorder => InnerLibBorder;
    }
}