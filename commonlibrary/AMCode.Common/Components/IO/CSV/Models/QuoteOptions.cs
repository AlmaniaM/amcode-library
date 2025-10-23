namespace AMCode.Common.IO.CSV.Models
{
    /// <summary>
    /// Options for ways to handle quoted text in CSV files.
    /// </summary>
    public enum QuoteOption
    {
        /// <summary>
        /// Surround values with double quotes (") regardless of value types.
        /// </summary>
        AddQuotes,
        /// <summary>
        /// Only surround values with double quotes (") if absolutely needed.
        /// </summary>
        Auto
    }
}