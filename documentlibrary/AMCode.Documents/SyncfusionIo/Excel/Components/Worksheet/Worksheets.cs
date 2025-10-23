using System;
using System.Collections;
using System.Collections.Generic;
using AMCode.SyncfusionIo.Xlsx.Extensions;
using Lib = Syncfusion.XlsIO;

namespace AMCode.SyncfusionIo.Xlsx.Internal
{
    /// <summary>
    /// A class designed to act as a collection of <see cref="IWorksheet"/> objects.
    /// </summary>
    public class Worksheets : IWorksheets, IInternalWorksheets
    {
        private readonly IInternalWorkbook internalWorkbook;

        /// <summary>
        /// Create an instance of the <see cref="IWorksheets"/> class.
        /// </summary>
        /// <param name="internalWorkbook">An instance of the <see cref="IInternalWorkbook"/> object.</param>
        /// <param name="libWorksheets">An instance of the <see cref="Lib.IWorksheets"/> object.</param>
        internal Worksheets(IInternalWorkbook internalWorkbook, Lib.IWorksheets libWorksheets)
        {
            InnerLibWorksheets = libWorksheets;
            this.internalWorkbook = internalWorkbook;
        }

        /// <inheritdoc/>
        public IWorksheet this[string sheetName] => GetWorksheet(sheetName);

        /// <inheritdoc/>
        public IWorksheet this[int index] => GetWorksheet(index);

        /// <inheritdoc/>
        public int Count => InnerLibWorksheets.Count;

        /// <inheritdoc/>
        public IStyles Styles => Workbook.Styles;

        /// <inheritdoc/>
        public IWorkbook Workbook => internalWorkbook;

        /// <inheritdoc/>
        IInternalStyles IInternalWorksheets.InternalStyles => internalWorkbook.InternalStyles;

        /// <inheritdoc/>
        public Lib.IWorkbook InnerLibWorkbook => internalWorkbook.InnerLibWorkbook;

        /// <inheritdoc cref="IInternalWorksheets.InnerLibWorksheets"/>
        internal Lib.IWorksheets InnerLibWorksheets { get; }

        /// <inheritdoc/>
        Lib.IWorksheets IInternalWorksheets.InnerLibWorksheets => InnerLibWorksheets;

        /// <inheritdoc cref="IInternalWorksheets.InternalWorkbook"/>
        internal IInternalWorkbook InternalWorkbook { get; }

        /// <inheritdoc/>
        IInternalWorkbook IInternalWorksheets.InternalWorkbook => InternalWorkbook;

        /// <inheritdoc/>
        public IWorksheet Create() => new Worksheet(this, InnerLibWorksheets.Create());

        /// <inheritdoc/>
        public IWorksheet Create(string name) => new Worksheet(this, InnerLibWorksheets.Create(name));

        /// <inheritdoc/>
        public IEnumerator<IWorksheet> GetEnumerator() => InnerLibWorksheets.ToWorksheetsEnumerator(this);

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => InnerLibWorksheets.ToWorksheetsEnumerator(this);

        /// <inheritdoc/>
        public IWorksheet GetWorksheet(string sheetName)
        {
            try
            {
                return new Worksheet(this, InnerLibWorksheets[sheetName]);
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <inheritdoc/>
        public IWorksheet GetWorksheet(int index)
        {
            try
            {
                if (index < 0)
                {
                    return default;
                }

                if (index >= InnerLibWorksheets.Count)
                {
                    return default;
                }

                return new Worksheet(this, InnerLibWorksheets[index]);
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <inheritdoc/>
        public void Remove(int index)
        {
            var worksheet = GetWorksheet(index);

            if (worksheet != default)
            {
                InnerLibWorksheets.Remove(index);
            }
        }

        /// <inheritdoc/>
        public void Remove(string sheetName)
        {
            var worksheet = GetWorksheet(sheetName);

            if (worksheet != default)
            {
                InnerLibWorksheets.Remove(sheetName);
            }
        }

        /// <inheritdoc/>
        public void Remove(IWorksheet sheet)
        {
            if (sheet != null)
            {
                InnerLibWorksheets.Remove(sheet.Name);
            }
        }
    }
}