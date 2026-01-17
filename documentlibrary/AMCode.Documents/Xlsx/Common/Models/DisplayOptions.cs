namespace AMCode.Documents.Xlsx
{
    public class DisplayOptions
    {
        public bool ShowGridlines { get; set; } = true;
        public bool ShowRowColHeaders { get; set; } = true;
        public bool ShowFormulas { get; set; }
        public bool ShowZeros { get; set; } = true;
        public bool RightToLeft { get; set; }
        public int ZoomLevel { get; set; } = 100;
    }
}
