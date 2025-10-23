using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Xlsx;

namespace AMCode.Documents.Xlsx.Infrastructure.Adapters
{
    /// <summary>
    /// Adapter for workbook metadata operations
    /// Wraps workbook properties and provides domain interface implementation
    /// </summary>
    public class WorkbookMetadataAdapter : IWorkbookMetadata
    {
        private readonly IWorkbookProperties _properties;

        /// <summary>
        /// Initializes a new instance of the WorkbookMetadataAdapter class
        /// </summary>
        /// <param name="properties">The underlying workbook properties implementation</param>
        /// <exception cref="ArgumentNullException">Thrown when properties is null</exception>
        public WorkbookMetadataAdapter(IWorkbookProperties properties)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        /// <summary>
        /// Gets or sets the author of the workbook
        /// </summary>
        public string Author
        {
            get => _properties.Author;
            set => _properties.Author = value;
        }

        /// <summary>
        /// Gets or sets the title of the workbook
        /// </summary>
        public string Title
        {
            get => _properties.Title;
            set => _properties.Title = value;
        }

        /// <summary>
        /// Gets or sets the subject of the workbook
        /// </summary>
        public string Subject
        {
            get => _properties.Subject;
            set => _properties.Subject = value;
        }

        /// <summary>
        /// Gets or sets the company associated with the workbook
        /// </summary>
        public string Company
        {
            get => _properties.Company;
            set => _properties.Company = value;
        }

        /// <summary>
        /// Gets or sets the creation date of the workbook
        /// </summary>
        public DateTime Created
        {
            get => _properties.Created;
            set => _properties.Created = value;
        }

        /// <summary>
        /// Gets or sets the last modified date of the workbook
        /// </summary>
        public DateTime Modified
        {
            get => _properties.Modified;
            set => _properties.Modified = value;
        }

        /// <summary>
        /// Gets or sets the category of the workbook
        /// </summary>
        public string Category
        {
            get => _properties.Category;
            set => _properties.Category = value;
        }

        /// <summary>
        /// Gets or sets the keywords associated with the workbook
        /// </summary>
        public string Keywords
        {
            get => _properties.Keywords;
            set => _properties.Keywords = value;
        }

        /// <summary>
        /// Gets or sets the comments for the workbook
        /// </summary>
        public string Comments
        {
            get => _properties.Comments;
            set => _properties.Comments = value;
        }

        /// <summary>
        /// Gets or sets the manager of the workbook
        /// </summary>
        public string Manager
        {
            get => _properties.Manager;
            set => _properties.Manager = value;
        }

        /// <summary>
        /// Gets or sets the application that created the workbook
        /// </summary>
        public string Application
        {
            get => _properties.Application;
            set => _properties.Application = value;
        }

        /// <summary>
        /// Gets or sets the version of the application that created the workbook
        /// </summary>
        public string ApplicationVersion
        {
            get => _properties.ApplicationVersion;
            set => _properties.ApplicationVersion = value;
        }

        /// <summary>
        /// Gets or sets the template used for the workbook
        /// </summary>
        public string Template
        {
            get => _properties.Template;
            set => _properties.Template = value;
        }

        /// <summary>
        /// Gets or sets the revision number of the workbook
        /// </summary>
        public int Revision
        {
            get => _properties.Revision;
            set => _properties.Revision = value;
        }

        /// <summary>
        /// Gets or sets the total editing time in minutes
        /// </summary>
        public int TotalEditingTime
        {
            get => _properties.TotalEditingTime;
            set => _properties.TotalEditingTime = value;
        }

        /// <summary>
        /// Gets or sets the number of pages in the workbook
        /// </summary>
        public int Pages
        {
            get => _properties.Pages;
            set => _properties.Pages = value;
        }

        /// <summary>
        /// Gets or sets the number of words in the workbook
        /// </summary>
        public int Words
        {
            get => _properties.Words;
            set => _properties.Words = value;
        }

        /// <summary>
        /// Gets or sets the number of characters in the workbook
        /// </summary>
        public int Characters
        {
            get => _properties.Characters;
            set => _properties.Characters = value;
        }

        /// <summary>
        /// Gets or sets the number of characters with spaces in the workbook
        /// </summary>
        public int CharactersWithSpaces
        {
            get => _properties.CharactersWithSpaces;
            set => _properties.CharactersWithSpaces = value;
        }

        /// <summary>
        /// Gets or sets the number of lines in the workbook
        /// </summary>
        public int Lines
        {
            get => _properties.Lines;
            set => _properties.Lines = value;
        }

        /// <summary>
        /// Gets or sets the number of paragraphs in the workbook
        /// </summary>
        public int Paragraphs
        {
            get => _properties.Paragraphs;
            set => _properties.Paragraphs = value;
        }

        /// <summary>
        /// Sets a custom property with the specified name and value
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="value">The property value</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SetProperty(string name, object value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result.Failure("Property name cannot be null or empty");
                }

                var result = _properties.SetProperty(name, value);
                if (result.IsSuccess)
                {
                    return Result.Success();
                }
                return Result.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error setting property '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a custom property by name
        /// </summary>
        /// <param name="name">The property name</param>
        /// <returns>Result containing the property value or error information</returns>
        public Result<object> GetProperty(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result<object>.Failure("Property name cannot be null or empty");
                }

                var result = _properties.GetProperty(name);
                if (result.IsSuccess)
                {
                    return Result<object>.Success(result.Value);
                }
                return Result<object>.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result<object>.Failure($"Error getting property '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a custom property by name with type conversion
        /// </summary>
        /// <typeparam name="T">The expected type of the property value</typeparam>
        /// <param name="name">The property name</param>
        /// <returns>Result containing the property value or error information</returns>
        public Result<T> GetProperty<T>(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result<T>.Failure("Property name cannot be null or empty");
                }

                var result = _properties.GetProperty<T>(name);
                if (result.IsSuccess)
                {
                    return Result<T>.Success(result.Value);
                }
                return Result<T>.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Error getting property '{name}' as {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Removes a custom property by name
        /// </summary>
        /// <param name="name">The property name</param>
        /// <returns>Result indicating success or failure</returns>
        public Result RemoveProperty(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result.Failure("Property name cannot be null or empty");
                }

                var result = _properties.RemoveProperty(name);
                if (result.IsSuccess)
                {
                    return Result.Success();
                }
                return Result.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error removing property '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all custom property names
        /// </summary>
        /// <returns>Result containing the collection of property names or error information</returns>
        public Result<IEnumerable<string>> GetPropertyNames()
        {
            try
            {
                var result = _properties.GetPropertyNames();
                if (result.IsSuccess)
                {
                    return Result<IEnumerable<string>>.Success(result.Value);
                }
                return Result<IEnumerable<string>>.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<string>>.Failure($"Error getting property names: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if a custom property exists
        /// </summary>
        /// <param name="name">The property name</param>
        /// <returns>True if the property exists, false otherwise</returns>
        public bool HasProperty(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return false;
                }

                return _properties.HasProperty(name);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets all custom properties as a dictionary
        /// </summary>
        /// <returns>Result containing the dictionary of properties or error information</returns>
        public Result<Dictionary<string, object>> GetAllProperties()
        {
            try
            {
                var result = _properties.GetAllProperties();
                if (result.IsSuccess)
                {
                    return Result<Dictionary<string, object>>.Success(result.Value);
                }
                return Result<Dictionary<string, object>>.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result<Dictionary<string, object>>.Failure($"Error getting all properties: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Clears all custom properties
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result ClearAllProperties()
        {
            try
            {
                var result = _properties.ClearAllProperties();
                if (result.IsSuccess)
                {
                    return Result.Success();
                }
                return Result.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error clearing all properties: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates the last modified timestamp to the current time
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result UpdateModifiedTime()
        {
            try
            {
                var result = _properties.UpdateModifiedTime();
                if (result.IsSuccess)
                {
                    return Result.Success();
                }
                return Result.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error updating modified time: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Resets the workbook metadata to default values
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result ResetToDefaults()
        {
            try
            {
                var result = _properties.ResetToDefaults();
                if (result.IsSuccess)
                {
                    return Result.Success();
                }
                return Result.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error resetting to defaults: {ex.Message}", ex);
            }
        }
    }
}
