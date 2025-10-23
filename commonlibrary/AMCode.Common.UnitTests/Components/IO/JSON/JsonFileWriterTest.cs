using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Common.IO;
using AMCode.Common.IO.JSON;
using AMCode.Common.UnitTests.IO.Json.Models;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.IO.Json.JsonFileWriterTests
{
    [TestFixture]
    public class JsonFileWriterTest
    {
        private TestHelper testHelper;

        [SetUp]
        public void SetUp() => testHelper = new TestHelper();

        [TearDown]
        public void Cleanup()
        {
            var workDirectoryPath = testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext);

            if (Directory.Exists(workDirectoryPath))
            {
                var files = Directory.GetFiles(workDirectoryPath);
                files
                    .Where(filePath => !Path.GetFileName(new FileInfo(filePath).Name).Equals("tmp.txt"))
                    .ToList()
                    .ForEach(filePath => File.Delete(filePath));
            }
        }

        [Test]
        public void ShouldWriteJsonObjectToFile()
        {
            var mockObject = new JsonObjectMock
            {
                DoubleTestProperty = 200.20,
                IntTestProperty = 200,
                StringTestProperty = "TestValue2"
            };

            var filePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), "JsonFileWriterTest_WriteJsonObjectToFile.json");
            JsonFileWriter.Write(filePath, mockObject);

            Assert.IsTrue(File.Exists(filePath));

            var mockObjectDeserialized = JsonFileReader.Read<JsonObjectMock>(filePath);

            Assert.AreEqual("TestValue2", mockObjectDeserialized.StringTestProperty);
            Assert.AreEqual(200, mockObjectDeserialized.IntTestProperty);
            Assert.AreEqual(200.20, mockObjectDeserialized.DoubleTestProperty);
        }

        [Test]
        public async Task ShouldAsyncWriteJsonObjectToFile()
        {
            var mockObject = new JsonObjectMock
            {
                DoubleTestProperty = 200.20,
                IntTestProperty = 200,
                StringTestProperty = "TestValue2"
            };

            var filePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), "JsonFileWriterTest_WriteJsonObjectToFile.json");
            await JsonFileWriter.WriteAsync(filePath, mockObject);

            Assert.IsTrue(File.Exists(filePath));

            var mockObjectDeserialized = await JsonFileReader.ReadAsync<JsonObjectMock>(filePath);

            Assert.AreEqual("TestValue2", mockObjectDeserialized.StringTestProperty);
            Assert.AreEqual(200, mockObjectDeserialized.IntTestProperty);
            Assert.AreEqual(200.20, mockObjectDeserialized.DoubleTestProperty);
        }
    }
}