using System.IO;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx.Internal
{
    /// <summary>
    /// A class designed to emulate an Excel workbook.
    /// </summary>
    public class Workbook : IWorkbook, IInternalWorkbook
    {
        private readonly IInternalWorkbooks internalWorkbooks;

        /// <summary>
        /// Create an instance of the <see cref="Workbook"/> class.
        /// </summary>
        /// <param name="internalWorkbooks">An instance of an <see cref="IInternalWorkbooks"/> object.</param>
        /// <param name="libWorkbook">Pass in an instance of a <see cref="Lib.IWorkbook"/> object.</param>
        internal Workbook(IInternalWorkbooks internalWorkbooks, Lib.IWorkbook libWorkbook)
        {
            InnerLibWorkbook = libWorkbook;
            this.internalWorkbooks = internalWorkbooks;
        }

        /// <inheritdoc/>
        public IStyles Styles => new Styles(InnerLibWorkbook.Styles);

        /// <inheritdoc/>
        public IWorksheets Worksheets => new Worksheets(this, InnerLibWorkbook.Worksheets);

        /// <inheritdoc/>
        public IWorkbooks Workbooks => internalWorkbooks;

        /// <inheritdoc/>
        IInternalStyles IInternalWorkbook.InternalStyles => new Styles(InnerLibWorkbook.Styles);

        /// <inheritdoc/>
        public Lib.IWorkbook InnerLibWorkbook { get; }

        /// <inheritdoc/>
        public void Close() => InnerLibWorkbook.Close();

        /// <inheritdoc/>
        public void SaveAs(Stream stream) => InnerLibWorkbook.SaveAs(stream);
    }
}