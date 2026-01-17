using AMCode.SyncfusionIo.Xlsx;

namespace AMCode.Exports.Book
{
    /// <summary>
    /// A factory class designed to build <see cref="IBookFactory{TColumn}"/> of type <see cref="IExcelDataColumn"/> objects.
    /// </summary>
    public class ExcelBookFactory : IExcelBookFactory
    {
        /// <summary>
        /// Create an instance of the <see cref="ExcelBookFactory"/> class.
        /// </summary>
        public ExcelBookFactory() { }

        /// <summary>
        /// Create an instance of the <see cref="ExcelBookFactory"/> class.
        /// </summary>
        /// <param name="maxRowsPerSheet">Provide a number representing the maximum number or rows allowed per
        /// book sheet.</param>
        public ExcelBookFactory(int maxRowsPerSheet)
        {
            this.maxRowsPerSheet = maxRowsPerSheet;
        }

        private int maxRowsPerSheet { get; set; }

        /// <summary>
        /// Create an <see cref="IBook{TColumn}"/> of type <see cref="IExcelDataColumn"/>.
        /// </summary>
        /// <returns>An <see cref="IBook{TColumn}"/> of type <see cref="IExcelDataColumn"/>.</returns>
        public IBook<IExcelDataColumn> CreateBook() => new ExcelBook(new ExcelApplication())
        {
            MaxRowsPerSheet = maxRowsPerSheet
        };
    }
}