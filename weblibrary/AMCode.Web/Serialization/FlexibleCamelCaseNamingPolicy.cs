using System.Text.Json;

namespace AMCode.Web.Serialization
{
    /// <summary>
    /// Custom JSON naming policy that converts C# property names to camelCase for serialization.
    /// When combined with PropertyNameCaseInsensitive = true, this allows the API to:
    /// - Serialize responses in camelCase (JavaScript/TypeScript convention)
    /// - Accept both camelCase and PascalCase in requests (backward compatibility)
    /// </summary>
    public class FlexibleCamelCaseNamingPolicy : JsonNamingPolicy
    {
        /// <summary>
        /// Converts a property name to camelCase format
        /// </summary>
        /// <param name="name">The property name to convert</param>
        /// <returns>The camelCase version of the property name</returns>
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;

            // Convert to camelCase for serialization (output)
            return char.ToLowerInvariant(name[0]) + name.Substring(1);
        }
    }
}
