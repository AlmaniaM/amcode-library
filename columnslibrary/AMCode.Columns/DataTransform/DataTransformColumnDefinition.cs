using AMCode.Columns.Core;
using System;

namespace AMCode.Columns.DataTransform
{
    /// <summary>
    /// Represents a column definition for data transformation operations.
    /// </summary>
    public class DataTransformColumnDefinition : IDataTransformColumnDefinition
    {
        /// <summary>
        /// Gets or sets the column name information.
        /// </summary>
        public IColumnName ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the property name for the column.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the data transformation function.
        /// </summary>
        public System.Delegate DataTransformFunction { get; set; }

        /// <summary>
        /// Gets or sets the value formatter for the column.
        /// </summary>
        public IValueFormatter ValueFormatter { get; set; }

        /// <summary>
        /// Gets the field name for the column.
        /// </summary>
        public string FieldName => ColumnName?.FieldName ?? string.Empty;

        /// <summary>
        /// Gets the formatter for the column.
        /// </summary>
        public IValueFormatter GetFormatter()
        {
            return ValueFormatter;
        }

        /// <summary>
        /// Gets or sets the column name (from IColumnDefinition).
        /// </summary>
        public string Name
        {
            get => ColumnName?.DisplayName ?? string.Empty;
            set
            {
                if (ColumnName == null)
                    ColumnName = new ColumnName();
                ColumnName.DisplayName = value;
            }
        }

        /// <summary>
        /// Gets or sets the column type (from IColumnDefinition).
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets whether the column is nullable (from IColumnDefinition).
        /// </summary>
        public bool IsNullable { get; set; } = true;
    }
}
