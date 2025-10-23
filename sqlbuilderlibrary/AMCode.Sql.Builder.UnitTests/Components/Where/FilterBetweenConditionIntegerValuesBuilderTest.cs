using System.Collections.Generic;
using System.Linq;
using AMCode.Common.FilterStructures;
using AMCode.Sql.Where.Internal;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Where.FilterBetweenConditionIntegerValuesBuilderTests
{
    [TestFixture]
    public class FilterBetweenConditionIntegerValuesBuilderTest
    {
        private Mock<IComparer<IFilterItem>> comparerMoq;
        private bool isIdFilter = false;
        private FilterBetweenConditionValuesBuilder valuesBuilder;

        [SetUp]
        public void SetUp()
        {
            comparerMoq = new();
            comparerMoq.Setup(moq => moq.Compare(It.IsAny<IFilterItem>(), It.IsAny<IFilterItem>())).Returns((IFilterItem left, IFilterItem right) =>
            {
                var leftValue = isIdFilter ? left.FilterId : left.FilterVal;
                var rightValue = isIdFilter ? right.FilterId : right.FilterVal;

                return leftValue.CompareTo(rightValue);
            });
        }

        [TestCase(false, "'Value 10'")]
        [TestCase(true, "1")]
        public void ShouldGetSingleValueWhenOnlyOneFilter(bool isIdFilter, string expectedValue)
        {
            this.isIdFilter = isIdFilter;
            var filterItems = integerFilterItems.Where((IFilterItem filterItem, int i) => i == 1).ToList();
            valuesBuilder = new FilterBetweenConditionValuesBuilder(filterItems, comparerMoq.Object, isIdFilter);
            Assert.That(valuesBuilder.CreateFilterConditionValue(), Is.EqualTo(expectedValue));
        }

        [TestCase(false, "")]
        [TestCase(true, "")]
        public void ShouldGetEmptyStringWhenOnlyNoneValueExists(bool isIdFilter, string expectedValue)
        {
            this.isIdFilter = isIdFilter;
            var filterItems = integerFilterItems.Where((IFilterItem filterItem, int i) => i == 0).ToList();
            valuesBuilder = new FilterBetweenConditionValuesBuilder(filterItems, comparerMoq.Object, isIdFilter);
            Assert.That(valuesBuilder.CreateFilterConditionValue(), Is.EqualTo(expectedValue));
        }

        [TestCase(false, "'Value 10' AND 'Value 50'")]
        [TestCase(true, "1 AND 5")]
        public void ShouldGetValueAndValueWhenMoreThanOneValidFilter(bool isIdFilter, string expectedValue)
        {
            this.isIdFilter = isIdFilter;
            var filterItems = integerFilterItems.Where((IFilterItem filterItem, int i) => i != 0).ToList();
            valuesBuilder = new FilterBetweenConditionValuesBuilder(filterItems, comparerMoq.Object, isIdFilter);
            Assert.That(valuesBuilder.CreateFilterConditionValue(), Is.EqualTo(expectedValue));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void ShouldGetZeroAsCountWhenNoValidFilterItems(bool isIdFilter, bool emptyFilter)
        {
            this.isIdFilter = isIdFilter;
            var filterItems = emptyFilter ? new List<IFilterItem>() : integerFilterItems.Where((IFilterItem filterItem, int i) => i == 0).ToList();
            valuesBuilder = new FilterBetweenConditionValuesBuilder(filterItems, comparerMoq.Object, isIdFilter);
            Assert.That(valuesBuilder.Count, Is.EqualTo(0));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void ShouldGetOneAsCountWhenOneValidFilterItemExists(bool isIdFilter, bool singleFilter)
        {
            this.isIdFilter = isIdFilter;
            var filterItems = integerFilterItems.Where((IFilterItem filterItem, int i) => singleFilter ? i == 1 : i == 0 || i == 1).ToList();
            valuesBuilder = new FilterBetweenConditionValuesBuilder(filterItems, comparerMoq.Object, isIdFilter);
            Assert.That(valuesBuilder.Count, Is.EqualTo(1));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void ShouldGetTwoAsCountWhenTwoOrMoreValidFilterItemExists(bool isIdFilter, bool doubleFilter)
        {
            this.isIdFilter = isIdFilter;
            var filterItems = integerFilterItems.Where((IFilterItem filterItem, int i) => !doubleFilter || i == 2 || i == 3).ToList();
            valuesBuilder = new FilterBetweenConditionValuesBuilder(filterItems, comparerMoq.Object, isIdFilter);
            Assert.That(valuesBuilder.Count, Is.EqualTo(2));
        }

        private IList<IFilterItem> integerFilterItems => new List<IFilterItem>
            {
                new FilterItem
                {
                    FilterVal = "-",
                    FilterId = "-",
                    Selected = true,
                },
                new FilterItem
                {
                    FilterVal = "Value 10",
                    FilterId = "1",
                    Selected = true,
                },
                new FilterItem
                {
                    FilterVal = "Value 30",
                    FilterId = "3",
                    Selected = true,
                },
                new FilterItem
                {
                    FilterVal = "Value 50",
                    FilterId = "5",
                    Selected = true,
                }
            };
    }
}