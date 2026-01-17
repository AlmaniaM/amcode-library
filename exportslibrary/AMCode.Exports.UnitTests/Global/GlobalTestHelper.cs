using AMCode.Documents.Xlsx;

namespace AMCode.Exports.UnitTests.Global
{
    public class GlobalTestHelper
    {
        /// <summary>
        /// Create a <see cref="ExcelTestResources"/> object. The contents are the references to the application and
        /// underlying library references that were used to create all objects.
        /// </summary>
        /// <returns></returns>
        public static ExcelTestResources GetExcelTestResources()
        {
            var excelApplication = new ExcelApplication();
            var workbooks = excelApplication.Workbooks;
            var workbook = workbooks.Create();
            var worksheets = workbook.Worksheets;
            var worksheetName = "Test Sheet";
            var worksheet = worksheets.Create(worksheetName);

            return new ExcelTestResources
            {
                Application = excelApplication,
                Styles = workbook.Styles,
                Workbooks = workbooks,
                Workbook = workbook,
                Worksheets = worksheets,
                Worksheet = worksheet,
                WorksheetName = worksheetName
            };
        }
    }
}