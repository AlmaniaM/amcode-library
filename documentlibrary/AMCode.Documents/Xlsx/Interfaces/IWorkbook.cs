using System;
using System.Collections.Generic;
using System.IO;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Represents an Excel workbook
    /// </summary>
    public interface IWorkbook : IDisposable
    {
        /// <summary>
        /// Collection of worksheets
        /// </summary>
        IEnumerable<IWorksheet> Worksheets { get; }

        /// <summary>
        /// Gets the number of worksheets in this workbook
        /// </summary>
        int WorksheetCount { get; }

        /// <summary>
        /// Gets a worksheet by index
        /// </summary>
        /// <param name="index">The worksheet index (0-based)</param>
        /// <returns>Result containing the worksheet or error information</returns>
        Result<IWorksheet> GetWorksheet(int index);

        /// <summary>
        /// Gets a worksheet by name
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>Result containing the worksheet or error information</returns>
        Result<IWorksheet> GetWorksheet(string name);

        /// <summary>
        /// Adds a new worksheet to the workbook
        /// </summary>
        /// <param name="name">The name of the new worksheet</param>
        /// <returns>Result containing the new worksheet or error information</returns>
        Result<IWorksheet> AddWorksheet(string name);

        /// <summary>
        /// Removes a worksheet from the workbook
        /// </summary>
        /// <param name="index">The worksheet index (0-based)</param>
        /// <returns>Result indicating success or failure</returns>
        Result RemoveWorksheet(int index);

        /// <summary>
        /// Removes a worksheet from the workbook
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>Result indicating success or failure</returns>
        Result RemoveWorksheet(string name);

        /// <summary>
        /// Saves the workbook to the specified stream
        /// </summary>
        /// <param name="stream">The stream to save the workbook to</param>
        /// <returns>Result indicating success or failure</returns>
        Result SaveAs(Stream stream);

        /// <summary>
        /// Saves the workbook to the specified file path
        /// </summary>
        /// <param name="filePath">The file path to save the workbook to</param>
        /// <returns>Result indicating success or failure</returns>
        Result SaveAs(string filePath);

        /// <summary>
        /// Checks if a worksheet exists with the specified name
        /// </summary>
        /// <param name="name">The worksheet name to check</param>
        /// <returns>True if the worksheet exists, false otherwise</returns>
        bool HasWorksheet(string name);

        /// <summary>
        /// Gets the index of a worksheet by name
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>Result containing the worksheet index or error information</returns>
        Result<int> GetWorksheetIndex(string name);

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

        /// <summary>
        /// Close the workbook
        /// </summary>
        void Close();
    }
}
