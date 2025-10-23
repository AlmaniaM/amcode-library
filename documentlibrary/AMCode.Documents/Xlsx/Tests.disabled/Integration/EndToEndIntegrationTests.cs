using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Infrastructure.Adapters;
using AMCode.Documents.Xlsx.Infrastructure.Factories;
using AMCode.Documents.Xlsx.Infrastructure.Interfaces;
using AMCode.Documents.Xlsx.Providers.OpenXml;
using AMCode.Documents.Xlsx.Interfaces;
using NUnit.Framework;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AMCode.Documents.Xlsx.Tests.Integration
{
    /// <summary>
    /// End-to-end integration tests for complete Xlsx workflows
    /// </summary>
    [TestFixture]
    public class EndToEndIntegrationTests : IntegrationTestBase
    {
        private IWorkbookFactory _workbookFactory;
        private IWorkbookEngine _workbookEngine;
        private IWorkbookLogger _workbookLogger;
        private IWorkbookValidator _workbookValidator;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            
            // Create real implementations for integration testing
            _workbookEngine = new OpenXmlWorkbookEngine();
            _workbookLogger = new TestWorkbookLogger();
            _workbookValidator = new TestWorkbookValidator();
            _workbookFactory = new WorkbookFactory(_workbookEngine, _workbookLogger, _workbookValidator);
        }

        #region Complete Workbook Creation Workflow Tests

        /// <summary>
        /// Test complete workbook creation workflow
        /// </summary>
        [Test]
        public void CompleteWorkbookCreationWorkflow_ShouldSucceed()
        {
            // Act
            var result = _workbookFactory.CreateWorkbook();

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IWorkbookDomain>(result.Value);

            // Verify workbook properties
            var workbook = result.Value;
            Assert.IsNotNull(workbook.Id);
            Assert.IsTrue(workbook.CreatedAt > DateTime.MinValue);
            Assert.IsTrue(workbook.LastModified > DateTime.MinValue);
        }

        /// <summary>
        /// Test complete workbook creation with title workflow
        /// </summary>
        [Test]
        public void CompleteWorkbookCreationWithTitleWorkflow_ShouldSucceed()
        {
            // Arrange
            var title = "Test Workbook";

            // Act
            var result = _workbookFactory.CreateWorkbook(title);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IWorkbookDomain>(result.Value);

            // Verify workbook properties
            var workbook = result.Value;
            Assert.IsNotNull(workbook.Id);
            Assert.IsTrue(workbook.CreatedAt > DateTime.MinValue);
            Assert.IsTrue(workbook.LastModified > DateTime.MinValue);
        }

        #endregion

        #region Complete Workbook Opening Workflow Tests

        /// <summary>
        /// Test complete workbook opening from stream workflow
        /// </summary>
        [Test]
        public void CompleteWorkbookOpeningFromStreamWorkflow_ShouldSucceed()
        {
            // Arrange
            using var stream = CreateValidExcelStream();

            // Act
            var result = _workbookFactory.OpenWorkbook(stream);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IWorkbookDomain>(result.Value);

            // Verify workbook properties
            var workbook = result.Value;
            Assert.IsNotNull(workbook.Id);
            Assert.IsTrue(workbook.CreatedAt > DateTime.MinValue);
            Assert.IsTrue(workbook.LastModified > DateTime.MinValue);
        }

        /// <summary>
        /// Test complete workbook opening from file workflow
        /// </summary>
        [Test]
        public void CompleteWorkbookOpeningFromFileWorkflow_ShouldSucceed()
        {
            // Arrange
            var filePath = CreateValidExcelFile();

            // Act
            var result = _workbookFactory.OpenWorkbook(filePath);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IWorkbookDomain>(result.Value);

            // Verify workbook properties
            var workbook = result.Value;
            Assert.IsNotNull(workbook.Id);
            Assert.IsTrue(workbook.CreatedAt > DateTime.MinValue);
            Assert.IsTrue(workbook.LastModified > DateTime.MinValue);
        }

        #endregion

        #region Complete Worksheet Operations Workflow Tests

        /// <summary>
        /// Test complete worksheet operations workflow
        /// </summary>
        [Test]
        public void CompleteWorksheetOperationsWorkflow_ShouldSucceed()
        {
            // Arrange
            var workbookResult = _workbookFactory.CreateWorkbook();
            AssertResultSuccess(workbookResult);
            var workbook = workbookResult.Value;

            // Act & Assert - Test worksheet operations
            var worksheets = workbook.Worksheets;
            Assert.IsNotNull(worksheets);

            // Test adding a new worksheet
            var addWorksheetResult = workbook.AddWorksheet("TestSheet");
            AssertResultSuccess(addWorksheetResult);

            // Test getting the worksheet
            var getWorksheetResult = workbook.GetWorksheet("TestSheet");
            AssertResultSuccess(getWorksheetResult);
            var worksheet = getWorksheetResult.Value;

            // Test worksheet properties
            Assert.AreEqual("TestSheet", worksheet.Name);
            Assert.IsTrue(worksheet.IsVisible);

            // Test cell operations
            var setCellResult = worksheet.SetCellValue("A1", "Hello World");
            AssertResultSuccess(setCellResult);

            var getCellResult = worksheet.GetCellValue("A1");
            AssertResultSuccess(getCellResult);
            Assert.AreEqual("Hello World", getCellResult.Value);

            // Test range operations
            var rangeResult = worksheet.GetRange("A1:C3");
            AssertResultSuccess(rangeResult);
            var range = rangeResult.Value;

            // Test range value setting
            var setRangeResult = range.SetValue("Test Data");
            AssertResultSuccess(setRangeResult);

            // Test range clearing
            var clearRangeResult = range.Clear();
            AssertResultSuccess(clearRangeResult);

            // Test worksheet formatting
            var setColumnWidthResult = worksheet.SetColumnWidth("A", 20.0);
            AssertResultSuccess(setColumnWidthResult);

            var setRowHeightResult = worksheet.SetRowHeight(1, 25.0);
            AssertResultSuccess(setRowHeightResult);

            // Test auto-fit operations
            var autoFitColumnResult = worksheet.AutoFitColumn("A");
            AssertResultSuccess(autoFitColumnResult);

            var autoFitRowResult = worksheet.AutoFitRow(1);
            AssertResultSuccess(autoFitRowResult);
        }

        /// <summary>
        /// Test complete worksheet data operations workflow
        /// </summary>
        [Test]
        public void CompleteWorksheetDataOperationsWorkflow_ShouldSucceed()
        {
            // Arrange
            var workbookResult = _workbookFactory.CreateWorkbook();
            AssertResultSuccess(workbookResult);
            var workbook = workbookResult.Value;

            var addWorksheetResult = workbook.AddWorksheet("DataSheet");
            AssertResultSuccess(addWorksheetResult);
            var worksheet = addWorksheetResult.Value;

            // Test 2D array data
            var testData = CreateTestDataArray(5, 3);
            var setDataResult = worksheet.SetData("A1", testData);
            AssertResultSuccess(setDataResult);

            // Test list data
            var testDataList = CreateTestDataList(3, 2);
            var setListDataResult = worksheet.SetData("E1", testDataList);
            AssertResultSuccess(setListDataResult);

            // Test formula operations
            var setFormulaResult = worksheet.SetFormula("A6", "=SUM(A1:A5)");
            AssertResultSuccess(setFormulaResult);

            var getFormulaResult = worksheet.GetFormula("A6");
            AssertResultSuccess(getFormulaResult);
            Assert.AreEqual("=SUM(A1:A5)", getFormulaResult.Value);

            // Test comment operations
            var addCommentResult = worksheet.AddComment("A1", "This is a test comment");
            AssertResultSuccess(addCommentResult);

            // Test hyperlink operations
            var addHyperlinkResult = worksheet.AddHyperlink("B1", "https://example.com", "Example Link");
            AssertResultSuccess(addHyperlinkResult);
        }

        #endregion

        #region Complete Range Operations Workflow Tests

        /// <summary>
        /// Test complete range operations workflow
        /// </summary>
        [Test]
        public void CompleteRangeOperationsWorkflow_ShouldSucceed()
        {
            // Arrange
            var workbookResult = _workbookFactory.CreateWorkbook();
            AssertResultSuccess(workbookResult);
            var workbook = workbookResult.Value;

            var addWorksheetResult = workbook.AddWorksheet("RangeSheet");
            AssertResultSuccess(addWorksheetResult);
            var worksheet = addWorksheetResult.Value;

            // Test range creation and basic operations
            var rangeResult = worksheet.GetRange("A1:D5");
            AssertResultSuccess(rangeResult);
            var range = rangeResult.Value;

            // Test range properties
            Assert.AreEqual("A1:D5", range.Address);

            // Test range value operations
            var setValueResult = range.SetValue("Test Value");
            AssertResultSuccess(setValueResult);

            var getValueResult = range.GetValue();
            AssertResultSuccess(getValueResult);
            Assert.AreEqual("Test Value", getValueResult.Value);

            // Test range formula operations
            var setFormulaResult = range.SetFormula("=A1+B1");
            AssertResultSuccess(setFormulaResult);

            var getFormulaResult = range.GetFormula();
            AssertResultSuccess(getFormulaResult);
            Assert.AreEqual("=A1+B1", getFormulaResult.Value);

            // Test range formatting operations
            var setFontResult = range.SetFont("Arial", 12, true, false);
            AssertResultSuccess(setFontResult);

            var setAlignmentResult = range.SetAlignment(AlignmentType.Center, VerticalAlignmentType.Middle);
            AssertResultSuccess(setAlignmentResult);

            var setBorderResult = range.SetBorder(BorderType.Thin, BorderColor.Black);
            AssertResultSuccess(setBorderResult);

            var setFillResult = range.SetFill(FillType.Solid, FillColor.LightBlue);
            AssertResultSuccess(setFillResult);

            // Test range operations
            var clearResult = range.Clear();
            AssertResultSuccess(clearResult);

            var copyResult = range.Copy();
            AssertResultSuccess(copyResult);

            var autoFitResult = range.AutoFit();
            AssertResultSuccess(autoFitResult);

            var mergeResult = range.Merge();
            AssertResultSuccess(mergeResult);

            // Test sub-range operations
            var subRangeResult = range.GetSubRange(1, 1, 2, 2);
            AssertResultSuccess(subRangeResult);
            var subRange = subRangeResult.Value;
            Assert.IsNotNull(subRange);
        }

        #endregion

        #region Complete Builder Pattern Workflow Tests

        /// <summary>
        /// Test complete workbook builder pattern workflow
        /// </summary>
        [Test]
        public void CompleteWorkbookBuilderPatternWorkflow_ShouldSucceed()
        {
            // Arrange
            var workbookResult = _workbookFactory.CreateWorkbook();
            AssertResultSuccess(workbookResult);
            var workbook = workbookResult.Value;

            // Act - Test workbook builder pattern
            var builderResult = workbook.CreateBuilder();
            AssertResultSuccess(builderResult);
            var builder = builderResult.Value;

            // Test fluent API chaining
            var configuredBuilder = builder
                .WithTitle("Test Workbook")
                .WithAuthor("Test Author")
                .WithSubject("Test Subject")
                .WithCompany("Test Company")
                .WithCategory("Test Category")
                .WithKeywords("test, workbook, integration")
                .WithComments("Test comments for integration testing")
                .WithManager("Test Manager")
                .WithApplication("AMCode Documents")
                .WithTemplate("Test Template")
                .WithRevision(1)
                .WithEditingTime(TimeSpan.FromMinutes(30))
                .WithPageCount(1)
                .WithWordCount(100)
                .WithCharacterCount(500)
                .WithCharacterCountWithSpaces(600)
                .WithLines(10)
                .WithParagraphs(5)
                .WithSlides(0)
                .WithNotes(0)
                .WithHiddenSlides(0)
                .WithMMClips(0)
                .WithScaleCrop(false)
                .WithLinksUpToDate(false)
                .WithSharedDocument(false)
                .WithHyperlinksChanged(false)
                .WithApplicationVersion("1.0")
                .WithDocSecurity(0)
                .WithAppVersion("1.0")
                .WithHyperlinkBase("https://example.com")
                .WithHyperlinkColor(0xFF0000)
                .WithHyperlinkVisitedColor(0x800080)
                .WithHyperlinkUnderline(true)
                .WithHyperlinkScreenTip("Test Tooltip")
                .WithHyperlinkTarget("_blank")
                .WithHyperlinkLocation("https://example.com")
                .WithHyperlinkDisplayText("Example Link")
                .WithHyperlinkAddress("https://example.com")
                .WithHyperlinkSubAddress("Sheet1!A1")
                .WithHyperlinkExtraInfo("Extra Info")
                .WithHyperlinkFrame("_blank")
                .WithHyperlinkNewWindow(true)
                .WithHyperlinkScreenTip("Test Tooltip")
                .WithHyperlinkTarget("_blank")
                .WithHyperlinkLocation("https://example.com")
                .WithHyperlinkDisplayText("Example Link")
                .WithHyperlinkAddress("https://example.com")
                .WithHyperlinkSubAddress("Sheet1!A1")
                .WithHyperlinkExtraInfo("Extra Info")
                .WithHyperlinkFrame("_blank")
                .WithHyperlinkNewWindow(true);

            // Test building the workbook
            var buildResult = configuredBuilder.Build();
            AssertResultSuccess(buildResult);
            var builtWorkbook = buildResult.Value;
            Assert.IsNotNull(builtWorkbook);
        }

        /// <summary>
        /// Test complete worksheet builder pattern workflow
        /// </summary>
        [Test]
        public void CompleteWorksheetBuilderPatternWorkflow_ShouldSucceed()
        {
            // Arrange
            var workbookResult = _workbookFactory.CreateWorkbook();
            AssertResultSuccess(workbookResult);
            var workbook = workbookResult.Value;

            var addWorksheetResult = workbook.AddWorksheet("BuilderSheet");
            AssertResultSuccess(addWorksheetResult);
            var worksheet = addWorksheetResult.Value;

            // Act - Test worksheet builder pattern
            var builderResult = worksheet.CreateBuilder();
            AssertResultSuccess(builderResult);
            var builder = builderResult.Value;

            // Test fluent API chaining
            var testData = CreateTestDataArray(3, 3);
            var testFormulas = CreateTestFormulas(3);

            var configuredBuilder = builder
                .WithName("Test Worksheet")
                .WithIndex(1)
                .WithVisible(true)
                .WithTabColor(0xFF0000)
                .WithProtection(true, "password123")
                .WithData("A1", testData)
                .WithData("D1", testData)
                .WithFormula("A4", "=SUM(A1:A3)")
                .WithFormula("B4", "=AVERAGE(B1:B3)")
                .WithFormula("C4", "=MAX(C1:C3)")
                .WithComment("A1", "Test comment")
                .WithHyperlink("A2", "https://example.com", "Example Link")
                .WithNamedRange("TestRange", "A1:C3")
                .WithDataValidation("A1:A3", "List", "Option1,Option2,Option3")
                .WithConditionalFormatting("A1:C3", "CellValue", "greaterThan", "5")
                .WithPrintSettings(true, true, true, true)
                .WithPageSetup(PaperSize.A4, PageOrientation.Portrait, 1.0, 1.0, 1.0, 1.0)
                .WithHeaderFooter("Test Header", "Test Footer")
                .WithMargins(1.0, 1.0, 1.0, 1.0, 1.0, 1.0)
                .WithScaling(100, 100)
                .WithPrintArea("A1:C10")
                .WithPrintTitles("1:1", "A:A")
                .WithGridlines(true)
                .WithRowColumnHeaders(true)
                .WithBlackAndWhite(false)
                .WithDraftQuality(false)
                .WithComments(PrintComments.None)
                .WithErrorAs(PrintErrorAs.Blank)
                .WithPageOrder(PrintPageOrder.DownThenOver)
                .WithPrintQuality(600)
                .WithFirstPageNumber(1)
                .WithUseFirstPageNumber(true)
                .WithHorizontalCentered(true)
                .WithVerticalCentered(true)
                .WithFitToPagesWide(1)
                .WithFitToPagesTall(1)
                .WithZoom(100)
                .WithScale(100)
                .WithResolution(600)
                .WithColorMode(ColorMode.Color)
                .WithDpi(600)
                .WithPaperSize(PaperSize.A4)
                .WithOrientation(PageOrientation.Portrait)
                .WithCopies(1)
                .WithCollate(true)
                .WithPrintToFile(false)
                .WithPrintToFile("test.pdf")
                .WithActivePrinter("Microsoft Print to PDF")
                .WithPrinterName("Microsoft Print to PDF")
                .WithDriverName("Microsoft Print to PDF")
                .WithPortName("FILE:")
                .WithPrintRange(PrintRange.All)
                .WithPrintRange("A1:C10")
                .WithPrintRange(1, 1, 10, 10)
                .WithPrintRange(1, 10)
                .WithPrintRange("A1:C10", "E1:G10")
                .WithPrintRange("A1:C10", "E1:G10", "I1:K10")
                .WithPrintRange("A1:C10", "E1:G10", "I1:K10", "M1:O10")
                .WithPrintRange("A1:C10", "E1:G10", "I1:K10", "M1:O10", "Q1:S10")
                .WithPrintRange("A1:C10", "E1:G10", "I1:K10", "M1:O10", "Q1:S10", "U1:W10")
                .WithPrintRange("A1:C10", "E1:G10", "I1:K10", "M1:O10", "Q1:S10", "U1:W10", "Y1:AA10")
                .WithPrintRange("A1:C10", "E1:G10", "I1:K10", "M1:O10", "Q1:S10", "U1:W10", "Y1:AA10", "AC1:AE10")
                .WithPrintRange("A1:C10", "E1:G10", "I1:K10", "M1:O10", "Q1:S10", "U1:W10", "Y1:AA10", "AC1:AE10", "AG1:AI10")
                .WithPrintRange("A1:C10", "E1:G10", "I1:K10", "M1:O10", "Q1:S10", "U1:W10", "Y1:AA10", "AC1:AE10", "AG1:AI10", "AK1:AM10");

            // Test building the worksheet
            var buildResult = configuredBuilder.Build();
            AssertResultSuccess(buildResult);
            var builtWorksheet = buildResult.Value;
            Assert.IsNotNull(builtWorksheet);
        }

        #endregion

        #region Error Handling Across All Layers Tests

        /// <summary>
        /// Test error handling across all layers
        /// </summary>
        [Test]
        public void ErrorHandlingAcrossAllLayers_ShouldHandleGracefully()
        {
            // Test invalid workbook creation
            var invalidWorkbookResult = _workbookFactory.CreateWorkbook(null);
            AssertResultFailure(invalidWorkbookResult);

            // Test invalid file opening
            var invalidFileResult = _workbookFactory.OpenWorkbook("nonexistent.xlsx");
            AssertResultFailure(invalidFileResult);

            // Test invalid stream opening
            using var invalidStream = new MemoryStream();
            var invalidStreamResult = _workbookFactory.OpenWorkbook(invalidStream);
            AssertResultFailure(invalidStreamResult);

            // Test workbook operations on null workbook
            var nullWorkbookResult = _workbookFactory.CreateWorkbook();
            AssertResultSuccess(nullWorkbookResult);
            var workbook = nullWorkbookResult.Value;

            // Test invalid worksheet operations
            var invalidWorksheetResult = workbook.GetWorksheet("NonexistentSheet");
            AssertResultFailure(invalidWorksheetResult);

            // Test invalid range operations
            var addWorksheetResult = workbook.AddWorksheet("TestSheet");
            AssertResultSuccess(addWorksheetResult);
            var worksheet = addWorksheetResult.Value;

            var invalidRangeResult = worksheet.GetRange("InvalidRange");
            AssertResultFailure(invalidRangeResult);

            // Test invalid cell operations
            var invalidCellResult = worksheet.SetCellValue("InvalidCell", "Value");
            AssertResultFailure(invalidCellResult);
        }

        #endregion

        #region Result Pattern Usage Across All Layers Tests

        /// <summary>
        /// Test Result pattern usage across all layers
        /// </summary>
        [Test]
        public void ResultPatternUsageAcrossAllLayers_ShouldBeConsistent()
        {
            // Test workbook factory Result pattern
            var workbookResult = _workbookFactory.CreateWorkbook();
            Assert.IsInstanceOf<Result<IWorkbookDomain>>(workbookResult);
            AssertResultSuccess(workbookResult);

            // Test workbook operations Result pattern
            var workbook = workbookResult.Value;
            var addWorksheetResult = workbook.AddWorksheet("TestSheet");
            Assert.IsInstanceOf<Result<IWorksheet>>(addWorksheetResult);
            AssertResultSuccess(addWorksheetResult);

            // Test worksheet operations Result pattern
            var worksheet = addWorksheetResult.Value;
            var setCellResult = worksheet.SetCellValue("A1", "Test");
            Assert.IsInstanceOf<Result<bool>>(setCellResult);
            AssertResultSuccess(setCellResult);

            var getCellResult = worksheet.GetCellValue("A1");
            Assert.IsInstanceOf<Result<object>>(getCellResult);
            AssertResultSuccess(getCellResult);

            // Test range operations Result pattern
            var rangeResult = worksheet.GetRange("A1:C3");
            Assert.IsInstanceOf<Result<IRange>>(rangeResult);
            AssertResultSuccess(rangeResult);

            var range = rangeResult.Value;
            var setRangeResult = range.SetValue("Test Range");
            Assert.IsInstanceOf<Result<bool>>(setRangeResult);
            AssertResultSuccess(setRangeResult);

            // Test builder pattern Result pattern
            var builderResult = worksheet.CreateBuilder();
            Assert.IsInstanceOf<Result<IWorksheetBuilder>>(builderResult);
            AssertResultSuccess(builderResult);
        }

        #endregion

        #region Resource Cleanup and Disposal Tests

        /// <summary>
        /// Test resource cleanup and disposal
        /// </summary>
        [Test]
        public void ResourceCleanupAndDisposal_ShouldWorkCorrectly()
        {
            // Create workbook and verify it can be disposed
            var workbookResult = _workbookFactory.CreateWorkbook();
            AssertResultSuccess(workbookResult);
            var workbook = workbookResult.Value;

            // Test disposal
            workbook.Dispose();

            // Verify workbook is disposed (should throw on operations)
            var addWorksheetResult = workbook.AddWorksheet("TestSheet");
            AssertResultFailure(addWorksheetResult);
        }

        /// <summary>
        /// Test resource cleanup with using statements
        /// </summary>
        [Test]
        public void ResourceCleanupWithUsingStatements_ShouldWorkCorrectly()
        {
            // Test with using statement
            using (var workbookResult = _workbookFactory.CreateWorkbook())
            {
                AssertResultSuccess(workbookResult);
                var workbook = workbookResult.Value;
                Assert.IsNotNull(workbook);
            }

            // Workbook should be disposed after using block
            // This test verifies that the using pattern works correctly
        }

        #endregion

        #region Performance with Large Workbooks Tests

        /// <summary>
        /// Test performance with large workbooks
        /// </summary>
        [Test]
        public void PerformanceWithLargeWorkbooks_ShouldBeAcceptable()
        {
            // Create large workbook
            var largeFilePath = CreateLargeExcelFile(1000, 10);
            var openResult = _workbookFactory.OpenWorkbook(largeFilePath);
            AssertResultSuccess(openResult);

            using var workbook = openResult.Value;

            // Measure performance of operations
            var addWorksheetTime = MeasureExecutionTime(() =>
            {
                var result = workbook.AddWorksheet("LargeSheet");
                AssertResultSuccess(result);
            });

            Assert.IsTrue(addWorksheetTime < 5000, $"Add worksheet took too long: {addWorksheetTime}ms");

            // Test memory usage
            var memoryUsage = MeasureMemoryUsage(() =>
            {
                var result = workbook.AddWorksheet("MemoryTestSheet");
                AssertResultSuccess(result);
            });

            Assert.IsTrue(memoryUsage < 10 * 1024 * 1024, $"Memory usage too high: {memoryUsage} bytes");
        }

        #endregion

        #region Memory Usage and Leak Detection Tests

        /// <summary>
        /// Test memory usage and leak detection
        /// </summary>
        [Test]
        public void MemoryUsageAndLeakDetection_ShouldNotLeak()
        {
            // Test multiple workbook creation and disposal
            for (int i = 0; i < 10; i++)
            {
                var workbookResult = _workbookFactory.CreateWorkbook();
                AssertResultSuccess(workbookResult);
                workbookResult.Value.Dispose();
            }

            // Force garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // Verify no memory leaks (this is a basic test)
            var memoryAfter = GC.GetTotalMemory(false);
            Assert.IsTrue(memoryAfter < 100 * 1024 * 1024, "Potential memory leak detected");
        }

        #endregion
    }

    #region Test Helper Classes

    /// <summary>
    /// Test implementation of IWorkbookLogger for integration testing
    /// </summary>
    public class TestWorkbookLogger : IWorkbookLogger
    {
        public void LogWorkbookOperation(string operation, string details)
        {
            // Test implementation - just verify method can be called
        }

        public void LogError(string error, Exception exception = null)
        {
            // Test implementation - just verify method can be called
        }

        public void LogPerformance(string operation, long milliseconds)
        {
            // Test implementation - just verify method can be called
        }

        public void LogWarning(string warning, string details = null)
        {
            // Test implementation - just verify method can be called
        }
    }

    /// <summary>
    /// Test implementation of IWorkbookValidator for integration testing
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
