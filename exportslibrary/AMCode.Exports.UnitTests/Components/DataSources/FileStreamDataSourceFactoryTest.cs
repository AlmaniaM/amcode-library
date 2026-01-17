using System.IO;
using System.Threading.Tasks;
using AMCode.Exports.DataSource;
using AMCode.Storage.Local;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.DataSources.FileStreamDataSourceFactoryTests
{
    [TestFixture]
    public class FileStreamDataSourceFactoryTest
    {
        private FileStreamDataSourceFactory dataSourceFactory;

        [SetUp]
        public void SetUp() => dataSourceFactory = new FileStreamDataSourceFactory();

        [Test]
        public async Task ShouldReturnAFileStreamDataSource()
        {
            var dataSource = await dataSourceFactory.CreateAsync(new MemoryStream());

            Assert.That(dataSource, Is.TypeOf<FileStreamDataSource>());
        }

        [Test]
        public async Task ShouldReturnAFileStreamDataSourceWithCustomDirectory()
        {
            dataSourceFactory = new FileStreamDataSourceFactory(Path.Combine("Some", "New", "Directory"));

            var dataSource = await dataSourceFactory.CreateAsync(new MemoryStream());

            Assert.That(dataSource, Is.TypeOf<FileStreamDataSource>());
        }
    }
}