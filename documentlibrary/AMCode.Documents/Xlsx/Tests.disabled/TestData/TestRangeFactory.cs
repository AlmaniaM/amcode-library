using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Interfaces;

namespace AMCode.Documents.Xlsx.Tests.TestData
{
    /// <summary>
    /// Factory for creating test ranges with various configurations
    /// </summary>
    public static class TestRangeFactory
    {
        #region Basic Test Ranges

        /// <summary>
        /// Creates a basic test range
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateBasicTestRange(IWorksheet worksheet, string address = "A1:C3")
        {
            if (worksheet == null)
                return Result<IRange>.Failure("Worksheet is null");

            var rangeResult = worksheet.GetRange(address);
            if (!rangeResult.IsSuccess)
                return rangeResult;

            return Result<IRange>.Success(rangeResult.Value);
        }

        /// <summary>
        /// Creates a test range with specific properties
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <param name="value">Value to set in the range</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithValue(IWorksheet worksheet, string address, object value)
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Set value
            var setValueResult = range.SetValue(value);
            if (!setValueResult.IsSuccess)
                return Result<IRange>.Failure(setValueResult.Error);

            return Result<IRange>.Success(range);
        }

        /// <summary>
        /// Creates a test range with formula
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <param name="formula">Formula to set in the range</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithFormula(IWorksheet worksheet, string address, string formula)
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Set formula
            var setFormulaResult = range.SetFormula(formula);
            if (!setFormulaResult.IsSuccess)
                return Result<IRange>.Failure(setFormulaResult.Error);

            return Result<IRange>.Success(range);
        }

        #endregion

        #region Test Ranges with Data

        /// <summary>
        /// Creates a test range with 2D array data
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <param name="rows">Number of rows of data</param>
        /// <param name="columns">Number of columns of data</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithArrayData(IWorksheet worksheet, string address, int rows = 5, int columns = 3)
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add array data
            var testData = TestDataGenerator.CreateTestDataArray(rows, columns);
            var setDataResult = range.SetData(testData);
            if (!setDataResult.IsSuccess)
                return Result<IRange>.Failure(setDataResult.Error);

            return Result<IRange>.Success(range);
        }

        /// <summary>
        /// Creates a test range with list data
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <param name="rows">Number of rows of data</param>
        /// <param name="columns">Number of columns of data</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithListData(IWorksheet worksheet, string address, int rows = 5, int columns = 3)
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add list data
            var testData = TestDataGenerator.CreateTestDataList(rows, columns);
            var setDataResult = range.SetData(testData);
            if (!setDataResult.IsSuccess)
                return Result<IRange>.Failure(setDataResult.Error);

            return Result<IRange>.Success(range);
        }

        /// <summary>
        /// Creates a test range with formulas
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <param name="formulaCount">Number of formulas to add</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithFormulas(IWorksheet worksheet, string address, int formulaCount = 5)
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add formulas
            var formulas = TestDataGenerator.CreateTestFormulas(formulaCount);
            for (int i = 0; i < formulas.Length; i++)
            {
                var setFormulaResult = range.SetFormula(formulas[i]);
                if (!setFormulaResult.IsSuccess)
                    return Result<IRange>.Failure(setFormulaResult.Error);
            }

            return Result<IRange>.Success(range);
        }

        #endregion

        #region Test Ranges with Formatting

        /// <summary>
        /// Creates a test range with basic formatting
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithFormatting(IWorksheet worksheet, string address = "A1:C3")
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add basic formatting
            var setFontResult = range.SetFont("Arial", 12, true, false);
            if (!setFontResult.IsSuccess)
                return Result<IRange>.Failure(setFontResult.Error);

            var setAlignmentResult = range.SetAlignment(AlignmentType.Center, VerticalAlignmentType.Middle);
            if (!setAlignmentResult.IsSuccess)
                return Result<IRange>.Failure(setAlignmentResult.Error);

            var setBorderResult = range.SetBorder(BorderType.Thin, BorderColor.Black);
            if (!setBorderResult.IsSuccess)
                return Result<IRange>.Failure(setBorderResult.Error);

            var setFillResult = range.SetFill(FillType.Solid, FillColor.LightBlue);
            if (!setFillResult.IsSuccess)
                return Result<IRange>.Failure(setFillResult.Error);

            return Result<IRange>.Success(range);
        }

        /// <summary>
        /// Creates a test range with advanced formatting
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithAdvancedFormatting(IWorksheet worksheet, string address = "A1:E5")
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add advanced formatting
            var setFontResult = range.SetFont("Times New Roman", 14, true, true);
            if (!setFontResult.IsSuccess)
                return Result<IRange>.Failure(setFontResult.Error);

            var setFontColorResult = range.SetFontColor(0xFF0000);
            if (!setFontColorResult.IsSuccess)
                return Result<IRange>.Failure(setFontColorResult.Error);

            var setAlignmentResult = range.SetAlignment(AlignmentType.Right, VerticalAlignmentType.Top);
            if (!setAlignmentResult.IsSuccess)
                return Result<IRange>.Failure(setAlignmentResult.Error);

            var setBorderResult = range.SetBorder(BorderType.Thick, BorderColor.Red);
            if (!setBorderResult.IsSuccess)
                return Result<IRange>.Failure(setBorderResult.Error);

            var setFillResult = range.SetFill(FillType.Gradient, FillColor.Yellow);
            if (!setFillResult.IsSuccess)
                return Result<IRange>.Failure(setFillResult.Error);

            var setNumberFormatResult = range.SetNumberFormat("0.00");
            if (!setNumberFormatResult.IsSuccess)
                return Result<IRange>.Failure(setNumberFormatResult.Error);

            return Result<IRange>.Success(range);
        }

        /// <summary>
        /// Creates a test range with multiple formatting styles
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithMultipleFormattingStyles(IWorksheet worksheet, string address = "A1:G7")
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add multiple formatting styles
            var formattingStyles = new[]
            {
                new { Font = "Arial", Size = 10, Bold = false, Italic = false, Color = 0x000000, Alignment = AlignmentType.Left, VerticalAlignment = VerticalAlignmentType.Top },
                new { Font = "Times New Roman", Size = 12, Bold = true, Italic = false, Color = 0xFF0000, Alignment = AlignmentType.Center, VerticalAlignment = VerticalAlignmentType.Middle },
                new { Font = "Courier New", Size = 14, Bold = false, Italic = true, Color = 0x0000FF, Alignment = AlignmentType.Right, VerticalAlignment = VerticalAlignmentType.Bottom },
                new { Font = "Calibri", Size = 11, Bold = true, Italic = true, Color = 0x00FF00, Alignment = AlignmentType.Justify, VerticalAlignment = VerticalAlignmentType.Middle },
                new { Font = "Verdana", Size = 13, Bold = false, Italic = false, Color = 0x800080, Alignment = AlignmentType.Left, VerticalAlignment = VerticalAlignmentType.Top }
            };

            for (int i = 0; i < formattingStyles.Length; i++)
            {
                var style = formattingStyles[i];
                
                // Create sub-range for each style
                var subRangeResult = range.GetSubRange(i + 1, 1, 1, 1);
                if (!subRangeResult.IsSuccess)
                    return Result<IRange>.Failure(subRangeResult.Error);

                var subRange = subRangeResult.Value;

                var setFontResult = subRange.SetFont(style.Font, style.Size, style.Bold, style.Italic);
                if (!setFontResult.IsSuccess)
                    return Result<IRange>.Failure(setFontResult.Error);

                var setFontColorResult = subRange.SetFontColor(style.Color);
                if (!setFontColorResult.IsSuccess)
                    return Result<IRange>.Failure(setFontColorResult.Error);

                var setAlignmentResult = subRange.SetAlignment(style.Alignment, style.VerticalAlignment);
                if (!setAlignmentResult.IsSuccess)
                    return Result<IRange>.Failure(setAlignmentResult.Error);
            }

            return Result<IRange>.Success(range);
        }

        #endregion

        #region Test Ranges with Data Validation

        /// <summary>
        /// Creates a test range with data validation
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <param name="validationType">Type of validation</param>
        /// <param name="validationFormula">Validation formula</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithDataValidation(IWorksheet worksheet, string address, string validationType, string validationFormula)
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add data validation
            var setDataValidationResult = range.SetDataValidation(validationType, validationFormula);
            if (!setDataValidationResult.IsSuccess)
                return Result<IRange>.Failure(setDataValidationResult.Error);

            return Result<IRange>.Success(range);
        }

        /// <summary>
        /// Creates a test range with multiple data validation rules
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithMultipleDataValidation(IWorksheet worksheet, string address = "A1:E5")
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add multiple data validation rules
            var validationRules = new[]
            {
                new { Type = "List", Formula = "Option1,Option2,Option3" },
                new { Type = "WholeNumber", Formula = "1,100" },
                new { Type = "Decimal", Formula = "0,1000" },
                new { Type = "Date", Formula = "1/1/2020,12/31/2030" },
                new { Type = "TextLength", Formula = "1,50" }
            };

            for (int i = 0; i < validationRules.Length; i++)
            {
                var rule = validationRules[i];
                
                // Create sub-range for each validation rule
                var subRangeResult = range.GetSubRange(i + 1, 1, 1, 1);
                if (!subRangeResult.IsSuccess)
                    return Result<IRange>.Failure(subRangeResult.Error);

                var subRange = subRangeResult.Value;

                var setDataValidationResult = subRange.SetDataValidation(rule.Type, rule.Formula);
                if (!setDataValidationResult.IsSuccess)
                    return Result<IRange>.Failure(setDataValidationResult.Error);
            }

            return Result<IRange>.Success(range);
        }

        #endregion

        #region Test Ranges with Conditional Formatting

        /// <summary>
        /// Creates a test range with conditional formatting
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <param name="condition">Condition type</param>
        /// <param name="operator">Operator</param>
        /// <param name="value">Value</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithConditionalFormatting(IWorksheet worksheet, string address, string condition, string @operator, string value)
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add conditional formatting
            var setConditionalFormattingResult = range.SetConditionalFormatting(condition, @operator, value);
            if (!setConditionalFormattingResult.IsSuccess)
                return Result<IRange>.Failure(setConditionalFormattingResult.Error);

            return Result<IRange>.Success(range);
        }

        /// <summary>
        /// Creates a test range with multiple conditional formatting rules
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithMultipleConditionalFormatting(IWorksheet worksheet, string address = "A1:E5")
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add multiple conditional formatting rules
            var conditionalFormattingRules = new[]
            {
                new { Condition = "CellValue", Operator = "greaterThan", Value = "5" },
                new { Condition = "CellValue", Operator = "lessThan", Value = "3" },
                new { Condition = "CellValue", Operator = "equal", Value = "7" },
                new { Condition = "CellValue", Operator = "between", Value = "1,10" },
                new { Condition = "CellValue", Operator = "notEqual", Value = "0" }
            };

            for (int i = 0; i < conditionalFormattingRules.Length; i++)
            {
                var rule = conditionalFormattingRules[i];
                
                // Create sub-range for each conditional formatting rule
                var subRangeResult = range.GetSubRange(i + 1, 1, 1, 1);
                if (!subRangeResult.IsSuccess)
                    return Result<IRange>.Failure(subRangeResult.Error);

                var subRange = subRangeResult.Value;

                var setConditionalFormattingResult = subRange.SetConditionalFormatting(rule.Condition, rule.Operator, rule.Value);
                if (!setConditionalFormattingResult.IsSuccess)
                    return Result<IRange>.Failure(setConditionalFormattingResult.Error);
            }

            return Result<IRange>.Success(range);
        }

        #endregion

        #region Test Ranges with Comments and Hyperlinks

        /// <summary>
        /// Creates a test range with comments
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <param name="commentCount">Number of comments to add</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithComments(IWorksheet worksheet, string address, int commentCount = 5)
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add comments
            for (int i = 1; i <= commentCount; i++)
            {
                var addCommentResult = range.AddComment($"Comment {i}");
                if (!addCommentResult.IsSuccess)
                    return Result<IRange>.Failure(addCommentResult.Error);
            }

            return Result<IRange>.Success(range);
        }

        /// <summary>
        /// Creates a test range with hyperlinks
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <param name="hyperlinkCount">Number of hyperlinks to add</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithHyperlinks(IWorksheet worksheet, string address, int hyperlinkCount = 5)
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add hyperlinks
            for (int i = 1; i <= hyperlinkCount; i++)
            {
                var addHyperlinkResult = range.AddHyperlink($"https://example.com/{i}", $"Link {i}");
                if (!addHyperlinkResult.IsSuccess)
                    return Result<IRange>.Failure(addHyperlinkResult.Error);
            }

            return Result<IRange>.Success(range);
        }

        /// <summary>
        /// Creates a test range with both comments and hyperlinks
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithCommentsAndHyperlinks(IWorksheet worksheet, string address = "A1:C3")
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add comments
            for (int i = 1; i <= 3; i++)
            {
                var addCommentResult = range.AddComment($"Comment {i}");
                if (!addCommentResult.IsSuccess)
                    return Result<IRange>.Failure(addCommentResult.Error);
            }

            // Add hyperlinks
            for (int i = 1; i <= 3; i++)
            {
                var addHyperlinkResult = range.AddHyperlink($"https://example.com/{i}", $"Link {i}");
                if (!addHyperlinkResult.IsSuccess)
                    return Result<IRange>.Failure(addHyperlinkResult.Error);
            }

            return Result<IRange>.Success(range);
        }

        #endregion

        #region Test Ranges with Operations

        /// <summary>
        /// Creates a test range with various operations
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithOperations(IWorksheet worksheet, string address = "A1:D4")
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add data first
            var testData = TestDataGenerator.CreateTestDataArray(4, 4);
            var setDataResult = range.SetData(testData);
            if (!setDataResult.IsSuccess)
                return Result<IRange>.Failure(setDataResult.Error);

            // Test various operations
            var clearResult = range.Clear();
            if (!clearResult.IsSuccess)
                return Result<IRange>.Failure(clearResult.Error);

            var setValueResult = range.SetValue("Test Value");
            if (!setValueResult.IsSuccess)
                return Result<IRange>.Failure(setValueResult.Error);

            var copyResult = range.Copy();
            if (!copyResult.IsSuccess)
                return Result<IRange>.Failure(copyResult.Error);

            var autoFitResult = range.AutoFit();
            if (!autoFitResult.IsSuccess)
                return Result<IRange>.Failure(autoFitResult.Error);

            var mergeResult = range.Merge();
            if (!mergeResult.IsSuccess)
                return Result<IRange>.Failure(mergeResult.Error);

            return Result<IRange>.Success(range);
        }

        /// <summary>
        /// Creates a test range with sub-ranges
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithSubRanges(IWorksheet worksheet, string address = "A1:F6")
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add data
            var testData = TestDataGenerator.CreateTestDataArray(6, 6);
            var setDataResult = range.SetData(testData);
            if (!setDataResult.IsSuccess)
                return Result<IRange>.Failure(setDataResult.Error);

            // Create sub-ranges
            var subRanges = new[]
            {
                new { StartRow = 1, StartColumn = 1, EndRow = 2, EndColumn = 2 },
                new { StartRow = 3, StartColumn = 3, EndRow = 4, EndColumn = 4 },
                new { StartRow = 5, StartColumn = 5, EndRow = 6, EndColumn = 6 }
            };

            foreach (var subRangeConfig in subRanges)
            {
                var subRangeResult = range.GetSubRange(subRangeConfig.StartRow, subRangeConfig.StartColumn, subRangeConfig.EndRow, subRangeConfig.EndColumn);
                if (!subRangeResult.IsSuccess)
                    return Result<IRange>.Failure(subRangeResult.Error);

                var subRange = subRangeResult.Value;

                // Set different values for each sub-range
                var setValueResult = subRange.SetValue($"SubRange {subRangeConfig.StartRow},{subRangeConfig.StartColumn}");
                if (!setValueResult.IsSuccess)
                    return Result<IRange>.Failure(setValueResult.Error);
            }

            return Result<IRange>.Success(range);
        }

        #endregion

        #region Test Ranges with Errors

        /// <summary>
        /// Creates a test range that will cause errors
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateTestRangeWithErrors(IWorksheet worksheet, string address = "A1:C3")
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add data that will cause errors
            var invalidData = new object[,] { { null }, { "" }, { new object() } };
            var setDataResult = range.SetData(invalidData);
            if (!setDataResult.IsSuccess)
                return Result<IRange>.Failure(setDataResult.Error);

            // Add invalid formulas
            var invalidFormulas = new[] { "=INVALID_FORMULA", "=SUM(INVALID_RANGE)", "=#REF!" };
            for (int i = 0; i < invalidFormulas.Length; i++)
            {
                var setFormulaResult = range.SetFormula(invalidFormulas[i]);
                // Don't check for success as these are intentionally invalid
            }

            return Result<IRange>.Success(range);
        }

        #endregion

        #region Large Test Ranges

        /// <summary>
        /// Creates a large test range for performance testing
        /// </summary>
        /// <param name="worksheet">Worksheet to get the range from</param>
        /// <param name="address">Range address</param>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <returns>Result containing the test range</returns>
        public static Result<IRange> CreateLargeTestRange(IWorksheet worksheet, string address, int rows = 100, int columns = 10)
        {
            var result = CreateBasicTestRange(worksheet, address);
            if (!result.IsSuccess)
                return result;

            var range = result.Value;

            // Add large dataset
            var testData = TestDataGenerator.CreateTestDataArray(rows, columns);
            var setDataResult = range.SetData(testData);
            if (!setDataResult.IsSuccess)
                return Result<IRange>.Failure(setDataResult.Error);

            return Result<IRange>.Success(range);
        }

        #endregion
    }
}
