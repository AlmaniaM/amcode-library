using AMCode.Common.Extensions.FilterNames;
using AMCode.Common.FilterStructures;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Extensions.FilterNameExtensionsTests
{
    [TestFixture]
    public class FilterNameExtensionsTest
    {
        [Test]
        public void ShouldConvertIFilterNameToFilterNameClassInstance()
        {
            IFilterName filterName = new FilterName
            {
                FieldName = "TestFilter",
                DisplayName = "Test Filter"
            };

            Assert.AreEqual("TestFilter", filterName.ToFilterName().FieldName);
            Assert.AreEqual("Test Filter", filterName.ToFilterName().DisplayName);
        }

        [Test]
        public void ShouldConvertNullIFilterNameToFilterNameClassInstanceWithNullProperties()
        {
            IFilterName filterName = null;

            Assert.IsNull(filterName.ToFilterName().FieldName);
            Assert.IsNull(filterName.ToFilterName().DisplayName);
        }
    }
}