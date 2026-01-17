namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Options for creating a new workbook
    /// </summary>
    public class WorkbookCreationOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether to create a default worksheet
        /// </summary>
        public bool CreateDefaultWorksheet { get; set; } = true;

        /// <summary>
        /// Gets or sets the default worksheet name
        /// </summary>
        public string DefaultWorksheetName { get; set; } = "Sheet1";

        /// <summary>
        /// Gets or sets the calculation mode
        /// </summary>
        public CalculationMode CalculationMode { get; set; } = CalculationMode.Automatic;

        /// <summary>
        /// Gets or sets the date system
        /// </summary>
        public DateSystem DateSystem { get; set; } = DateSystem.System1900;
    }
}
