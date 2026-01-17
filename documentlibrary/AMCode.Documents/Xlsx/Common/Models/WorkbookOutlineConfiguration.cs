namespace AMCode.Documents.Xlsx
{
    public class WorkbookOutlineConfiguration
    {
        public SummaryRowPosition SummaryRowPosition { get; set; } = SummaryRowPosition.Below;
        public SummaryColumnPosition SummaryColumnPosition { get; set; } = SummaryColumnPosition.Right;
        public bool ApplyStyles { get; set; }
        public int MaxOutlineLevel { get; set; } = 7;
    }
}
