namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Represents cell protection settings
    /// </summary>
    public class ProtectionSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the cell is locked
        /// </summary>
        public bool Locked { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether formulas are hidden
        /// </summary>
        public bool Hidden { get; set; }
    }
}
