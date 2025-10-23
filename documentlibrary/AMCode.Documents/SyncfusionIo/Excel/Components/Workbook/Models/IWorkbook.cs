using System.IO;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx
{
    /// <summary>
    /// An interface designed to emulate an Excel workbook.
    /// </summary>
    public interface IWorkbook
    {
        /// <summary>
        /// Get an instance of a <see cref="IStyles"/> object.
        /// </summary>
        IStyles Styles { get; }

        /// <summary>
        /// Get an <see cref="IWorkbooks"/> object.
        /// </summary>
        IWorkbooks Workbooks { get; }

        /// <summary>
        /// Get an <see cref="IWorksheets"/> object.
        /// </summary>
        IWorksheets Worksheets { get; }

        /// <summary>
        /// Close the current workbook.
        /// </summary>
        void Close();

        /// <summary>
        /// Saves the current <see cref="IWorkbook"/> to the provided <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">Provide a <see cref="Stream"/> to save the workbook to.</param>
        void SaveAs(Stream stream);
    }

    internal interface IInternalWorkbook : IWorkbook
    {
        /// <summary>
        /// Get the underlying <see cref="IInternalStyles"/> object.
        /// </summary>
        IInternalStyles InternalStyles { get; }

        /// <summary>
        /// Get the underlying <see cref="Lib.IWorkbook"/> object.
        /// </summary>
        Lib.IWorkbook InnerLibWorkbook { get; }
    }
}