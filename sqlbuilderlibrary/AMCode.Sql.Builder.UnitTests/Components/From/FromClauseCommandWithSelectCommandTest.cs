using System.Text;
using AMCode.Sql.Commands;
using AMCode.Sql.From;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.From.FromClauseCommandTests
{
    [TestFixture]
    public class FromClauseCommandWithSelectCommandTest
    {
        private readonly string aliasName = "TestAlias";
        private string expectedSelectString;
        private string expectedString;
        private readonly string schemaName = "TestSchema";
        private readonly string tableName = "TestTable";

        private Mock<ISelectCommand> selectCommandMoq;

        [SetUp]
        public void SetUp()
        {
            var selectString = $"SELECT * FROM {schemaName}.{tableName}";

            selectCommandMoq = new();
            selectCommandMoq.SetupGet(moq => moq.IsValid).Returns(true);
            selectCommandMoq.SetupGet(moq => moq.InvalidCommandMessage).Returns("Test error message");
            selectCommandMoq.SetupGet(moq => moq.CommandType).Returns("SELECT");
            selectCommandMoq.Setup(moq => moq.CreateCommand()).Returns(selectString);

            expectedSelectString = new StringBuilder()
                .AppendLine("(")
                .AppendLine(selectString)
                .Append($") AS {aliasName}")
                .ToString();

            expectedString = new StringBuilder()
                .Append("FROM ")
                .Append(expectedSelectString)
                .ToString();
        }

        [Test]
        public void ShouldBuildFromClauseCommandWithSubQuery()
            => Assert.AreEqual(expectedString, new FromClauseCommand(selectCommandMoq.Object, aliasName).CreateCommand());

        [Test]
        public void ShouldBuildFromClauseCommandWithSubQueryToString()
            => Assert.AreEqual(expectedString, new FromClauseCommand(selectCommandMoq.Object, aliasName).ToString());

        [Test]
        public void ShouldBuildFromClauseCommandWithSubQueryUsingSetFrom()
        {
            var fromClauseCommand = new FromClauseCommand();
            fromClauseCommand.SetFrom(selectCommandMoq.Object, aliasName);
            Assert.AreEqual(expectedString, fromClauseCommand.CreateCommand());
        }

        [Test]
        public void ShouldBuildFromClauseValue()
            => Assert.AreEqual(expectedSelectString, new FromClauseCommand(selectCommandMoq.Object, aliasName).GetCommandValue());

        [Test]
        public void ShouldReturnValidCommandWithSelectCommand()
        {
            var fromClauseCommand = new FromClauseCommand(selectCommandMoq.Object, aliasName);
            Assert.IsTrue(fromClauseCommand.IsValid);
        }

        [Test]
        public void ShouldGetInvalidMessageFromSelectCommand()
        {
            selectCommandMoq.SetupGet(moq => moq.IsValid).Returns(false);
            Assert.AreEqual("Test error message", new FromClauseCommand(selectCommandMoq.Object, string.Empty).InvalidCommandMessage);
        }
    }
}