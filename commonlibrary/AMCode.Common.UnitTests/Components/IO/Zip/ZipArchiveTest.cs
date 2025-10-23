using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.IO.CSV;
using AMCode.Common.IO.Zip;
using AMCode.Common.UnitTests.IO.Zip.Models;
using NUnit.Framework;
using ZipLib = System.IO.Compression;

namespace AMCode.Common.UnitTests.IO.Zip.ZipArchiveTests
{
    [TestFixture]
    public class ZipArchiveTest
    {
        private readonly int bufferSize = 16000;
        private IEnumerable<IZipEntry> zipEntries;
        private TestHelper testHelper;
        private readonly string zipFileCsvName = "TestCsv";
        private ZipArchive zipArchive;
        private string zipFileName;
        private string zipFileNameWithExtension;

        [SetUp]
        public void SetUp()
        {
            testHelper = new();
            zipFileName = "TestZipFile";
            zipFileNameWithExtension = $"{zipFileName}.zip";

            FileInfo createFile(int i)
            {
                if (i % 2 == 0)
                {
                    return null;
                }

                var filePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), $"{zipFileCsvName}{i + 1}.csv");
                using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                fileStream.Write(Encoding.UTF8.GetBytes($"TestLine:{i}"));
                fileStream.Flush();
                fileStream.Dispose();

                return new FileInfo(filePath);
            }

            Stream createStream(int i)
            {
                if (i % 2 != 0)
                {
                    return null;
                }

                var stream = new MemoryStream();
                using var streamWriter = new StreamWriter(stream, leaveOpen: true);

                streamWriter.Write($"TestLine:{i}");
                streamWriter.Flush();
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                return stream;
            }

            zipEntries = Enumerable.Range(0, 5).Select(index =>
            {
                var data = createStream(index);
                var file = createFile(index);

                return new ZipEntry
                {
                    Data = data,
                    File = file,
                    Name = $"{zipFileCsvName}{index + 1}.csv"
                };
            });
        }

        [TearDown]
        public void Teardown()
        {
            var directoryPath = testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext);
            var zipFilePath = Path.Combine(directoryPath, zipFileNameWithExtension);
            File.Delete(zipFilePath);

            var csvFiles = Directory.GetFiles(directoryPath, "*.csv");
            foreach (var csvFile in csvFiles)
            {
                File.Delete(csvFile);
            }
        }

        [Test]
        public async Task ShouldZipArchiveUsingAddEntryAndResultsShouldBeReadable()
        {
            zipArchive = new();

            foreach (var zipEntry in zipEntries)
            {
                if (zipEntry.Data != null)
                {
                    zipArchive.AddEntry(zipEntry.Name, zipEntry.Data);
                }
                else
                {
                    zipArchive.AddEntry(zipEntry.Name, zipEntry.File);
                }
            }

            var zipResult = await zipArchive.CreateZipAsync(zipFileName);

            verifyZipFile(zipResult.Name, zipResult.Data);
        }

        [Test]
        public async Task ShouldZipArchiveUsingAddEntryWithFunctionAndResultsShouldBeReadable()
        {
            zipArchive = new();

            foreach (var zipEntry in zipEntries)
            {
                if (zipEntry.Data != null)
                {
                    zipArchive.AddEntry(zipEntry.Name, () => Task.FromResult(zipEntry.Data));
                }
                else
                {
                    zipArchive.AddEntry(zipEntry.Name, () => Task.FromResult((Stream)new FileStream(zipEntry.File.FullName, FileMode.Open, FileAccess.Read)));
                }
            }

            var zipResult = await zipArchive.CreateZipAsync(zipFileName);

            verifyZipFile(zipResult.Name, zipResult.Data);
        }

        [Test]
        public void ShouldZipArchiveUsingConstructorAndResultsShouldBeReadable()
        {
            zipArchive = new(zipEntries);

            var zipResult = zipArchive.CreateZip(zipFileName);

            verifyZipFile(zipResult.Name, zipResult.Data);
        }

        [Test]
        public void ShouldCreateZipToCustomStreamWithBuffer()
        {
            zipArchive = new(bufferSize);

            foreach (var zipEntry in zipEntries)
            {
                zipArchive.AddEntry(zipEntry);
            }

            using var memoryStream = new MemoryStream();

            var zipResult = zipArchive.CreateZip("test-file.zip", memoryStream);

            verifyZipFile(zipResult.Name, memoryStream);
        }

        [Test]
        public async Task ShouldCreateZipToCustomStreamWithBufferAsync()
        {
            zipArchive = new(bufferSize);

            foreach (var zipEntry in zipEntries)
            {
                zipArchive.AddEntry(zipEntry);
            }

            using var memoryStream = new MemoryStream();

            var zipResult = await zipArchive.CreateZipAsync("test-file.zip", memoryStream);

            verifyZipFile(zipResult.Name, memoryStream);
        }

        [Test]
        public void ShouldCancelZipOperation()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            zipArchive = new(bufferSize);

            foreach (var zipEntry in zipEntries)
            {
                zipArchive.AddEntry(zipEntry);
            }

            using var memoryStream = new MemoryStream();
            cancellationTokenSource.Cancel();
            Assert.ThrowsAsync<TaskCanceledException>(() => zipArchive.CreateZipAsync("test-file.zip", memoryStream, cancellationTokenSource.Token));
        }

        [TestCase(CompressionLevel.Fastest)]
        [TestCase(CompressionLevel.NoCompression)]
        [TestCase(CompressionLevel.Optimal)]
        public async Task ShouldZipArchiveDifferentCompressionLevelsAndResultsShouldBeReadable(CompressionLevel compressionLevel)
        {
            zipArchive = new(zipEntries)
            {
                Compression = compressionLevel
            };

            var zipResult = await zipArchive.CreateZipAsync(zipFileName);

            verifyZipFile(zipResult.Name, zipResult.Data);
        }

        [Test]
        public void ShouldAddZipEntries()
        {
            zipArchive = new();

            foreach (var zipEntry in zipEntries)
            {
                zipArchive.AddEntry(zipEntry);
            }

            Assert.That(zipArchive.ZipEntries.Count, Is.EqualTo(zipEntries.Count()));
        }

        [Test]
        public void ShouldAddZipEntriesUsingStreamData()
        {
            zipArchive = new();

            foreach (var zipEntry in zipEntries)
            {
                zipArchive.AddEntry(zipEntry.Name, zipEntry.Data);
            }

            Assert.That(zipArchive.ZipEntries.Count, Is.EqualTo(zipEntries.Count()));
        }

        [Test]
        public void ShouldAddZipEntriesUsingFileInfo()
        {
            zipArchive = new();

            foreach (var zipEntry in zipEntries)
            {
                zipArchive.AddEntry(zipEntry.Name, zipEntry.File);
            }

            Assert.That(zipArchive.ZipEntries.Count, Is.EqualTo(zipEntries.Count()));
        }

        [Test]
        public void ShouldAddZipEntriesUsingGetDataAsync()
        {
            zipArchive = new();

            foreach (var zipEntry in zipEntries)
            {
                zipArchive.AddEntry(zipEntry.Name, () => Task.FromResult(zipEntry.Data));
            }

            Assert.That(zipArchive.ZipEntries.Count, Is.EqualTo(zipEntries.Count()));
        }

        [Test]
        public void ShouldDisposeOfTheZipArchiveResult()
        {
            zipArchive = new();

            foreach (var zipEntry in zipEntries)
            {
                zipArchive.AddEntry(zipEntry);
            }

            var result = zipArchive.CreateZip("test-file.zip");

            Assert.That(result.Data.CanRead, Is.True);

            result.Dispose();

            Assert.That(result.Data.CanRead, Is.False);
        }

        /// <summary>
        /// Verify that the zip file was built correctly.
        /// </summary>
        /// <param name="fileName">The zip file name.</param>
        /// <param name="stream">The <see cref="Stream"/> to save and verify.</param>
        private void verifyZipFile(string fileName, Stream stream)
        {
            var zipFilePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), fileName);

            using var fileStream = new FileStream(zipFilePath, File.Exists(zipFilePath) ? FileMode.Truncate : FileMode.CreateNew);
            stream.CopyTo(fileStream);
            // Make sure to dispose in order to save contents to the actual file
            fileStream.Dispose();

            using var createdZipFile = new FileStream(zipFilePath, FileMode.Open);
            using var libZipArchive = new ZipLib.ZipArchive(createdZipFile, ZipLib.ZipArchiveMode.Update);

            Assert.AreEqual(5, libZipArchive.Entries.Count);
            libZipArchive.Entries.ForEach((entry, index) =>
            {
                var expectedName = $"{zipFileCsvName}{index + 1}.csv";
                Assert.AreEqual(expectedName, entry.Name);
                var csv = new CSVReader(false).GetExpandoList(entry.Open());

                Assert.AreEqual($"TestLine:{index}", csv[0].GetValue<string>($"Header1"));
            });
        }
    }
}