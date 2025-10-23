using System.Linq;
using AMCode.Common.FilterStructures;
using AMCode.Sql.Where;
using AMCode.Sql.Where.Models;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Where.IntegrationWhereClauseTests
{
    [TestFixture]
    public class IntegrationWhereClauseTest
    {
        [TestCase(1, WhereClauseBuilderType.Data)]
        [TestCase(1, WhereClauseBuilderType.GlobalFilters)]
        [TestCase(10, WhereClauseBuilderType.Data)]
        [TestCase(10, WhereClauseBuilderType.GlobalFilters)]
        [TestCase(20, WhereClauseBuilderType.Data)]
        [TestCase(20, WhereClauseBuilderType.GlobalFilters)]
        public void ShouldBuildWhereClauseSectionWithoutLastSelectedFilter(int filterCount, WhereClauseBuilderType whereClauseBuilderType)
        {
            var filter = new Filter
            {
                FilterIdName = new FilterName
                {
                    DisplayName = "Filter ID #1",
                    FieldName = "FilterId1"
                },
                FilterItems = Enumerable.Range(1, filterCount).Select(index => new FilterItem
                {
                    Disabled = false,
                    FilterId = $"{index}",
                    FilterVal = $"#{index}",
                    Selected = true
                }).ToArray(),
                FilterName = new FilterName
                {
                    DisplayName = "Filter #1",
                    FieldName = "Filter1"
                },
                Required = false
            };

            var whereClauseBuilderFactory = new WhereClauseBuilderFactory();
            var whereClause = whereClauseBuilderFactory.Create(whereClauseBuilderType);
            var filterConditionFactory = new FilterConditionOrganizerFactory();
            var filterInConditionBuilder = filterConditionFactory.Create(filter, "NotThisFilter", whereClauseBuilderType, string.Empty);
            filterInConditionBuilder.AddFilterCondition(whereClause);

            var filterValues = string.Join(',', Enumerable.Range(1, filterCount).ToList());
            var result = whereClause.CreateWhereClause();
            var expected = $"WHERE FilterId1 IN ({filterValues}) ";

            Assert.AreEqual(expected, result.CreateCommand());
            Assert.AreEqual(expected, result.ToString());
        }

        [TestCase(1, WhereClauseBuilderType.Data)]
        [TestCase(1, WhereClauseBuilderType.GlobalFilters)]
        [TestCase(10, WhereClauseBuilderType.Data)]
        [TestCase(10, WhereClauseBuilderType.GlobalFilters)]
        [TestCase(20, WhereClauseBuilderType.Data)]
        [TestCase(20, WhereClauseBuilderType.GlobalFilters)]
        public void ShouldBuildWhereClauseSectionWithoutLastSelectedFilterPlusNoneValue(int filterCount, WhereClauseBuilderType whereClauseBuilderType)
        {
            var dlFilter = new Filter
            {
                FilterIdName = new FilterName
                {
                    DisplayName = "Filter ID #1",
                    FieldName = "FilterId1"
                },
                FilterItems = Enumerable.Range(1, filterCount).Select(index => new FilterItem
                {
                    Disabled = false,
                    FilterId = index == filterCount ? FilterItemValueTypes.None : $"{index}",
                    FilterVal = index == filterCount ? FilterItemValueTypes.None : $"#{index}",
                    Selected = true
                }).ToArray(),
                FilterName = new FilterName
                {
                    DisplayName = "Filter #1",
                    FieldName = "Filter1"
                },
                Required = false
            };

            var whereClauseBuilderFactory = new WhereClauseBuilderFactory();
            var whereClause = whereClauseBuilderFactory.Create(whereClauseBuilderType);
            var filterConditionFactory = new FilterConditionOrganizerFactory();
            var filterInConditionBuilder = filterConditionFactory.Create(dlFilter, "NotThisFilter", whereClauseBuilderType, string.Empty);
            filterInConditionBuilder.AddFilterCondition(whereClause);

            var filterValues = string.Join(',', Enumerable.Range(1, filterCount - 1).ToList());
            var result = whereClause.CreateWhereClause();
            var filterInString = filterValues.Length > 0 ? $"FilterId1 IN ({filterValues})" : string.Empty;
            var expected = string.Empty;

            expected = filterValues.Length > 0 ? $"WHERE (FilterId1 IN ({filterValues}) OR FilterId1 IS NULL) " : "WHERE FilterId1 IS NULL ";

            Assert.AreEqual(expected, result.CreateCommand());
            Assert.AreEqual(expected, result.ToString());
        }

        [TestCase(1, WhereClauseBuilderType.Data)]
        [TestCase(1, WhereClauseBuilderType.GlobalFilters)]
        [TestCase(10, WhereClauseBuilderType.Data)]
        [TestCase(10, WhereClauseBuilderType.GlobalFilters)]
        [TestCase(20, WhereClauseBuilderType.Data)]
        [TestCase(20, WhereClauseBuilderType.GlobalFilters)]
        public void ShouldBuildWhereClauseSectionWithLastSelectedFilterPlusNoneValue(int filterCount, WhereClauseBuilderType whereClauseBuilderType)
        {
            var dlFilter = new Filter
            {
                FilterIdName = new FilterName
                {
                    DisplayName = "Filter ID #1",
                    FieldName = "FilterId1"
                },
                FilterItems = Enumerable.Range(1, filterCount).Select(index => new FilterItem
                {
                    Disabled = false,
                    FilterId = index == filterCount ? FilterItemValueTypes.None : $"{index}",
                    FilterVal = index == filterCount ? FilterItemValueTypes.None : $"#{index}",
                    Selected = true
                }).ToArray(),
                FilterName = new FilterName
                {
                    DisplayName = "Filter #1",
                    FieldName = "Filter1"
                },
                Required = false
            };

            var whereClauseBuilderFactory = new WhereClauseBuilderFactory();
            var whereClause = whereClauseBuilderFactory.Create(whereClauseBuilderType);
            var filterConditionFactory = new FilterConditionOrganizerFactory();
            var filterInConditionBuilder = filterConditionFactory.Create(dlFilter, "FilterId1", whereClauseBuilderType, string.Empty);
            filterInConditionBuilder.AddFilterCondition(whereClause);

            var filterValues = string.Join(',', Enumerable.Range(1, filterCount - 1).ToList());
            var result = whereClause.CreateWhereClause();
            var filterInString = filterValues.Length > 0 ? $"FilterId1 IN ({filterValues})" : string.Empty;
            var expected = string.Empty;

            expected = filterValues.Length > 0
                ? whereClauseBuilderType == WhereClauseBuilderType.GlobalFilters
                    ? $"WHERE (FilterId1 IN ({filterValues}) OR FilterId1 IS NULL) OR (FilterId1 IN ({filterValues}) OR FilterId1 IS NULL) "
                    : $"WHERE (FilterId1 IN ({filterValues}) OR FilterId1 IS NULL) "
                : whereClauseBuilderType == WhereClauseBuilderType.GlobalFilters
                    ? "WHERE FilterId1 IS NULL OR FilterId1 IS NULL "
                    : "WHERE FilterId1 IS NULL ";

            Assert.AreEqual(expected, result.CreateCommand());
            Assert.AreEqual(expected, result.ToString());
        }
    }
}