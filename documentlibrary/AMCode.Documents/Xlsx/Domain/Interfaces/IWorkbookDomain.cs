using System;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Xlsx.Domain.Interfaces
{
    /// <summary>
    /// Domain interface for workbook operations
    /// Combines content, metadata, and domain-specific operations
    /// </summary>
    public interface IWorkbookDomain : IWorkbookContent, IWorkbookMetadata, IDisposable
    {
        /// <summary>
        /// Gets the unique identifier for this workbook
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the creation timestamp of this workbook
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// Gets the last modification timestamp of this workbook
        /// </summary>
        DateTime LastModified { get; }

        /// <summary>
        /// Closes the workbook and releases resources
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result Close();

        /// <summary>
        /// Gets a value indicating whether the workbook is disposed
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Refreshes the workbook data and updates timestamps
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result Refresh();

        /// <summary>
        /// Validates the workbook structure and content
        /// </summary>
        /// <returns>Result indicating validation success or failure with details</returns>
        Result Validate();

        /// <summary>
        /// Gets the workbook size in bytes
        /// </summary>
        /// <returns>Result containing the size in bytes or error information</returns>
        Result<long> GetSize();

        /// <summary>
        /// Gets the workbook file format version
        /// </summary>
        /// <returns>Result containing the format version or error information</returns>
        Result<string> GetFormatVersion();

        /// <summary>
        /// Checks if the workbook is read-only
        /// </summary>
        /// <returns>True if the workbook is read-only, false otherwise</returns>
        bool IsReadOnly { get; }

        /// <summary>
        /// Sets the workbook to read-only mode
        /// </summary>
        /// <param name="readOnly">True to make read-only, false to make writable</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetReadOnly(bool readOnly);

        /// <summary>
        /// Gets the workbook protection status
        /// </summary>
        /// <returns>True if the workbook is protected, false otherwise</returns>
        bool IsProtected { get; }

        /// <summary>
        /// Protects the workbook with a password
        /// </summary>
        /// <param name="password">The protection password</param>
        /// <returns>Result indicating success or failure</returns>
        Result Protect(string password);

        /// <summary>
        /// Unprotects the workbook with a password
        /// </summary>
        /// <param name="password">The protection password</param>
        /// <returns>Result indicating success or failure</returns>
        Result Unprotect(string password);

        /// <summary>
        /// Gets the workbook calculation mode
        /// </summary>
        /// <returns>Result containing the calculation mode or error information</returns>
        Result<CalculationMode> GetCalculationMode();

        /// <summary>
        /// Sets the workbook calculation mode
        /// </summary>
        /// <param name="mode">The calculation mode</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetCalculationMode(CalculationMode mode);

        /// <summary>
        /// Forces calculation of all formulas in the workbook
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result Calculate();

        /// <summary>
        /// Gets the workbook's default font settings
        /// </summary>
        /// <returns>Result containing the font settings or error information</returns>
        Result<FontSettings> GetDefaultFont();

        /// <summary>
        /// Sets the workbook's default font settings
        /// </summary>
        /// <param name="fontSettings">The font settings</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetDefaultFont(FontSettings fontSettings);
    }

    /// <summary>
    /// Enumeration of workbook calculation modes
    /// </summary>
    public enum CalculationMode
    {
        /// <summary>
        /// Automatic calculation
        /// </summary>
        Automatic,

        /// <summary>
        /// Automatic calculation except for data tables
        /// </summary>
        AutomaticExceptTables,

        /// <summary>
        /// Manual calculation
        /// </summary>
        Manual
    }

    /// <summary>
    /// Structure representing font settings
    /// </summary>
    public struct FontSettings
    {
        /// <summary>
        /// Gets or sets the font name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the font size
        /// </summary>
        public double Size { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the font is bold
        /// </summary>
        public bool Bold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the font is italic
        /// </summary>
        public bool Italic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the font is underlined
        /// </summary>
        public bool Underline { get; set; }

        /// <summary>
        /// Gets or sets the font color
        /// </summary>
        public string Color { get; set; }
    }
}
