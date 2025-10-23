using AMCode.Xlsx;

namespace DlExportsLibrary.UnitTests.Global
{
    public class ExcelTestResources
    {
        public IExcelApplication Application { get; set; }
        public IStyles Styles { get; set; }
        public IWorkbooks Workbooks { get; set; }
        public IWorkbook Workbook { get; set; }
        public IWorksheets Worksheets { get; set; }
        public IWorksheet Worksheet { get; set; }
        public string WorksheetName { get; set; }
    }
}