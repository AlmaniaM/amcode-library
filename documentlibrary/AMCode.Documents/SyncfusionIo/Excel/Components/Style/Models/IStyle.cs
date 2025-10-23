using AMCode.Common.Xlsx;
using AMCode.SyncfusionIo.Xlsx.Common;using AMCode.SyncfusionIo.Xlsx.Drawing;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx
{
    /// <summary>
    /// Represents a styles object for a cell.
    /// </summary>
    public interface IStyle
    {
        /// <summary>
        /// Gets a <see cref="IBorders"/> collection that represents the borders of a style in the Range.
        /// </summary>
        /// <remarks>
        /// Borders including a Range defined as part of a conditional format will be returned.
        /// </remarks>
        IBorders Borders { get; }

        /// <summary>
        /// Returns or sets the RGB color to cell.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Sets a known Excel color by its <see cref="ExcelKnownColors"/> value.
        /// </summary>
        ExcelKnownColors ColorIndex { get; set; }

        /// <summary>
        /// Gets / Sets fill pattern.
        /// </summary>
        ExcelPattern FillPattern { get; set; }

        /// <summary>
        /// Gets <see cref="IFont"/> object for this extended format.
        /// </summary>
        IFont Font { get; }

        /// <summary>
        /// Horizontal alignment.
        /// </summary>
        ExcelHAlign HorizontalAlignment { get; set; }

        /// <summary>
        /// Get the name of the style;
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns or sets the number format code for the Syncfusion.XlsIO.IStyle object.
        /// </summary>
        string NumberFormat { get; set; }

        /// <summary>
        /// Returns or sets the color of the interior pattern as a RGB Color value.
        /// </summary>
        Color PatternColor { get; set; }
    }

    internal interface IInternalStyle : IStyle
    {
        /// <summary>
        /// Get the internal <see cref="Lib.IStyle"/> object.
        /// </summary>
        Lib.IStyle InnerLibStyle { get; }
    }
}