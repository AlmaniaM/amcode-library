using System;
using System.Collections.Generic;
using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx;
using IWorksheet = AMCode.Documents.Xlsx.IWorksheet;
using IWorkbook = AMCode.Documents.Xlsx.IWorkbook;

namespace AMCode.Documents.Xlsx.Infrastructure.Adapters
{
    /// <summary>
    /// Adapter for workbook content operations
    /// Wraps existing IWorkbook from Xlsx/Interfaces and provides domain interface implementation
    /// </summary>
    public class WorkbookContentAdapter : IWorkbookContent
    {
        private readonly IWorkbook _workbook;

        /// <summary>
        /// Initializes a new instance of the WorkbookContentAdapter class
        /// </summary>
        /// <param name="workbook">The underlying workbook implementation</param>
        /// <exception cref="ArgumentNullException">Thrown when workbook is null</exception>
        public WorkbookContentAdapter(IWorkbook workbook)
        {
            _workbook = workbook ?? throw new ArgumentNullException(nameof(workbook));
        }

        /// <summary>
        /// Gets the collection of worksheets in this workbook
        /// </summary>
        public IEnumerable<IWorksheet> Worksheets => _workbook.Worksheets;

        /// <summary>
        /// Gets the number of worksheets in this workbook
        /// </summary>
        public int WorksheetCount => _workbook.WorksheetCount;

        /// <summary>
        /// Gets a worksheet by index
        /// </summary>
        /// <param name="index">The worksheet index (0-based)</param>
        /// <returns>Result containing the worksheet or error information</returns>
        public Result<IWorksheet> GetWorksheet(int index)
        {
            try
            {
                var result = _workbook.GetWorksheet(index);
                if (result.IsSuccess)
                {
                    return Result<IWorksheet>.Success(result.Value);
                }
                return Result<IWorksheet>.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result<IWorksheet>.Failure($"Error getting worksheet at index {index}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a worksheet by name
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>Result containing the worksheet or error information</returns>
        public Result<IWorksheet> GetWorksheet(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result<IWorksheet>.Failure("Worksheet name cannot be null or empty");
                }

                var result = _workbook.GetWorksheet(name);
                if (result.IsSuccess)
                {
                    return Result<IWorksheet>.Success(result.Value);
                }
                return Result<IWorksheet>.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result<IWorksheet>.Failure($"Error getting worksheet '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Adds a new worksheet to the workbook
        /// </summary>
        /// <param name="name">The name of the new worksheet</param>
        /// <returns>Result containing the new worksheet or error information</returns>
        public Result<IWorksheet> AddWorksheet(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result<IWorksheet>.Failure("Worksheet name cannot be null or empty");
                }

                var result = _workbook.AddWorksheet(name);
                if (result.IsSuccess)
                {
                    return Result<IWorksheet>.Success(result.Value);
                }
                return Result<IWorksheet>.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result<IWorksheet>.Failure($"Error adding worksheet '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Removes a worksheet from the workbook
        /// </summary>
        /// <param name="index">The worksheet index (0-based)</param>
        /// <returns>Result indicating success or failure</returns>
        public Result RemoveWorksheet(int index)
        {
            try
            {
                if (index < 0)
                {
                    return Result.Failure("Worksheet index cannot be negative");
                }

                var result = _workbook.RemoveWorksheet(index);
                if (result.IsSuccess)
                {
                    return Result.Success();
                }
                return Result.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error removing worksheet at index {index}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Removes a worksheet from the workbook
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>Result indicating success or failure</returns>
        public Result RemoveWorksheet(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result.Failure("Worksheet name cannot be null or empty");
                }

                var result = _workbook.RemoveWorksheet(name);
                if (result.IsSuccess)
                {
                    return Result.Success();
                }
                return Result.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error removing worksheet '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Saves the workbook to the specified stream
        /// </summary>
        /// <param name="stream">The stream to save the workbook to</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SaveAs(Stream stream)
        {
            try
            {
                if (stream == null)
                {
                    return Result.Failure("Stream cannot be null");
                }

                var result = _workbook.SaveAs(stream);
                if (result.IsSuccess)
                {
                    return Result.Success();
                }
                return Result.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error saving workbook to stream: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Saves the workbook to the specified file path
        /// </summary>
        /// <param name="filePath">The file path to save the workbook to</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SaveAs(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    return Result.Failure("File path cannot be null or empty");
                }

                var result = _workbook.SaveAs(filePath);
                if (result.IsSuccess)
                {
                    return Result.Success();
                }
                return Result.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error saving workbook to '{filePath}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Saves the workbook with a new name
        /// </summary>
        /// <param name="stream">The stream to save the workbook to</param>
        /// <param name="fileName">The new file name</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SaveAs(Stream stream, string fileName)
        {
            try
            {
                if (stream == null)
                {
                    return Result.Failure("Stream cannot be null");
                }

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return Result.Failure("File name cannot be null or empty");
                }

                // For now, we'll use the basic SaveAs method
                // In a real implementation, this might involve additional logic
                var result = _workbook.SaveAs(stream);
                if (result.IsSuccess)
                {
                    return Result.Success();
                }
                return Result.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error saving workbook to stream with name '{fileName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Saves the workbook with a new name to the specified file path
        /// </summary>
        /// <param name="filePath">The file path to save the workbook to</param>
        /// <param name="fileName">The new file name</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SaveAs(string filePath, string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    return Result.Failure("File path cannot be null or empty");
                }

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return Result.Failure("File name cannot be null or empty");
                }

                // For now, we'll use the basic SaveAs method
                // In a real implementation, this might involve additional logic
                var result = _workbook.SaveAs(filePath);
                if (result.IsSuccess)
                {
                    return Result.Success();
                }
                return Result.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error saving workbook to '{filePath}' with name '{fileName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if a worksheet exists with the specified name
        /// </summary>
        /// <param name="name">The worksheet name to check</param>
        /// <returns>True if the worksheet exists, false otherwise</returns>
        public bool HasWorksheet(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return false;
                }

                return _workbook.HasWorksheet(name);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the index of a worksheet by name
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>Result containing the worksheet index or error information</returns>
        public Result<int> GetWorksheetIndex(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result<int>.Failure("Worksheet name cannot be null or empty");
                }

                var result = _workbook.GetWorksheetIndex(name);
                if (result.IsSuccess)
                {
                    return Result<int>.Success(result.Value);
                }
                return Result<int>.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error getting worksheet index for '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Renames a worksheet
        /// </summary>
        /// <param name="oldName">The current worksheet name</param>
        /// <param name="newName">The new worksheet name</param>
        /// <returns>Result indicating success or failure</returns>
        public Result RenameWorksheet(string oldName, string newName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(oldName))
                {
                    return Result.Failure("Old worksheet name cannot be null or empty");
                }

                if (string.IsNullOrWhiteSpace(newName))
                {
                    return Result.Failure("New worksheet name cannot be null or empty");
                }

                var result = _workbook.RenameWorksheet(oldName, newName);
                if (result.IsSuccess)
                {
                    return Result.Success();
                }
                return Result.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error renaming worksheet from '{oldName}' to '{newName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Renames a worksheet by index
        /// </summary>
        /// <param name="index">The worksheet index (0-based)</param>
        /// <param name="newName">The new worksheet name</param>
        /// <returns>Result indicating success or failure</returns>
        public Result RenameWorksheet(int index, string newName)
        {
            try
            {
                if (index < 0)
                {
                    return Result.Failure("Worksheet index cannot be negative");
                }

                if (string.IsNullOrWhiteSpace(newName))
                {
                    return Result.Failure("New worksheet name cannot be null or empty");
                }

                var result = _workbook.RenameWorksheet(index, newName);
                if (result.IsSuccess)
                {
                    return Result.Success();
                }
                return Result.Failure(result.Error);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error renaming worksheet at index {index} to '{newName}': {ex.Message}", ex);
            }
        }
    }
}
