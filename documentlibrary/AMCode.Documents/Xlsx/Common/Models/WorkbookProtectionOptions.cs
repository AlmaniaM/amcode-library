namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Options for protecting a workbook
    /// </summary>
    public class WorkbookProtectionOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether structure is protected
        /// </summary>
        public bool ProtectStructure { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether windows are protected
        /// </summary>
        public bool ProtectWindows { get; set; }

        /// <summary>
        /// Gets or sets the protection password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow inserting worksheets
        /// </summary>
        public bool AllowInsertWorksheets { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow deleting worksheets
        /// </summary>
        public bool AllowDeleteWorksheets { get; set; }
    }
}
