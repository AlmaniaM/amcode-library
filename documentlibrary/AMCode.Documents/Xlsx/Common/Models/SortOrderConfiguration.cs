namespace AMCode.Documents.Xlsx
{
    public class SortOrderConfiguration
    {
        public string ColumnOrRow { get; set; }
        public bool Ascending { get; set; } = true;
        public bool CaseSensitive { get; set; }
        public int SortByColumn { get; set; }
    }
}
