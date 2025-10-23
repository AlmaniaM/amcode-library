using AMCode.Sql.From;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.From.TableClauseCommandTests
{
    [TestFixture]
    public class TableClauseCommandTest
    {
        private readonly string aliasName = "TestAlias";
        private string expectedFrom;
        private string expectedFromWithAlias;
        private readonly string schemaName = "TestSchema";
        private readonly string tableName = "TestTable";

        [SetUp]
        public void SetUp()
        {
            expectedFrom = $"{schemaName}.{tableName}";
            expectedFromWithAlias = $"{schemaName}.{tableName} AS {aliasName}";
        }

        [Test]
        public void ShouldBuildFromClauseCommand()
        {
            var tableClauseCommand = new TableClauseCommand
            {
                Schema = schemaName,
                TableName = tableName
            };

            Assert.AreEqual(expectedFrom, tableClauseCommand.CreateCommand());
        }

        [Test]
        public void ShouldBuildFromClauseCommandToString()
        {
            var tableClauseCommand = new TableClauseCommand
            {
                Schema = schemaName,
                TableName = tableName
            };

            Assert.AreEqual(expectedFrom, tableClauseCommand.ToString());
        }

        [Test]
        public void ShouldBuildFromClauseCommandWithAlias()
        {
            var tableClauseCommand = new TableClauseCommand
            {
                Alias = aliasName,
                Schema = schemaName,
                TableName = tableName
            };

            Assert.AreEqual(expectedFromWithAlias, tableClauseCommand.CreateCommand());
        }

        [Test]
        public void ShouldBuildFromClauseCommandWithAliasToString()
        {
            var tableClauseCommand = new TableClauseCommand
            {
                Alias = aliasName,
                Schema = schemaName,
                TableName = tableName
            };

            Assert.AreEqual(expectedFromWithAlias, tableClauseCommand.ToString());
        }

        [Test]
        public void ShouldBuildFromClauseValue()
        {
            var tableClauseCommand = new TableClauseCommand
            {
                Alias = aliasName,
                Schema = schemaName,
                TableName = tableName
            };

            Assert.AreEqual(expectedFromWithAlias, tableClauseCommand.GetCommandValue());
        }

        [Test]
        public void ShouldReturnValidCommandWithTableName()
        {
            var tableClauseCommand = new TableClauseCommand
            {
                TableName = tableName
            };

            Assert.IsTrue(tableClauseCommand.IsValid);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ShouldGetInvalidMessageAboutInvalidTableName(string table)
        {
            var tableClauseCommand = new TableClauseCommand
            {
                TableName = table
            };

            var value = table is null ? "null" : table;
            Assert.AreEqual($"The value \"'{value}'\" is not valid for a table name.", tableClauseCommand.InvalidCommandMessage);
        }
    }
}