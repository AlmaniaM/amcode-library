namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Options for opening an existing workbook
    /// </summary>
    public class WorkbookOpenOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether to open in read-only mode
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to repair corrupted files
        /// </summary>
        public bool RepairMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to update links
        /// </summary>
        public bool UpdateLinks { get; set; } = true;

        /// <summary>
        /// Gets or sets the password for protected workbooks
        /// </summary>
        public string Password { get; set; }
    }
}
