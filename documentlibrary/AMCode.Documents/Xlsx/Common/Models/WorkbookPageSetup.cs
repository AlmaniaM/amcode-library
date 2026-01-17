namespace AMCode.Documents.Xlsx
{
    public class WorkbookPageSetup
    {
        public PrintOrientation Orientation { get; set; } = PrintOrientation.Portrait;
        public string PaperSize { get; set; } = "Letter";
        public double ScaleFactor { get; set; } = 100.0;
    }
}
