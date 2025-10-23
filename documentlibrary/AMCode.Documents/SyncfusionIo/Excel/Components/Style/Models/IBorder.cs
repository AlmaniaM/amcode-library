using AMCode.SyncfusionIo.Xlsx.Drawing;
using AMCode.SyncfusionIo.Xlsx.Common;using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx
{
    /// <summary>
    /// An interface designed to represent the border of an object.
    /// </summary>
    public interface IBorder
    {
        /// <summary>
        /// Gets or sets a color of the border from <see cref="ExcelKnownColors"/> enumeration.
        /// </summary>
        ExcelKnownColors KnownColor { get; set; }

        /// <summary>
        /// Sets or gets the RGB color of the border from <see cref="Drawing.Color"/> structure.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Returns or sets the line style for the border.
        /// </summary>
        ExcelLineStyle LineStyle { get; set; }

        /// <summary>
        /// This property is used only by Diagonal borders. For any other border index property
        /// will have no influence.
        /// </summary>
        bool ShowDiagonalLine { get; set; }
    }

    internal interface IInternalBorder : IBorder
    {
        /// <summary>
        /// Get the underlying <see cref="Lib.IBorder"/> object.
        /// </summary>
        Lib.IBorder InnerLibBorder { get; }
    }
}