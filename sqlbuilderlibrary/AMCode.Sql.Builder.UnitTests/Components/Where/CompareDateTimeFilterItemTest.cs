using System;
using AMCode.Common.FilterStructures;
using AMCode.Common.Util;
using AMCode.Sql.Components.Where.Comparers;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Components.Where.CompareDateTimeFilterItemTests
{
    [TestFixture]
    public class CompareDateTimeFilterItemTest
    {
        private CompareDateTimeFilterItem compareDateTime;

        [TestCase("yyyy-MM-dd", true, "2022-01-01", "2022-01-30", -1)]
        [TestCase("yyyy-MM-dd", true, "2022-01-01", "2022-01-01", 0)]
        [TestCase("yyyy-MM-dd", true, "2022-01-28", "2022-01-01", 1)]
        [TestCase("MM/dd/yyyy", false, "01/01/2022", "01/30/2022", -1)]
        [TestCase("MM/dd/yyyy", false, "01/01/2022", "01/01/2022", 0)]
        [TestCase("MM/dd/yyyy", false, "01/28/2022", "01/01/2022", 1)]
        public void ShouldCompareTwoFilterItemsCorrectly(string format, bool isIdFilter, string leftDateValue, string rightDateValue, int expectedCompareResult)
        {
            compareDateTime = new CompareDateTimeFilterItem(format, isIdFilter);
            var (leftFilterItem, rightFilterItem) = createFilterItems(leftDateValue, rightDateValue, isIdFilter);
            Assert.That(compareDateTime.Compare(leftFilterItem, rightFilterItem), Is.EqualTo(expectedCompareResult));
        }

        [Test]
        public void ShouldThrowArgumentException()
        {
            compareDateTime = new CompareDateTimeFilterItem("yyyy/MM/dd", false);
            var (leftFilterItem, rightFilterItem) = createFilterItems("2022-03-20", "2022-03-01", false);

            var exception = Assert.Throws<ArgumentException>(() => compareDateTime.Compare(leftFilterItem, rightFilterItem));

            var header = ExceptionUtil.CreateExceptionHeader<IFilterItem, IFilterItem, int>(compareDateTime.Compare);
            var expectedMessage = $"{header} Error: Cannot parse the give date. Date was \"2022-03-20\".";
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        /// <summary>
        /// Create a left and right <see cref="IFilterItem"/>.
        /// </summary>
        /// <param name="leftDateValue">The value for the left filter item.</param>
        /// <param name="rightDateValue">The value for the right filter item.</param>
        /// <param name="isIdFilter">Whether or not it's an ID filter.</param>
        /// <returns>A tuple of <see cref="IFilterItem"/>s.</returns>
        private (IFilterItem, IFilterItem) createFilterItems(string leftDateValue, string rightDateValue, bool isIdFilter)
        {
            var leftFilterItem = new FilterItem();
            var rightFilterItem = new FilterItem();

            if (isIdFilter)
            {
                leftFilterItem.FilterId = leftDateValue;
                rightFilterItem.FilterId = rightDateValue;
            }
            else
            {
                leftFilterItem.FilterVal = leftDateValue;
                rightFilterItem.FilterVal = rightDateValue;
            }

            leftFilterItem.Selected = rightFilterItem.Selected = true;

            return (leftFilterItem, rightFilterItem);
        }
    }
}