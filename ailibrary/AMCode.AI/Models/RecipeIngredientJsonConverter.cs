using System.Text.Json;
using System.Text.Json.Serialization;

namespace AMCode.AI.Models;

/// <summary>
/// Custom JSON converter for RecipeIngredient that handles both string and structured amount/unit formats
/// </summary>
public class RecipeIngredientJsonConverter : JsonConverter<RecipeIngredient>
{
    public override RecipeIngredient Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token");
        }

        var ingredient = new RecipeIngredient();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return ingredient;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected PropertyName token");
            }

            var propertyName = reader.GetString();
            reader.Read();

            switch (propertyName?.ToLowerInvariant())
            {
                case "name":
                    ingredient.Name = reader.GetString() ?? string.Empty;
                    break;

                case "amount":
                    ingredient.Amount = ExtractStringValue(ref reader);
                    break;

                case "unit":
                    ingredient.Unit = ExtractStringValue(ref reader);
                    break;

                case "text":
                    ingredient.Text = reader.GetString() ?? string.Empty;
                    break;

                case "preparation":
                    ingredient.Preparation = reader.GetString() ?? string.Empty;
                    break;

                case "directions":
                case "instructions": // Handle both "directions" and "instructions" field names
                    ingredient.Directions = reader.GetString() ?? string.Empty;
                    break;

                case "notes":
                    ingredient.Notes = reader.GetString() ?? string.Empty;
                    break;

                default:
                    // Skip unknown properties
                    SkipValue(ref reader);
                    break;
            }
        }

        throw new JsonException("Unexpected end of JSON");
    }

    public override void Write(Utf8JsonWriter writer, RecipeIngredient value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", value.Name);
        writer.WriteString("amount", value.Amount);
        writer.WriteString("unit", value.Unit);
        writer.WriteString("text", value.Text);
        writer.WriteString("preparation", value.Preparation);
        writer.WriteString("directions", value.Directions);
        writer.WriteString("notes", value.Notes);
        writer.WriteEndObject();
    }

    /// <summary>
    /// Extracts a string value from JSON, handling both string and structured object formats
    /// </summary>
    private static string ExtractStringValue(ref Utf8JsonReader reader)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString() ?? string.Empty;
        }

        if (reader.TokenType == JsonTokenType.Null)
        {
            return string.Empty;
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            // Handle structured format: { "numeric": 0.5, "text": "1/2" } or { "unit": "cup", "text": "cup" }
            string? textValue = null;
            string? unitValue = null;

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propName = reader.GetString();
                    reader.Read();

                    if (propName?.ToLowerInvariant() == "text")
                    {
                        if (reader.TokenType == JsonTokenType.String)
                        {
                            textValue = reader.GetString();
                        }
                        else if (reader.TokenType == JsonTokenType.Null)
                        {
                            // Skip null text
                        }
                        else
                        {
                            SkipValue(ref reader);
                        }
                    }
                    else if (propName?.ToLowerInvariant() == "unit")
                    {
                        if (reader.TokenType == JsonTokenType.String)
                        {
                            unitValue = reader.GetString();
                        }
                        else if (reader.TokenType == JsonTokenType.Null)
                        {
                            // Skip null unit
                        }
                        else
                        {
                            SkipValue(ref reader);
                        }
                    }
                    else if (propName?.ToLowerInvariant() == "numeric")
                    {
                        // Skip numeric value - we only need the text
                        // Handle null, number, or any other type
                        if (reader.TokenType == JsonTokenType.Null)
                        {
                            // Already positioned at null, just consume it
                            reader.Read();
                        }
                        else
                        {
                            SkipValue(ref reader);
                        }
                    }
                    else
                    {
                        // Skip unknown properties
                        SkipValue(ref reader);
                    }
                }
            }

            // Prefer text field, fallback to unit field, or empty string
            return textValue ?? unitValue ?? string.Empty;
        }

        // For other types (number, boolean), convert to string
        return reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetDecimal().ToString(),
            JsonTokenType.True => "true",
            JsonTokenType.False => "false",
            _ => string.Empty
        };
    }

    /// <summary>
    /// Skips a JSON value without deserializing it
    /// </summary>
    private static void SkipValue(ref Utf8JsonReader reader)
    {
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            int depth = 1;
            while (depth > 0 && reader.Read())
            {
                if (reader.TokenType == JsonTokenType.StartObject) depth++;
                else if (reader.TokenType == JsonTokenType.EndObject) depth--;
            }
        }
        else if (reader.TokenType == JsonTokenType.StartArray)
        {
            int depth = 1;
            while (depth > 0 && reader.Read())
            {
                if (reader.TokenType == JsonTokenType.StartArray) depth++;
                else if (reader.TokenType == JsonTokenType.EndArray) depth--;
            }
        }
        else if (reader.TokenType == JsonTokenType.Null)
        {
            // Null token - already consumed, just move past it
            // Actually, we're already positioned at null, so we need to read past it
            reader.Read();
        }
        else
        {
            // For primitive values (string, number, boolean), just read past them
            reader.Read();
        }
    }
}
