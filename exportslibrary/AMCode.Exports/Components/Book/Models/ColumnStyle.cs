using AMCode.SyncfusionIo.Xlsx;

namespace AMCode.Exports.Book
{
    /// <summary>
    /// A class designed to hold column styling information.
    /// </summary>
    public class ColumnStyle : IColumnStyle
    {
        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public IStyleParam Style { get; set; }

        /// <inheritdoc/>
        public double? Width { get; set; }
    }
}