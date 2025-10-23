using AMCode.SyncfusionIo.Xlsx.Drawing;
using AMCode.SyncfusionIo.Xlsx.Common;
namespace AMCode.SyncfusionIo.Xlsx.Common
{
    /// <summary>
    /// A class designed to contain border styles to apply.
    /// </summary>
    public class BorderStyle
    {
        /// <summary>
        /// The border color to apply.
        /// </summary>
        public Color Color { get; set; } = Color.DefaultCellBorder;

        /// <summary>
        /// The line style to apply.
        /// </summary>
        public ExcelLineStyle LineStyle { get; set; } = ExcelLineStyle.Thin;

        /// <summary>
        /// Whether or not to show a diagonal line.
        /// </summary>
        public bool ShowDiagonalLine { get; set; }
    }
}