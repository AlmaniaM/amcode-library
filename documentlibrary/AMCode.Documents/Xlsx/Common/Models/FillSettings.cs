namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Represents cell fill/background settings
    /// </summary>
    public class FillSettings
    {
        /// <summary>
        /// Gets or sets the background color in hex format
        /// </summary>
        public string BackgroundColor { get; set; } = "FFFFFF";

        /// <summary>
        /// Gets or sets the foreground color in hex format (for patterns)
        /// </summary>
        public string ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the fill pattern type (e.g., "Solid", "None", "Gray125")
        /// </summary>
        public string PatternType { get; set; } = "None";
    }
}
