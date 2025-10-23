using System.Collections.Generic;
using AMCode.Common.Xlsx;
using AMCode.SyncfusionIo.Xlsx.Common;
using AMCode.SyncfusionIo.Xlsx.Drawing;

namespace AMCode.SyncfusionIo.Xlsx
{
    /// <summary>
    /// An interface designed for setting styles.
    /// </summary>
    public interface IStyleParam
    {
        /// <summary>
        /// Set to true if font is bold. Null or false for not bold.
        /// </summary>
        bool? Bold { get; set; }

        /// <summary>
        /// A dictionary of border styles to apply.
        /// </summary>
        Dictionary<ExcelBordersIndex, BorderStyle> BorderStyles { get; set; }

        /// <inheritdoc cref="IStyle.Color"/>
        Color Color { get; set; }

        /// <inheritdoc cref="IStyle.ColorIndex"/>
        ExcelKnownColors? ColorIndex { get; set; }

        /// <inheritdoc cref="IStyle.FillPattern"/>
        ExcelPattern? FillPattern { get; set; }

        /// <summary>
        /// The font color to set.
        /// </summary>
        Color FontColor { get; set; }

        /// <summary>
        /// Set the font size.
        /// </summary>
        double? FontSize { get; set; }

        /// <inheritdoc cref="IStyle.HorizontalAlignment"/>
        ExcelHAlign? HorizontalAlignment { get; set; }

        /// <summary>
        /// Set to true if font is italic. Null or false if not.
        /// </summary>
        bool? Italic { get; set; }

        /// <inheritdoc cref="IStyle.NumberFormat"/>
        string NumberFormat { get; set; }

        /// <inheritdoc cref="IStyle.PatternColor"/>
        Color PatternColor { get; set; }
    }
}