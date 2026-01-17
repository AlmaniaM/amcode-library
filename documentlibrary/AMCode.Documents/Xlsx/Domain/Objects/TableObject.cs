namespace AMCode.Documents.Xlsx.Domain.Objects
{
    public class TableObject
    {
        public string Name { get; set; }
        public string Range { get; set; }
        public bool ShowHeaders { get; set; } = true;
        public bool ShowTotals { get; set; }
    }
}
