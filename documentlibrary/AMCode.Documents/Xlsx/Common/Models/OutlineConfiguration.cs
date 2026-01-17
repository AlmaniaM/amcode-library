namespace AMCode.Documents.Xlsx
{
    public class OutlineConfiguration
    {
        public SummaryRowPosition SummaryRowPosition { get; set; } = SummaryRowPosition.Below;
        public SummaryColumnPosition SummaryColumnPosition { get; set; } = SummaryColumnPosition.Right;
        public bool ApplyStyles { get; set; }
    }
}
