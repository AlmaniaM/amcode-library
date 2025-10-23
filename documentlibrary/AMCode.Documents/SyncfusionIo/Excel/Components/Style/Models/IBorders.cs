using System.Collections;
using AMCode.SyncfusionIo.Xlsx.Common;using AMCode.SyncfusionIo.Xlsx.Drawing;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx
{
    /// <summary>
    /// An interface designed to act as a collection of four Border objects that represent the four borders of a Range
    /// or Style object.
    /// </summary>
    public interface IBorders : IEnumerable
    {
        /// <summary>
        /// Returns a <see cref="IBorder"/> object that represents one of the <see cref="IBorders"/>
        /// of either an <see cref="IRange"/> of cells or a <see cref="IStyle"/>.
        /// </summary>
        /// <param name="index">The <see cref="ExcelBordersIndex"/>.</param>
        /// <returns>An <see cref="IBorder"/> object.</returns>
        IBorder this[ExcelBordersIndex index] { get; }

        /// <summary>
        /// Returns or sets the color from or to all the <see cref="IBorder"/> in the
        /// <see cref="IBorders"/> collection.
        /// </summary>
        /// <remarks>
        /// To set color for individual borders, we can use <see cref="KnownColor"/>
        /// or <see cref="Color"/> property.
        /// </remarks>
        ExcelKnownColors KnownColor { get; set; }

        /// <summary>
        /// Returns or sets RGB color from or to all the <see cref="IBorder"/>s in the
        /// <see cref="IBorders"/> collection.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Get the number of <see cref="IBorder"/> objects in the <see cref="IBorders"/> collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Get or sets the line style for the <see cref="IBorder"/>.
        /// </summary>
        ExcelLineStyle LineStyle { get; set; }

        /// <inheritdoc cref="this[ExcelBordersIndex]"/>
        IBorder GetBorder(ExcelBordersIndex index);
    }

    internal interface IInternalBorders : IBorders
    {
        /// <summary>
        /// Get the underlying <see cref="Lib.IBorders"/> object.
        /// </summary>
        Lib.IBorders InnerLibBorders { get; }
    }
}