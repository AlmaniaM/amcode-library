using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Interfaces;

namespace AMCode.Documents.Xlsx.Tests.TestData
{
    /// <summary>
    /// Factory for creating test worksheets with various configurations
    /// </summary>
    public static class TestWorksheetFactory
    {
        #region Basic Test Worksheets

        /// <summary>
        /// Creates a basic test worksheet
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateBasicTestWorksheet(IWorkbookDomain workbook, string name = "TestSheet")
        {
            if (workbook == null)
                return Result<IWorksheet>.Failure("Workbook is null");

            var addWorksheetResult = workbook.AddWorksheet(name);
            if (!addWorksheetResult.IsSuccess)
                return addWorksheetResult;

            return Result<IWorksheet>.Success(addWorksheetResult.Value);
        }

        /// <summary>
        /// Creates a test worksheet with specific properties
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <param name="isVisible">Whether the worksheet is visible</param>
        /// <param name="tabColor">Tab color (hex value)</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithProperties(IWorkbookDomain workbook, string name, bool isVisible, uint tabColor)
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Set properties
            worksheet.IsVisible = isVisible;
            worksheet.TabColor = tabColor;

            return Result<IWorksheet>.Success(worksheet);
        }

        #endregion

        #region Test Worksheets with Data

        /// <summary>
        /// Creates a test worksheet with sample data
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <param name="rows">Number of rows of data</param>
        /// <param name="columns">Number of columns of data</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithData(IWorkbookDomain workbook, string name = "DataSheet", int rows = 10, int columns = 5)
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add sample data
            var testData = TestDataGenerator.CreateTestDataArray(rows, columns);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            return Result<IWorksheet>.Success(worksheet);
        }

        /// <summary>
        /// Creates a test worksheet with list data
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <param name="rows">Number of rows of data</param>
        /// <param name="columns">Number of columns of data</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithListData(IWorkbookDomain workbook, string name = "ListDataSheet", int rows = 10, int columns = 5)
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add list data
            var testData = TestDataGenerator.CreateTestDataList(rows, columns);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            return Result<IWorksheet>.Success(worksheet);
        }

        /// <summary>
        /// Creates a test worksheet with formulas
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithFormulas(IWorkbookDomain workbook, string name = "FormulaSheet")
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add sample data first
            var testData = TestDataGenerator.CreateTestDataArray(5, 3);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            // Add formulas
            var formulas = TestDataGenerator.CreateTestFormulas(5);
            for (int i = 0; i < formulas.Length; i++)
            {
                var setFormulaResult = worksheet.SetFormula($"A{i + 6}", formulas[i]);
                if (!setFormulaResult.IsSuccess)
                    return Result<IWorksheet>.Failure(setFormulaResult.Error);
            }

            return Result<IWorksheet>.Success(worksheet);
        }

        #endregion

        #region Test Worksheets with Formatting

        /// <summary>
        /// Creates a test worksheet with basic formatting
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithFormatting(IWorkbookDomain workbook, string name = "FormattingSheet")
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(10, 5);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            // Add formatting
            var rangeResult = worksheet.GetRange("A1:E10");
            if (!rangeResult.IsSuccess)
                return Result<IWorksheet>.Failure(rangeResult.Error);

            var range = rangeResult.Value;

            // Set various formatting
            var setFontResult = range.SetFont("Arial", 12, true, false);
            if (!setFontResult.IsSuccess)
                return Result<IWorksheet>.Failure(setFontResult.Error);

            var setAlignmentResult = range.SetAlignment(AlignmentType.Center, VerticalAlignmentType.Middle);
            if (!setAlignmentResult.IsSuccess)
                return Result<IWorksheet>.Failure(setAlignmentResult.Error);

            var setBorderResult = range.SetBorder(BorderType.Thin, BorderColor.Black);
            if (!setBorderResult.IsSuccess)
                return Result<IWorksheet>.Failure(setBorderResult.Error);

            var setFillResult = range.SetFill(FillType.Solid, FillColor.LightBlue);
            if (!setFillResult.IsSuccess)
                return Result<IWorksheet>.Failure(setFillResult.Error);

            return Result<IWorksheet>.Success(worksheet);
        }

        /// <summary>
        /// Creates a test worksheet with advanced formatting
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithAdvancedFormatting(IWorkbookDomain workbook, string name = "AdvancedFormattingSheet")
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(15, 8);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            // Add various formatting ranges
            var ranges = new[]
            {
                new { Range = "A1:B5", Font = "Arial", Size = 12, Bold = true, Italic = false, Color = 0xFF0000 },
                new { Range = "C1:D5", Font = "Times New Roman", Size = 14, Bold = false, Italic = true, Color = 0x0000FF },
                new { Range = "E1:F5", Font = "Courier New", Size = 10, Bold = true, Italic = true, Color = 0x00FF00 },
                new { Range = "A6:H10", Font = "Calibri", Size = 11, Bold = false, Italic = false, Color = 0x000000 }
            };

            foreach (var rangeConfig in ranges)
            {
                var rangeResult = worksheet.GetRange(rangeConfig.Range);
                if (!rangeResult.IsSuccess)
                    return Result<IWorksheet>.Failure(rangeResult.Error);

                var range = rangeResult.Value;

                var setFontResult = range.SetFont(rangeConfig.Font, rangeConfig.Size, rangeConfig.Bold, rangeConfig.Italic);
                if (!setFontResult.IsSuccess)
                    return Result<IWorksheet>.Failure(setFontResult.Error);

                var setFontColorResult = range.SetFontColor(rangeConfig.Color);
                if (!setFontColorResult.IsSuccess)
                    return Result<IWorksheet>.Failure(setFontColorResult.Error);
            }

            return Result<IWorksheet>.Success(worksheet);
        }

        #endregion

        #region Test Worksheets with Comments and Hyperlinks

        /// <summary>
        /// Creates a test worksheet with comments
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <param name="commentCount">Number of comments to add</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithComments(IWorkbookDomain workbook, string name = "CommentsSheet", int commentCount = 10)
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(commentCount, 3);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            // Add comments
            for (int i = 1; i <= commentCount; i++)
            {
                var addCommentResult = worksheet.AddComment($"A{i}", $"Comment for cell A{i}");
                if (!addCommentResult.IsSuccess)
                    return Result<IWorksheet>.Failure(addCommentResult.Error);
            }

            return Result<IWorksheet>.Success(worksheet);
        }

        /// <summary>
        /// Creates a test worksheet with hyperlinks
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <param name="hyperlinkCount">Number of hyperlinks to add</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithHyperlinks(IWorkbookDomain workbook, string name = "HyperlinksSheet", int hyperlinkCount = 10)
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(hyperlinkCount, 3);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            // Add hyperlinks
            for (int i = 1; i <= hyperlinkCount; i++)
            {
                var addHyperlinkResult = worksheet.AddHyperlink($"B{i}", $"https://example.com/{i}", $"Link {i}");
                if (!addHyperlinkResult.IsSuccess)
                    return Result<IWorksheet>.Failure(addHyperlinkResult.Error);
            }

            return Result<IWorksheet>.Success(worksheet);
        }

        /// <summary>
        /// Creates a test worksheet with both comments and hyperlinks
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithCommentsAndHyperlinks(IWorkbookDomain workbook, string name = "CommentsHyperlinksSheet")
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(5, 3);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            // Add comments
            for (int i = 1; i <= 5; i++)
            {
                var addCommentResult = worksheet.AddComment($"A{i}", $"Comment for cell A{i}");
                if (!addCommentResult.IsSuccess)
                    return Result<IWorksheet>.Failure(addCommentResult.Error);
            }

            // Add hyperlinks
            for (int i = 1; i <= 5; i++)
            {
                var addHyperlinkResult = worksheet.AddHyperlink($"B{i}", $"https://example.com/{i}", $"Link {i}");
                if (!addHyperlinkResult.IsSuccess)
                    return Result<IWorksheet>.Failure(addHyperlinkResult.Error);
            }

            return Result<IWorksheet>.Success(worksheet);
        }

        #endregion

        #region Test Worksheets with Data Validation

        /// <summary>
        /// Creates a test worksheet with data validation
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithDataValidation(IWorkbookDomain workbook, string name = "DataValidationSheet")
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(10, 5);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            // Add data validation
            var rangeResult = worksheet.GetRange("A1:A10");
            if (!rangeResult.IsSuccess)
                return Result<IWorksheet>.Failure(rangeResult.Error);

            var range = rangeResult.Value;

            var setDataValidationResult = range.SetDataValidation("List", "Option1,Option2,Option3");
            if (!setDataValidationResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataValidationResult.Error);

            return Result<IWorksheet>.Success(worksheet);
        }

        /// <summary>
        /// Creates a test worksheet with multiple data validation rules
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithMultipleDataValidation(IWorkbookDomain workbook, string name = "MultipleDataValidationSheet")
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(10, 5);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            // Add multiple data validation rules
            var validationRules = new[]
            {
                new { Range = "A1:A10", Type = "List", Formula = "Option1,Option2,Option3" },
                new { Range = "B1:B10", Type = "WholeNumber", Formula = "1,100" },
                new { Range = "C1:C10", Type = "Decimal", Formula = "0,1000" },
                new { Range = "D1:D10", Type = "Date", Formula = "1/1/2020,12/31/2030" },
                new { Range = "E1:E10", Type = "TextLength", Formula = "1,50" }
            };

            foreach (var rule in validationRules)
            {
                var rangeResult = worksheet.GetRange(rule.Range);
                if (!rangeResult.IsSuccess)
                    return Result<IWorksheet>.Failure(rangeResult.Error);

                var range = rangeResult.Value;

                var setDataValidationResult = range.SetDataValidation(rule.Type, rule.Formula);
                if (!setDataValidationResult.IsSuccess)
                    return Result<IWorksheet>.Failure(setDataValidationResult.Error);
            }

            return Result<IWorksheet>.Success(worksheet);
        }

        #endregion

        #region Test Worksheets with Conditional Formatting

        /// <summary>
        /// Creates a test worksheet with conditional formatting
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithConditionalFormatting(IWorkbookDomain workbook, string name = "ConditionalFormattingSheet")
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(10, 5);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            // Add conditional formatting
            var rangeResult = worksheet.GetRange("A1:E10");
            if (!rangeResult.IsSuccess)
                return Result<IWorksheet>.Failure(rangeResult.Error);

            var range = rangeResult.Value;

            var setConditionalFormattingResult = range.SetConditionalFormatting("CellValue", "greaterThan", "5");
            if (!setConditionalFormattingResult.IsSuccess)
                return Result<IWorksheet>.Failure(setConditionalFormattingResult.Error);

            return Result<IWorksheet>.Success(worksheet);
        }

        /// <summary>
        /// Creates a test worksheet with multiple conditional formatting rules
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithMultipleConditionalFormatting(IWorkbookDomain workbook, string name = "MultipleConditionalFormattingSheet")
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(10, 5);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            // Add multiple conditional formatting rules
            var conditionalFormattingRules = new[]
            {
                new { Range = "A1:A10", Condition = "CellValue", Operator = "greaterThan", Value = "5" },
                new { Range = "B1:B10", Condition = "CellValue", Operator = "lessThan", Value = "3" },
                new { Range = "C1:C10", Condition = "CellValue", Operator = "equal", Value = "7" },
                new { Range = "D1:D10", Condition = "CellValue", Operator = "between", Value = "1,10" },
                new { Range = "E1:E10", Condition = "CellValue", Operator = "notEqual", Value = "0" }
            };

            foreach (var rule in conditionalFormattingRules)
            {
                var rangeResult = worksheet.GetRange(rule.Range);
                if (!rangeResult.IsSuccess)
                    return Result<IWorksheet>.Failure(rangeResult.Error);

                var range = rangeResult.Value;

                var setConditionalFormattingResult = range.SetConditionalFormatting(rule.Condition, rule.Operator, rule.Value);
                if (!setConditionalFormattingResult.IsSuccess)
                    return Result<IWorksheet>.Failure(setConditionalFormattingResult.Error);
            }

            return Result<IWorksheet>.Success(worksheet);
        }

        #endregion

        #region Test Worksheets with Print Settings

        /// <summary>
        /// Creates a test worksheet with print settings
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithPrintSettings(IWorkbookDomain workbook, string name = "PrintSettingsSheet")
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(20, 8);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            // Add print settings
            var setPrintSettingsResult = worksheet.SetPrintSettings(true, true, true, true);
            if (!setPrintSettingsResult.IsSuccess)
                return Result<IWorksheet>.Failure(setPrintSettingsResult.Error);

            var setPageSetupResult = worksheet.SetPageSetup(PaperSize.A4, PageOrientation.Portrait, 1.0, 1.0, 1.0, 1.0);
            if (!setPageSetupResult.IsSuccess)
                return Result<IWorksheet>.Failure(setPageSetupResult.Error);

            var setHeaderFooterResult = worksheet.SetHeaderFooter("Test Header", "Test Footer");
            if (!setHeaderFooterResult.IsSuccess)
                return Result<IWorksheet>.Failure(setHeaderFooterResult.Error);

            return Result<IWorksheet>.Success(worksheet);
        }

        #endregion

        #region Test Worksheets with Errors

        /// <summary>
        /// Creates a test worksheet that will cause errors
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithErrors(IWorkbookDomain workbook, string name = "ErrorSheet")
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add data that will cause errors
            var testData = TestDataGenerator.CreateTestDataArray(5, 3);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            // Add invalid formulas
            var invalidFormulas = new[] { "=INVALID_FORMULA", "=SUM(INVALID_RANGE)", "=#REF!" };
            for (int i = 0; i < invalidFormulas.Length; i++)
            {
                var setFormulaResult = worksheet.SetFormula($"A{i + 6}", invalidFormulas[i]);
                // Don't check for success as these are intentionally invalid
            }

            return Result<IWorksheet>.Success(worksheet);
        }

        /// <summary>
        /// Creates a test worksheet with invalid data
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateTestWorksheetWithInvalidData(IWorkbookDomain workbook, string name = "InvalidDataSheet")
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add invalid data
            var invalidData = new object[,] { { null }, { "" }, { new object() } };
            var setDataResult = worksheet.SetData("A1", invalidData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            return Result<IWorksheet>.Success(worksheet);
        }

        #endregion

        #region Large Test Worksheets

        /// <summary>
        /// Creates a large test worksheet for performance testing
        /// </summary>
        /// <param name="workbook">Workbook to add the worksheet to</param>
        /// <param name="name">Worksheet name</param>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <returns>Result containing the test worksheet</returns>
        public static Result<IWorksheet> CreateLargeTestWorksheet(IWorkbookDomain workbook, string name = "LargeSheet", int rows = 1000, int columns = 10)
        {
            var result = CreateBasicTestWorksheet(workbook, name);
            if (!result.IsSuccess)
                return result;

            var worksheet = result.Value;

            // Add large dataset
            var testData = TestDataGenerator.CreateTestDataArray(rows, columns);
            var setDataResult = worksheet.SetData("A1", testData);
            if (!setDataResult.IsSuccess)
                return Result<IWorksheet>.Failure(setDataResult.Error);

            return Result<IWorksheet>.Success(worksheet);
        }

        #endregion
    }
}
