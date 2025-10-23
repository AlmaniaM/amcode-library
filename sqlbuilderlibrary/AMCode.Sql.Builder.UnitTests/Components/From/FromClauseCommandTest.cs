using AMCode.Sql.Commands;
using AMCode.Sql.From;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.From.FromClauseCommandTests
{
    [TestFixture]
    public class FromClauseCommandTest
    {
        private readonly string aliasName = "TestAlias";
        private string expectedFrom;
        private string expectedFromWithAlias;
        private readonly string schemaName = "TestSchema";
        private readonly string tableName = "TestTable";

        private Mock<ISelectCommand> selectCommandMoq;

        [SetUp]
        public void SetUp()
        {
            selectCommandMoq = new();
            selectCommandMoq.SetupGet(moq => moq.IsValid).Returns(true);
            selectCommandMoq.SetupGet(moq => moq.CommandType).Returns("SELECT");
            selectCommandMoq.Setup(moq => moq.CreateCommand()).Returns($"SELECT * FROM {schemaName}.{tableName}");

            expectedFrom = $"FROM {schemaName}.{tableName}";
            expectedFromWithAlias = $"FROM {schemaName}.{tableName} AS {aliasName}";
        }

        [Test]
        public void ShouldBuildFromClauseCommand()
            => Assert.AreEqual(expectedFrom, new FromClauseCommand(schemaName, tableName).CreateCommand());

        [Test]
        public void ShouldBuildFromClauseCommandUsingSetFrom()
        {
            var fromClauseCommand = new FromClauseCommand();
            fromClauseCommand.SetFrom(schemaName, tableName);

            Assert.AreEqual(expectedFrom, fromClauseCommand.CreateCommand());
        }

        [Test]
        public void ShouldBuildFromClauseCommandToString()
            => Assert.AreEqual(expectedFrom, new FromClauseCommand(schemaName, tableName).ToString());

        [Test]
        public void ShouldBuildFromClauseCommandGetFrom()
            => Assert.AreEqual(expectedFrom, new FromClauseCommand(schemaName, tableName).ToString());

        [Test]
        public void ShouldBuildFromClauseCommandWithAlias()
            => Assert.AreEqual(expectedFromWithAlias, new FromClauseCommand(schemaName, tableName, aliasName).CreateCommand());

        [Test]
        public void ShouldBuildFromClauseCommandWithAliasUsingSetFrom()
        {
            var fromClauseCommand = new FromClauseCommand();
            fromClauseCommand.SetFrom(schemaName, tableName, aliasName);

            Assert.AreEqual(expectedFromWithAlias, fromClauseCommand.CreateCommand());
        }

        [Test]
        public void ShouldBuildFromClauseCommandWithAliasToString()
            => Assert.AreEqual(expectedFromWithAlias, new FromClauseCommand(schemaName, tableName, aliasName).ToString());

        [Test]
        public void ShouldBuildFromClauseCommandWithAliasGetFrom()
            => Assert.AreEqual(expectedFromWithAlias, new FromClauseCommand(schemaName, tableName, aliasName).GetFrom());

        [Test]
        public void ShouldBuildFromClauseValue()
            => Assert.AreEqual($"{schemaName}.{tableName} AS {aliasName}", new FromClauseCommand(schemaName, tableName, aliasName).GetCommandValue());

        [Test]
        public void ShouldReturnValidCommandWithTableName()
        {
            var fromClauseCommand = new FromClauseCommand(schemaName, tableName);
            Assert.IsTrue(fromClauseCommand.IsValid);
        }

        [Test]
        public void ShouldGetInvalidMessageAboutInnerFromCommandBeingNull()
            => Assert.AreEqual("The inner from command is null", new FromClauseCommand().InvalidCommandMessage);

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ShouldGetInvalidMessageAboutInvalidTableName(string table)
        {
            var value = table is null ? "null" : table;
            Assert.AreEqual($"The value \"'{value}'\" is not valid for a table name.", new FromClauseCommand(schemaName, table).InvalidCommandMessage);
        }
    }
}