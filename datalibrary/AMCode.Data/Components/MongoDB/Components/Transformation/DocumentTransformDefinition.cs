using System;
using AMCode.Columns.Core;
using AMCode.Columns.DataTransform;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Implementation of MongoDB document transformation definition.
    /// Provides configuration for transforming MongoDB documents during data access operations.
    /// </summary>
    public class DocumentTransformDefinition : IDocumentTransformDefinition
    {
        /// <summary>
        /// Initializes a new instance of the DocumentTransformDefinition class.
        /// </summary>
        /// <param name="fieldName">The name of the field in the MongoDB document.</param>
        /// <param name="propertyName">The name of the property in the target C# object.</param>
        /// <param name="formatter">The value formatter for the field.</param>
        /// <param name="isRequired">Whether this field is required.</param>
        /// <param name="defaultValue">The default value for the field if it's missing.</param>
        public DocumentTransformDefinition(
            string fieldName, 
            string propertyName, 
            IValueFormatter formatter = null, 
            bool isRequired = false, 
            object defaultValue = null)
        {
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _formatter = formatter;
            IsRequired = isRequired;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Gets the name of the field in the MongoDB document.
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// Gets the name of the property in the target C# object.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets a value indicating whether this field is required.
        /// </summary>
        public bool IsRequired { get; }

        /// <summary>
        /// Gets the default value for the field if it's missing.
        /// </summary>
        public object DefaultValue { get; }

        private readonly IValueFormatter _formatter;

        /// <summary>
        /// Gets the value formatter for the field.
        /// </summary>
        /// <returns>The value formatter for the field.</returns>
        public IValueFormatter GetFormatter()
        {
            return _formatter;
        }
    }
}
