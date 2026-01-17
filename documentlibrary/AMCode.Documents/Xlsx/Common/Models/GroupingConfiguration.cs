namespace AMCode.Documents.Xlsx
{
    public class GroupingConfiguration
    {
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public int StartColumn { get; set; }
        public int EndColumn { get; set; }
        public bool Collapsed { get; set; }
        public int Level { get; set; }
    }
}
