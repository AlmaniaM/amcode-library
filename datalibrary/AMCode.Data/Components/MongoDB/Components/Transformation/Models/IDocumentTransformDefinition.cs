using AMCode.Columns.Core;
using AMCode.Columns.DataTransform;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Interface for MongoDB document transformation definitions.
    /// Provides configuration for transforming MongoDB documents during data access operations.
    /// </summary>
    public interface IDocumentTransformDefinition
    {
        /// <summary>
        /// Gets the name of the field in the MongoDB document.
        /// </summary>
        string FieldName { get; }

        /// <summary>
        /// Gets the name of the property in the target C# object.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Gets the value formatter for the field.
        /// </summary>
        IValueFormatter GetFormatter();

        /// <summary>
        /// Gets a value indicating whether this field is required.
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        /// Gets the default value for the field if it's missing.
        /// </summary>
        object DefaultValue { get; }
    }
}
