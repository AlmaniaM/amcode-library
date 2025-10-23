using System;
using AMCode.SyncfusionIo.Xlsx.Internal;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx
{
    /// <summary>
    /// A class designed to represent an Excel application emulator.
    /// </summary>
    public class ExcelApplication : IExcelApplication, IDisposable
    {
        private readonly Lib.ExcelEngine excelEngine;
        private readonly Lib.IApplication excelApplication;

        /// <summary>
        /// Create an instance of the <see cref="ExcelApplication"/> class.
        /// </summary>
        public ExcelApplication()
        {
            excelEngine = new Lib.ExcelEngine();
            excelApplication = excelEngine.Excel;
            excelApplication.DefaultVersion = Lib.ExcelVersion.Xlsx;
            Workbooks = new Workbooks(this, excelApplication.Workbooks);
        }

        /// <inheritdoc/>
        public IWorkbooks Workbooks { get; }

        /// <inheritdoc/>
        public void Dispose() => excelEngine.Dispose();
    }
}