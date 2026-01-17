namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Specifies the calculation mode for workbook formulas
    /// </summary>
    public enum CalculationMode
    {
        /// <summary>
        /// Formulas are automatically recalculated when dependent values change
        /// </summary>
        Automatic = 0,

        /// <summary>
        /// Formulas are calculated automatically except for data tables
        /// </summary>
        AutomaticExceptTables = 1,

        /// <summary>
        /// Formulas are only calculated when explicitly requested
        /// </summary>
        Manual = 2
    }
}
