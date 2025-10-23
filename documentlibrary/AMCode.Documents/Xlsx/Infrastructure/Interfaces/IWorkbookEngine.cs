using System;
using System.Collections.Generic;
using System.IO;
using AMCode.Documents.Common.Models;

namespace AMCode.Xlsx.Infrastructure.Interfaces
{
    /// <summary>
    /// Infrastructure interface for workbook engine operations
    /// Provides low-level operations for creating and opening workbooks
    /// </summary>
    public interface IWorkbookEngine
    {
        /// <summary>
        /// Creates a new empty workbook
        /// </summary>
        /// <returns>Result containing the workbook engine instance or error information</returns>
        Result<IWorkbookEngineInstance> Create();

        /// <summary>
        /// Creates a new workbook with the specified title
        /// </summary>
        /// <param name="title">The title for the new workbook</param>
        /// <returns>Result containing the workbook engine instance or error information</returns>
        Result<IWorkbookEngineInstance> Create(string title);

        /// <summary>
        /// Creates a new workbook with the specified metadata
        /// </summary>
        /// <param name="metadata">The metadata for the new workbook</param>
        /// <returns>Result containing the workbook engine instance or error information</returns>
        Result<IWorkbookEngineInstance> Create(WorkbookCreationMetadata metadata);

        /// <summary>
        /// Opens an existing workbook from a stream
        /// </summary>
        /// <param name="stream">The stream containing the workbook data</param>
        /// <returns>Result containing the workbook engine instance or error information</returns>
        Result<IWorkbookEngineInstance> Open(Stream stream);

        /// <summary>
        /// Opens an existing workbook from a stream with specified options
        /// </summary>
        /// <param name="stream">The stream containing the workbook data</param>
        /// <param name="options">The options for opening the workbook</param>
        /// <returns>Result containing the workbook engine instance or error information</returns>
        Result<IWorkbookEngineInstance> Open(Stream stream, WorkbookOpenOptions options);

        /// <summary>
        /// Opens an existing workbook from a file path
        /// </summary>
        /// <param name="filePath">The file path to the workbook</param>
        /// <returns>Result containing the workbook engine instance or error information</returns>
        Result<IWorkbookEngineInstance> Open(string filePath);

        /// <summary>
        /// Opens an existing workbook from a file path with specified options
        /// </summary>
        /// <param name="filePath">The file path to the workbook</param>
        /// <param name="options">The options for opening the workbook</param>
        /// <returns>Result containing the workbook engine instance or error information</returns>
        Result<IWorkbookEngineInstance> Open(string filePath, WorkbookOpenOptions options);

        /// <summary>
        /// Validates that a file is a valid Excel workbook
        /// </summary>
        /// <param name="filePath">The file path to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        Result<bool> IsValidWorkbook(string filePath);

        /// <summary>
        /// Validates that a stream contains a valid Excel workbook
        /// </summary>
        /// <param name="stream">The stream to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        Result<bool> IsValidWorkbook(Stream stream);

        /// <summary>
        /// Gets information about a workbook file without opening it
        /// </summary>
        /// <param name="filePath">The file path to the workbook</param>
        /// <returns>Result containing workbook information or error information</returns>
        Result<WorkbookInfo> GetWorkbookInfo(string filePath);

        /// <summary>
        /// Gets information about a workbook stream without opening it
        /// </summary>
        /// <param name="stream">The stream containing the workbook data</param>
        /// <returns>Result containing workbook information or error information</returns>
        Result<WorkbookInfo> GetWorkbookInfo(Stream stream);

        /// <summary>
        /// Gets the supported file formats
        /// </summary>
        /// <returns>Result containing the supported file formats or error information</returns>
        Result<IEnumerable<string>> GetSupportedFormats();

        /// <summary>
        /// Gets the engine version
        /// </summary>
        /// <returns>Result containing the engine version or error information</returns>
        Result<string> GetVersion();

        /// <summary>
        /// Gets the engine capabilities
        /// </summary>
        /// <returns>Result containing the engine capabilities or error information</returns>
        Result<EngineCapabilities> GetCapabilities();
    }

    /// <summary>
    /// Interface representing a workbook engine instance
    /// </summary>
    public interface IWorkbookEngineInstance : IDisposable
    {
        /// <summary>
        /// Gets the underlying workbook object
        /// </summary>
        object Workbook { get; }

        /// <summary>
        /// Gets the workbook type
        /// </summary>
        WorkbookType Type { get; }

        /// <summary>
        /// Gets a value indicating whether the instance is disposed
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Saves the workbook to a stream
        /// </summary>
        /// <param name="stream">The stream to save to</param>
        /// <returns>Result indicating success or failure</returns>
        Result Save(Stream stream);

        /// <summary>
        /// Saves the workbook to a file path
        /// </summary>
        /// <param name="filePath">The file path to save to</param>
        /// <returns>Result indicating success or failure</returns>
        Result Save(string filePath);

        /// <summary>
        /// Closes the workbook
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result Close();

        /// <summary>
        /// Refreshes the workbook data
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result Refresh();

        /// <summary>
        /// Gets the workbook properties
        /// </summary>
        /// <returns>Result containing the workbook properties or error information</returns>
        Result<WorkbookProperties> GetProperties();

        /// <summary>
        /// Sets the workbook properties
        /// </summary>
        /// <param name="properties">The properties to set</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetProperties(WorkbookProperties properties);

        /// <summary>
        /// Gets the number of worksheets
        /// </summary>
        /// <returns>Result containing the number of worksheets or error information</returns>
        Result<int> GetWorksheetCount();

        /// <summary>
        /// Gets the worksheet names
        /// </summary>
        /// <returns>Result containing the worksheet names or error information</returns>
        Result<IEnumerable<string>> GetWorksheetNames();

        /// <summary>
        /// Gets a worksheet by index
        /// </summary>
        /// <param name="index">The worksheet index (0-based)</param>
        /// <returns>Result containing the worksheet or error information</returns>
        Result<object> GetWorksheet(int index);

        /// <summary>
        /// Gets a worksheet by name
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>Result containing the worksheet or error information</returns>
        Result<object> GetWorksheet(string name);

        /// <summary>
        /// Adds a new worksheet
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>Result containing the new worksheet or error information</returns>
        Result<object> AddWorksheet(string name);

        /// <summary>
        /// Removes a worksheet by index
        /// </summary>
        /// <param name="index">The worksheet index (0-based)</param>
        /// <returns>Result indicating success or failure</returns>
        Result RemoveWorksheet(int index);

        /// <summary>
        /// Removes a worksheet by name
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>Result indicating success or failure</returns>
        Result RemoveWorksheet(string name);

        /// <summary>
        /// Renames a worksheet
        /// </summary>
        /// <param name="oldName">The current worksheet name</param>
        /// <param name="newName">The new worksheet name</param>
        /// <returns>Result indicating success or failure</returns>
        Result RenameWorksheet(string oldName, string newName);

        /// <summary>
        /// Renames a worksheet by index
        /// </summary>
        /// <param name="index">The worksheet index (0-based)</param>
        /// <param name="newName">The new worksheet name</param>
        /// <returns>Result indicating success or failure</returns>
        Result RenameWorksheet(int index, string newName);
    }

    /// <summary>
    /// Enumeration of workbook types
    /// </summary>
    public enum WorkbookType
    {
        /// <summary>
        /// Excel 2007+ format (.xlsx)
        /// </summary>
        Excel2007,

        /// <summary>
        /// Excel 2007+ macro-enabled format (.xlsm)
        /// </summary>
        Excel2007Macro,

        /// <summary>
        /// Excel 2007+ binary format (.xlsb)
        /// </summary>
        Excel2007Binary,

        /// <summary>
        /// Excel 2007+ template format (.xltx)
        /// </summary>
        Excel2007Template,

        /// <summary>
        /// Excel 2007+ macro-enabled template format (.xltm)
        /// </summary>
        Excel2007MacroTemplate,

        /// <summary>
        /// Legacy Excel format (.xls)
        /// </summary>
        ExcelLegacy,

        /// <summary>
        /// CSV format
        /// </summary>
        Csv,

        /// <summary>
        /// Unknown format
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Structure representing workbook properties
    /// </summary>
    public struct WorkbookProperties
    {
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the keywords
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the comments
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the manager
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// Gets or sets the application
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets the application version
        /// </summary>
        public string ApplicationVersion { get; set; }

        /// <summary>
        /// Gets or sets the template
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets the revision number
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// Gets or sets the total editing time in minutes
        /// </summary>
        public int TotalEditingTime { get; set; }

        /// <summary>
        /// Gets or sets the number of pages
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// Gets or sets the number of words
        /// </summary>
        public int Words { get; set; }

        /// <summary>
        /// Gets or sets the number of characters
        /// </summary>
        public int Characters { get; set; }

        /// <summary>
        /// Gets or sets the number of characters with spaces
        /// </summary>
        public int CharactersWithSpaces { get; set; }

        /// <summary>
        /// Gets or sets the number of lines
        /// </summary>
        public int Lines { get; set; }

        /// <summary>
        /// Gets or sets the number of paragraphs
        /// </summary>
        public int Paragraphs { get; set; }

        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the last modified date
        /// </summary>
        public DateTime Modified { get; set; }
    }

    /// <summary>
    /// Structure representing engine capabilities
    /// </summary>
    public struct EngineCapabilities
    {
        /// <summary>
        /// Gets or sets whether the engine supports creating workbooks
        /// </summary>
        public bool SupportsCreate { get; set; }

        /// <summary>
        /// Gets or sets whether the engine supports opening workbooks
        /// </summary>
        public bool SupportsOpen { get; set; }

        /// <summary>
        /// Gets or sets whether the engine supports saving workbooks
        /// </summary>
        public bool SupportsSave { get; set; }

        /// <summary>
        /// Gets or sets whether the engine supports templates
        /// </summary>
        public bool SupportsTemplates { get; set; }

        /// <summary>
        /// Gets or sets whether the engine supports macros
        /// </summary>
        public bool SupportsMacros { get; set; }

        /// <summary>
        /// Gets or sets whether the engine supports protection
        /// </summary>
        public bool SupportsProtection { get; set; }

        /// <summary>
        /// Gets or sets whether the engine supports calculation
        /// </summary>
        public bool SupportsCalculation { get; set; }

        /// <summary>
        /// Gets or sets whether the engine supports formatting
        /// </summary>
        public bool SupportsFormatting { get; set; }

        /// <summary>
        /// Gets or sets whether the engine supports charts
        /// </summary>
        public bool SupportsCharts { get; set; }

        /// <summary>
        /// Gets or sets whether the engine supports images
        /// </summary>
        public bool SupportsImages { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of worksheets supported
        /// </summary>
        public int MaxWorksheets { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of rows per worksheet
        /// </summary>
        public int MaxRows { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of columns per worksheet
        /// </summary>
        public int MaxColumns { get; set; }

        /// <summary>
        /// Gets or sets the maximum file size in bytes
        /// </summary>
        public long MaxFileSize { get; set; }
    }

    /// <summary>
    /// Structure representing workbook creation metadata
    /// </summary>
    public struct WorkbookCreationMetadata
    {
        /// <summary>
        /// Gets or sets the workbook title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the workbook author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the workbook subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the workbook company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the workbook category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the workbook keywords
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the workbook comments
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the workbook manager
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// Gets or sets the workbook application
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets the workbook application version
        /// </summary>
        public string ApplicationVersion { get; set; }

        /// <summary>
        /// Gets or sets the workbook template
        /// </summary>
        public string Template { get; set; }
    }

    /// <summary>
    /// Structure representing workbook open options
    /// </summary>
    public struct WorkbookOpenOptions
    {
        /// <summary>
        /// Gets or sets whether to open the workbook as read-only
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets whether to update links when opening
        /// </summary>
        public bool UpdateLinks { get; set; }

        /// <summary>
        /// Gets or sets the password for opening the workbook
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets whether to ignore read-only recommendations
        /// </summary>
        public bool IgnoreReadOnlyRecommendations { get; set; }

        /// <summary>
        /// Gets or sets whether to repair the workbook if corrupted
        /// </summary>
        public bool RepairCorrupted { get; set; }

        /// <summary>
        /// Gets or sets the delimiter for CSV files
        /// </summary>
        public char CsvDelimiter { get; set; }

        /// <summary>
        /// Gets or sets the encoding for text files
        /// </summary>
        public string TextEncoding { get; set; }
    }

    /// <summary>
    /// Structure representing workbook information
    /// </summary>
    public struct WorkbookInfo
    {
        /// <summary>
        /// Gets or sets the workbook file path
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the workbook file size in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets the workbook creation date
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the workbook last modified date
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        /// Gets or sets the workbook author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the workbook title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the workbook subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the workbook company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the number of worksheets
        /// </summary>
        public int WorksheetCount { get; set; }

        /// <summary>
        /// Gets or sets the worksheet names
        /// </summary>
        public string[] WorksheetNames { get; set; }

        /// <summary>
        /// Gets or sets whether the workbook is protected
        /// </summary>
        public bool IsProtected { get; set; }

        /// <summary>
        /// Gets or sets whether the workbook is read-only
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the workbook format version
        /// </summary>
        public string FormatVersion { get; set; }
    }
}
