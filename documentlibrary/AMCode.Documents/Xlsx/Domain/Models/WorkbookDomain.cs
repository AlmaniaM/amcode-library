using System;
using System.Collections.Generic;
using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx;
using IWorksheet = AMCode.Documents.Xlsx.IWorksheet;

namespace AMCode.Documents.Xlsx.Domain.Models
{
    /// <summary>
    /// Domain model for workbook operations
    /// Contains pure business logic without infrastructure dependencies
    /// </summary>
    public class WorkbookDomain : IWorkbookDomain
    {
        private readonly IWorkbookContent _content;
        private readonly IWorkbookMetadata _metadata;
        private bool _disposed = false;

        /// <summary>
        /// Gets the unique identifier for this workbook
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the creation timestamp of this workbook
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Gets the last modification timestamp of this workbook
        /// </summary>
        public DateTime LastModified { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the workbook is disposed
        /// </summary>
        public bool IsDisposed => _disposed;

        /// <summary>
        /// Gets a value indicating whether the workbook is read-only
        /// </summary>
        public bool IsReadOnly { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the workbook is protected
        /// </summary>
        public bool IsProtected { get; private set; }

        /// <summary>
        /// Gets the collection of worksheets in this workbook
        /// </summary>
        public IEnumerable<IWorksheet> Worksheets => _content.Worksheets;

        /// <summary>
        /// Gets the number of worksheets in this workbook
        /// </summary>
        public int WorksheetCount => _content.WorksheetCount;

        /// <summary>
        /// Gets or sets the author of the workbook
        /// </summary>
        public string Author
        {
            get => _metadata.Author;
            set => _metadata.Author = value;
        }

        /// <summary>
        /// Gets or sets the title of the workbook
        /// </summary>
        public string Title
        {
            get => _metadata.Title;
            set => _metadata.Title = value;
        }

        /// <summary>
        /// Gets or sets the subject of the workbook
        /// </summary>
        public string Subject
        {
            get => _metadata.Subject;
            set => _metadata.Subject = value;
        }

        /// <summary>
        /// Gets or sets the company associated with the workbook
        /// </summary>
        public string Company
        {
            get => _metadata.Company;
            set => _metadata.Company = value;
        }

        /// <summary>
        /// Gets or sets the creation date of the workbook
        /// </summary>
        public DateTime Created
        {
            get => _metadata.Created;
            set => _metadata.Created = value;
        }

        /// <summary>
        /// Gets or sets the last modified date of the workbook
        /// </summary>
        public DateTime Modified
        {
            get => _metadata.Modified;
            set => _metadata.Modified = value;
        }

        /// <summary>
        /// Gets or sets the category of the workbook
        /// </summary>
        public string Category
        {
            get => _metadata.Category;
            set => _metadata.Category = value;
        }

        /// <summary>
        /// Gets or sets the keywords associated with the workbook
        /// </summary>
        public string Keywords
        {
            get => _metadata.Keywords;
            set => _metadata.Keywords = value;
        }

        /// <summary>
        /// Gets or sets the comments for the workbook
        /// </summary>
        public string Comments
        {
            get => _metadata.Comments;
            set => _metadata.Comments = value;
        }

        /// <summary>
        /// Gets or sets the manager of the workbook
        /// </summary>
        public string Manager
        {
            get => _metadata.Manager;
            set => _metadata.Manager = value;
        }

        /// <summary>
        /// Gets or sets the application that created the workbook
        /// </summary>
        public string Application
        {
            get => _metadata.Application;
            set => _metadata.Application = value;
        }

        /// <summary>
        /// Gets or sets the version of the application that created the workbook
        /// </summary>
        public string ApplicationVersion
        {
            get => _metadata.ApplicationVersion;
            set => _metadata.ApplicationVersion = value;
        }

        /// <summary>
        /// Gets or sets the template used for the workbook
        /// </summary>
        public string Template
        {
            get => _metadata.Template;
            set => _metadata.Template = value;
        }

        /// <summary>
        /// Gets or sets the revision number of the workbook
        /// </summary>
        public int Revision
        {
            get => _metadata.Revision;
            set => _metadata.Revision = value;
        }

        /// <summary>
        /// Gets or sets the total editing time in minutes
        /// </summary>
        public int TotalEditingTime
        {
            get => _metadata.TotalEditingTime;
            set => _metadata.TotalEditingTime = value;
        }

        /// <summary>
        /// Gets or sets the number of pages in the workbook
        /// </summary>
        public int Pages
        {
            get => _metadata.Pages;
            set => _metadata.Pages = value;
        }

        /// <summary>
        /// Gets or sets the number of words in the workbook
        /// </summary>
        public int Words
        {
            get => _metadata.Words;
            set => _metadata.Words = value;
        }

        /// <summary>
        /// Gets or sets the number of characters in the workbook
        /// </summary>
        public int Characters
        {
            get => _metadata.Characters;
            set => _metadata.Characters = value;
        }

        /// <summary>
        /// Gets or sets the number of characters with spaces in the workbook
        /// </summary>
        public int CharactersWithSpaces
        {
            get => _metadata.CharactersWithSpaces;
            set => _metadata.CharactersWithSpaces = value;
        }

        /// <summary>
        /// Gets or sets the number of lines in the workbook
        /// </summary>
        public int Lines
        {
            get => _metadata.Lines;
            set => _metadata.Lines = value;
        }

        /// <summary>
        /// Gets or sets the number of paragraphs in the workbook
        /// </summary>
        public int Paragraphs
        {
            get => _metadata.Paragraphs;
            set => _metadata.Paragraphs = value;
        }

        /// <summary>
        /// Initializes a new instance of the WorkbookDomain class
        /// </summary>
        /// <param name="content">The workbook content adapter</param>
        /// <param name="metadata">The workbook metadata adapter</param>
        /// <exception cref="ArgumentNullException">Thrown when content or metadata is null</exception>
        public WorkbookDomain(IWorkbookContent content, IWorkbookMetadata metadata)
        {
            _content = content ?? throw new ArgumentNullException(nameof(content));
            _metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            LastModified = CreatedAt;
        }

        /// <summary>
        /// Gets a worksheet by index
        /// </summary>
        /// <param name="index">The worksheet index (0-based)</param>
        /// <returns>Result containing the worksheet or error information</returns>
        public Result<IWorksheet> GetWorksheet(int index)
        {
            if (_disposed) return Result<IWorksheet>.Failure(new ObjectDisposedException(nameof(WorkbookDomain)));
            return _content.GetWorksheet(index);
        }

        /// <summary>
        /// Gets a worksheet by name
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>Result containing the worksheet or error information</returns>
        public Result<IWorksheet> GetWorksheet(string name)
        {
            if (_disposed) return Result<IWorksheet>.Failure(new ObjectDisposedException(nameof(WorkbookDomain)));
            return _content.GetWorksheet(name);
        }

        /// <summary>
        /// Adds a new worksheet to the workbook
        /// </summary>
        /// <param name="name">The name of the new worksheet</param>
        /// <returns>Result containing the new worksheet or error information</returns>
        public Result<IWorksheet> AddWorksheet(string name)
        {
            if (_disposed) return Result<IWorksheet>.Failure(new ObjectDisposedException(nameof(WorkbookDomain)));
            if (IsReadOnly) return Result<IWorksheet>.Failure("Workbook is read-only");
            
            var result = _content.AddWorksheet(name);
            if (result.IsSuccess)
            {
                LastModified = DateTime.UtcNow;
            }
            return result;
        }

        /// <summary>
        /// Removes a worksheet from the workbook
        /// </summary>
        /// <param name="index">The worksheet index (0-based)</param>
        /// <returns>Result indicating success or failure</returns>
        public Result RemoveWorksheet(int index)
        {
            if (_disposed) return Result.Failure(new ObjectDisposedException(nameof(WorkbookDomain)));
            if (IsReadOnly) return Result.Failure("Workbook is read-only");
            
            var result = _content.RemoveWorksheet(index);
            if (result.IsSuccess)
            {
                LastModified = DateTime.UtcNow;
            }
            return result;
        }

        /// <summary>
        /// Removes a worksheet from the workbook
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>Result indicating success or failure</returns>
        public Result RemoveWorksheet(string name)
        {
            if (_disposed) return Result.Failure(new ObjectDisposedException(nameof(WorkbookDomain)));
            if (IsReadOnly) return Result.Failure("Workbook is read-only");
            
            var result = _content.RemoveWorksheet(name);
            if (result.IsSuccess)
            {
                LastModified = DateTime.UtcNow;
            }
            return result;
        }

        /// <summary>
        /// Saves the workbook to the specified stream
        /// </summary>
        /// <param name="stream">The stream to save the workbook to</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SaveAs(Stream stream)
        {
            if (_disposed) return Result.Failure(new ObjectDisposedException(nameof(WorkbookDomain)));
            
            var result = _content.SaveAs(stream);
            if (result.IsSuccess)
            {
                LastModified = DateTime.UtcNow;
            }
            return result;
        }

        /// <summary>
        /// Saves the workbook to the specified file path
        /// </summary>
        /// <param name="filePath">The file path to save the workbook to</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SaveAs(string filePath)
        {
            if (_disposed) return Result.Failure(new ObjectDisposedException(nameof(WorkbookDomain)));
            
            var result = _content.SaveAs(filePath);
            if (result.IsSuccess)
            {
                LastModified = DateTime.UtcNow;
            }
            return result;
        }

        /// <summary>
        /// Saves the workbook with a new name
        /// </summary>
        /// <param name="stream">The stream to save the workbook to</param>
        /// <param name="fileName">The new file name</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SaveAs(Stream stream, string fileName)
        {
            if (_disposed) return "Workbook has been disposed";
            
            var result = _content.SaveAs(stream, fileName);
            if (result.IsSuccess)
            {
                LastModified = DateTime.UtcNow;
            }
            return result;
        }

        /// <summary>
        /// Saves the workbook with a new name to the specified file path
        /// </summary>
        /// <param name="filePath">The file path to save the workbook to</param>
        /// <param name="fileName">The new file name</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SaveAs(string filePath, string fileName)
        {
            if (_disposed) return "Workbook has been disposed";
            
            var result = _content.SaveAs(filePath, fileName);
            if (result.IsSuccess)
            {
                LastModified = DateTime.UtcNow;
            }
            return result;
        }

        /// <summary>
        /// Checks if a worksheet exists with the specified name
        /// </summary>
        /// <param name="name">The worksheet name to check</param>
        /// <returns>True if the worksheet exists, false otherwise</returns>
        public bool HasWorksheet(string name)
        {
            if (_disposed) return false;
            return _content.HasWorksheet(name);
        }

        /// <summary>
        /// Gets the index of a worksheet by name
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>Result containing the worksheet index or error information</returns>
        public Result<int> GetWorksheetIndex(string name)
        {
            if (_disposed) return "Workbook has been disposed";
            return _content.GetWorksheetIndex(name);
        }

        /// <summary>
        /// Renames a worksheet
        /// </summary>
        /// <param name="oldName">The current worksheet name</param>
        /// <param name="newName">The new worksheet name</param>
        /// <returns>Result indicating success or failure</returns>
        public Result RenameWorksheet(string oldName, string newName)
        {
            if (_disposed) return "Workbook has been disposed";
            if (IsReadOnly) return "Workbook is read-only";
            
            var result = _content.RenameWorksheet(oldName, newName);
            if (result.IsSuccess)
            {
                LastModified = DateTime.UtcNow;
            }
            return result;
        }

        /// <summary>
        /// Renames a worksheet by index
        /// </summary>
        /// <param name="index">The worksheet index (0-based)</param>
        /// <param name="newName">The new worksheet name</param>
        /// <returns>Result indicating success or failure</returns>
        public Result RenameWorksheet(int index, string newName)
        {
            if (_disposed) return "Workbook has been disposed";
            if (IsReadOnly) return "Workbook is read-only";
            
            var result = _content.RenameWorksheet(index, newName);
            if (result.IsSuccess)
            {
                LastModified = DateTime.UtcNow;
            }
            return result;
        }

        /// <summary>
        /// Sets a custom property with the specified name and value
        /// </summary>
        /// <param name="name">The property name</param>
        /// <param name="value">The property value</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SetProperty(string name, object value)
        {
            if (_disposed) return "Workbook has been disposed";
            if (IsReadOnly) return "Workbook is read-only";
            
            var result = _metadata.SetProperty(name, value);
            if (result.IsSuccess)
            {
                LastModified = DateTime.UtcNow;
            }
            return result;
        }

        /// <summary>
        /// Gets a custom property by name
        /// </summary>
        /// <param name="name">The property name</param>
        /// <returns>Result containing the property value or error information</returns>
        public Result<object> GetProperty(string name)
        {
            if (_disposed) return "Workbook has been disposed";
            return _metadata.GetProperty(name);
        }

        /// <summary>
        /// Gets a custom property by name with type conversion
        /// </summary>
        /// <typeparam name="T">The expected type of the property value</typeparam>
        /// <param name="name">The property name</param>
        /// <returns>Result containing the property value or error information</returns>
        public Result<T> GetProperty<T>(string name)
        {
            if (_disposed) return "Workbook has been disposed";
            return _metadata.GetProperty<T>(name);
        }

        /// <summary>
        /// Removes a custom property by name
        /// </summary>
        /// <param name="name">The property name</param>
        /// <returns>Result indicating success or failure</returns>
        public Result RemoveProperty(string name)
        {
            if (_disposed) return "Workbook has been disposed";
            if (IsReadOnly) return "Workbook is read-only";
            
            var result = _metadata.RemoveProperty(name);
            if (result.IsSuccess)
            {
                LastModified = DateTime.UtcNow;
            }
            return result;
        }

        /// <summary>
        /// Gets all custom property names
        /// </summary>
        /// <returns>Result containing the collection of property names or error information</returns>
        public Result<IEnumerable<string>> GetPropertyNames()
        {
            if (_disposed) return "Workbook has been disposed";
            return _metadata.GetPropertyNames();
        }

        /// <summary>
        /// Checks if a custom property exists
        /// </summary>
        /// <param name="name">The property name</param>
        /// <returns>True if the property exists, false otherwise</returns>
        public bool HasProperty(string name)
        {
            if (_disposed) return false;
            return _metadata.HasProperty(name);
        }

        /// <summary>
        /// Gets all custom properties as a dictionary
        /// </summary>
        /// <returns>Result containing the dictionary of properties or error information</returns>
        public Result<Dictionary<string, object>> GetAllProperties()
        {
            if (_disposed) return "Workbook has been disposed";
            return _metadata.GetAllProperties();
        }

        /// <summary>
        /// Clears all custom properties
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result ClearAllProperties()
        {
            if (_disposed) return "Workbook has been disposed";
            if (IsReadOnly) return "Workbook is read-only";
            
            var result = _metadata.ClearAllProperties();
            if (result.IsSuccess)
            {
                LastModified = DateTime.UtcNow;
            }
            return result;
        }

        /// <summary>
        /// Updates the last modified timestamp to the current time
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result UpdateModifiedTime()
        {
            if (_disposed) return Result.Failure("Workbook has been disposed");
            
            var result = _metadata.UpdateModifiedTime();
            if (result.IsSuccess)
            {
                LastModified = DateTime.UtcNow;
            }
            return result;
        }

        /// <summary>
        /// Resets the workbook metadata to default values
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result ResetToDefaults()
        {
            if (_disposed) return "Workbook has been disposed";
            if (IsReadOnly) return "Workbook is read-only";
            
            var result = _metadata.ResetToDefaults();
            if (result.IsSuccess)
            {
                LastModified = DateTime.UtcNow;
            }
            return result;
        }

        /// <summary>
        /// Closes the workbook and releases resources
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result Close()
        {
            if (_disposed) return "Workbook has already been disposed";
            
            try
            {
                // Note: IWorkbookMetadata doesn't have a Close() method
                // Only the underlying workbook engine needs to be closed
                LastModified = DateTime.UtcNow;
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error closing workbook: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Refreshes the workbook data and updates timestamps
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result Refresh()
        {
            if (_disposed) return "Workbook has been disposed";
            
            LastModified = DateTime.UtcNow;
            return Result.Success();
        }

        /// <summary>
        /// Validates the workbook structure and content
        /// </summary>
        /// <returns>Result indicating validation success or failure with details</returns>
        public Result Validate()
        {
            if (_disposed) return "Workbook has been disposed";
            
            // Basic validation - more comprehensive validation would be done by a validator
            if (WorksheetCount == 0)
            {
                return Result.Failure("Workbook must contain at least one worksheet");
            }
            
            return Result.Success();
        }

        /// <summary>
        /// Gets the workbook size in bytes
        /// </summary>
        /// <returns>Result containing the size in bytes or error information</returns>
        public Result<long> GetSize()
        {
            if (_disposed) return "Workbook has been disposed";
            
            // This would typically be implemented by the infrastructure layer
            return Result<long>.Success(0L);
        }

        /// <summary>
        /// Gets the workbook file format version
        /// </summary>
        /// <returns>Result containing the format version or error information</returns>
        public Result<string> GetFormatVersion()
        {
            if (_disposed) return Result<string>.Failure("Workbook has been disposed");
            
            // This would typically be implemented by the infrastructure layer
            return Result<string>.Success("Excel 2007+");
        }

        /// <summary>
        /// Sets the workbook to read-only mode
        /// </summary>
        /// <param name="readOnly">True to make read-only, false to make writable</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SetReadOnly(bool readOnly)
        {
            if (_disposed) return "Workbook has been disposed";
            
            IsReadOnly = readOnly;
            LastModified = DateTime.UtcNow;
            return Result.Success();
        }

        /// <summary>
        /// Protects the workbook with a password
        /// </summary>
        /// <param name="password">The protection password</param>
        /// <returns>Result indicating success or failure</returns>
        public Result Protect(string password)
        {
            if (_disposed) return "Workbook has been disposed";
            if (string.IsNullOrEmpty(password)) return "Password cannot be null or empty";
            
            IsProtected = true;
            LastModified = DateTime.UtcNow;
            return Result.Success();
        }

        /// <summary>
        /// Unprotects the workbook with a password
        /// </summary>
        /// <param name="password">The protection password</param>
        /// <returns>Result indicating success or failure</returns>
        public Result Unprotect(string password)
        {
            if (_disposed) return "Workbook has been disposed";
            if (string.IsNullOrEmpty(password)) return "Password cannot be null or empty";
            
            IsProtected = false;
            LastModified = DateTime.UtcNow;
            return Result.Success();
        }

        /// <summary>
        /// Gets the workbook calculation mode
        /// </summary>
        /// <returns>Result containing the calculation mode or error information</returns>
        public Result<CalculationMode> GetCalculationMode()
        {
            if (_disposed) return "Workbook has been disposed";
            
            // This would typically be implemented by the infrastructure layer
            return Result<CalculationMode>.Success(CalculationMode.Automatic);
        }

        /// <summary>
        /// Sets the workbook calculation mode
        /// </summary>
        /// <param name="mode">The calculation mode</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SetCalculationMode(CalculationMode mode)
        {
            if (_disposed) return "Workbook has been disposed";
            if (IsReadOnly) return "Workbook is read-only";
            
            // This would typically be implemented by the infrastructure layer
            LastModified = DateTime.UtcNow;
            return Result.Success();
        }

        /// <summary>
        /// Forces calculation of all formulas in the workbook
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result Calculate()
        {
            if (_disposed) return "Workbook has been disposed";
            
            // This would typically be implemented by the infrastructure layer
            LastModified = DateTime.UtcNow;
            return Result.Success();
        }

        /// <summary>
        /// Gets the workbook's default font settings
        /// </summary>
        /// <returns>Result containing the font settings or error information</returns>
        public Result<FontSettings> GetDefaultFont()
        {
            if (_disposed) return "Workbook has been disposed";
            
            // This would typically be implemented by the infrastructure layer
            return Result<FontSettings>.Success(new FontSettings
            {
                Name = "Calibri",
                Size = 11.0,
                Bold = false,
                Italic = false,
                Underline = false,
                Color = "000000"
            });
        }

        /// <summary>
        /// Sets the workbook's default font settings
        /// </summary>
        /// <param name="fontSettings">The font settings</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SetDefaultFont(FontSettings fontSettings)
        {
            if (_disposed) return "Workbook has been disposed";
            if (IsReadOnly) return "Workbook is read-only";
            
            // This would typically be implemented by the infrastructure layer
            LastModified = DateTime.UtcNow;
            return Result.Success();
        }

        /// <summary>
        /// Disposes of the workbook and releases all resources
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Close();
                _disposed = true;
            }
        }
    }
}
