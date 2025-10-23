using System.Collections.Generic;
using AMCode.Common.FilterStructures;
using AMCode.Sql.Where.Internal;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Components.Where.FilterBetweenConditionBuilderBaseTests
{
    [TestFixture]
    public class FilterBetweenConditionBuilderBaseTest
    {
        private FilterBetweenConditionBuilder betweenClauseBuilder;

        [Test]
        public void ShouldBuildBetweenClauseOnly()
        {
            var filter = createFilter("02/01/2022", "02/20/2022", false);

            betweenClauseBuilder = new FilterBetweenConditionBuilder(filter, new ComparerFactory("MM/dd/yyyy"), string.Empty);

            var conditionSection = betweenClauseBuilder.CreateFilterClause();

            Assert.That(conditionSection.CreateFilterClauseString(true), Is.EqualTo("TestFilter BETWEEN '02/01/2022' AND '02/20/2022'"));
        }

        [TestCase(true, ">=")]
        [TestCase(false, "<=")]
        public void ShouldBuildGreaterOrLessThanClauseOnly(bool greaterThan, string sign)
        {
            var filter = createFilter("02/01/2022", "02/20/2022", false);
            filter.FilterItems.RemoveAt(0);

            betweenClauseBuilder = new FilterBetweenConditionBuilder(filter, new ComparerFactory("MM/dd/yyyy"), string.Empty, greaterThan);

            var conditionSection = betweenClauseBuilder.CreateFilterClause();

            Assert.That(conditionSection.CreateFilterClauseString(true), Is.EqualTo($"TestFilter {sign} '02/20/2022'"));
        }

        [TestCase(true, ">=")]
        [TestCase(false, "<=")]
        public void ShouldBuildGreaterOrLessThanClauseWithIsNull(bool greaterThan, string sign)
        {
            var filter = createFilter("02/01/2022", "02/20/2022", false);
            filter.FilterItems[0] = new FilterItem
            {
                FilterVal = "-",
                Selected = true,
            };

            betweenClauseBuilder = new FilterBetweenConditionBuilder(filter, new ComparerFactory("MM/dd/yyyy"), string.Empty, greaterThan);

            var conditionSection = betweenClauseBuilder.CreateFilterClause();

            Assert.That(conditionSection.CreateFilterClauseString(true), Is.EqualTo($"(TestFilter {sign} '02/20/2022' OR TestFilter IS NULL)"));
        }

        [Test]
        public void ShouldBuildBetweenClauseAndIsNull()
        {
            var filter = createFilter("02/01/2022", "02/20/2022", false);
            filter.FilterItems.Insert(0, new FilterItem
            {
                FilterVal = "-",
                Selected = true,
            });

            betweenClauseBuilder = new FilterBetweenConditionBuilder(filter, new ComparerFactory("MM/dd/yyyy"), string.Empty);

            var conditionSection = betweenClauseBuilder.CreateFilterClause();

            Assert.That(conditionSection.CreateFilterClauseString(true), Is.EqualTo("(TestFilter BETWEEN '02/01/2022' AND '02/20/2022' OR TestFilter IS NULL)"));
        }

        [Test]
        public void ShouldBuildIsNullClause()
        {
            var filter = createFilter("02/01/2022", "02/20/2022", false);
            filter.FilterItems = new List<IFilterItem>
            {
                new FilterItem
                {
                    FilterVal = "-",
                    Selected = true,
                }
            };

            betweenClauseBuilder = new FilterBetweenConditionBuilder(filter, new ComparerFactory("MM/dd/yyyy"), string.Empty);

            var conditionSection = betweenClauseBuilder.CreateFilterClause();

            Assert.That(conditionSection.CreateFilterClauseString(true), Is.EqualTo("TestFilter IS NULL"));
        }

        /// <summary>
        /// Create an <see cref="IFilter"/>.
        /// </summary>
        /// <param name="leftDateValue">The value for the left filter item.</param>
        /// <param name="rightDateValue">The value for the right filter item.</param>
        /// <param name="isIdFilter">Whether or not it's an ID filter.</param>
        /// <returns>An <see cref="IFilter"/>.</returns>
        private IFilter createFilter(string leftDateValue, string rightDateValue, bool isIdFilter)
        {
            var filter = new Filter();
            var leftFilterItem = new FilterItem();
            var rightFilterItem = new FilterItem();

            if (isIdFilter)
            {
                leftFilterItem.FilterId = leftDateValue;
                rightFilterItem.FilterId = rightDateValue;
                filter.FilterIdName = new FilterName
                {
                    DisplayName = "Test Filter",
                    FieldName = "TestFilter"
                };
            }
            else
            {
                leftFilterItem.FilterVal = leftDateValue;
                rightFilterItem.FilterVal = rightDateValue;
                filter.FilterName = new FilterName
                {
                    DisplayName = "Test Filter",
                    FieldName = "TestFilter"
                };
            }

            leftFilterItem.Selected = rightFilterItem.Selected = true;
            filter.FilterItems = new List<IFilterItem>
            {
                leftFilterItem,
                rightFilterItem
            };

            return filter;
        }
    }
}