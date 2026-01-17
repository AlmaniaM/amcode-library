namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Options for cloning a workbook
    /// </summary>
    public class WorkbookCloneOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether to clone worksheets
        /// </summary>
        public bool CloneWorksheets { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to clone styles
        /// </summary>
        public bool CloneStyles { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to clone formulas
        /// </summary>
        public bool CloneFormulas { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to clone metadata
        /// </summary>
        public bool CloneMetadata { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to clone named ranges
        /// </summary>
        public bool CloneNamedRanges { get; set; } = true;
    }
}
