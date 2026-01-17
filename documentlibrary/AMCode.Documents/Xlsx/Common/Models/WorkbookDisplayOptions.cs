namespace AMCode.Documents.Xlsx
{
    /// <summary>
    /// Display options for a workbook
    /// </summary>
    public class WorkbookDisplayOptions
    {
        public bool ShowHorizontalScrollBar { get; set; } = true;
        public bool ShowVerticalScrollBar { get; set; } = true;
        public bool ShowSheetTabs { get; set; } = true;
        public int FirstSheetIndex { get; set; }
        public int ActiveSheetIndex { get; set; }
        public int TabRatio { get; set; } = 600;
    }
}
