using System.Collections.Generic;
using AMCode.Common.Xlsx;
using AMCode.SyncfusionIo.Xlsx.Common;
using AMCode.SyncfusionIo.Xlsx.Drawing;

namespace AMCode.SyncfusionIo.Xlsx
{
    /// <summary>
    /// A class designed for setting styles.
    /// </summary>
    public class StyleParam : IStyleParam
    {
        /// <inheritdoc/>
        public bool? Bold { get; set; }

        /// <inheritdoc/>
        public Dictionary<ExcelBordersIndex, BorderStyle> BorderStyles { get; set; }

        /// <inheritdoc/>
        public Color Color { get; set; }

        /// <inheritdoc/>
        public ExcelKnownColors? ColorIndex { get; set; }

        /// <inheritdoc/>
        public ExcelPattern? FillPattern { get; set; }

        /// <inheritdoc/>
        public Color FontColor { get; set; }

        /// <inheritdoc/>
        public double? FontSize { get; set; }

        /// <inheritdoc/>
        public ExcelHAlign? HorizontalAlignment { get; set; }

        /// <inheritdoc/>
        public bool? Italic { get; set; }

        /// <inheritdoc/>
        public string NumberFormat { get; set; }

        /// <inheritdoc/>
        public Color PatternColor { get; set; }
    }
}