namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Specifies the order in which formulas are calculated
    /// </summary>
    public enum CalculationOrder
    {
        /// <summary>
        /// Calculate in natural order (dependencies first)
        /// </summary>
        Natural = 0,

        /// <summary>
        /// Calculate by row order
        /// </summary>
        ByRow = 1,

        /// <summary>
        /// Calculate by column order
        /// </summary>
        ByColumn = 2
    }
}
