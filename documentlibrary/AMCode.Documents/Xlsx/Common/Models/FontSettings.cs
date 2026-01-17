namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Represents font settings for cells or workbook defaults
    /// </summary>
    public class FontSettings
    {
        /// <summary>
        /// Gets or sets the font name (e.g., "Calibri", "Arial")
        /// </summary>
        public string Name { get; set; } = "Calibri";

        /// <summary>
        /// Gets or sets the font size in points
        /// </summary>
        public double Size { get; set; } = 11.0;

        /// <summary>
        /// Gets or sets a value indicating whether the font is bold
        /// </summary>
        public bool Bold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the font is italic
        /// </summary>
        public bool Italic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the font is underlined
        /// </summary>
        public bool Underline { get; set; }

        /// <summary>
        /// Gets or sets the font color in hex format (e.g., "000000" for black)
        /// </summary>
        public string Color { get; set; } = "000000";

        /// <summary>
        /// Gets or sets a value indicating whether the font has strikethrough
        /// </summary>
        public bool Strikethrough { get; set; }
    }
}
