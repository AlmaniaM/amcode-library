using System;
using System.Collections.Generic;
using System.Dynamic;
using MongoDB.Bson;
using Microsoft.Extensions.Logging;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Utility class for converting BSON values to C# types.
    /// Provides comprehensive type conversion support for MongoDB document mapping.
    /// </summary>
    public static class BsonValueConverter
    {
        /// <summary>
        /// Converts a BSON value to the specified C# type.
        /// </summary>
        /// <typeparam name="T">The target C# type.</typeparam>
        /// <param name="bsonValue">The BSON value to convert.</param>
        /// <param name="logger">Optional logger for conversion events.</param>
        /// <returns>The converted value.</returns>
        public static T ConvertTo<T>(BsonValue bsonValue, ILogger logger = null)
        {
            if (bsonValue == null || bsonValue.IsBsonNull)
            {
                return default(T);
            }

            try
            {
                return (T)ConvertTo(bsonValue, typeof(T), logger);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to convert BSON value {BsonValue} to type {TargetType}", 
                    bsonValue, typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// Converts a BSON value to the specified C# type.
        /// </summary>
        /// <param name="bsonValue">The BSON value to convert.</param>
        /// <param name="targetType">The target C# type.</param>
        /// <param name="logger">Optional logger for conversion events.</param>
        /// <returns>The converted value.</returns>
        public static object ConvertTo(BsonValue bsonValue, Type targetType, ILogger logger = null)
        {
            if (bsonValue == null || bsonValue.IsBsonNull)
            {
                return GetDefaultValue(targetType);
            }

            try
            {
                // Handle nullable types
                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    targetType = Nullable.GetUnderlyingType(targetType);
                }

                // Direct BSON type mapping
                switch (bsonValue.BsonType)
                {
                    case BsonType.Document:
                        return ConvertDocument(bsonValue.AsBsonDocument, targetType, logger);
                    
                    case BsonType.Array:
                        return ConvertArray(bsonValue.AsBsonArray, targetType, logger);
                    
                    case BsonType.String:
                        return ConvertString(bsonValue.AsString, targetType);
                    
                    case BsonType.Int32:
                        return ConvertInt32(bsonValue.AsInt32, targetType);
                    
                    case BsonType.Int64:
                        return ConvertInt64(bsonValue.AsInt64, targetType);
                    
                    case BsonType.Double:
                        return ConvertDouble(bsonValue.AsDouble, targetType);
                    
                    case BsonType.Decimal128:
                        return ConvertDecimal(bsonValue.AsDecimal128, targetType);
                    
                    case BsonType.Boolean:
                        return ConvertBoolean(bsonValue.AsBoolean, targetType);
                    
                    case BsonType.DateTime:
                        return ConvertDateTime(bsonValue.ToUniversalTime(), targetType);
                    
                    case BsonType.ObjectId:
                        return ConvertObjectId(bsonValue.AsObjectId, targetType);
                    
                    case BsonType.Binary:
                        return ConvertBinary(bsonValue.AsBsonBinaryData, targetType);
                    
                    default:
                        logger?.LogWarning("Unsupported BSON type {BsonType} for conversion to {TargetType}", 
                            bsonValue.BsonType, targetType.Name);
                        return bsonValue.ToJson();
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to convert BSON value {BsonValue} to type {TargetType}", 
                    bsonValue, targetType.Name);
                throw;
            }
        }

        private static object ConvertDocument(BsonDocument document, Type targetType, ILogger logger)
        {
            if (targetType == typeof(ExpandoObject))
            {
                return ConvertToExpandoObject(document, logger);
            }

            if (targetType == typeof(BsonDocument))
            {
                return document;
            }

            // Try to convert to a strongly-typed object
            return ConvertToStronglyTypedObject(document, targetType, logger);
        }

        private static object ConvertArray(BsonArray array, Type targetType, ILogger logger)
        {
            if (targetType.IsArray)
            {
                var elementType = targetType.GetElementType();
                var convertedArray = Array.CreateInstance(elementType, array.Count);
                
                for (int i = 0; i < array.Count; i++)
                {
                    convertedArray.SetValue(ConvertTo(array[i], elementType, logger), i);
                }
                
                return convertedArray;
            }

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var elementType = targetType.GetGenericArguments()[0];
                var list = (System.Collections.IList)Activator.CreateInstance(targetType);
                
                foreach (var item in array)
                {
                    list.Add(ConvertTo(item, elementType, logger));
                }
                
                return list;
            }

            return array.ToJson();
        }

        private static object ConvertString(string value, Type targetType)
        {
            if (targetType == typeof(string))
            {
                return value;
            }

            if (targetType == typeof(DateTime))
            {
                return DateTime.Parse(value);
            }

            if (targetType == typeof(Guid))
            {
                return Guid.Parse(value);
            }

            return Convert.ChangeType(value, targetType);
        }

        private static object ConvertInt32(int value, Type targetType)
        {
            return Convert.ChangeType(value, targetType);
        }

        private static object ConvertInt64(long value, Type targetType)
        {
            return Convert.ChangeType(value, targetType);
        }

        private static object ConvertDouble(double value, Type targetType)
        {
            return Convert.ChangeType(value, targetType);
        }

        private static object ConvertDecimal(Decimal128 value, Type targetType)
        {
            if (targetType == typeof(decimal))
            {
                return Decimal128.ToDecimal(value);
            }

            return Convert.ChangeType(Decimal128.ToDouble(value), targetType);
        }

        private static object ConvertBoolean(bool value, Type targetType)
        {
            return Convert.ChangeType(value, targetType);
        }

        private static object ConvertDateTime(DateTime value, Type targetType)
        {
            if (targetType == typeof(DateTime))
            {
                return value;
            }

            if (targetType == typeof(DateTimeOffset))
            {
                return new DateTimeOffset(value);
            }

            return Convert.ChangeType(value, targetType);
        }

        private static object ConvertObjectId(ObjectId value, Type targetType)
        {
            if (targetType == typeof(ObjectId))
            {
                return value;
            }

            if (targetType == typeof(string))
            {
                return value.ToString();
            }

            return Convert.ChangeType(value.ToString(), targetType);
        }

        private static object ConvertBinary(BsonBinaryData value, Type targetType)
        {
            if (targetType == typeof(byte[]))
            {
                return value.Bytes;
            }

            return Convert.ChangeType(value.Bytes, targetType);
        }

        private static ExpandoObject ConvertToExpandoObject(BsonDocument document, ILogger logger)
        {
            var expando = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)expando;

            foreach (var element in document)
            {
                dictionary[element.Name] = ConvertTo(element.Value, typeof(object), logger);
            }

            return expando;
        }

        private static object ConvertToStronglyTypedObject(BsonDocument document, Type targetType, ILogger logger)
        {
            var instance = Activator.CreateInstance(targetType);
            var properties = targetType.GetProperties();

            foreach (var property in properties)
            {
                if (document.TryGetValue(property.Name, out var bsonValue))
                {
                    try
                    {
                        var convertedValue = ConvertTo(bsonValue, property.PropertyType, logger);
                        property.SetValue(instance, convertedValue);
                    }
                    catch (Exception ex)
                    {
                        logger?.LogWarning(ex, "Failed to set property {PropertyName} on type {TargetType}", 
                            property.Name, targetType.Name);
                    }
                }
            }

            return instance;
        }

        private static object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}
