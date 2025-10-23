using System.Collections.Generic;
using AMCode.Common.Extensions.FilterItems;
using AMCode.Common.Extensions.Filters;
using AMCode.Common.FilterStructures;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Extensions.FilterItemExtensionsTests
{
    [TestFixture]
    public class FilterItemExtensionsTest
    {
        [Test]
        public void ShouldContainAValueInTheListOfIFilterItems()
        {
            var filterItems = new List<IFilterItem>()
            {
                new FilterItem
                {
                    FilterVal = "Value0"
                },
                new FilterItem
                {
                    FilterVal = "Value1"
                }
            };

            Assert.IsTrue(filterItems.Contains("Value0", false));
        }

        [Test]
        public void ShouldContainAValueInTheListOfIFilterItemsWithIds()
        {
            var filterItems = new List<IFilterItem>()
            {
                new FilterItem
                {
                    FilterVal = "Value0",
                    FilterId = "ValueId0"
                },
                new FilterItem
                {
                    FilterVal = "Value1",
                    FilterId = "ValueId1"
                }
            };

            Assert.IsTrue(filterItems.Contains("ValueId1", true));
        }

        [Test]
        public void ShouldContainAValueInTheIFilterItem()
        {
            var filterItem = new FilterItem
            {
                FilterVal = "Value1"
            };

            Assert.IsTrue(filterItem.Contains("Value1", false));
        }

        [Test]
        public void ShouldContainAValueInTheIFilterItemWithId()
        {
            var filterItem = new FilterItem
            {
                FilterVal = "Value0",
                FilterId = "ValueId0"
            };

            Assert.IsTrue(filterItem.Contains("ValueId0", true));
        }

        [Test]
        public void ShouldConvertIFilterItemIntoFilterItemClassInstance()
        {
            IFilterItem filterItem = new FilterItem
            {
                Disabled = false,
                FilterVal = "Value0",
                FilterId = "ValueId0",
                Selected = true
            };

            Assert.That(filterItem.ToFilterItem().Disabled, Is.False);
            Assert.AreEqual("Value0", filterItem.ToFilterItem().FilterVal);
            Assert.AreEqual("ValueId0", filterItem.ToFilterItem().FilterId);
            Assert.AreEqual(true, filterItem.ToFilterItem().Selected);
        }

        [Test]
        public void ShouldConvertNullIFilterItemIntoFilterItemClassInstanceWithNullProperties()
        {
            IFilterItem filterItem = null;

            Assert.IsFalse(filterItem.ToFilterItem().Disabled);
            Assert.IsNull(filterItem.ToFilterItem().FilterVal);
            Assert.IsNull(filterItem.ToFilterItem().FilterId);
            Assert.IsFalse(filterItem.ToFilterItem().Selected);
        }
    }
}