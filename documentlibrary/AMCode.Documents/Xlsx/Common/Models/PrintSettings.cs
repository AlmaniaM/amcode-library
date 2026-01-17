namespace AMCode.Documents.Xlsx
{
    public class PrintSettings
    {
        public int Copies { get; set; } = 1;
        public bool Collate { get; set; } = true;
        public string PrintArea { get; set; }
        public string PrintTitles { get; set; }
        public int FirstPageNumber { get; set; } = 1;
        public bool UseFirstPageNumber { get; set; }
    }
}
