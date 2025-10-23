using System.Linq;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Sql.Where;
using AMCode.Sql.Where.Internal;
using AMCode.Sql.Where.Models;
using AMCode.Sql.Builder.UnitTests.Globals;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Where.WhereClauseTests
{
    [TestFixture]
    public class WhereClauseTest
    {
        [Test]
        public void ShouldBuildSimpleWhereClause()
        {
            var whereClause = new WhereClause(new WhereClauseBuilderFactory(), new FilterConditionOrganizerFactory(), WhereClauseBuilderType.GlobalFilters);

            // Build filters list
            var filters = new TestFixture()
                .Filters
                    .AddFilterWith
                        .FilterIdName("FilterId1", "Filter ID #1")
                        .FilterName("Filter1", "Filter #1")
                        .FilterItem("#1", "1", true, true)
                        .FilterItem("#2", "2", true, true)
                        .FilterItem("#3", "3", true, true)
                        .Save()
                    .Build();

            var actual = whereClause.CreateClause(new WhereClauseSelectedFiltersParam { SelectedFilters = filters, LastSelectedFilter = "SomeFilter" }, string.Empty);
            var expected = $"WHERE FilterId1 IN (1,2,3) ";

            Assert.AreEqual(expected, actual.CreateCommand());
            Assert.AreEqual(expected, actual.ToString());
        }

        [TestCase(1, WhereClauseBuilderType.Data)]
        [TestCase(1, WhereClauseBuilderType.GlobalFilters)]
        [TestCase(5, WhereClauseBuilderType.Data)]
        [TestCase(5, WhereClauseBuilderType.GlobalFilters)]
        public void ShouldBuildWhereClauseWithNFilters(int filterCount, WhereClauseBuilderType builderType)
        {
            var whereClauseBuilder = new WhereClauseBuilder();
            var whereClauseFactory = new WhereClauseTypeFactory(new WhereClauseBuilderFactory(), new FilterConditionOrganizerFactory());
            var whereClause = whereClauseFactory.Create(builderType);

            var filtersBuilder = new TestFixture().Filters;

            Enumerable.Range(1, filterCount).ForEach(index => filtersBuilder = filtersBuilder
                    .AddFilterWith
                        .FilterIdName($"FilterId{index}", $"Filter ID #{index}")
                        .FilterName($"Filter{index}", $"Filter #{index}")
                        .FilterItem("#1", "1", true, true)
                        .FilterItem("#2", "2", true, true)
                        .FilterItem("#3", "3", true, true)
                        .Save());

            var filters = filtersBuilder.Build();

            var actual = whereClause.CreateClause(new WhereClauseSelectedFiltersParam { SelectedFilters = filters, LastSelectedFilter = "SomeFilter" }, string.Empty);

            var expectedFilterConditionString = string.Join(" AND ", Enumerable.Range(1, filterCount).Select(index => $"FilterId{index} IN (1,2,3)"));
            var expected = $"WHERE {expectedFilterConditionString} ";

            Assert.AreEqual(expected, actual.CreateCommand());
            Assert.AreEqual(expected, actual.ToString());
        }

        [TestCase(1, WhereClauseBuilderType.GlobalFilters)]
        [TestCase(5, WhereClauseBuilderType.GlobalFilters)]
        public void ShouldBuildGlobalFilterWhereClauseWithNFiltersAndLastSelectedFilter(int filterCount, WhereClauseBuilderType builderType)
        {
            var whereClauseBuilder = new WhereClauseBuilder();
            var whereClauseFactory = new WhereClauseTypeFactory(new WhereClauseBuilderFactory(), new FilterConditionOrganizerFactory());
            var whereClause = whereClauseFactory.Create(builderType);

            var filtersBuilder = new TestFixture().Filters;

            Enumerable.Range(1, filterCount).ForEach(index => filtersBuilder = filtersBuilder
                    .AddFilterWith
                        .FilterIdName($"FilterId{index}", $"Filter ID #{index}")
                        .FilterName($"Filter{index}", $"Filter #{index}")
                        .FilterItem("#1", "1", true, true)
                        .FilterItem("#2", "2", true, true)
                        .FilterItem("#3", "3", true, true)
                        .Save());

            var filters = filtersBuilder.Build();

            var actual = whereClause.CreateClause(new WhereClauseSelectedFiltersParam { SelectedFilters = filters, LastSelectedFilter = "FilterId1" }, string.Empty);

            var expectedFilterConditionString = string.Join(" AND ", Enumerable.Range(1, filterCount).Select(index => $"FilterId{index} IN (1,2,3)"));
            var expected = $"WHERE FilterId1 IN (1,2,3) OR {expectedFilterConditionString} ";

            Assert.AreEqual(expected, actual.CreateCommand());
            Assert.AreEqual(expected, actual.ToString());
        }
    }
}