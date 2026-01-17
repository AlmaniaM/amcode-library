using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx;

namespace AMCode.Documents.Xlsx.Domain.Interfaces
{
    /// <summary>
    /// Domain interface for workbook validation operations
    /// Provides comprehensive validation capabilities for workbooks, worksheets, and ranges
    /// </summary>
    public interface IWorkbookValidator
    {
        /// <summary>
        /// Validates a workbook for structural integrity and content validity
        /// </summary>
        /// <param name="workbook">The workbook to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        Result<ValidationResult> ValidateWorkbook(IWorkbookDomain workbook);

        /// <summary>
        /// Validates a worksheet for structural integrity and content validity
        /// </summary>
        /// <param name="worksheet">The worksheet to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        Result<ValidationResult> ValidateWorksheet(IWorksheet worksheet);

        /// <summary>
        /// Validates a range for structural integrity and content validity
        /// </summary>
        /// <param name="range">The range to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        Result<ValidationResult> ValidateRange(AMCode.Documents.Xlsx.IRange range);

        /// <summary>
        /// Validates a cell reference string format
        /// </summary>
        /// <param name="cellReference">The cell reference to validate (e.g., "A1", "B10")</param>
        /// <returns>Result containing validation results or error information</returns>
        Result<ValidationResult> ValidateCellReference(string cellReference);

        /// <summary>
        /// Validates a range reference string format
        /// </summary>
        /// <param name="rangeReference">The range reference to validate (e.g., "A1:C10")</param>
        /// <returns>Result containing validation results or error information</returns>
        Result<ValidationResult> ValidateRangeReference(string rangeReference);

        /// <summary>
        /// Validates a worksheet name for Excel compatibility
        /// </summary>
        /// <param name="worksheetName">The worksheet name to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        Result<ValidationResult> ValidateWorksheetName(string worksheetName);

        /// <summary>
        /// Validates a workbook file path
        /// </summary>
        /// <param name="filePath">The file path to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        Result<ValidationResult> ValidateFilePath(string filePath);

        /// <summary>
        /// Validates a formula expression
        /// </summary>
        /// <param name="formula">The formula to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        Result<ValidationResult> ValidateFormula(string formula);

        /// <summary>
        /// Validates a cell value for type compatibility
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="expectedType">The expected data type</param>
        /// <returns>Result containing validation results or error information</returns>
        Result<ValidationResult> ValidateCellValue(object value, Type expectedType);

        /// <summary>
        /// Validates workbook metadata for completeness and correctness
        /// </summary>
        /// <param name="metadata">The metadata to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        Result<ValidationResult> ValidateMetadata(IWorkbookMetadata metadata);

        /// <summary>
        /// Validates workbook protection settings
        /// </summary>
        /// <param name="workbook">The workbook to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        Result<ValidationResult> ValidateProtection(IWorkbookDomain workbook);

        /// <summary>
        /// Validates workbook calculation settings
        /// </summary>
        /// <param name="workbook">The workbook to validate</param>
        /// <returns>Result containing validation results or error information</returns>
        Result<ValidationResult> ValidateCalculationSettings(IWorkbookDomain workbook);

        /// <summary>
        /// Gets validation rules for a specific validation type
        /// </summary>
        /// <param name="validationType">The type of validation</param>
        /// <returns>Result containing the validation rules or error information</returns>
        Result<IEnumerable<ValidationRule>> GetValidationRules(ValidationType validationType);

        /// <summary>
        /// Sets custom validation rules for a specific validation type
        /// </summary>
        /// <param name="validationType">The type of validation</param>
        /// <param name="rules">The custom validation rules</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetValidationRules(ValidationType validationType, IEnumerable<ValidationRule> rules);

        /// <summary>
        /// Clears all custom validation rules
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result ClearValidationRules();

        /// <summary>
        /// Validates multiple items in batch
        /// </summary>
        /// <param name="items">The items to validate</param>
        /// <param name="validationType">The type of validation to perform</param>
        /// <returns>Result containing validation results for all items or error information</returns>
        Result<IEnumerable<ValidationResult>> ValidateBatch(IEnumerable<object> items, ValidationType validationType);
    }

    /// <summary>
    /// Structure representing the result of a validation operation
    /// </summary>
    public struct ValidationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the validation passed
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the validation message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the severity of the validation result
        /// </summary>
        public ValidationSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the error code associated with the validation result
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets additional details about the validation result
        /// </summary>
        public Dictionary<string, object> Details { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the validation was performed
        /// </summary>
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// Enumeration of validation severity levels
    /// </summary>
    public enum ValidationSeverity
    {
        /// <summary>
        /// Information level validation result
        /// </summary>
        Information,

        /// <summary>
        /// Warning level validation result
        /// </summary>
        Warning,

        /// <summary>
        /// Error level validation result
        /// </summary>
        Error
    }

    /// <summary>
    /// Enumeration of validation types
    /// </summary>
    public enum ValidationType
    {
        /// <summary>
        /// Workbook validation
        /// </summary>
        Workbook,

        /// <summary>
        /// Worksheet validation
        /// </summary>
        Worksheet,

        /// <summary>
        /// Range validation
        /// </summary>
        Range,

        /// <summary>
        /// Cell reference validation
        /// </summary>
        CellReference,

        /// <summary>
        /// Range reference validation
        /// </summary>
        RangeReference,

        /// <summary>
        /// Worksheet name validation
        /// </summary>
        WorksheetName,

        /// <summary>
        /// File path validation
        /// </summary>
        FilePath,

        /// <summary>
        /// Formula validation
        /// </summary>
        Formula,

        /// <summary>
        /// Metadata validation
        /// </summary>
        Metadata,

        /// <summary>
        /// Protection validation
        /// </summary>
        Protection,

        /// <summary>
        /// Calculation settings validation
        /// </summary>
        CalculationSettings
    }

    /// <summary>
    /// Structure representing a validation rule
    /// </summary>
    public struct ValidationRule
    {
        /// <summary>
        /// Gets or sets the name of the validation rule
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the validation rule
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the expression to evaluate for validation
        /// </summary>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the severity of the validation rule
        /// </summary>
        public ValidationSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the error message to display when validation fails
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the validation rule is enabled
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}