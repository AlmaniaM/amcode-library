using System;
using AMCode.SyncfusionIo.Xlsx.Common;using System.Collections;
using AMCode.SyncfusionIo.Xlsx.Drawing;
using AMCode.SyncfusionIo.Xlsx.Extensions;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx.Internal
{
    /// <summary>
    /// A class designed to represent a collection of <see cref="IBorder"/> objects.
    /// </summary>
    public class Borders : IBorders, IInternalBorders
    {
        /// <summary>
        /// Create an instance of the <see cref="IBorders"/> class.
        /// </summary>
        /// <param name="libBorders">Provide a <see cref="Lib.IBorders"/> object.</param>
        internal Borders(Lib.IBorders libBorders)
        {
            InnerLibBorders = libBorders;
        }

        /// <inheritdoc/>
        public IBorder this[ExcelBordersIndex index] => GetBorder(index);

        /// <inheritdoc/>
        public ExcelKnownColors KnownColor
        {
            get => (ExcelKnownColors)InnerLibBorders.Color;
            set => InnerLibBorders.Color = (Lib.ExcelKnownColors)value;
        }

        /// <inheritdoc/>
        public Color Color
        {
            get => new Color(InnerLibBorders.ColorRGB);
            set => InnerLibBorders.ColorRGB = value.InnerLibColor;
        }

        /// <inheritdoc/>
        public int Count => InnerLibBorders.Count;

        /// <inheritdoc/>
        public ExcelLineStyle LineStyle
        {
            get => (ExcelLineStyle)InnerLibBorders.LineStyle;
            set => InnerLibBorders.LineStyle = (Lib.ExcelLineStyle)value;
        }

        /// <inheritdoc cref="IInternalBorder.InnerLibBorder"/>
        internal Lib.IBorders InnerLibBorders { get; }

        /// <inheritdoc/>
        Lib.IBorders IInternalBorders.InnerLibBorders => InnerLibBorders;

        /// <inheritdoc/>
        public IBorder GetBorder(ExcelBordersIndex index)
        {
            try
            {
                return new Border(InnerLibBorders[(Lib.ExcelBordersIndex)index]);
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <inheritdoc/>
        public IEnumerator GetEnumerator() => InnerLibBorders.ToBordersEnumerator();
    }
}