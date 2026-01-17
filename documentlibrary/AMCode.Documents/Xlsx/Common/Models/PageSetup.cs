namespace AMCode.Documents.Xlsx
{
    public class PageSetup
    {
        public PrintOrientation Orientation { get; set; } = PrintOrientation.Portrait;
        public string PaperSize { get; set; } = "Letter";
        public double ScaleFactor { get; set; } = 100.0;
        public int FitToWidth { get; set; }
        public int FitToHeight { get; set; }
        public PrintMargins Margins { get; set; } = new PrintMargins();
    }
}
