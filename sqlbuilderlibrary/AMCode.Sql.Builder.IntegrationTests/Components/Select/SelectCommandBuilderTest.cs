using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMCode.Sql.Commands.Models;
using AMCode.Sql.Commands;
using AMCode.Sql.GroupBy;
using AMCode.Sql.OrderBy;
using AMCode.Sql.Select;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.IntegrationTests.Select.SelectCommandBuilderTests
{
    [TestFixture]
    public class SelectCommandBuilderTest
    {
        private IGroupByClauseCommand groupByClause;
        private IOrderByClauseCommand orderByClause;
        private IEnumerable<IDataQueryColumnDefinition> queryColumns;
        private ISelectClauseCommand selectClause;
        private readonly string schemaName = "TestSchema";
        private readonly string tableName = "TestTable";

        [SetUp]
        public void SetUp()
        {
            queryColumns = Enumerable.Range(1, 10).Select(index => createColumn($"Column{index}").Object);

            selectClause = new SelectClauseCommand(getColumns());

            groupByClause = new GroupByClauseCommand(getColumns());

            orderByClause = new OrderByClauseCommand(getColumns().Select(columnName => $"{columnName} ASC"));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldBuildSelectQuery(bool end)
        {
            var selectCommand = new SelectCommandBuilder()
                .Select(selectClause)
                .From(schemaName, tableName)
                .End(end)
                .Build();

            var expectedSelectString = new StringBuilder()
                .AppendLine($"SELECT {string.Join(", ", getColumns())}")
                .Append($"FROM {schemaName}.{tableName}{getEnd(end)}")
                .ToString();

            Assert.AreEqual(expectedSelectString, selectCommand.ToString());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldBuildSelectQueryWithGroupBy(bool end)
        {
            var selectCommand = new SelectCommandBuilder()
                .Select(selectClause)
                .From(schemaName, tableName)
                .GroupBy(groupByClause)
                .End(end)
                .Build();

            var expectedSelectString = new StringBuilder()
                .AppendLine($"SELECT {string.Join(", ", getColumns())}")
                .AppendLine($"FROM {schemaName}.{tableName}")
                .Append($"GROUP BY {string.Join(", ", getColumns())}{getEnd(end)}")
                .ToString();

            Assert.AreEqual(expectedSelectString, selectCommand.ToString());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldBuildSelectQueryWithGroupByAndOrderBy(bool end)
        {
            var selectCommand = new SelectCommandBuilder()
                .Select(selectClause)
                .From(schemaName, tableName)
                .GroupBy(groupByClause)
                .OrderBy(orderByClause)
                .End(end)
                .Build();

            var expectedSelectString = new StringBuilder()
                .AppendLine($"SELECT {string.Join(", ", getColumns())}")
                .AppendLine($"FROM {schemaName}.{tableName}")
                .AppendLine($"GROUP BY {string.Join(", ", getColumns())}")
                .Append($"ORDER BY {string.Join(", ", getColumns().Select(columnName => $"{columnName} ASC"))}{getEnd(end)}")
                .ToString();

            Assert.AreEqual(expectedSelectString, selectCommand.ToString());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldBuildSelectQueryWithGroupByAndOrderByAndLimitOffset(bool end)
        {
            var selectCommand = new SelectCommandBuilder()
                .Select(selectClause)
                .From(schemaName, tableName)
                .GroupBy(groupByClause)
                .OrderBy(orderByClause)
                .Offset(1)
                .Limit(100)
                .End(end)
                .Build();

            var expectedSelectString = new StringBuilder()
                .AppendLine($"SELECT {string.Join(", ", getColumns())}")
                .AppendLine($"FROM {schemaName}.{tableName}")
                .AppendLine($"GROUP BY {string.Join(", ", getColumns())}")
                .AppendLine($"ORDER BY {string.Join(", ", getOrderByColumns())}")
                .AppendLine("OFFSET 1")
                .Append($"LIMIT 100{getEnd(end)}")
                .ToString();

            Assert.AreEqual(expectedSelectString, selectCommand.ToString());
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldBuildSelectCountQuery(bool end)
        {
            var selectCommand = new SelectCommandBuilder()
                .Select(selectClause)
                .From(schemaName, tableName)
                .GroupBy(groupByClause)
                .Indent(1)
                .End(false)
                .Build();

            var selectCountCommand = new SelectCommandBuilder()
                .Select(new string[] { "* AS RowCount" })
                .From(selectCommand, "Table")
                .GroupBy(groupByClause)
                .OrderBy(orderByClause)
                .Offset(1)
                .Limit(100)
                .End(end)
                .Build();

            var expectedSelectString = new StringBuilder()
                .AppendLine($"SELECT * AS RowCount")
                .AppendLine("FROM (")
                .AppendLine($"\tSELECT {string.Join(", ", getColumns())}")
                .AppendLine($"\tFROM {schemaName}.{tableName}")
                .AppendLine($"\tGROUP BY {string.Join(", ", getColumns())}")
                .AppendLine(") AS Table")
                .AppendLine($"GROUP BY {string.Join(", ", getColumns())}")
                .AppendLine($"ORDER BY {string.Join(", ", getOrderByColumns())}")
                .AppendLine("OFFSET 1")
                .Append($"LIMIT 100{getEnd(end)}")
                .ToString();

            Assert.AreEqual(expectedSelectString, selectCountCommand.ToString());
        }

        /// <summary>
        /// Get a collection of <see cref="string"/> column names.
        /// </summary>
        /// <returns>A collection of <see cref="string"/> column names.</returns>
        private IEnumerable<string> getColumns() => queryColumns.Select(column => column.FieldName());

        /// <summary>
        /// Get a collection of <see cref="string"/> column ORDER BY expressions.
        /// </summary>
        /// <returns>A collection of <see cref="string"/> column ORDER BY expressions.</returns>
        private IEnumerable<string> getOrderByColumns() => getColumns().Select(columnName => $"{columnName} ASC");

        /// <summary>
        /// Get a semicolon (;) if <paramref name="end"/> is <c>true</c>.
        /// </summary>
        /// <param name="end">Provide <c>true</c> if you want a semicolon or <c>false</c> if not.</param>
        /// <returns>A <see cref="string"/> semicolon or empty if <paramref name="end"/> is <c>false</c>.</returns>
        private string getEnd(bool end) => end ? ";" : string.Empty;

        /// <summary>
        /// Create a <see cref="Mock"/> of an <see cref="IDataQueryColumnDefinition"/> object.
        /// </summary>
        /// <param name="name">The name of the columns.</param>
        /// <returns>A <see cref="Mock{T}"/> of an <see cref="IDataQueryColumnDefinition"/> object.</returns>
        private Mock<IDataQueryColumnDefinition> createColumn(string name)
        {
            var columnMoq = new Mock<IDataQueryColumnDefinition>();
            columnMoq.Setup(moq => moq.FieldName()).Returns(() => name);
            columnMoq.SetupGet(moq => moq.DisplayName).Returns(name);

            return columnMoq;
        }
    }
}