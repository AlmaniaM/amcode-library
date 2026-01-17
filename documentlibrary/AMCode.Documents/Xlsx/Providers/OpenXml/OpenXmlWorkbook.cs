using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx;

namespace AMCode.Documents.Xlsx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of the workbook interface
    /// Wraps DocumentFormat.OpenXml.Packaging.SpreadsheetDocument for Excel operations
    /// </summary>
    public class OpenXmlWorkbook : IWorkbook
    {
        private SpreadsheetDocument _document;
        private WorkbookPart _workbookPart;
        private List<OpenXmlWorksheet> _worksheets;
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the OpenXmlWorkbook class
        /// </summary>
        /// <param name="document">The underlying SpreadsheetDocument</param>
        /// <exception cref="ArgumentNullException">Thrown when document is null</exception>
        public OpenXmlWorkbook(SpreadsheetDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
            _workbookPart = _document.WorkbookPart ?? throw new InvalidOperationException("WorkbookPart is null");
            _worksheets = new List<OpenXmlWorksheet>();
            InitializeWorksheets();
        }

        /// <summary>
        /// Gets the collection of worksheets in this workbook
        /// </summary>
        public IEnumerable<IWorksheet> Worksheets => _worksheets;

        /// <summary>
        /// Gets the number of worksheets in this workbook
        /// </summary>
        public int WorksheetCount => _worksheets.Count;

        /// <summary>
        /// Gets a worksheet by index
        /// </summary>
        /// <param name="index">The worksheet index (0-based)</param>
        /// <returns>Result containing the worksheet or error information</returns>
        public Result<IWorksheet> GetWorksheet(int index)
        {
            if (_disposed) return Result<IWorksheet>.Failure(new ObjectDisposedException(nameof(OpenXmlWorkbook)));
            if (index < 0 || index >= _worksheets.Count)
                return Result<IWorksheet>.Failure($"Worksheet index {index} is out of range. Valid range: 0-{_worksheets.Count - 1}");

            return Result<IWorksheet>.Success(_worksheets[index]);
        }

        /// <summary>
        /// Gets a worksheet by name
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>Result containing the worksheet or error information</returns>
        public Result<IWorksheet> GetWorksheet(string name)
        {
            if (_disposed) return Result<IWorksheet>.Failure(new ObjectDisposedException(nameof(OpenXmlWorkbook)));
            if (string.IsNullOrWhiteSpace(name))
                return Result<IWorksheet>.Failure("Worksheet name cannot be null or empty");

            var worksheet = _worksheets.FirstOrDefault(w => string.Equals(w.Name, name, StringComparison.OrdinalIgnoreCase));
            if (worksheet == null)
                return Result<IWorksheet>.Failure($"Worksheet '{name}' not found");

            return Result<IWorksheet>.Success(worksheet);
        }

        /// <summary>
        /// Adds a new worksheet to the workbook
        /// </summary>
        /// <param name="name">The name of the new worksheet</param>
        /// <returns>Result containing the new worksheet or error information</returns>
        public Result<IWorksheet> AddWorksheet(string name)
        {
            if (_disposed) return Result<IWorksheet>.Failure(new ObjectDisposedException(nameof(OpenXmlWorkbook)));
            if (string.IsNullOrWhiteSpace(name))
                return Result<IWorksheet>.Failure("Worksheet name cannot be null or empty");

            try
            {
                // Check if worksheet with this name already exists
                if (HasWorksheet(name))
                    return Result<IWorksheet>.Failure($"Worksheet '{name}' already exists");

                // Create new worksheet part
                var worksheetPart = _workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Add sheet to workbook
                var sheets = _workbookPart.Workbook.GetFirstChild<Sheets>();
                if (sheets == null)
                {
                    sheets = new Sheets();
                    _workbookPart.Workbook.AppendChild(sheets);
                }

                var sheet = new Sheet()
                {
                    Id = _workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (uint)(_worksheets.Count + 1),
                    Name = name
                };
                sheets.Append(sheet);

                // Create OpenXmlWorksheet wrapper
                var openXmlWorksheet = new OpenXmlWorksheet(worksheetPart, name, _worksheets.Count);
                _worksheets.Add(openXmlWorksheet);

                return Result<IWorksheet>.Success(openXmlWorksheet);
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
            if (_disposed) return Result.Failure(new ObjectDisposedException(nameof(OpenXmlWorkbook)));
            if (index < 0 || index >= _worksheets.Count)
                return Result.Failure($"Worksheet index {index} is out of range. Valid range: 0-{_worksheets.Count - 1}");

            try
            {
                var worksheet = _worksheets[index];
                var worksheetName = worksheet.Name;

                // Remove sheet from workbook
                var sheets = _workbookPart.Workbook.GetFirstChild<Sheets>();
                var sheet = sheets?.Elements<Sheet>().FirstOrDefault(s => s.Name == worksheetName);
                if (sheet != null)
                {
                    sheet.Remove();
                }

                // Remove worksheet part
                var worksheetPart = _workbookPart.WorksheetParts.FirstOrDefault(wp => wp == worksheet.WorksheetPart);
                if (worksheetPart != null)
                {
                    _workbookPart.DeletePart(worksheetPart);
                }

                // Remove from our collection
                _worksheets.RemoveAt(index);

                // Update indices for remaining worksheets
                for (int i = index; i < _worksheets.Count; i++)
                {
                    _worksheets[i].UpdateIndex(i);
                }

                return Result.Success();
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
            if (_disposed) return Result.Failure(new ObjectDisposedException(nameof(OpenXmlWorkbook)));
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure("Worksheet name cannot be null or empty");

            var index = _worksheets.FindIndex(w => string.Equals(w.Name, name, StringComparison.OrdinalIgnoreCase));
            if (index == -1)
                return Result.Failure($"Worksheet '{name}' not found");

            return RemoveWorksheet(index);
        }

        /// <summary>
        /// Saves the workbook to the specified stream
        /// </summary>
        /// <param name="stream">The stream to save the workbook to</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SaveAs(Stream stream)
        {
            if (_disposed) return Result.Failure(new ObjectDisposedException(nameof(OpenXmlWorkbook)));
            if (stream == null) return Result.Failure("Stream cannot be null");

            try
            {
                // OpenXML doesn't have SaveAs for streams, need to clone
                using (var clone = _document.Clone(stream))
                {
                    clone.Save();
                }
                return Result.Success();
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
            if (_disposed) return Result.Failure(new ObjectDisposedException(nameof(OpenXmlWorkbook)));
            if (string.IsNullOrWhiteSpace(filePath))
                return Result.Failure("File path cannot be null or empty");

            try
            {
                // OpenXML doesn't have SaveAs, need to clone to a new file
                using (var fileStream = File.Create(filePath))
                {
                    using (var clone = _document.Clone(fileStream))
                    {
                        clone.Save();
                    }
                }
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error saving workbook to file '{filePath}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if a worksheet exists with the specified name
        /// </summary>
        /// <param name="name">The worksheet name to check</param>
        /// <returns>True if the worksheet exists, false otherwise</returns>
        public bool HasWorksheet(string name)
        {
            if (_disposed) return false;
            if (string.IsNullOrWhiteSpace(name)) return false;

            return _worksheets.Any(w => string.Equals(w.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the index of a worksheet by name
        /// </summary>
        /// <param name="name">The worksheet name</param>
        /// <returns>Result containing the worksheet index or error information</returns>
        public Result<int> GetWorksheetIndex(string name)
        {
            if (_disposed) return Result<int>.Failure(new ObjectDisposedException(nameof(OpenXmlWorkbook)));
            if (string.IsNullOrWhiteSpace(name))
                return Result<int>.Failure("Worksheet name cannot be null or empty");

            var index = _worksheets.FindIndex(w => string.Equals(w.Name, name, StringComparison.OrdinalIgnoreCase));
            if (index == -1)
                return Result<int>.Failure($"Worksheet '{name}' not found");

            return Result<int>.Success(index);
        }

        /// <summary>
        /// Renames a worksheet
        /// </summary>
        /// <param name="oldName">The current worksheet name</param>
        /// <param name="newName">The new worksheet name</param>
        /// <returns>Result indicating success or failure</returns>
        public Result RenameWorksheet(string oldName, string newName)
        {
            if (_disposed) return Result.Failure(new ObjectDisposedException(nameof(OpenXmlWorkbook)));
            if (string.IsNullOrWhiteSpace(oldName))
                return Result.Failure("Old worksheet name cannot be null or empty");
            if (string.IsNullOrWhiteSpace(newName))
                return Result.Failure("New worksheet name cannot be null or empty");

            try
            {
                // Check if old worksheet exists
                var worksheet = _worksheets.FirstOrDefault(w => string.Equals(w.Name, oldName, StringComparison.OrdinalIgnoreCase));
                if (worksheet == null)
                    return Result.Failure($"Worksheet '{oldName}' not found");

                // Check if new name already exists
                if (HasWorksheet(newName))
                    return Result.Failure($"Worksheet '{newName}' already exists");

                // Update the sheet name in the workbook
                var sheets = _workbookPart.Workbook.GetFirstChild<Sheets>();
                var sheet = sheets?.Elements<Sheet>().FirstOrDefault(s => s.Name == oldName);
                if (sheet != null)
                {
                    sheet.Name = newName;
                }

                // Update the worksheet name
                worksheet.UpdateName(newName);

                return Result.Success();
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
            if (_disposed) return Result.Failure(new ObjectDisposedException(nameof(OpenXmlWorkbook)));
            if (index < 0 || index >= _worksheets.Count)
                return Result.Failure($"Worksheet index {index} is out of range. Valid range: 0-{_worksheets.Count - 1}");
            if (string.IsNullOrWhiteSpace(newName))
                return Result.Failure("New worksheet name cannot be null or empty");

            try
            {
                var worksheet = _worksheets[index];
                var oldName = worksheet.Name;

                // Check if new name already exists
                if (HasWorksheet(newName))
                    return Result.Failure($"Worksheet '{newName}' already exists");

                // Update the sheet name in the workbook
                var sheets = _workbookPart.Workbook.GetFirstChild<Sheets>();
                var sheet = sheets?.Elements<Sheet>().FirstOrDefault(s => s.Name == oldName);
                if (sheet != null)
                {
                    sheet.Name = newName;
                }

                // Update the worksheet name
                worksheet.UpdateName(newName);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error renaming worksheet at index {index} to '{newName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Closes the workbook
        /// </summary>
        public void Close()
        {
            if (!_disposed)
            {
                _document?.Dispose();
                _disposed = true;
            }
        }

        /// <summary>
        /// Disposes of the workbook and releases all resources
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Close();
                _document?.Dispose();
                _disposed = true;
            }
        }

        /// <summary>
        /// Initializes the worksheets collection from the workbook
        /// </summary>
        private void InitializeWorksheets()
        {
            try
            {
                var sheets = _workbookPart.Workbook.GetFirstChild<Sheets>();
                if (sheets == null) return;

                var worksheetParts = _workbookPart.WorksheetParts.ToList();
                var sheetElements = sheets.Elements<Sheet>().ToList();

                for (int i = 0; i < sheetElements.Count; i++)
                {
                    var sheet = sheetElements[i];
                    var worksheetPart = _workbookPart.GetPartById(sheet.Id) as WorksheetPart;
                    
                    if (worksheetPart != null)
                    {
                        var openXmlWorksheet = new OpenXmlWorksheet(worksheetPart, sheet.Name, i);
                        _worksheets.Add(openXmlWorksheet);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but don't throw - allow workbook to be created with empty worksheets
                System.Diagnostics.Debug.WriteLine($"Error initializing worksheets: {ex.Message}");
            }
        }
    }
}
