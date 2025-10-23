using AMCode.Common.Extensions.FilterItems;
using AMCode.Common.Extensions.Filters;
using AMCode.Common.FilterStructures;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Extensions.FilterExtensionsTests
{
    [TestFixture]
    public class FilterExtensionsTest
    {
        private readonly Filter testFilter = new()
        {
            FilterName = new FilterName
            {
                FieldName = "TestName",
                DisplayName = "Test Name"
            },
            FilterItems = new FilterItem[]
                {
                    new FilterItem
                    {
                        Disabled = false,
                        FilterVal = "One",
                        Selected = true
                    }
                }
        };

        private readonly Filter testFilterWithId = new()
        {
            FilterName = new FilterName
            {
                FieldName = "TestName",
                DisplayName = "Test Name"
            },
            FilterIdName = new FilterName
            {
                FieldName = "TestNameId",
                DisplayName = "Test Name Id"
            },
            FilterItems = new FilterItem[]
                {
                    new FilterItem
                    {
                        Disabled = false,
                        FilterId = "1",
                        FilterVal = "One",
                        Selected = true
                    }
                }
        };

        private readonly Filter testFilterEmpty = new()
        {
            FilterName = new FilterName
            {
                FieldName = "TestName",
                DisplayName = "Test Name"
            }
        };

        [Test]
        public void ShouldFindValueWithContains()
        {
            // Should be true
            Assert.IsTrue(testFilter.Contains("One"));
            Assert.IsTrue(testFilterWithId.Contains("1"));
            Assert.IsFalse(testFilter.Contains("1"));
            Assert.IsFalse(testFilterWithId.Contains("One"));

            // Should be false
            Assert.IsFalse(testFilterEmpty.Contains("One"));
        }

        [Test]
        public void ShouldPassIsFilterCheck()
        {
            // Should be true
            Assert.IsTrue(testFilter.IsFilter("TestName"));
            Assert.IsTrue(testFilterWithId.IsFilter("TestNameId"));
            Assert.IsTrue(testFilterWithId.IsFilter("TestNameId", true));

            // Should be false
            Assert.IsFalse(testFilter.IsFilter("TestNameId"));
            Assert.IsFalse(testFilter.IsFilter("TestNameId", true));
            Assert.IsFalse(testFilterWithId.IsFilter("TestName", true));
        }

        [Test]
        public void ShouldCloneFilter()
        {
            var clonedFilter = testFilter.Clone();
            Assert.AreNotEqual(clonedFilter, testFilterEmpty);
            Assert.AreEqual(clonedFilter.FilterName.FieldName, testFilter.FilterName.FieldName);
            Assert.AreEqual(clonedFilter.FilterItems[0].FilterVal, testFilter.FilterItems[0].FilterVal);
        }
    }
}