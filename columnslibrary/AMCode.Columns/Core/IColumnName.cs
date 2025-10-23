namespace AMCode.Columns.Core
{
    /// <summary>
    /// Represents column name information.
    /// </summary>
    public interface IColumnName
    {
        /// <summary>
        /// Gets or sets the display name of the column.
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the field name of the column.
        /// </summary>
        string FieldName { get; set; }
    }
}
