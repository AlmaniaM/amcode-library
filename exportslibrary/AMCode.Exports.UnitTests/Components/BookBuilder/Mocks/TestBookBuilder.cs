using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Storage;

namespace AMCode.Exports.UnitTests.BookBuilder.Mocks
{
    public class TestBookBuilder : IBookBuilder<TestDataColumn>
    {
        public Stream BuildBookValue { get; set; }

        public Task<Stream> BuildBookAsync(int _, int _1, IEnumerable<TestDataColumn> _2, CancellationToken _3 = default)
            => Task.FromResult(BuildBookValue);
        public Task<Stream> BuildBookAsync(int _, int _1, IEnumerable<IBookDataColumn> _2, CancellationToken _3 = default)
            => Task.FromResult(BuildBookValue);
        Task<IStreamDataSourceAsync> IBookBuilder<TestDataColumn>.BuildBookAsync(int startRow, int count, IEnumerable<TestDataColumn> columns, CancellationToken cancellationToken)
            => throw new System.NotImplementedException();

        Task<IStreamDataSourceAsync> IBookBuilder.BuildBookAsync(int startRow, int count, IEnumerable<IBookDataColumn> columns, CancellationToken cancellationToken)
            => throw new System.NotImplementedException();
    }
}