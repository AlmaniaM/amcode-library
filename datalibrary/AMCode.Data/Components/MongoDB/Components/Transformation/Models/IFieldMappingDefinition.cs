using System;
using MongoDB.Bson;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Interface for MongoDB field mapping definitions.
    /// Provides configuration for mapping MongoDB document fields to C# object properties.
    /// </summary>
    public interface IFieldMappingDefinition
    {
        /// <summary>
        /// Gets the name of the source field in the MongoDB document.
        /// </summary>
        string SourceField { get; }

        /// <summary>
        /// Gets the name of the target property in the C# object.
        /// </summary>
        string TargetProperty { get; }

        /// <summary>
        /// Gets the target type for the field conversion.
        /// </summary>
        Type TargetType { get; }

        /// <summary>
        /// Gets the custom converter function for the field.
        /// </summary>
        Func<BsonValue, object> CustomConverter { get; }
    }
}
