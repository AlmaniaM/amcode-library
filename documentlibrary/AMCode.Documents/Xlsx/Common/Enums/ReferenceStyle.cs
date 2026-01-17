namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Specifies the cell reference style
    /// </summary>
    public enum ReferenceStyle
    {
        /// <summary>
        /// A1 reference style (columns as letters, rows as numbers: A1, B2, etc.)
        /// </summary>
        A1 = 0,

        /// <summary>
        /// R1C1 reference style (both rows and columns as numbers: R1C1, R2C2, etc.)
        /// </summary>
        R1C1 = 1
    }
}
