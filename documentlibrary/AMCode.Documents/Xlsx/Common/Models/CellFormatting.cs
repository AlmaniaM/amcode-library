namespace AMCode.Documents.Xlsx
{
    public class CellFormatting
    {
        public FontSettings Font { get; set; }
        public AlignmentSettings Alignment { get; set; }
        public BorderSettings Border { get; set; }
        public FillSettings Fill { get; set; }
        public ProtectionSettings Protection { get; set; }
        public string NumberFormat { get; set; }
    }
}
