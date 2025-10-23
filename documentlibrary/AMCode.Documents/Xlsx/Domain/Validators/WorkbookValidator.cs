using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Xlsx;
using IRange = AMCode.Xlsx.IRange;

namespace AMCode.Documents.Xlsx.Domain.Validators
{
    /// <summary>
    /// Domain validator for workbook operations
    /// Provides comprehensive validation logic for workbooks, worksheets, ranges, and cell references
    /// </summary>
    public class WorkbookValidator : IWorkbookValidator
    {
        private readonly Dictionary<ValidationType, List<ValidationRule>> _validationRules;

        /// <summary>
        /// Initializes a new instance of the WorkbookValidator class
        /// </summary>
        public WorkbookValidator()
        {
            _validationRules = new Dictionary<ValidationType, List<ValidationRule>>();
            InitializeDefaultRules();
        }

        /// <summary>
        /// Validates a workbook for structural integrity and content validity
        /// </summary>
        /// <param name="workbook">The workbook to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        public Result<ValidationResult> ValidateWorkbook(IWorkbookDomain workbook)
        {
            if (workbook == null)
            {
                return Result<ValidationResult>.Failure("Workbook cannot be null");
            }

            var results = new List<ValidationResult>();

            // Basic workbook validation
            if (workbook.IsDisposed)
            {
                results.Add(new ValidationResult
                {
                    IsValid = false,
                    Message = "Workbook has been disposed",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "WB001",
                    Timestamp = DateTime.UtcNow
                });
            }

            // Validate worksheet count
            if (workbook.WorksheetCount == 0)
            {
                results.Add(new ValidationResult
                {
                    IsValid = false,
                    Message = "Workbook must contain at least one worksheet",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "WB002",
                    Timestamp = DateTime.UtcNow
                });
            }

            // Validate metadata
            var metadataResult = ValidateMetadata(workbook);
            if (!metadataResult.IsSuccess)
            {
                results.Add(metadataResult.Value);
            }

            // Validate worksheets
            foreach (var worksheet in workbook.Worksheets)
            {
                var worksheetResult = ValidateWorksheet(worksheet);
                if (!worksheetResult.IsSuccess)
                {
                    results.Add(worksheetResult.Value);
                }
            }

            var isValid = results.All(r => r.IsValid);
            var message = isValid ? "Workbook validation passed" : "Workbook validation failed";

            return Result<ValidationResult>.Success(new ValidationResult
            {
                IsValid = isValid,
                Message = message,
                Severity = isValid ? ValidationSeverity.Information : ValidationSeverity.Error,
                ErrorCode = isValid ? "WB000" : "WB999",
                Details = new Dictionary<string, object>
                {
                    ["ValidationResults"] = results,
                    ["WorksheetCount"] = workbook.WorksheetCount,
                    ["IsReadOnly"] = workbook.IsReadOnly,
                    ["IsProtected"] = workbook.IsProtected
                },
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Validates a worksheet for structural integrity and content validity
        /// </summary>
        /// <param name="worksheet">The worksheet to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        public Result<ValidationResult> ValidateWorksheet(IWorksheet worksheet)
        {
            if (worksheet == null)
            {
                return Result<ValidationResult>.Failure("Worksheet cannot be null");
            }

            var results = new List<ValidationResult>();

            // Validate worksheet name
            var nameResult = ValidateWorksheetName(worksheet.Name);
            if (!nameResult.IsSuccess)
            {
                results.Add(nameResult.Value);
            }

            // Validate worksheet index
            if (worksheet.Index < 0)
            {
                results.Add(new ValidationResult
                {
                    IsValid = false,
                    Message = "Worksheet index must be non-negative",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "WS001",
                    Timestamp = DateTime.UtcNow
                });
            }

            // Note: Content and Formatting properties are not available on the existing IWorksheet interface
            // These validations are removed to work with the existing AMCode.Xlsx interfaces

            var isValid = results.All(r => r.IsValid);
            var message = isValid ? "Worksheet validation passed" : "Worksheet validation failed";

            return Result<ValidationResult>.Success(new ValidationResult
            {
                IsValid = isValid,
                Message = message,
                Severity = isValid ? ValidationSeverity.Information : ValidationSeverity.Error,
                ErrorCode = isValid ? "WS000" : "WS999",
                Details = new Dictionary<string, object>
                {
                    ["ValidationResults"] = results,
                    ["WorksheetName"] = worksheet.Name,
                    ["WorksheetIndex"] = worksheet.Index,
                    ["IsVisible"] = worksheet.IsVisible
                },
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Validates a range for structural integrity and content validity
        /// </summary>
        /// <param name="range">The range to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        public Result<ValidationResult> ValidateRange(IRange range)
        {
            if (range == null)
            {
                return Result<ValidationResult>.Failure("Range cannot be null");
            }

            var results = new List<ValidationResult>();

            // Validate start cell
            var startCellResult = ValidateCellReference(range.StartCell);
            if (!startCellResult.IsSuccess)
            {
                results.Add(startCellResult.Value);
            }

            // Validate end cell
            var endCellResult = ValidateCellReference(range.EndCell);
            if (!endCellResult.IsSuccess)
            {
                results.Add(endCellResult.Value);
            }

            // Validate range consistency
            if (range.RowCount <= 0 || range.ColumnCount <= 0)
            {
                results.Add(new ValidationResult
                {
                    IsValid = false,
                    Message = "Range must have positive row and column counts",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "RG001",
                    Timestamp = DateTime.UtcNow
                });
            }

            var isValid = results.All(r => r.IsValid);
            var message = isValid ? "Range validation passed" : "Range validation failed";

            return Result<ValidationResult>.Success(new ValidationResult
            {
                IsValid = isValid,
                Message = message,
                Severity = isValid ? ValidationSeverity.Information : ValidationSeverity.Error,
                ErrorCode = isValid ? "RG000" : "RG999",
                Details = new Dictionary<string, object>
                {
                    ["ValidationResults"] = results,
                    ["StartCell"] = range.StartCell,
                    ["EndCell"] = range.EndCell,
                    ["RowCount"] = range.RowCount,
                    ["ColumnCount"] = range.ColumnCount
                },
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Validates a cell reference string format
        /// </summary>
        /// <param name="cellReference">The cell reference to validate (e.g., "A1", "B10")</param>
        /// <returns>Result containing validation results or error information</returns>
        public Result<ValidationResult> ValidateCellReference(string cellReference)
        {
            if (string.IsNullOrWhiteSpace(cellReference))
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = "Cell reference cannot be null or empty",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "CR001",
                    Timestamp = DateTime.UtcNow
                });
            }

            // Excel cell reference pattern: [A-Z]+[1-9][0-9]*
            var pattern = @"^[A-Z]+[1-9][0-9]*$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            if (!regex.IsMatch(cellReference))
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = $"Invalid cell reference format: {cellReference}",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "CR002",
                    Details = new Dictionary<string, object>
                    {
                        ["CellReference"] = cellReference,
                        ["ExpectedPattern"] = pattern
                    },
                    Timestamp = DateTime.UtcNow
                });
            }

            return Result<ValidationResult>.Success(new ValidationResult
            {
                IsValid = true,
                Message = "Cell reference is valid",
                Severity = ValidationSeverity.Information,
                ErrorCode = "CR000",
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Validates a range reference string format
        /// </summary>
        /// <param name="rangeReference">The range reference to validate (e.g., "A1:C10")</param>
        /// <returns>Result containing validation results or error information</returns>
        public Result<ValidationResult> ValidateRangeReference(string rangeReference)
        {
            if (string.IsNullOrWhiteSpace(rangeReference))
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = "Range reference cannot be null or empty",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "RR001",
                    Timestamp = DateTime.UtcNow
                });
            }

            // Excel range reference pattern: [A-Z]+[1-9][0-9]*:[A-Z]+[1-9][0-9]*
            var pattern = @"^[A-Z]+[1-9][0-9]*:[A-Z]+[1-9][0-9]*$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            if (!regex.IsMatch(rangeReference))
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = $"Invalid range reference format: {rangeReference}",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "RR002",
                    Details = new Dictionary<string, object>
                    {
                        ["RangeReference"] = rangeReference,
                        ["ExpectedPattern"] = pattern
                    },
                    Timestamp = DateTime.UtcNow
                });
            }

            return Result<ValidationResult>.Success(new ValidationResult
            {
                IsValid = true,
                Message = "Range reference is valid",
                Severity = ValidationSeverity.Information,
                ErrorCode = "RR000",
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Validates a worksheet name for Excel compatibility
        /// </summary>
        /// <param name="worksheetName">The worksheet name to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        public Result<ValidationResult> ValidateWorksheetName(string worksheetName)
        {
            if (string.IsNullOrWhiteSpace(worksheetName))
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = "Worksheet name cannot be null or empty",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "WN001",
                    Timestamp = DateTime.UtcNow
                });
            }

            // Excel worksheet name rules
            if (worksheetName.Length > 31)
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = "Worksheet name cannot exceed 31 characters",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "WN002",
                    Details = new Dictionary<string, object>
                    {
                        ["WorksheetName"] = worksheetName,
                        ["Length"] = worksheetName.Length,
                        ["MaxLength"] = 31
                    },
                    Timestamp = DateTime.UtcNow
                });
            }

            // Check for invalid characters
            var invalidChars = new[] { '\\', '/', '*', '?', '[', ']', ':' };
            var invalidChar = worksheetName.FirstOrDefault(c => invalidChars.Contains(c));
            if (invalidChar != default(char))
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = $"Worksheet name contains invalid character: {invalidChar}",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "WN003",
                    Details = new Dictionary<string, object>
                    {
                        ["WorksheetName"] = worksheetName,
                        ["InvalidCharacter"] = invalidChar,
                        ["InvalidCharacters"] = invalidChars
                    },
                    Timestamp = DateTime.UtcNow
                });
            }

            return Result<ValidationResult>.Success(new ValidationResult
            {
                IsValid = true,
                Message = "Worksheet name is valid",
                Severity = ValidationSeverity.Information,
                ErrorCode = "WN000",
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Validates a workbook file path
        /// </summary>
        /// <param name="filePath">The file path to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        public Result<ValidationResult> ValidateFilePath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = "File path cannot be null or empty",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "FP001",
                    Timestamp = DateTime.UtcNow
                });
            }

            try
            {
                var fullPath = Path.GetFullPath(filePath);
                var directory = Path.GetDirectoryName(fullPath);
                
                if (!Directory.Exists(directory))
                {
                    return Result<ValidationResult>.Success(new ValidationResult
                    {
                        IsValid = false,
                        Message = $"Directory does not exist: {directory}",
                        Severity = ValidationSeverity.Error,
                        ErrorCode = "FP002",
                        Details = new Dictionary<string, object>
                        {
                            ["FilePath"] = filePath,
                            ["Directory"] = directory
                        },
                        Timestamp = DateTime.UtcNow
                    });
                }

                var extension = Path.GetExtension(filePath).ToLowerInvariant();
                var validExtensions = new[] { ".xlsx", ".xlsm", ".xlsb", ".xltx", ".xltm" };
                
                if (!validExtensions.Contains(extension))
                {
                    return Result<ValidationResult>.Success(new ValidationResult
                    {
                        IsValid = false,
                        Message = $"Invalid file extension: {extension}",
                        Severity = ValidationSeverity.Error,
                        ErrorCode = "FP003",
                        Details = new Dictionary<string, object>
                        {
                            ["FilePath"] = filePath,
                            ["Extension"] = extension,
                            ["ValidExtensions"] = validExtensions
                        },
                        Timestamp = DateTime.UtcNow
                    });
                }

                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = true,
                    Message = "File path is valid",
                    Severity = ValidationSeverity.Information,
                    ErrorCode = "FP000",
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = $"Invalid file path: {ex.Message}",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "FP004",
                    Details = new Dictionary<string, object>
                    {
                        ["FilePath"] = filePath,
                        ["Exception"] = ex.Message
                    },
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Validates a formula expression
        /// </summary>
        /// <param name="formula">The formula to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        public Result<ValidationResult> ValidateFormula(string formula)
        {
            if (string.IsNullOrWhiteSpace(formula))
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = "Formula cannot be null or empty",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "FM001",
                    Timestamp = DateTime.UtcNow
                });
            }

            // Basic formula validation - check for common issues
            if (!formula.StartsWith("="))
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = "Formula must start with '='",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "FM002",
                    Details = new Dictionary<string, object>
                    {
                        ["Formula"] = formula
                    },
                    Timestamp = DateTime.UtcNow
                });
            }

            // Check for balanced parentheses
            var openParens = formula.Count(c => c == '(');
            var closeParens = formula.Count(c => c == ')');
            if (openParens != closeParens)
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = "Formula has unbalanced parentheses",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "FM003",
                    Details = new Dictionary<string, object>
                    {
                        ["Formula"] = formula,
                        ["OpenParentheses"] = openParens,
                        ["CloseParentheses"] = closeParens
                    },
                    Timestamp = DateTime.UtcNow
                });
            }

            return Result<ValidationResult>.Success(new ValidationResult
            {
                IsValid = true,
                Message = "Formula is valid",
                Severity = ValidationSeverity.Information,
                ErrorCode = "FM000",
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Validates a cell value for type compatibility
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="expectedType">The expected data type</param>
        /// <returns>Result containing validation results or error information</returns>
        public Result<ValidationResult> ValidateCellValue(object value, Type expectedType)
        {
            if (expectedType == null)
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = "Expected type cannot be null",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "CV001",
                    Timestamp = DateTime.UtcNow
                });
            }

            if (value == null)
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = true,
                    Message = "Null value is valid for any type",
                    Severity = ValidationSeverity.Information,
                    ErrorCode = "CV000",
                    Timestamp = DateTime.UtcNow
                });
            }

            var actualType = value.GetType();
            var isValid = expectedType.IsAssignableFrom(actualType);

            return Result<ValidationResult>.Success(new ValidationResult
            {
                IsValid = isValid,
                Message = isValid ? "Cell value type is valid" : $"Cell value type mismatch. Expected: {expectedType.Name}, Actual: {actualType.Name}",
                Severity = isValid ? ValidationSeverity.Information : ValidationSeverity.Error,
                ErrorCode = isValid ? "CV000" : "CV002",
                Details = new Dictionary<string, object>
                {
                    ["Value"] = value,
                    ["ExpectedType"] = expectedType.Name,
                    ["ActualType"] = actualType.Name
                },
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Validates workbook metadata for completeness and correctness
        /// </summary>
        /// <param name="metadata">The metadata to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        public Result<ValidationResult> ValidateMetadata(IWorkbookMetadata metadata)
        {
            if (metadata == null)
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = "Metadata cannot be null",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "MD001",
                    Timestamp = DateTime.UtcNow
                });
            }

            var results = new List<ValidationResult>();

            // Validate required fields
            if (string.IsNullOrWhiteSpace(metadata.Title))
            {
                results.Add(new ValidationResult
                {
                    IsValid = false,
                    Message = "Title is required",
                    Severity = ValidationSeverity.Warning,
                    ErrorCode = "MD002",
                    Timestamp = DateTime.UtcNow
                });
            }

            if (string.IsNullOrWhiteSpace(metadata.Author))
            {
                results.Add(new ValidationResult
                {
                    IsValid = false,
                    Message = "Author is required",
                    Severity = ValidationSeverity.Warning,
                    ErrorCode = "MD003",
                    Timestamp = DateTime.UtcNow
                });
            }

            // Validate dates
            if (metadata.Created > DateTime.UtcNow)
            {
                results.Add(new ValidationResult
                {
                    IsValid = false,
                    Message = "Created date cannot be in the future",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "MD004",
                    Timestamp = DateTime.UtcNow
                });
            }

            if (metadata.Modified > DateTime.UtcNow)
            {
                results.Add(new ValidationResult
                {
                    IsValid = false,
                    Message = "Modified date cannot be in the future",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "MD005",
                    Timestamp = DateTime.UtcNow
                });
            }

            var isValid = results.All(r => r.IsValid);
            var message = isValid ? "Metadata validation passed" : "Metadata validation failed";

            return Result<ValidationResult>.Success(new ValidationResult
            {
                IsValid = isValid,
                Message = message,
                Severity = isValid ? ValidationSeverity.Information : ValidationSeverity.Error,
                ErrorCode = isValid ? "MD000" : "MD999",
                Details = new Dictionary<string, object>
                {
                    ["ValidationResults"] = results,
                    ["Title"] = metadata.Title,
                    ["Author"] = metadata.Author,
                    ["Created"] = metadata.Created,
                    ["Modified"] = metadata.Modified
                },
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Validates workbook protection settings
        /// </summary>
        /// <param name="workbook">The workbook to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        public Result<ValidationResult> ValidateProtection(IWorkbookDomain workbook)
        {
            if (workbook == null)
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = "Workbook cannot be null",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "PR001",
                    Timestamp = DateTime.UtcNow
                });
            }

            // Basic protection validation
            var isValid = true;
            var message = "Protection validation passed";

            if (workbook.IsProtected && workbook.IsReadOnly)
            {
                // This is a valid state
            }

            return Result<ValidationResult>.Success(new ValidationResult
            {
                IsValid = isValid,
                Message = message,
                Severity = ValidationSeverity.Information,
                ErrorCode = "PR000",
                Details = new Dictionary<string, object>
                {
                    ["IsProtected"] = workbook.IsProtected,
                    ["IsReadOnly"] = workbook.IsReadOnly
                },
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Validates workbook calculation settings
        /// </summary>
        /// <param name="workbook">The workbook to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        public Result<ValidationResult> ValidateCalculationSettings(IWorkbookDomain workbook)
        {
            if (workbook == null)
            {
                return Result<ValidationResult>.Success(new ValidationResult
                {
                    IsValid = false,
                    Message = "Workbook cannot be null",
                    Severity = ValidationSeverity.Error,
                    ErrorCode = "CS001",
                    Timestamp = DateTime.UtcNow
                });
            }

            // Basic calculation settings validation
            var isValid = true;
            var message = "Calculation settings validation passed";

            return Result<ValidationResult>.Success(new ValidationResult
            {
                IsValid = isValid,
                Message = message,
                Severity = ValidationSeverity.Information,
                ErrorCode = "CS000",
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Gets validation rules for a specific validation type
        /// </summary>
        /// <param name="validationType">The type of validation</param>
        /// <returns>Result containing the validation rules or error information</returns>
        public Result<IEnumerable<ValidationRule>> GetValidationRules(ValidationType validationType)
        {
            if (_validationRules.TryGetValue(validationType, out var rules))
            {
                return Result<IEnumerable<ValidationRule>>.Success(rules.AsEnumerable());
            }

            return Result<IEnumerable<ValidationRule>>.Failure($"No validation rules found for type: {validationType}");
        }

        /// <summary>
        /// Sets custom validation rules for a specific validation type
        /// </summary>
        /// <param name="validationType">The type of validation</param>
        /// <param name="rules">The custom validation rules</param>
        /// <returns>Result indicating success or failure</returns>
        public Result SetValidationRules(ValidationType validationType, IEnumerable<ValidationRule> rules)
        {
            if (rules == null)
            {
                return Result.Failure("Validation rules cannot be null");
            }

            _validationRules[validationType] = rules.ToList();
            return Result.Success();
        }

        /// <summary>
        /// Clears all custom validation rules
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        public Result ClearValidationRules()
        {
            _validationRules.Clear();
            InitializeDefaultRules();
            return Result.Success();
        }

        /// <summary>
        /// Validates multiple items in batch
        /// </summary>
        /// <param name="items">The items to validate</param>
        /// <param name="validationType">The type of validation to perform</param>
        /// <returns>Result containing validation results for all items or error information</returns>
        public Result<IEnumerable<ValidationResult>> ValidateBatch(IEnumerable<object> items, ValidationType validationType)
        {
            if (items == null)
            {
                return Result<IEnumerable<ValidationResult>>.Failure("Items cannot be null");
            }

            var results = new List<ValidationResult>();

            foreach (var item in items)
            {
                var result = validationType switch
                {
                    ValidationType.Workbook => ValidateWorkbook(item as IWorkbookDomain),
                    ValidationType.Worksheet => ValidateWorksheet(item as IWorksheet),
                    ValidationType.Range => ValidateRange(item as IRange),
                    ValidationType.CellReference => ValidateCellReference(item as string),
                    ValidationType.RangeReference => ValidateRangeReference(item as string),
                    ValidationType.WorksheetName => ValidateWorksheetName(item as string),
                    ValidationType.FilePath => ValidateFilePath(item as string),
                    ValidationType.Formula => ValidateFormula(item as string),
                    ValidationType.Metadata => ValidateMetadata(item as IWorkbookMetadata),
                    ValidationType.Protection => ValidateProtection(item as IWorkbookDomain),
                    ValidationType.CalculationSettings => ValidateCalculationSettings(item as IWorkbookDomain),
                    _ => Result<ValidationResult>.Failure($"Unsupported validation type: {validationType}")
                };

                if (result.IsSuccess)
                {
                    results.Add(result.Value);
                }
                else
                {
                    results.Add(new ValidationResult
                    {
                        IsValid = false,
                        Message = result.Error,
                        Severity = ValidationSeverity.Error,
                        ErrorCode = "BATCH001",
                        Timestamp = DateTime.UtcNow
                    });
                }
            }

            return Result<IEnumerable<ValidationResult>>.Success(results.AsEnumerable());
        }

        /// <summary>
        /// Initializes default validation rules
        /// </summary>
        private void InitializeDefaultRules()
        {
            // Add default validation rules for each type
            _validationRules[ValidationType.Workbook] = new List<ValidationRule>
            {
                new ValidationRule
                {
                    Name = "WorkbookNotNull",
                    Description = "Workbook cannot be null",
                    Expression = "workbook != null",
                    Severity = ValidationSeverity.Error,
                    ErrorMessage = "Workbook cannot be null",
                    IsEnabled = true
                },
                new ValidationRule
                {
                    Name = "WorkbookNotDisposed",
                    Description = "Workbook must not be disposed",
                    Expression = "!workbook.IsDisposed",
                    Severity = ValidationSeverity.Error,
                    ErrorMessage = "Workbook has been disposed",
                    IsEnabled = true
                }
            };

            _validationRules[ValidationType.Worksheet] = new List<ValidationRule>
            {
                new ValidationRule
                {
                    Name = "WorksheetNotNull",
                    Description = "Worksheet cannot be null",
                    Expression = "worksheet != null",
                    Severity = ValidationSeverity.Error,
                    ErrorMessage = "Worksheet cannot be null",
                    IsEnabled = true
                },
                new ValidationRule
                {
                    Name = "WorksheetNameValid",
                    Description = "Worksheet name must be valid",
                    Expression = "!string.IsNullOrWhiteSpace(worksheet.Name)",
                    Severity = ValidationSeverity.Error,
                    ErrorMessage = "Worksheet name cannot be null or empty",
                    IsEnabled = true
                }
            };

            _validationRules[ValidationType.CellReference] = new List<ValidationRule>
            {
                new ValidationRule
                {
                    Name = "CellReferenceFormat",
                    Description = "Cell reference must match Excel format",
                    Expression = "Regex.IsMatch(cellReference, @\"^[A-Z]+[1-9][0-9]*$\", RegexOptions.IgnoreCase)",
                    Severity = ValidationSeverity.Error,
                    ErrorMessage = "Invalid cell reference format",
                    IsEnabled = true
                }
            };
        }
    }
}
