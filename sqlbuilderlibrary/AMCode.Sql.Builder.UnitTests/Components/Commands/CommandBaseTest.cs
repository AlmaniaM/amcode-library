using System.Text;
using AMCode.Sql.Commands;
using AMCode.Sql.Commands.Models;
using AMCode.Sql.From;
using AMCode.Sql.Where;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Commands.CommandBaseTests
{
    [TestFixture]
    public class CommandBaseTest
    {
        [TestCase("WHERE", "TestColumn IN (0) and TestColumn2 = 'test'", false)]
        [TestCase("where", "TestColumn IN (0) and TestColumn2 IN ('test', 'test2')", true)]
        public void ShouldBuildTableWithWhereClause(string whereClause, string filters, bool lowerCase)
        {
            var commandBase = new CommandBase
            {
                From = new FromClauseCommand("Test", "TableName"),
                Where = createWhereClauseCommand($"{whereClause} {filters}", true).Object
            };

            var sb = new StringBuilder()
                .AppendLine("FROM Test.TableName")
                .Append(lowerCase ? "where " : "WHERE ").Append(filters);

            Assert.AreEqual(sb.ToString(), commandBase.ToString());
        }

        [Test]
        public void ShouldBuildTableWithNoWhereClause()
        {
            var commandBase = new CommandBase
            {
                From = new FromClauseCommand("Test", "TableName")
            };

            var sb = new StringBuilder().Append("FROM Test.TableName");

            Assert.AreEqual(sb.ToString(), commandBase.ToString());
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ShouldNotBuildCommandWhenNoTableExists(string table)
        {
            var commandBase = new CommandBase
            {
                From = new FromClauseCommand(string.Empty, table)
            };

            Assert.AreEqual(string.Empty, commandBase.ToString());
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ShouldBeInvalidWhenNoTableExists(string table)
        {
            var commandBase = new CommandBase
            {
                From = new FromClauseCommand(string.Empty, table)
            };

            Assert.IsFalse(commandBase.IsValid);
        }

        [Test]
        public void ShouldGetInvalidMessageWhenFromIsNull()
        {
            var commandBase = new CommandBase
            {
                From = null
            };

            Assert.AreEqual($"You must provide a valid table. Current table value is 'null'.", commandBase.InvalidCommandMessage);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ShouldGetInvalidMessageWhenNoTableExists(string table)
        {
            var commandBase = new CommandBase
            {
                From = new FromClauseCommand(string.Empty, table)
            };

            var value = table is null ? "null" : table;
            Assert.AreEqual($"The value \"'{value}'\" is not valid for a table name.", commandBase.InvalidCommandMessage);
        }

        /// <summary>
        /// Create a <see cref="Mock"/> of the <see cref="IWhereClauseCommand"/> interface.
        /// </summary>
        /// <param name="commandValue">The <see cref="string"/> command to return from the <see cref="IClauseCommand.CreateCommand()"/>
        /// function.</param>
        /// <param name="isValid">The <see cref="bool"/> value to return from the <see cref="IValidCommand.IsValid"/> property.</param>
        /// <returns>A <see cref="Mock"/> of the <see cref="IWhereClauseCommand"/> object.</returns>
        private Mock<IWhereClauseCommand> createWhereClauseCommand(string commandValue, bool isValid)
        {
            var whereClauseCommandMoq = new Mock<IWhereClauseCommand>();
            whereClauseCommandMoq.Setup(moq => moq.IsValid).Returns(() => isValid);
            whereClauseCommandMoq.Setup(moq => moq.CreateCommand()).Returns(() => commandValue);
            return whereClauseCommandMoq;
        }

        /// <summary>
        /// Creates a <see cref="IClauseCommand"/> object.
        /// </summary>
        /// <typeparam name="T">An <see cref="IClauseCommand"/> object.</typeparam>
        /// <param name="isValid">Whether or not the command is valid.</param>
        /// <param name="command">The clause command to return by the <see cref="IClauseCommand"/> object.</param>
        /// <param name="invalidMessage">The message to return when a command is not valid.</param>
        /// <returns>A <see cref="IClauseCommand"/> object</returns>
        private Mock<T> createCommand<T>(bool isValid, string command, string invalidMessage = "")
            where T : class, IClauseCommand
        {
            var tMoq = new Mock<T>();
            tMoq.Setup(moq => moq.IsValid).Returns(() => isValid);
            tMoq.Setup(moq => moq.CreateCommand()).Returns(() => command);
            tMoq.Setup(moq => moq.InvalidCommandMessage).Returns(() => invalidMessage);
            return tMoq;
        }
    }
}