namespace AMCode.Documents.Xlsx.Domain.Objects
{
    public class WorksheetObject
    {
        public IWorksheet Worksheet { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
    }
}
