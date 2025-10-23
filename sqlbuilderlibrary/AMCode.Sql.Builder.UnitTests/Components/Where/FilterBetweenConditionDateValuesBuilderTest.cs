using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AMCode.Common.FilterStructures;
using AMCode.Sql.Where.Internal;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Where.FilterBetweenConditionDateValuesBuilderTests
{
    [TestFixture]
    public class FilterBetweenConditionDateValuesBuilderTest
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
                var format = isIdFilter ? "MM/dd/yyyy" : "yyyy-MM-dd";
                var leftValue = DateTime.ParseExact(isIdFilter ? left.FilterId : left.FilterVal, format, null, DateTimeStyles.None);
                var rightValue = DateTime.ParseExact(isIdFilter ? right.FilterId : right.FilterVal, format, null, DateTimeStyles.None);

                return leftValue > rightValue ? 1 : leftValue == rightValue ? 0 : -1;
            });
        }

        [TestCase(false, "'2022-01-01'")]
        [TestCase(true, "'01/01/2022'")]
        public void ShouldGetSingleValueWhenOnlyOneFilter(bool isIdFilter, string expectedValue)
        {
            this.isIdFilter = isIdFilter;
            var filterItems = dateFilterItems.Where((IFilterItem filterItem, int i) => i == 1).ToList();
            valuesBuilder = new FilterBetweenConditionValuesBuilder(filterItems, comparerMoq.Object, isIdFilter);
            Assert.That(valuesBuilder.CreateFilterConditionValue(), Is.EqualTo(expectedValue));
        }

        [TestCase(false, "")]
        [TestCase(true, "")]
        public void ShouldGetEmptyStringWhenOnlyNoneValueExists(bool isIdFilter, string expectedValue)
        {
            this.isIdFilter = isIdFilter;
            var filterItems = dateFilterItems.Where((IFilterItem filterItem, int i) => i == 0).ToList();
            valuesBuilder = new FilterBetweenConditionValuesBuilder(filterItems, comparerMoq.Object, isIdFilter);
            Assert.That(valuesBuilder.CreateFilterConditionValue(), Is.EqualTo(expectedValue));
        }

        [TestCase(false, "'2022-01-01' AND '2022-01-30'")]
        [TestCase(true, "'01/01/2022' AND '01/30/2022'")]
        public void ShouldGetValueAndValueWhenMoreThanOneValidFilter(bool isIdFilter, string expectedValue)
        {
            this.isIdFilter = isIdFilter;
            var filterItems = dateFilterItems.Where((IFilterItem filterItem, int i) => i != 0).ToList();
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
            var filterItems = emptyFilter ? new List<IFilterItem>() : dateFilterItems.Where((IFilterItem filterItem, int i) => i == 0).ToList();
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
            var filterItems = dateFilterItems.Where((IFilterItem filterItem, int i) => singleFilter ? i == 1 : i == 0 || i == 1).ToList();
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
            var filterItems = dateFilterItems.Where((IFilterItem filterItem, int i) => !doubleFilter || i == 2 || i == 3).ToList();
            valuesBuilder = new FilterBetweenConditionValuesBuilder(filterItems, comparerMoq.Object, isIdFilter);
            Assert.That(valuesBuilder.Count, Is.EqualTo(2));
        }

        private IList<IFilterItem> dateFilterItems => new List<IFilterItem>
            {
                new FilterItem
                {
                    FilterVal = "-",
                    FilterId = "-",
                    Selected = true,
                },
                new FilterItem
                {
                    FilterVal = "2022-01-01",
                    FilterId = "01/01/2022",
                    Selected = true,
                },
                new FilterItem
                {
                    FilterVal = "2022-01-15",
                    FilterId = "01/15/2022",
                    Selected = true,
                },
                new FilterItem
                {
                    FilterVal = "2022-01-30",
                    FilterId = "01/30/2022",
                    Selected = true,
                }
            };
    }
}