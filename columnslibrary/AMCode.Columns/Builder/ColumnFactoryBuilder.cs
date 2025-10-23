using AMCode.Columns.Core;
using AMCode.Columns.DataTransform;
using System;

namespace AMCode.Columns.Builder
{
    /// <summary>
    /// Builder for creating column definitions.
    /// </summary>
    public class ColumnFactoryBuilder
    {
        private IColumnName _columnName;
        private string _propertyName;
        private Delegate _dataTransformFunction;
        private IValueFormatter _valueFormatter;

        /// <summary>
        /// Sets the column name for the builder.
        /// </summary>
        /// <param name="columnName">The column name to set.</param>
        /// <returns>The current builder instance.</returns>
        public ColumnFactoryBuilder ColumnName(IColumnName columnName)
        {
            _columnName = columnName;
            return this;
        }

        /// <summary>
        /// Sets the property name for the builder.
        /// </summary>
        /// <param name="propertyName">The property name to set.</param>
        /// <returns>The current builder instance.</returns>
        public ColumnFactoryBuilder PropertyName(string propertyName)
        {
            _propertyName = propertyName;
            return this;
        }

        /// <summary>
        /// Sets the data transformation function for the builder.
        /// </summary>
        /// <param name="dataTransformFunction">The data transformation function to set.</param>
        /// <returns>The current builder instance.</returns>
        public ColumnFactoryBuilder DataTransformation(Delegate dataTransformFunction = null)
        {
            _dataTransformFunction = dataTransformFunction;
            return this;
        }

        /// <summary>
        /// Sets the value formatter for the builder.
        /// </summary>
        /// <param name="valueFormatter">The value formatter to set.</param>
        /// <returns>The current builder instance.</returns>
        public ColumnFactoryBuilder ValueFormatter(IValueFormatter valueFormatter)
        {
            _valueFormatter = valueFormatter;
            return this;
        }

        /// <summary>
        /// Builds the column definition.
        /// </summary>
        /// <returns>The built column definition.</returns>
        public IDataTransformColumnDefinition Build()
        {
            return new DataTransformColumnDefinition
            {
                ColumnName = _columnName,
                PropertyName = _propertyName,
                DataTransformFunction = _dataTransformFunction,
                ValueFormatter = _valueFormatter
            };
        }
    }
}
