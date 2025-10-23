using System;
using AMCode.Columns.Core;

namespace AMCode.Columns.DataTransform
{
    /// <summary>
    /// Represents a column definition for data transformation operations.
    /// </summary>
    public interface IDataTransformColumnDefinition : IColumnDefinition
    {
        /// <summary>
        /// Gets or sets the column name information.
        /// </summary>
        IColumnName ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the property name for the column.
        /// </summary>
        string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the data transformation function.
        /// </summary>
        Delegate DataTransformFunction { get; set; }

        /// <summary>
        /// Gets or sets the value formatter for the column.
        /// </summary>
        IValueFormatter ValueFormatter { get; set; }

        /// <summary>
        /// Gets the field name for the column.
        /// </summary>
        string FieldName { get; }

        /// <summary>
        /// Gets the formatter for the column.
        /// </summary>
        IValueFormatter GetFormatter();
    }
}
