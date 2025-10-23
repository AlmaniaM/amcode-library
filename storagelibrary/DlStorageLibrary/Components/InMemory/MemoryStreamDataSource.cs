using System.Collections.Generic;
using System.IO;

namespace DemandLink.Storage.Memory
{
    /// <summary>
    /// A class designed to store <see cref="Stream"/>s in memory.
    /// </summary>
    public class MemoryStreamDataSource : BaseStreamDataSource, IStreamDataSourceAsync
    {
        /// <summary>
        /// Create an instance of the <see cref="MemoryStreamDataSource"/> class.
        /// </summary>
        public MemoryStreamDataSource() : this(new Dictionary<string, Stream>()) { }

        /// <summary>
        /// Create an instance of the <see cref="MemoryStreamDataSource"/> class.
        /// </summary>
        /// <param name="streamStore">Provide an <see cref="IDictionary{TKey, TValue}"/> for storing <see cref="Stream"/>s.</param>
        public MemoryStreamDataSource(IDictionary<string, Stream> streamStore) : base(new StreamStorage(streamStore)) { }
    }
}