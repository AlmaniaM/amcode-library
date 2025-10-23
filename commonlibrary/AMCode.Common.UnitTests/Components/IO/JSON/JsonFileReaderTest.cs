using System.Threading.Tasks;
using AMCode.Common.IO.JSON;
using AMCode.Common.UnitTests.IO.Json.Models;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.IO.Json.JsonReaderTests
{
    [TestFixture]
    public class JsonFileReaderTest
    {
        private TestHelper testHelper;

        [SetUp]
        public void SetUp() => testHelper = new TestHelper();

        [Test]
        public void ShouldReadJsonFileIntoObject()
        {
            var filePath = testHelper.GetFilePath("JsonFileReaderTest_Json_Object_Mock.json", TestContext.CurrentContext);
            var mockJsonObject = JsonFileReader.Read<JsonObjectMock>(filePath);

            Assert.AreEqual("TestValue", mockJsonObject.StringTestProperty);
            Assert.AreEqual(100, mockJsonObject.IntTestProperty);
            Assert.AreEqual(100.50, mockJsonObject.DoubleTestProperty);
        }

        [Test]
        public async Task ShouldAsyncReadJsonFileIntoObject()
        {
            var filePath = testHelper.GetFilePath("JsonFileReaderTest_Json_Object_Mock.json", TestContext.CurrentContext);
            var mockJsonObject = await JsonFileReader.ReadAsync<JsonObjectMock>(filePath);

            Assert.AreEqual("TestValue", mockJsonObject.StringTestProperty);
            Assert.AreEqual(100, mockJsonObject.IntTestProperty);
            Assert.AreEqual(100.50, mockJsonObject.DoubleTestProperty);
        }
    }
}