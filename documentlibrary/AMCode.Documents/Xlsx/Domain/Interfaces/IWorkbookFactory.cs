using System;
using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;

namespace AMCode.Documents.Xlsx.Domain.Interfaces
{
    /// <summary>
    /// Domain interface for workbook factory operations
    /// Provides methods for creating and opening workbooks
    /// </summary>
    public interface IWorkbookFactory
    {
        /// <summary>
        /// Creates a new empty workbook
        /// </summary>
        /// <returns>Result containing the new workbook or error information</returns>
        Result<IWorkbookDomain> CreateWorkbook();

        /// <summary>
        /// Creates a new workbook with the specified title
        /// </summary>
        /// <param name="title">The title for the new workbook</param>
        /// <returns>Result containing the new workbook or error information</returns>
        Result<IWorkbookDomain> CreateWorkbook(string title);

        /// <summary>
        /// Creates a new workbook with the specified title and author
        /// </summary>
        /// <param name="title">The title for the new workbook</param>
        /// <param name="author">The author for the new workbook</param>
        /// <returns>Result containing the new workbook or error information</returns>
        Result<IWorkbookDomain> CreateWorkbook(string title, string author);

        /// <summary>
        /// Creates a new workbook with the specified metadata
        /// </summary>
        /// <param name="metadata">The metadata for the new workbook</param>
        /// <returns>Result containing the new workbook or error information</returns>
        Result<IWorkbookDomain> CreateWorkbook(WorkbookCreationMetadata metadata);

        /// <summary>
        /// Opens an existing workbook from a stream
        /// </summary>
        /// <param name="stream">The stream containing the workbook data</param>
        /// <returns>Result containing the opened workbook or error information</returns>
        Result<IWorkbookDomain> OpenWorkbook(Stream stream);

        /// <summary>
        /// Opens an existing workbook from a stream with specified options
        /// </summary>
        /// <param name="stream">The stream containing the workbook data</param>
        /// <param name="options">The options for opening the workbook</param>
        /// <returns>Result containing the opened workbook or error information</returns>
        Result<IWorkbookDomain> OpenWorkbook(Stream stream, WorkbookOpenOptions options);

        /// <summary>
        /// Opens an existing workbook from a file path
        /// </summary>
        /// <param name="filePath">The file path to the workbook</param>
        /// <returns>Result containing the opened workbook or error information</returns>
        Result<IWorkbookDomain> OpenWorkbook(string filePath);

        /// <summary>
        /// Opens an existing workbook from a file path with specified options
        /// </summary>
        /// <param name="filePath">The file path to the workbook</param>
        /// <param name="options">The options for opening the workbook</param>
        /// <returns>Result containing the opened workbook or error information</returns>
        Result<IWorkbookDomain> OpenWorkbook(string filePath, WorkbookOpenOptions options);

        /// <summary>
        /// Creates a workbook from a template
        /// </summary>
        /// <param name="templatePath">The path to the template file</param>
        /// <returns>Result containing the new workbook or error information</returns>
        Result<IWorkbookDomain> CreateFromTemplate(string templatePath);

        /// <summary>
        /// Creates a workbook from a template with specified options
        /// </summary>
        /// <param name="templatePath">The path to the template file</param>
        /// <param name="options">The options for creating the workbook</param>
        /// <returns>Result containing the new workbook or error information</returns>
        Result<IWorkbookDomain> CreateFromTemplate(string templatePath, WorkbookCreationOptions options);

        /// <summary>
        /// Creates a workbook from a template stream
        /// </summary>
        /// <param name="templateStream">The stream containing the template data</param>
        /// <returns>Result containing the new workbook or error information</returns>
        Result<IWorkbookDomain> CreateFromTemplate(Stream templateStream);

        /// <summary>
        /// Creates a workbook from a template stream with specified options
        /// </summary>
        /// <param name="templateStream">The stream containing the template data</param>
        /// <param name="options">The options for creating the workbook</param>
        /// <returns>Result containing the new workbook or error information</returns>
        Result<IWorkbookDomain> CreateFromTemplate(Stream templateStream, WorkbookCreationOptions options);

        /// <summary>
        /// Clones an existing workbook
        /// </summary>
        /// <param name="workbook">The workbook to clone</param>
        /// <returns>Result containing the cloned workbook or error information</returns>
        Result<IWorkbookDomain> CloneWorkbook(IWorkbookDomain workbook);

        /// <summary>
        /// Clones an existing workbook with specified options
        /// </summary>
        /// <param name="workbook">The workbook to clone</param>
        /// <param name="options">The options for cloning the workbook</param>
        /// <returns>Result containing the cloned workbook or error information</returns>
        Result<IWorkbookDomain> CloneWorkbook(IWorkbookDomain workbook, WorkbookCloneOptions options);

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
    /// Structure representing workbook creation options
    /// </summary>
    public struct WorkbookCreationOptions
    {
        /// <summary>
        /// Gets or sets whether to create with default worksheets
        /// </summary>
        public bool CreateDefaultWorksheets { get; set; }

        /// <summary>
        /// Gets or sets the number of default worksheets to create
        /// </summary>
        public int DefaultWorksheetCount { get; set; }

        /// <summary>
        /// Gets or sets the names of default worksheets
        /// </summary>
        public string[] DefaultWorksheetNames { get; set; }

        /// <summary>
        /// Gets or sets whether to enable auto-save
        /// </summary>
        public bool EnableAutoSave { get; set; }

        /// <summary>
        /// Gets or sets the auto-save interval in minutes
        /// </summary>
        public int AutoSaveInterval { get; set; }

        /// <summary>
        /// Gets or sets whether to enable calculation
        /// </summary>
        public bool EnableCalculation { get; set; }

        /// <summary>
        /// Gets or sets the calculation mode
        /// </summary>
        public CalculationMode CalculationMode { get; set; }
    }

    /// <summary>
    /// Structure representing workbook clone options
    /// </summary>
    public struct WorkbookCloneOptions
    {
        /// <summary>
        /// Gets or sets whether to clone worksheets
        /// </summary>
        public bool CloneWorksheets { get; set; }

        /// <summary>
        /// Gets or sets whether to clone metadata
        /// </summary>
        public bool CloneMetadata { get; set; }

        /// <summary>
        /// Gets or sets whether to clone formatting
        /// </summary>
        public bool CloneFormatting { get; set; }

        /// <summary>
        /// Gets or sets whether to clone formulas
        /// </summary>
        public bool CloneFormulas { get; set; }

        /// <summary>
        /// Gets or sets whether to clone data only
        /// </summary>
        public bool CloneDataOnly { get; set; }

        /// <summary>
        /// Gets or sets whether to clone protection settings
        /// </summary>
        public bool CloneProtection { get; set; }
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
