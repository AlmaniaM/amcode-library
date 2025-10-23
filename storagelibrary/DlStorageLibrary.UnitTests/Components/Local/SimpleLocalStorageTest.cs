using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DemandLink.Common.Extensions.Dictionary;
using DemandLink.Storage;
using DemandLink.Storage.Local;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DlStorageLibrary.UnitTests.Local.SimpleLocalStorageTests
{
    [TestFixture]
    public class SimpleLocalStorageTest
    {
        private string expectedFileDirectory;
        private string expectedFilePath;
        private readonly string fileName = "test-local-download.json";
        private SimpleLocalStorage storage;
        private readonly LocalTestHelper testHelper = new();

        [SetUp]
        public void SetUp()
        {
            storage = new SimpleLocalStorage(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext));

            expectedFileDirectory = testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext);

            expectedFilePath = Path.Combine(expectedFileDirectory, fileName);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(expectedFileDirectory) && Directory.GetFiles(expectedFileDirectory).Length > 0)
            {
                Directory.GetFiles(expectedFileDirectory).ToList().ForEach(filePath => File.Delete(filePath));
            }
        }

        [Test]
        public async Task ShouldSaveLocalFile()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);
            using var stream = new FileStream(filePath, FileMode.Open);

            // Save the file
            var updatedFileName = await storage.CreateFileAsync(fileName, stream);

            Assert.That(File.Exists(expectedFilePath), Is.True);
        }

        [Test]
        public async Task ShouldDeleteLocalFile()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);
            await testHelper.CopyFiles(filePath, expectedFilePath);

            Assert.That(File.Exists(expectedFilePath), Is.True);

            await storage.DeleteFileAsync(fileName);

            Assert.That(File.Exists(expectedFilePath), Is.False);
        }

        [Test]
        public async Task ShouldDownloadLocalFile()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);

            if (File.Exists(expectedFilePath))
            {
                File.Delete(expectedFilePath);
            }

            await testHelper.CopyFiles(filePath, expectedFilePath);

            Assert.IsTrue(File.Exists(expectedFilePath));

            using var responseStream = await storage.DownloadFileAsync(fileName);

            using var streamReader = new StreamReader(responseStream);

            var fileContents = streamReader.ReadToEnd();

            var jsonFile = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileContents);

            Assert.AreEqual("download", jsonFile.GetValue("key"));
        }

        [Test]
        public void ShouldThrowExceptionForDownloadLocalFileWhenNoFileExists()
        {
            Assert.ThrowsAsync<FileDoesNotExistException>(
                () => storage.DownloadFileAsync(fileName),
                $"[{nameof(SimpleLocalStorage)}][{nameof(SimpleLocalStorage.DownloadFileAsync)}](fileName, cancellationToken) Error: The specified file \"{fileName}\" does not exist."
            );
        }

        [Test]
        public async Task ShouldReturnTrueIfFileExistsAndFalseAfterFileIsDeleted()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);

            if (File.Exists(expectedFilePath))
            {
                File.Delete(expectedFilePath);
            }

            await testHelper.CopyFiles(filePath, expectedFilePath);

            Assert.That(await storage.ExistsAsync(fileName), Is.True);

            await storage.DeleteFileAsync(fileName);

            Assert.That(await storage.ExistsAsync(fileName), Is.False);
        }

        [Test]
        public async Task ShouldReturnFalseIfFileDirectoryDoesntExist()
        {
            var nonExistingcontainerName = "test-non-existing-container";

            var storage = new SimpleLocalStorage(Path.Combine(testHelper.GetTestDirectoryPath(TestContext.CurrentContext), "NonExistingDirectory", nonExistingcontainerName));

            Assert.IsFalse(await storage.ExistsAsync(fileName));
        }
    }
}