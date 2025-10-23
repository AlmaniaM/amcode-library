using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    /// A delegate for retrieving a range of data.
    /// </summary>
    /// <param name="start">The starting row of the data to fetch.</param>
    /// <param name="count">The number of records to fetch.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling requests.</param>
    /// <returns>An <see cref="IList{T}"/> of <see cref="ExpandoObject"/>s <see cref="Task"/>.</returns>
    public delegate Task<IList<ExpandoObject>> ExportDataRangeFetch(int start, int count, CancellationToken cancellationToken = default);

    /// <summary>
    /// An interface designed to provide configuration data/functionality for a <see cref="IBookBuilder{TColumn}"/> object.
    /// </summary>
    public interface IBookBuilderConfig
    {
        /// <summary>
        /// A function to fetch records
        /// </summary>
        ExportDataRangeFetch FetchDataAsync { get; set; }

        /// <summary>
        /// The maximum number or records the builder can fetch per data chunk request.
        /// </summary>
        int MaxRowsPerDataFetch { get; set; }
    }
}