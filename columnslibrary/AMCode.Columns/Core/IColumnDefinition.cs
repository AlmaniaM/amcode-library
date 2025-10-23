using System;

namespace AMCode.Columns.Core
{
    /// <summary>
    /// Represents a basic column definition.
    /// </summary>
    public interface IColumnDefinition
    {
        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the column type.
        /// </summary>
        Type Type { get; set; }

        /// <summary>
        /// Gets or sets whether the column is nullable.
        /// </summary>
        bool IsNullable { get; set; }
    }
}
