namespace AMCode.Columns.Core
{
    /// <summary>
    /// Represents column name information.
    /// </summary>
    public class ColumnName : IColumnName
    {
        /// <summary>
        /// Gets or sets the display name of the column.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the field name of the column.
        /// </summary>
        public string FieldName { get; set; }
    }
}
