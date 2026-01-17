using AMCode.Exports.Book;

namespace AMCode.Exports.UnitTests.BookBuilder.Mocks
{
    public class TestDataColumn : IBookDataColumn
    {
        public string DataFieldName { get; set; }
        public string WorksheetHeaderName { get; set; }
    }
}