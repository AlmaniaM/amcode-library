using System.IO;
using System.Threading.Tasks;
using AMCode.Exports.DataSource;
using AMCode.Storage.Memory;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.DataSources.MemoryStreamDataSourceFactoryTests
{
    [TestFixture]
    public class MemoryStreamDataSourceFactoryTest
    {
        private MemoryStreamDataSourceFactory dataSourceFactory;

        [SetUp]
        public void SetUp() => dataSourceFactory = new MemoryStreamDataSourceFactory();

        [Test]
        public async Task ShouldReturnAMemoryStreamDataSource()
        {
            var dataSource = await dataSourceFactory.CreateAsync(new MemoryStream());

            Assert.That(dataSource, Is.TypeOf<MemoryStreamDataSource>());
        }
    }
}