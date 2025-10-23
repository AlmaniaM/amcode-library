using System.Text;
using AMCode.Sql.Commands;
using AMCode.Sql.Commands.Models;
using AMCode.Sql.From;
using AMCode.Sql.GroupBy;
using AMCode.Sql.OrderBy;
using AMCode.Sql.Select;
using AMCode.Sql.Where;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Commands.SelectCommandTests
{
    [TestFixture]
    public class SelectCommandTest
    {
        [Test]
        public void ShouldNotDuplicateClausesWhenClausesAreProvided()
        {
            var expectedSelectCommand = new SelectCommand
            {
                EndCommand = true,
                GroupBy = createCommand<IGroupByClauseCommand>(true, "GROUP BY TestColumn1, TestColumn2, TestColumn3").Object,
                Limit = 300,
                Offset = 20,
                OrderBy = createCommand<IOrderByClauseCommand>(true, "order by TestColumn1, TestColumn2, TestColumn3").Object,
                Select = createCommand<ISelectClauseCommand>(true, "SELECT TestColumn1, TestColumn2, TestColumn3").Object,
                From = createCommand<IFromClauseCommand>(true, "from Test.TableName").Object,
                Where = createCommand<IWhereClauseCommand>(true, "where TestColumn IN (0) and TestColumn2 = 'test'").Object
            };

            var sb = new StringBuilder()
                .AppendLine("SELECT TestColumn1, TestColumn2, TestColumn3")
                .AppendLine("from Test.TableName")
                .AppendLine("where TestColumn IN (0) and TestColumn2 = 'test'")
                .AppendLine("GROUP BY TestColumn1, TestColumn2, TestColumn3")
                .AppendLine("order by TestColumn1, TestColumn2, TestColumn3")
                .AppendLine($"OFFSET 20")
                .Append($"LIMIT 300").Append(';');

            Assert.AreEqual(sb.ToString(), expectedSelectCommand.CreateCommand());
            Assert.AreEqual(sb.ToString(), expectedSelectCommand.ToString());
        }

        [Test]
        public void ShouldBuildSelectWithoutOptionalClauses()
        {
            var expectedSelectCommand = new SelectCommand
            {
                EndCommand = true,
                Select = createCommand<ISelectClauseCommand>(true, "SELECT TestColumn1, TestColumn2, TestColumn3").Object,
                From = createCommand<IFromClauseCommand>(true, "FROM Test.TableName").Object,
            };

            var sb = new StringBuilder()
                .AppendLine("SELECT TestColumn1, TestColumn2, TestColumn3")
                .Append("FROM Test.TableName").Append(';');

            Assert.AreEqual(sb.ToString(), expectedSelectCommand.CreateCommand());
            Assert.AreEqual(sb.ToString(), expectedSelectCommand.ToString());
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ShouldCreateEmptySelectCommandWhenTableIsNotProvided(string table)
        {
            var expectedSelectCommand = new SelectCommand
            {
                GroupBy = createCommand<IGroupByClauseCommand>(true, "GROUP BY TestColumn1, TestColumn2, TestColumn3").Object,
                Limit = 300,
                Offset = 20,
                OrderBy = createCommand<IOrderByClauseCommand>(true, "order by TestColumn1, TestColumn2, TestColumn3").Object,
                Select = createCommand<ISelectClauseCommand>(true, "SELECT TestColumn1, TestColumn2, TestColumn3").Object,
                From = createCommand<IFromClauseCommand>(false, table).Object,
                Where = createCommand<IWhereClauseCommand>(true, "where TestColumn IN (0) and TestColumn2 = 'test'").Object
            };

            Assert.AreEqual(string.Empty, expectedSelectCommand.CreateCommand());
            Assert.AreEqual(string.Empty, expectedSelectCommand.ToString());
        }

        [Test]
        public void ShouldCreateEmptySelectCommandWhenSelectIsNotProvided()
        {
            var expectedSelectCommand = new SelectCommand
            {
                GroupBy = createCommand<IGroupByClauseCommand>(true, "GROUP BY TestColumn1, TestColumn2, TestColumn3").Object,
                Limit = 300,
                Offset = 20,
                OrderBy = createCommand<IOrderByClauseCommand>(true, "order by TestColumn1, TestColumn2, TestColumn3").Object,
                Select = createCommand<ISelectClauseCommand>(false, string.Empty).Object,
                From = createCommand<IFromClauseCommand>(true, "Test.TableName").Object,
                Where = createCommand<IWhereClauseCommand>(true, "where TestColumn IN (0) and TestColumn2 = 'test'").Object
            };

            Assert.AreEqual(string.Empty, expectedSelectCommand.CreateCommand());
            Assert.AreEqual(string.Empty, expectedSelectCommand.ToString());
        }

        [Test]
        public void ShouldNotBeAValidCommandWhenNoSelect()
        {
            var expectedSelectCommand = new SelectCommand
            {
                GroupBy = createCommand<IGroupByClauseCommand>(true, "GROUP BY TestColumn1, TestColumn2, TestColumn3").Object,
                Limit = 300,
                Offset = 20,
                OrderBy = createCommand<IOrderByClauseCommand>(true, "order by TestColumn1, TestColumn2, TestColumn3").Object,
                Select = createCommand<ISelectClauseCommand>(false, string.Empty).Object,
                From = createCommand<IFromClauseCommand>(true, "TestTable").Object,
                Where = createCommand<IWhereClauseCommand>(true, "where TestColumn IN (0) and TestColumn2 = 'test'").Object
            };

            Assert.IsFalse(expectedSelectCommand.IsValid);
        }

        [Test]
        public void ShouldShowMessageForNoSelectClause()
        {
            var expectedSelectCommand = new SelectCommand
            {
                GroupBy = createCommand<IGroupByClauseCommand>(true, "GROUP BY TestColumn1, TestColumn2, TestColumn3").Object,
                Limit = 300,
                Offset = 20,
                OrderBy = createCommand<IOrderByClauseCommand>(true, "order by TestColumn1, TestColumn2, TestColumn3").Object,
                Select = createCommand<ISelectClauseCommand>(false, string.Empty, "Invalid select clause").Object,
                From = createCommand<IFromClauseCommand>(true, "TestTable").Object,
                Where = createCommand<IWhereClauseCommand>(true, "where TestColumn IN (0) and TestColumn2 = 'test'").Object
            };

            Assert.AreEqual("Cannot construct SELECT clause. Error(s): Invalid select clause", expectedSelectCommand.InvalidCommandMessage);
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