using AMCode.SyncfusionIo.Xlsx.Drawing;
using AMCode.SyncfusionIo.Xlsx.Common;using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx
{
    /// <summary>
    /// An interface designed to contain the font attributes (font name, font size, color and so on) for an object.
    /// </summary>
    public interface IFont
    {
        /// <summary>
        /// True if the font is bold
        /// </summary>
        bool Bold { get; set; }

        /// <summary>
        /// Returns or sets the primary color of the object
        /// </summary>
        ExcelKnownColors KnownColor { get; set; }

        /// <summary>
        /// Gets / sets font color from the System
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// True if the font style is italic
        /// </summary>
        bool Italic { get; set; }

        /// <summary>
        /// Returns or sets the size of the font
        /// </summary>
        double Size { get; set; }

        /// <summary>
        /// Returns or sets the font name
        /// </summary>
        string FontName { get; set; }
    }

    internal interface IInternalFont : IFont
    {
        /// <summary>
        /// Get the underlying <see cref="Lib.IFont"/> object.
        /// </summary>
        Lib.IFont InnerLibFont { get; }
    }
}