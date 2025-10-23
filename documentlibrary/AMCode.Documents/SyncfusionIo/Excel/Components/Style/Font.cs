using AMCode.SyncfusionIo.Xlsx.Drawing;
using AMCode.SyncfusionIo.Xlsx.Common;using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx.Internal
{
    /// <summary>
    /// A class designed to assign font information to cells/ranges.
    /// </summary>
    public class Font : IFont, IInternalFont
    {
        internal Font(Lib.IFont libFont)
        {
            InnerLibFont = libFont;
        }

        /// <inheritdoc/>
        public bool Bold
        {
            get => InnerLibFont.Bold;
            set => InnerLibFont.Bold = value;
        }

        /// <inheritdoc/>
        public ExcelKnownColors KnownColor
        {
            get => (ExcelKnownColors)InnerLibFont.Color;
            set => InnerLibFont.Color = (Lib.ExcelKnownColors)value;
        }

        /// <inheritdoc/>
        public Color Color
        {
            get => new Color(InnerLibFont.RGBColor);
            set => InnerLibFont.RGBColor = value.InnerLibColor;
        }

        /// <inheritdoc cref="IInternalFont.InnerLibFont"/>
        internal Lib.IFont InnerLibFont { get; }

        /// <inheritdoc/>
        Lib.IFont IInternalFont.InnerLibFont => InnerLibFont;

        /// <inheritdoc/>
        public bool Italic
        {
            get => InnerLibFont.Italic;
            set => InnerLibFont.Italic = value;
        }

        /// <inheritdoc/>
        public double Size
        {
            get => InnerLibFont.Size;
            set => InnerLibFont.Size = value;
        }

        /// <inheritdoc/>
        public string FontName
        {
            get => InnerLibFont.FontName;
            set => InnerLibFont.FontName = value;
        }
    }
}