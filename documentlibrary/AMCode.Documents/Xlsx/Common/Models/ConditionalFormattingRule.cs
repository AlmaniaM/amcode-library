namespace AMCode.Documents.Xlsx
{
    public class ConditionalFormattingRule
    {
        public string Type { get; set; }
        public string Operator { get; set; }
        public string Formula1 { get; set; }
        public string Formula2 { get; set; }
        public int Priority { get; set; }
        public bool StopIfTrue { get; set; }
        public FontSettings Font { get; set; }
        public FillSettings Fill { get; set; }
        public BorderSettings Border { get; set; }
    }
}
