using AMCode.Documents.Common.Drawing;
using AMCode.Documents.Common.Enums;

namespace AMCode.Documents.Common.Models
{
    /// <summary>
    /// Represents border styling information
    /// </summary>
    public class BorderStyle
    {
        /// <summary>
        /// Border line style
        /// </summary>
        public BorderLineStyle LineStyle { get; set; } = BorderLineStyle.Single;

        /// <summary>
        /// Border color
        /// </summary>
        public Color Color { get; set; } = Color.Black;

        /// <summary>
        /// Border sides
        /// </summary>
        public BorderSides Sides { get; set; } = BorderSides.All;

        /// <summary>
        /// Border width in points
        /// </summary>
        public double Width { get; set; } = 0.5;
    }
}
