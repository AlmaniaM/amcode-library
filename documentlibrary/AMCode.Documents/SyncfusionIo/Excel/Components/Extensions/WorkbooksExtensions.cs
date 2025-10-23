using System.Collections;
using System.Collections.Generic;
using AMCode.SyncfusionIo.Xlsx.Internal;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx.Extensions
{
    internal static class WorkbooksExtensions
    {
        /// <summary>
        /// Convert a <see cref="Lib.IWorkbook"/> <see cref="IEnumerator"/> into an <see cref="IWorkbook"/> <see cref="IEnumerator{T}"/>.
        /// </summary>
        /// <param name="workbooks">A <see cref="Lib.IWorkbook"/> object.</param>
        /// <param name="internalIIWorkbooks">An <see cref="IInternalWorkbooks"/> object.</param>
        /// <returns>An <see cref="IWorkbook"/> <see cref="IEnumerator{T}"/>.</returns>
        public static IEnumerator<IWorkbook> ToWorkbooksEnumerator(this Lib.IWorkbooks workbooks, IInternalWorkbooks internalIIWorkbooks)
        {
            if (workbooks is null)
            {
                return default;
            }

            var workbooksEnumerator = workbooks.GetEnumerator();
            var iWorkbooksList = new List<Workbook>();

            while (workbooksEnumerator.MoveNext())
            {
                iWorkbooksList.Add(new Workbook(internalIIWorkbooks, workbooksEnumerator.Current));
            }

            return iWorkbooksList.GetEnumerator();
        }
    }
}