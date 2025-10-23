using System.IO;
using AMCode.Common.FilterStructures;
using AMCode.Common.UnitTests.Filters.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Filters.FilterTests
{
    [TestFixture]
    public class FilterDeserializationTest
    {
        private TestHelper testHelper;

        [SetUp]
        public void SetUp() => testHelper = new TestHelper();

        [Test]
        public void ShouldDeserializeJsonToFilter()
        {
            var filePath = testHelper.GetFilePath("filter-data.json", TestContext.CurrentContext);
            var jsonData = File.ReadAllText(filePath);
            var filter = JsonConvert.DeserializeObject<Filter>(jsonData);

            Assert.That(filter, Is.Not.Null);
            Assert.That(filter.FilterName.DisplayName, Is.EqualTo("Filter 1"));
            Assert.That(filter.FilterName.FieldName, Is.EqualTo("Filter1"));
            Assert.That(filter.FilterIdName.DisplayName, Is.EqualTo("Filter 1 ID"));
            Assert.That(filter.FilterIdName.FieldName, Is.EqualTo("Filter1Id"));
            Assert.That(filter.FilterItems[0].Disabled, Is.False);
            Assert.That(filter.FilterItems[0].FilterId, Is.Null);
            Assert.That(filter.FilterItems[0].FilterVal, Is.EqualTo("Value 1"));
            Assert.That(filter.FilterItems[0].Selected, Is.False);
            Assert.That(filter.FilterItems[1].Disabled, Is.True);
            Assert.That(filter.FilterItems[1].FilterVal, Is.EqualTo("Value 2"));
            Assert.That(filter.FilterItems[1].FilterId, Is.EqualTo("2"));
            Assert.That(filter.FilterItems[1].Selected, Is.True);
        }
    }
}