namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Represents cell alignment settings
    /// </summary>
    public class AlignmentSettings
    {
        /// <summary>
        /// Gets or sets the horizontal alignment (e.g., "Left", "Center", "Right", "General")
        /// </summary>
        public string Horizontal { get; set; } = "General";

        /// <summary>
        /// Gets or sets the vertical alignment (e.g., "Top", "Center", "Bottom")
        /// </summary>
        public string Vertical { get; set; } = "Bottom";

        /// <summary>
        /// Gets or sets a value indicating whether text should wrap in the cell
        /// </summary>
        public bool WrapText { get; set; }

        /// <summary>
        /// Gets or sets the text rotation angle in degrees (0-360)
        /// </summary>
        public int TextRotation { get; set; }

        /// <summary>
        /// Gets or sets the indent level
        /// </summary>
        public int Indent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether text should shrink to fit
        /// </summary>
        public bool ShrinkToFit { get; set; }
    }
}
