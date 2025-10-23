using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Dictionary;
using AMCode.Storage;
using AMCode.Storage.Memory;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AMCode.Storage.UnitTests.Memory.StreamStorageTests
{
    [TestFixture]
    public class StreamStorageTest
    {
        private readonly string fileName = "test-in-memory-download.json";
        private StreamStorage storage;
        private IDictionary<string, Stream> streamStorage;
        private readonly LocalTestHelper testHelper = new();

        [SetUp]
        public void SetUp()
        {
            streamStorage = new Dictionary<string, Stream>();

            storage = new StreamStorage(streamStorage);
        }

        [TearDown]
        public void TearDown() => storage.Dispose();

        [Test]
        public async Task ShouldSaveStream()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);
            using var stream = new FileStream(filePath, FileMode.Open);

            // Save the file
            var updatedFileName = await storage.CreateFileAsync(fileName, stream);

            Assert.That(streamStorage.ContainsKey(fileName), Is.True);
            Assert.That(streamStorage[fileName], Is.EqualTo(stream));
        }

        [Test]
        public async Task ShouldDeleteStream()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);
            using var stream = new MemoryStream();

            streamStorage.Add(fileName, stream);

            await storage.DeleteFileAsync(fileName);

            Assert.That(streamStorage.ContainsKey(fileName), Is.False);
        }

        [Test]
        public async Task ShouldDownloadStream()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);
            using var stream = new FileStream(filePath, FileMode.Open);

            streamStorage.Add(fileName, stream);

            using var responseStream = await storage.DownloadFileAsync(fileName);

            using var streamReader = new StreamReader(responseStream);

            var streamContents = streamReader.ReadToEnd();

            var jsonFile = JsonConvert.DeserializeObject<Dictionary<string, string>>(streamContents);

            Assert.AreEqual("download", jsonFile.GetValue("key"));
        }

        [Test]
        public void ShouldThrowExceptionForDownloadStreamWhenNoStreamExists()
        {
            Assert.ThrowsAsync<FileDoesNotExistException>(
                () => storage.DownloadFileAsync(fileName),
                $"[{nameof(StreamStorage)}][{nameof(StreamStorage.DownloadFileAsync)}](fileName, cancellationToken) Error: The specified file \"{fileName}\" does not exist."
            );
        }

        [Test]
        public async Task ShouldReturnTrueIfStreamExistsAndFalseAfterStreamIsDeleted()
        {
            using var stream = new MemoryStream();

            streamStorage.Add(fileName, stream);

            Assert.That(await storage.ExistsAsync(fileName), Is.True);

            await storage.DeleteFileAsync(fileName);

            Assert.That(await storage.ExistsAsync(fileName), Is.False);
        }
    }
}