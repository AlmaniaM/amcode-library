using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Xlsx.Domain.Interfaces
{
    /// <summary>
    /// Domain interface for workbook metadata operations
    /// Provides access to workbook properties and custom metadata
    /// </summary>
    public interface IWorkbookMetadata
    {
        /// <summary>
        /// Gets or sets the author of the workbook
        /// </summary>
        string Author { get; set; }

        /// <summary>
        /// Gets or sets the title of the workbook
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the subject of the workbook
        /// </summary>
        string Subject { get; set; }

        /// <summary>
        /// Gets or sets the company associated with the workbook
        /// </summary>
        string Company { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the workbook
        /// </summary>
        DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the last modified date of the workbook
        /// </summary>
        DateTime Modified { get; set; }

        /// <summary>
        /// Gets or sets the category of the workbook
        /// </summary>
        string Category { get; set; }

        /// <summary>
        /// Gets or sets the keywords associated with the workbook
        /// </summary>
        string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the comments for the workbook
        /// </summary>
        string Comments { get; set; }

        /// <summary>
        /// Gets or sets the manager of the workbook
        /// </summary>
        string Manager { get; set; }

        /// <summary>
        /// Gets or sets the application that created the workbook
        /// </summary>
        string Application { get; set; }

        /// <summary>
        /// Gets or sets the version of the application that created the workbook
        /// </summary>
        string ApplicationVersion { get; set; }

        /// <summary>
        /// Gets or sets the template used for the workbook
        /// </summary>
        string Template { get; set; }

        /// <summary>
        /// Gets or sets the revision number of the workbook
        /// </summary>
        int Revision { get; set; }

        /// <summary>
        /// Gets or sets the total editing time in minutes
        /// </summary>
        int TotalEditingTime { get; set; }

        /// <summary>
        /// Gets or sets the number of pages in the workbook
        /// </summary>
        int Pages { get; set; }

        /// <summary>
        /// Gets or sets the number of words in the workbook
        /// </summary>
        int Words { get; set; }

        /// <summary>
        /// Gets or sets the number of characters in the workbook
        /// </summary>
        int Characters { get; set; }

        /// <summary>
        /// Gets or sets the number of characters with spaces in the workbook
        /// </summary>
        int CharactersWithSpaces { get; set; }

        /// <summary>
        /// Gets or sets the number of lines in the workbook
        /// </summary>
        int Lines { get; set; }

        /// <summary>
        /// Gets or sets the number of paragraphs in the workbook
        /// </summary>
        int Paragraphs { get; set; }

        /// <summary>
        /// Sets a custom property with the specified name and value
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="value">The property value</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetProperty(string name, object value);

        /// <summary>
        /// Gets a custom property by name
        /// </summary>
        /// <param name="name">The property name</param>
        /// <returns>Result containing the property value or error information</returns>
        Result<object> GetProperty(string name);

        /// <summary>
        /// Gets a custom property by name with type conversion
        /// </summary>
        /// <typeparam name="T">The expected type of the property value</typeparam>
        /// <param name="name">The property name</param>
        /// <returns>Result containing the property value or error information</returns>
        Result<T> GetProperty<T>(string name);

        /// <summary>
        /// Removes a custom property by name
        /// </summary>
        /// <param name="name">The property name</param>
        /// <returns>Result indicating success or failure</returns>
        Result RemoveProperty(string name);

        /// <summary>
        /// Gets all custom property names
        /// </summary>
        /// <returns>Result containing the collection of property names or error information</returns>
        Result<IEnumerable<string>> GetPropertyNames();

        /// <summary>
        /// Checks if a custom property exists
        /// </summary>
        /// <param name="name">The property name</param>
        /// <returns>True if the property exists, false otherwise</returns>
        bool HasProperty(string name);

        /// <summary>
        /// Gets all custom properties as a dictionary
        /// </summary>
        /// <returns>Result containing the dictionary of properties or error information</returns>
        Result<Dictionary<string, object>> GetAllProperties();

        /// <summary>
        /// Clears all custom properties
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result ClearAllProperties();

        /// <summary>
        /// Updates the last modified timestamp to the current time
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result UpdateModifiedTime();

        /// <summary>
        /// Resets the workbook metadata to default values
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result ResetToDefaults();
    }
}
