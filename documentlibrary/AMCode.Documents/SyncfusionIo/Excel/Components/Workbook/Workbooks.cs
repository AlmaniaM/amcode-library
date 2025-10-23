using System.Collections;
using System.Collections.Generic;
using System.IO;
using AMCode.SyncfusionIo.Xlsx.Extensions;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx.Internal
{
    /// <summary>
    /// A class designed to mimic an excel workbook.
    /// </summary>
    public class Workbooks : IWorkbooks, IInternalWorkbooks
    {
        /// <summary>
        /// Create an instance of the <see cref="Workbooks"/> class.
        /// </summary>
        /// <param name="application">Provide an <see cref="IExcelApplication"/> object.</param>
        /// <param name="libWorkbooks">Provide a <see cref="Lib.IWorkbooks"/> instance.</param>
        internal Workbooks(IExcelApplication application, Lib.IWorkbooks libWorkbooks)
        {
            InnerWorkbooks = libWorkbooks;
            Application = application;
        }

        /// <inheritdoc/>
        public IWorkbook this[int index] => GetWorkbook(index);

        /// <inheritdoc/>
        public IExcelApplication Application { get; }

        /// <inheritdoc/>
        public Lib.IWorkbooks InnerWorkbooks { get; }

        /// <inheritdoc/>
        public int Count => InnerWorkbooks.Count;

        /// <inheritdoc/>
        public IWorkbook Create()
        {
            var workbook = InnerWorkbooks.Create(0);
            return new Workbook(this, workbook);
        }

        /// <inheritdoc/>
        public IWorkbook GetWorkbook(int index)
        {
            if (index < 0)
            {
                return default;
            }

            if (index >= InnerWorkbooks.Count)
            {
                return default;
            }

            return new Workbook(this, InnerWorkbooks[index]);
        }

        /// <summary>
        /// Get an <see cref="IEnumerator{T}"/> of <see cref="IWorkbook"/> objects.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> of <see cref="IWorkbook"/> objects.</returns>
        public IEnumerator<IWorkbook> GetEnumerator() => InnerWorkbooks.ToWorkbooksEnumerator(this);

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => InnerWorkbooks.ToWorkbooksEnumerator(this);

        /// <inheritdoc/>
        public IWorkbook Open(Stream stream) => new Workbook(this, InnerWorkbooks.Open(stream));
    }
}