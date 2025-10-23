using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Dictionary;
using AMCode.Storage.Memory;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AMCode.Storage.UnitTests.Memory.StreamDataSourceTests
{
    [TestFixture]
    public class StreamDataSourceTest
    {
        private MemoryStreamDataSource dataSource;
        private readonly string fileName = "test-in-memory-download.json";
        private IDictionary<string, Stream> streamStore;
        private readonly LocalTestHelper testHelper = new();

        [SetUp]
        public void SetUp()
        {
            streamStore = new Dictionary<string, Stream>();
            dataSource = new(streamStore);
        }

        [TearDown]
        public void TearDown() => dataSource.Dispose();

        [Test]
        public async Task ShouldSetStream()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);
            using var stream = new FileStream(filePath, FileMode.Open);

            await dataSource.SetStreamAsync(stream);

            Assert.That(streamStore.ContainsKey(dataSource.FileName), Is.True);
        }

        [Test]
        public async Task ShouldGetStream()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);
            using var stream = new FileStream(filePath, FileMode.Open);

            streamStore.Add(dataSource.FileName, stream);

            var storedStream = await dataSource.GetStreamAsync();

            var streamReader = new StreamReader(storedStream);

            var blobContents = streamReader.ReadToEnd();

            var jsonFile = JsonConvert.DeserializeObject<Dictionary<string, string>>(blobContents);

            Assert.AreEqual("download", jsonFile.GetValue("key"));
        }

        [Test]
        public void ShouldDisposeOfStream()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);
            using var stream = new FileStream(filePath, FileMode.Open);

            streamStore.Add(dataSource.FileName, stream);

            dataSource.Dispose();

            Assert.That(streamStore.ContainsKey(dataSource.FileName), Is.False);
        }
    }
}