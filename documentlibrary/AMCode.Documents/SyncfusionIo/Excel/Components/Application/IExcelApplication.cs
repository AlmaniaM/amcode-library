namespace AMCode.SyncfusionIo.Xlsx
{
    /// <summary>
    /// An interface designed to represent an Excel application emulator.
    /// </summary>
    public interface IExcelApplication
    {
        /// <summary>
        /// An <see cref="IWorkbooks"/> object representing a collection of
        /// <see cref="IWorkbook"/>s.
        /// </summary>
        IWorkbooks Workbooks { get; }

        /// <summary>
        /// Dispose of any disposable objects.
        /// </summary>
        void Dispose();
    }
}