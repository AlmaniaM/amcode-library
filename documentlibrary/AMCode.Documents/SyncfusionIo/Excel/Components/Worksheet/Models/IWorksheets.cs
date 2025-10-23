using System.Collections;
using System.Collections.Generic;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx
{
    /// <summary>
    /// An interface designed to emulate a collection of <see cref="IWorksheet"/>s.
    /// </summary>
    public interface IWorksheets : IEnumerable<IWorksheet>, IEnumerable
    {
        /// <summary>
        /// Get an <see cref="IWorksheet"/> based on its name.
        /// </summary>
        /// <param name="sheetName">The name of th worksheet.</param>
        /// <returns>An <see cref="IWorksheet"/> object.</returns>
        IWorksheet this[string sheetName] { get; }

        /// <summary>
        /// Get an <see cref="IWorksheet"/> based on its index.
        /// </summary>
        /// <param name="index">The index of th worksheet.</param>
        /// <returns>An <see cref="IWorksheet"/> object.</returns>
        IWorksheet this[int index] { get; }

        /// <summary>
        /// The number of worksheets available.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Get an instance of a <see cref="IStyles"/> object.
        /// </summary>
        IStyles Styles { get; }

        /// <summary>
        /// Get the parent <see cref="IWorkbook"/> object.
        /// </summary>
        IWorkbook Workbook { get; }

        /// <summary>
        /// Create a new <see cref="IWorksheet"/>.
        /// </summary>
        /// <returns>A reference to the new <see cref="IWorksheet"/>.</returns>
        IWorksheet Create();

        /// <summary>
        /// Create a new <see cref="IWorksheet"/>.
        /// </summary>
        /// <param name="name">The name to give the worksheet.</param>
        /// <returns>A reference to the new <see cref="IWorksheet"/>.</returns>
        IWorksheet Create(string name);

        /// <summary>
        /// Get an <see cref="IWorksheet"/> based on its name.
        /// </summary>
        /// <param name="sheetName">The name of th worksheet.</param>
        /// <returns>An <see cref="IWorksheet"/> object.</returns>
        IWorksheet GetWorksheet(string sheetName);

        /// <summary>
        /// Get an <see cref="IWorksheet"/> based on its index.
        /// </summary>
        /// <param name="index">The index of th worksheet.</param>
        /// <returns>An <see cref="IWorksheet"/> object.</returns>
        IWorksheet GetWorksheet(int index);

        /// <summary>
        /// Removes specified worksheet from the collection.
        /// </summary>
        /// <param name="index">Index of the sheet to remove.</param>
        void Remove(int index);

        /// <summary>
        /// Removes specified worksheet from the collection.
        /// </summary>
        /// <param name="sheetName">Name of the sheet to remove.</param>
        void Remove(string sheetName);

        /// <summary>
        /// Remove worksheet from collection.
        /// </summary>
        /// <param name="sheet">Reference on worksheet to remove.</param>
        void Remove(IWorksheet sheet);
    }

    internal interface IInternalWorksheets : IWorksheets
    {
        /// <summary>
        /// Get the underlying <see cref="IInternalStyles"/> object.
        /// </summary>
        IInternalStyles InternalStyles { get; }

        /// <summary>
        /// Get the underlying <see cref="Lib.IWorkbook"/> object.
        /// </summary>
        Lib.IWorkbook InnerLibWorkbook { get; }

        /// <summary>
        /// Get a reference to the <see cref="IInternalWorkbook"/> object.
        /// </summary>
        IInternalWorkbook InternalWorkbook { get; }

        /// <summary>
        /// Get the underlying <see cref="Lib.IWorksheets"/> object.
        /// </summary>
        Lib.IWorksheets InnerLibWorksheets { get; }
    }
}