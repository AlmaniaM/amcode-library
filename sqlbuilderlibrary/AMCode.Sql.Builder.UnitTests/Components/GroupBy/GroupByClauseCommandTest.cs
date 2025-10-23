using System.Collections.Generic;
using AMCode.Sql.GroupBy;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.GroupBy.GroupByClauseCommandTests
{
    [TestFixture]
    public class GroupByClauseCommandTest
    {
        [Test]
        public void ShouldBuildGroupByClauseCommand()
        {
            var groupByClauseCommand = new GroupByClauseCommand(new List<string> { "Column1", "Column2" });
            Assert.AreEqual("GROUP BY Column1, Column2", groupByClauseCommand.CreateCommand());
        }

        [Test]
        public void ShouldBuildGroupByClauseToString()
        {
            var groupByClauseCommand = new GroupByClauseCommand(new List<string> { "Column1", "Column2", "Column3" });
            Assert.AreEqual("GROUP BY Column1, Column2, Column3", groupByClauseCommand.ToString());
        }

        [Test]
        public void ShouldBuildGroupByClauseValue()
        {
            var groupByClauseCommand = new GroupByClauseCommand(new List<string> { "Column1", "Column2" });
            Assert.AreEqual("Column1, Column2", groupByClauseCommand.GetCommandValue());
        }

        [TestCase(" TestColumn1 ")]
        [TestCase("TestColumn1, TestColumn2")]
        public void ShouldReturnValidCommandWhenOnlyOneExpressionAndIsValid(string expression)
        {
            var groupByClauseCommand = new GroupByClauseCommand();
            groupByClauseCommand.SetGroupByExpression(expression);
            Assert.IsTrue(groupByClauseCommand.IsValid);
        }

        [Test]
        public void ShouldReturnValidCommandWhenMoreThanOneExpression()
        {
            var groupByClauseCommand = new GroupByClauseCommand(new List<string> { "Column1", "SUM(Column2)" });
            Assert.IsTrue(groupByClauseCommand.IsValid);
        }

        [TestCase(" TestColumn1,")]
        [TestCase(null)]
        [TestCase("  , TestColumn1, TestColumn2")]
        [TestCase("  , TestColumn1, MAX(TestColumn2)")]
        [TestCase("    ")]
        [TestCase("")]
        [TestCase(",")]
        [TestCase(",,,")]
        public void ShouldReturnInvalidCommandWhenOnlyOneExpressionAndIsInvalid(string expression)
        {
            var groupByClauseCommand = new GroupByClauseCommand();
            groupByClauseCommand.SetGroupByExpression(expression);
            Assert.IsFalse(groupByClauseCommand.IsValid);
        }

        [TestCase(" TestColumn1,")]
        [TestCase(null)]
        [TestCase("  , TestColumn1, TestColumn2")]
        [TestCase("    ")]
        [TestCase("")]
        [TestCase(",")]
        [TestCase(",,,")]
        public void ShouldReturnInvalidCommandWhenMoreThanOneExpressionAndOneIsInvalid(string expression)
        {
            var groupByClauseCommand = new GroupByClauseCommand(new List<string> { "Column1", "Column2" });
            groupByClauseCommand.SetGroupByExpression(expression);
            Assert.IsFalse(groupByClauseCommand.IsValid);
        }

        [Test]
        public void ShouldGetInvalidMessageAboutNoQueryExpressions()
        {
            var groupByClauseCommand = new GroupByClauseCommand();
            Assert.AreEqual("There are no GROUP BY expressions to build a GROUP BY clause with.", groupByClauseCommand.InvalidCommandMessage);
        }

        [Test]
        public void ShouldGetInvalidMessageAboutInvalidQueryExpressions()
        {
            var groupByClauseCommand = new GroupByClauseCommand(new List<string> { null, ",Column", "Column2" });
            Assert.AreEqual($"There are invalid GROUP BY expressions. Values are 'null', ',Column'.", groupByClauseCommand.InvalidCommandMessage);
        }
    }
}