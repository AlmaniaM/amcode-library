namespace AMCode.Documents.Xlsx
{
    public class WorkbookPrintSettings
    {
        public int Copies { get; set; } = 1;
        public bool Collate { get; set; } = true;
        public string PrinterName { get; set; }
        public string PaperSize { get; set; }
    }
}
