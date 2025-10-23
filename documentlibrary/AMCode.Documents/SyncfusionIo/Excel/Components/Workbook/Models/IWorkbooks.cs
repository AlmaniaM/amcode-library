using System.Collections;
using System.Collections.Generic;
using System.IO;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx
{
    /// <summary>
    /// An interface designed to represent a collection of <see cref="IWorkbook"/>s.
    /// </summary>
    public interface IWorkbooks : IEnumerable<IWorkbook>, IEnumerable
    {
        /// <summary>
        /// Get an <see cref="IWorkbook"/> object by its index.
        /// </summary>
        /// <param name="index">The index of the <see cref="IWorkbook"/> object.</param>
        /// <returns>The <see cref="IWorkbook"/> at the index.</returns>
        IWorkbook this[int index] { get; }

        /// <summary>
        /// Get an instance of the <see cref="IExcelApplication"/> object.
        /// </summary>
        IExcelApplication Application { get; }

        /// <summary>
        /// Get the number of objects in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Create a new <see cref="IWorkbook"/> object.
        /// </summary>
        /// <returns>The new <see cref="IWorkbook"/> object.</returns>
        IWorkbook Create();

        /// <summary>
        /// Get an <see cref="IWorkbook"/> object by its index.
        /// </summary>
        /// <param name="index">The index of the <see cref="IWorkbook"/> object.</param>
        /// <returns>The <see cref="IWorkbook"/> at the index.</returns>
        IWorkbook GetWorkbook(int index);

        /// <summary>
        /// Reads workbook from the stream.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> with workbook's data.</param>
        /// <returns>Newly created workbook.</returns>
        IWorkbook Open(Stream stream);
    }

    internal interface IInternalWorkbooks : IWorkbooks
    {
        /// <summary>
        /// Get the underlying <see cref="Lib.IWorkbooks"/> object.
        /// </summary>
        Lib.IWorkbooks InnerWorkbooks { get; }
    }
}