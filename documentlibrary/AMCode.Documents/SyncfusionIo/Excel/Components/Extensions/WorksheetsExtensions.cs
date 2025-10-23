using System.Collections;
using System.Collections.Generic;
using AMCode.SyncfusionIo.Xlsx.Internal;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx.Extensions
{
    internal static class WorksheetsExtensions
    {
        /// <summary>
        /// Convert a <see cref="Lib.IWorksheet"/> <see cref="IEnumerator"/> into an <see cref="IWorksheet"/> <see cref="IEnumerator{T}"/>.
        /// </summary>
        /// <param name="worksheets">A <see cref="Lib.IWorksheet"/> object.</param>
        /// <param name="internalWorksheets">An <see cref="IInternalWorksheets"/> object.</param>
        /// <returns>An <see cref="IWorksheet"/> <see cref="IEnumerator{T}"/>.</returns>
        public static IEnumerator<IWorksheet> ToWorksheetsEnumerator(this Lib.IWorksheets worksheets, IInternalWorksheets internalWorksheets)
        {
            if (worksheets is null)
            {
                return default;
            }

            var worksheetsEnumerator = worksheets.GetEnumerator();
            var iWorksheetsList = new List<IWorksheet>();

            while (worksheetsEnumerator.MoveNext())
            {
                iWorksheetsList.Add(new Worksheet(internalWorksheets, worksheetsEnumerator.Current));
            }

            return iWorksheetsList.GetEnumerator();
        }
    }
}