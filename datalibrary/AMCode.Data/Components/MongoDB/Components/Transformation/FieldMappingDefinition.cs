using System;
using MongoDB.Bson;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Implementation of MongoDB field mapping definition.
    /// Provides configuration for mapping MongoDB document fields to C# object properties.
    /// </summary>
    public class FieldMappingDefinition : IFieldMappingDefinition
    {
        /// <summary>
        /// Initializes a new instance of the FieldMappingDefinition class.
        /// </summary>
        /// <param name="sourceField">The name of the source field in the MongoDB document.</param>
        /// <param name="targetProperty">The name of the target property in the C# object.</param>
        /// <param name="targetType">The target type for the field conversion.</param>
        /// <param name="customConverter">The custom converter function for the field.</param>
        public FieldMappingDefinition(
            string sourceField, 
            string targetProperty, 
            Type targetType, 
            Func<BsonValue, object> customConverter = null)
        {
            SourceField = sourceField ?? throw new ArgumentNullException(nameof(sourceField));
            TargetProperty = targetProperty ?? throw new ArgumentNullException(nameof(targetProperty));
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            CustomConverter = customConverter;
        }

        /// <summary>
        /// Gets the name of the source field in the MongoDB document.
        /// </summary>
        public string SourceField { get; }

        /// <summary>
        /// Gets the name of the target property in the C# object.
        /// </summary>
        public string TargetProperty { get; }

        /// <summary>
        /// Gets the target type for the field conversion.
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// Gets the custom converter function for the field.
        /// </summary>
        public Func<BsonValue, object> CustomConverter { get; }
    }
}
