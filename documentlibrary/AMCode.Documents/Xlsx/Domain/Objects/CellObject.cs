namespace AMCode.Documents.Xlsx.Domain.Objects
{
    public class CellObject
    {
        public ICell Cell { get; set; }
        public string Reference { get; set; }
        public object Value { get; set; }
    }
}
