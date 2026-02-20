namespace AMCode.Exports.Recipes.Models
{
    /// <summary>
    /// Formatting options for Excel recipe exports.
    /// </summary>
    public class ExcelFormattingOptions
    {
        /// <summary>
        /// Whether to apply bold styling to the header row. Default: true.
        /// </summary>
        public bool BoldHeaders { get; set; } = true;

        /// <summary>
        /// Whether to auto-fit column widths. Default: true.
        /// </summary>
        public bool AutoFitColumns { get; set; } = true;

        /// <summary>
        /// Whether to freeze the header row. Default: true.
        /// </summary>
        public bool FreezeHeaderRow { get; set; } = true;

        /// <summary>
        /// Whether to apply alternating row colors for readability. Default: false.
        /// </summary>
        public bool AlternatingRowColors { get; set; } = false;

        /// <summary>
        /// Whether to apply table formatting (Excel table style). Default: true.
        /// </summary>
        public bool ApplyTableFormatting { get; set; } = true;

        /// <summary>
        /// Default column width in characters (used when AutoFitColumns is false). Default: 20.
        /// </summary>
        public int DefaultColumnWidth { get; set; } = 20;

        /// <summary>
        /// The default formatting options.
        /// </summary>
        public static ExcelFormattingOptions Default => new ExcelFormattingOptions();
    }
}
