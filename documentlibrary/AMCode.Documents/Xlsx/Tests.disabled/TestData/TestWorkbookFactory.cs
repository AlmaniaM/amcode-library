using System;
using System.Collections.Generic;
using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Infrastructure.Adapters;
using AMCode.Documents.Xlsx.Infrastructure.Factories;
using AMCode.Documents.Xlsx.Infrastructure.Interfaces;
using AMCode.Documents.Xlsx.Providers.OpenXml;
using AMCode.Documents.Xlsx.Interfaces;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AMCode.Documents.Xlsx.Tests.TestData
{
    /// <summary>
    /// Factory for creating test workbooks with various configurations
    /// </summary>
    public static class TestWorkbookFactory
    {
        private static readonly IWorkbookEngine _workbookEngine = new OpenXmlWorkbookEngine();
        private static readonly IWorkbookLogger _workbookLogger = new TestWorkbookLogger();
        private static readonly IWorkbookValidator _workbookValidator = new TestWorkbookValidator();
        private static readonly IWorkbookFactory _workbookFactory = new WorkbookFactory(_workbookEngine, _workbookLogger, _workbookValidator);

        #region Basic Test Workbooks

        /// <summary>
        /// Creates a basic test workbook
        /// </summary>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateBasicTestWorkbook()
        {
            return _workbookFactory.CreateWorkbook("Basic Test Workbook");
        }

        /// <summary>
        /// Creates a test workbook with specific title
        /// </summary>
        /// <param name="title">Workbook title</param>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookWithTitle(string title)
        {
            return _workbookFactory.CreateWorkbook(title);
        }

        /// <summary>
        /// Creates a test workbook with metadata
        /// </summary>
        /// <param name="title">Workbook title</param>
        /// <param name="author">Workbook author</param>
        /// <param name="subject">Workbook subject</param>
        /// <param name="company">Workbook company</param>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookWithMetadata(string title, string author, string subject, string company)
        {
            var result = _workbookFactory.CreateWorkbook(title);
            if (!result.IsSuccess)
                return result;

            var workbook = result.Value;
            
            // Set metadata
            workbook.Author = author;
            workbook.Subject = subject;
            workbook.Company = company;
            workbook.Created = DateTime.Now;
            workbook.Modified = DateTime.Now;

            return Result<IWorkbookDomain>.Success(workbook);
        }

        #endregion

        #region Test Workbooks with Data

        /// <summary>
        /// Creates a test workbook with sample data
        /// </summary>
        /// <param name="rows">Number of rows of data</param>
        /// <param name="columns">Number of columns of data</param>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookWithData(int rows = 10, int columns = 5)
        {
            var result = _workbookFactory.CreateWorkbook("Test Workbook with Data");
            if (!result.IsSuccess)
                return result;

            var workbook = result.Value;

            // Add worksheet with data
            var addWorksheetResult = workbook.AddWorksheet("DataSheet");
            if (!addWorksheetResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(addWorksheetResult.Error);

            var worksheet = addWorksheetResult.Value;

            // Add sample data
            var testData = TestDataGenerator.CreateTestDataArray(rows, columns);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setDataResult.Error);

            return Result<IWorkbookDomain>.Success(workbook);
        }

        /// <summary>
        /// Creates a test workbook with multiple worksheets
        /// </summary>
        /// <param name="worksheetCount">Number of worksheets to create</param>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookWithMultipleWorksheets(int worksheetCount = 3)
        {
            var result = _workbookFactory.CreateWorkbook("Test Workbook with Multiple Worksheets");
            if (!result.IsSuccess)
                return result;

            var workbook = result.Value;

            for (int i = 1; i <= worksheetCount; i++)
            {
                var addWorksheetResult = workbook.AddWorksheet($"Sheet{i}");
                if (!addWorksheetResult.IsSuccess)
                    return Result<IWorkbookDomain>.Failure(addWorksheetResult.Error);

                var worksheet = addWorksheetResult.Value;

                // Add some data to each worksheet
                var testData = TestDataGenerator.CreateTestDataArray(5, 3);
                var setDataResult = worksheet.SetData("A1", testData);
                if (!setDataResult.IsSuccess)
                    return Result<IWorkbookDomain>.Failure(setDataResult.Error);
            }

            return Result<IWorkbookDomain>.Success(workbook);
        }

        /// <summary>
        /// Creates a test workbook with formulas
        /// </summary>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookWithFormulas()
        {
            var result = _workbookFactory.CreateWorkbook("Test Workbook with Formulas");
            if (!result.IsSuccess)
                return result;

            var workbook = result.Value;

            var addWorksheetResult = workbook.AddWorksheet("FormulaSheet");
            if (!addWorksheetResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(addWorksheetResult.Error);

            var worksheet = addWorksheetResult.Value;

            // Add sample data
            var testData = TestDataGenerator.CreateTestDataArray(5, 3);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setDataResult.Error);

            // Add formulas
            var formulas = TestDataGenerator.CreateTestFormulas(5);
            for (int i = 0; i < formulas.Length; i++)
            {
                var setFormulaResult = worksheet.SetFormula($"A{i + 6}", formulas[i]);
                if (!setFormulaResult.IsSuccess)
                    return Result<IWorkbookDomain>.Failure(setFormulaResult.Error);
            }

            return Result<IWorkbookDomain>.Success(workbook);
        }

        #endregion

        #region Test Workbooks with Formatting

        /// <summary>
        /// Creates a test workbook with formatting
        /// </summary>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookWithFormatting()
        {
            var result = _workbookFactory.CreateWorkbook("Test Workbook with Formatting");
            if (!result.IsSuccess)
                return result;

            var workbook = result.Value;

            var addWorksheetResult = workbook.AddWorksheet("FormattingSheet");
            if (!addWorksheetResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(addWorksheetResult.Error);

            var worksheet = addWorksheetResult.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(10, 5);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setDataResult.Error);

            // Add formatting
            var rangeResult = worksheet.GetRange("A1:E10");
            if (!rangeResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(rangeResult.Error);

            var range = rangeResult.Value;

            // Set various formatting
            var setFontResult = range.SetFont("Arial", 12, true, false);
            if (!setFontResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setFontResult.Error);

            var setAlignmentResult = range.SetAlignment(AlignmentType.Center, VerticalAlignmentType.Middle);
            if (!setAlignmentResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setAlignmentResult.Error);

            var setBorderResult = range.SetBorder(BorderType.Thin, BorderColor.Black);
            if (!setBorderResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setBorderResult.Error);

            var setFillResult = range.SetFill(FillType.Solid, FillColor.LightBlue);
            if (!setFillResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setFillResult.Error);

            return Result<IWorkbookDomain>.Success(workbook);
        }

        /// <summary>
        /// Creates a test workbook with comments and hyperlinks
        /// </summary>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookWithCommentsAndHyperlinks()
        {
            var result = _workbookFactory.CreateWorkbook("Test Workbook with Comments and Hyperlinks");
            if (!result.IsSuccess)
                return result;

            var workbook = result.Value;

            var addWorksheetResult = workbook.AddWorksheet("CommentsHyperlinksSheet");
            if (!addWorksheetResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(addWorksheetResult.Error);

            var worksheet = addWorksheetResult.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(5, 3);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setDataResult.Error);

            // Add comments
            for (int i = 1; i <= 5; i++)
            {
                var addCommentResult = worksheet.AddComment($"A{i}", $"Comment for cell A{i}");
                if (!addCommentResult.IsSuccess)
                    return Result<IWorkbookDomain>.Failure(addCommentResult.Error);
            }

            // Add hyperlinks
            for (int i = 1; i <= 5; i++)
            {
                var addHyperlinkResult = worksheet.AddHyperlink($"B{i}", $"https://example.com/{i}", $"Link {i}");
                if (!addHyperlinkResult.IsSuccess)
                    return Result<IWorkbookDomain>.Failure(addHyperlinkResult.Error);
            }

            return Result<IWorkbookDomain>.Success(workbook);
        }

        #endregion

        #region Test Workbooks with Errors

        /// <summary>
        /// Creates a test workbook that will cause errors
        /// </summary>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookWithErrors()
        {
            var result = _workbookFactory.CreateWorkbook("Test Workbook with Errors");
            if (!result.IsSuccess)
                return result;

            var workbook = result.Value;

            var addWorksheetResult = workbook.AddWorksheet("ErrorSheet");
            if (!addWorksheetResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(addWorksheetResult.Error);

            var worksheet = addWorksheetResult.Value;

            // Add data that will cause errors
            var testData = TestDataGenerator.CreateTestDataArray(5, 3);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setDataResult.Error);

            // Add invalid formulas
            var invalidFormulas = new[] { "=INVALID_FORMULA", "=SUM(INVALID_RANGE)", "=#REF!" };
            for (int i = 0; i < invalidFormulas.Length; i++)
            {
                var setFormulaResult = worksheet.SetFormula($"A{i + 6}", invalidFormulas[i]);
                // Don't check for success as these are intentionally invalid
            }

            return Result<IWorkbookDomain>.Success(workbook);
        }

        /// <summary>
        /// Creates a test workbook with invalid data
        /// </summary>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookWithInvalidData()
        {
            var result = _workbookFactory.CreateWorkbook("Test Workbook with Invalid Data");
            if (!result.IsSuccess)
                return result;

            var workbook = result.Value;

            var addWorksheetResult = workbook.AddWorksheet("InvalidDataSheet");
            if (!addWorksheetResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(addWorksheetResult.Error);

            var worksheet = addWorksheetResult.Value;

            // Add invalid data
            var invalidData = new object[,] { { null }, { "" }, { new object() } };
            var setDataResult = worksheet.SetData("A1", invalidData);
            if (!setDataResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setDataResult.Error);

            return Result<IWorkbookDomain>.Success(workbook);
        }

        #endregion

        #region Large Test Workbooks

        /// <summary>
        /// Creates a large test workbook for performance testing
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateLargeTestWorkbook(int rows = 1000, int columns = 10)
        {
            var result = _workbookFactory.CreateWorkbook($"Large Test Workbook ({rows}x{columns})");
            if (!result.IsSuccess)
                return result;

            var workbook = result.Value;

            var addWorksheetResult = workbook.AddWorksheet("LargeSheet");
            if (!addWorksheetResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(addWorksheetResult.Error);

            var worksheet = addWorksheetResult.Value;

            // Add large dataset
            var testData = TestDataGenerator.CreateTestDataArray(rows, columns);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setDataResult.Error);

            return Result<IWorkbookDomain>.Success(workbook);
        }

        /// <summary>
        /// Creates a test workbook with multiple large worksheets
        /// </summary>
        /// <param name="worksheetCount">Number of worksheets</param>
        /// <param name="rowsPerSheet">Rows per worksheet</param>
        /// <param name="columnsPerSheet">Columns per worksheet</param>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookWithMultipleLargeWorksheets(int worksheetCount = 5, int rowsPerSheet = 500, int columnsPerSheet = 8)
        {
            var result = _workbookFactory.CreateWorkbook($"Test Workbook with {worksheetCount} Large Worksheets");
            if (!result.IsSuccess)
                return result;

            var workbook = result.Value;

            for (int i = 1; i <= worksheetCount; i++)
            {
                var addWorksheetResult = workbook.AddWorksheet($"LargeSheet{i}");
                if (!addWorksheetResult.IsSuccess)
                    return Result<IWorkbookDomain>.Failure(addWorksheetResult.Error);

                var worksheet = addWorksheetResult.Value;

                // Add large dataset to each worksheet
                var testData = TestDataGenerator.CreateTestDataArray(rowsPerSheet, columnsPerSheet);
                var setDataResult = worksheet.SetData("A1", testData);
                if (!setDataResult.IsSuccess)
                    return Result<IWorkbookDomain>.Failure(setDataResult.Error);
            }

            return Result<IWorkbookDomain>.Success(workbook);
        }

        #endregion

        #region Test Workbooks from Files

        /// <summary>
        /// Creates a test workbook from an existing file
        /// </summary>
        /// <param name="filePath">Path to existing Excel file</param>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return Result<IWorkbookDomain>.Failure($"File does not exist: {filePath}");

            return _workbookFactory.OpenWorkbook(filePath);
        }

        /// <summary>
        /// Creates a test workbook from a stream
        /// </summary>
        /// <param name="stream">Stream containing Excel data</param>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookFromStream(Stream stream)
        {
            if (stream == null)
                return Result<IWorkbookDomain>.Failure("Stream is null");

            return _workbookFactory.OpenWorkbook(stream);
        }

        #endregion

        #region Test Workbooks with Specific Scenarios

        /// <summary>
        /// Creates a test workbook for data validation testing
        /// </summary>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookForDataValidation()
        {
            var result = _workbookFactory.CreateWorkbook("Test Workbook for Data Validation");
            if (!result.IsSuccess)
                return result;

            var workbook = result.Value;

            var addWorksheetResult = workbook.AddWorksheet("ValidationSheet");
            if (!addWorksheetResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(addWorksheetResult.Error);

            var worksheet = addWorksheetResult.Value;

            // Add data validation
            var rangeResult = worksheet.GetRange("A1:A10");
            if (!rangeResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(rangeResult.Error);

            var range = rangeResult.Value;

            var setDataValidationResult = range.SetDataValidation("List", "Option1,Option2,Option3");
            if (!setDataValidationResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setDataValidationResult.Error);

            return Result<IWorkbookDomain>.Success(workbook);
        }

        /// <summary>
        /// Creates a test workbook for conditional formatting testing
        /// </summary>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookForConditionalFormatting()
        {
            var result = _workbookFactory.CreateWorkbook("Test Workbook for Conditional Formatting");
            if (!result.IsSuccess)
                return result;

            var workbook = result.Value;

            var addWorksheetResult = workbook.AddWorksheet("ConditionalFormattingSheet");
            if (!addWorksheetResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(addWorksheetResult.Error);

            var worksheet = addWorksheetResult.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(10, 5);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setDataResult.Error);

            // Add conditional formatting
            var rangeResult = worksheet.GetRange("A1:E10");
            if (!rangeResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(rangeResult.Error);

            var range = rangeResult.Value;

            var setConditionalFormattingResult = range.SetConditionalFormatting("CellValue", "greaterThan", "5");
            if (!setConditionalFormattingResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setConditionalFormattingResult.Error);

            return Result<IWorkbookDomain>.Success(workbook);
        }

        /// <summary>
        /// Creates a test workbook for print settings testing
        /// </summary>
        /// <returns>Result containing the test workbook</returns>
        public static Result<IWorkbookDomain> CreateTestWorkbookForPrintSettings()
        {
            var result = _workbookFactory.CreateWorkbook("Test Workbook for Print Settings");
            if (!result.IsSuccess)
                return result;

            var workbook = result.Value;

            var addWorksheetResult = workbook.AddWorksheet("PrintSettingsSheet");
            if (!addWorksheetResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(addWorksheetResult.Error);

            var worksheet = addWorksheetResult.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(20, 8);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setDataResult.Error);

            // Add print settings
            var setPrintSettingsResult = worksheet.SetPrintSettings(true, true, true, true);
            if (!setPrintSettingsResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setPrintSettingsResult.Error);

            var setPageSetupResult = worksheet.SetPageSetup(PaperSize.A4, PageOrientation.Portrait, 1.0, 1.0, 1.0, 1.0);
            if (!setPageSetupResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setPageSetupResult.Error);

            var setHeaderFooterResult = worksheet.SetHeaderFooter("Test Header", "Test Footer");
            if (!setHeaderFooterResult.IsSuccess)
                return Result<IWorkbookDomain>.Failure(setHeaderFooterResult.Error);

            return Result<IWorkbookDomain>.Success(workbook);
        }

        #endregion
    }

    #region Test Helper Classes

    /// <summary>
    /// Test implementation of IWorkbookLogger for test data generation
    /// </summary>
    public class TestWorkbookLogger : IWorkbookLogger
    {
        public void LogWorkbookOperation(string operation, string details)
        {
            // Test implementation - minimal logging for test data generation
        }

        public void LogError(string error, Exception exception = null)
        {
            // Test implementation - minimal logging for test data generation
        }

        public void LogPerformance(string operation, long milliseconds)
        {
            // Test implementation - minimal logging for test data generation
        }

        public void LogWarning(string warning, string details = null)
        {
            // Test implementation - minimal logging for test data generation
        }
    }

    /// <summary>
    /// Test implementation of IWorkbookValidator for test data generation
    /// </summary>
    public class TestWorkbookValidator : IWorkbookValidator
    {
        public Result<bool> ValidateWorkbook(IWorkbookDomain workbook)
        {
            return Result<bool>.Success(true);
        }

        public Result<bool> ValidateWorksheet(IWorksheet worksheet)
        {
            return Result<bool>.Success(true);
        }

        public Result<bool> ValidateRange(IRange range)
        {
            return Result<bool>.Success(true);
        }

        public Result<bool> ValidateCellReference(string cellReference)
        {
            return Result<bool>.Success(true);
        }
    }

    #endregion
}
