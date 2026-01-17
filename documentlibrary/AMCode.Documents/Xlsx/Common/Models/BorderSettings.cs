namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Represents cell border settings
    /// </summary>
    public class BorderSettings
    {
        /// <summary>
        /// Gets or sets the top border style (e.g., "Thin", "Medium", "Thick", "None")
        /// </summary>
        public string TopStyle { get; set; } = "None";

        /// <summary>
        /// Gets or sets the top border color in hex format
        /// </summary>
        public string TopColor { get; set; } = "000000";

        /// <summary>
        /// Gets or sets the bottom border style
        /// </summary>
        public string BottomStyle { get; set; } = "None";

        /// <summary>
        /// Gets or sets the bottom border color in hex format
        /// </summary>
        public string BottomColor { get; set; } = "000000";

        /// <summary>
        /// Gets or sets the left border style
        /// </summary>
        public string LeftStyle { get; set; } = "None";

        /// <summary>
        /// Gets or sets the left border color in hex format
        /// </summary>
        public string LeftColor { get; set; } = "000000";

        /// <summary>
        /// Gets or sets the right border style
        /// </summary>
        public string RightStyle { get; set; } = "None";

        /// <summary>
        /// Gets or sets the right border color in hex format
        /// </summary>
        public string RightColor { get; set; } = "000000";

        /// <summary>
        /// Gets or sets the diagonal border style
        /// </summary>
        public string DiagonalStyle { get; set; } = "None";

        /// <summary>
        /// Gets or sets the diagonal border color in hex format
        /// </summary>
        public string DiagonalColor { get; set; } = "000000";
    }
}
