using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Dictionary;
using AMCode.Storage.Local;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AMCode.Storage.UnitTests.Local.FileStreamDataSourceTests
{
    [TestFixture]
    public class FileStreamDataSourceTest
    {
        private string expectedFileDirectory;
        private string expectedFilePath;
        private FileStreamDataSource dataSource;
        private readonly string fileName = "test-local-download.json";
        private readonly LocalTestHelper testHelper = new();

        [SetUp]
        public void SetUp()
        {

            expectedFileDirectory = testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext);

            dataSource = new(expectedFileDirectory);

            expectedFilePath = Path.Combine(expectedFileDirectory, dataSource.FileName);
        }

        [TearDown]
        public void TearDown()
        {
            dataSource.Dispose();

            if (Directory.Exists(expectedFileDirectory) && Directory.GetFiles(expectedFileDirectory).Length > 0)
            {
                Directory.GetFiles(expectedFileDirectory).ToList().ForEach(filePath => File.Delete(filePath));
            }
        }

        [Test]
        public async Task ShouldSetStream()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);
            using var stream = new FileStream(filePath, FileMode.Open);

            await dataSource.SetStreamAsync(stream);

            Assert.That(File.Exists(expectedFilePath), Is.True);
        }

        [Test]
        public async Task ShouldGetStream()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);

            await testHelper.CopyFiles(filePath, expectedFilePath);

            var storedStream = await dataSource.GetStreamAsync();

            var streamReader = new StreamReader(storedStream);

            var blobContents = streamReader.ReadToEnd();

            var jsonFile = JsonConvert.DeserializeObject<Dictionary<string, string>>(blobContents);

            Assert.AreEqual("download", jsonFile.GetValue("key"));
        }

        [Test]
        public async Task ShouldDisposeOfStream()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);

            await testHelper.CopyFiles(filePath, expectedFilePath);

            dataSource.Dispose();

            Assert.That(File.Exists(expectedFilePath), Is.False);
        }
    }
}