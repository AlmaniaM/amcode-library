using System.Collections.Generic;
using AMCode.Sql.OrderBy;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.OrderBy.OrderByClauseCommandTests
{
    [TestFixture]
    public class OrderByClauseCommandTest
    {
        [Test]
        public void ShouldBuildOrderByClauseCommand()
        {
            var orderByClauseCommand = new OrderByClauseCommand(new List<string> { "Column1", "Column2" });
            Assert.AreEqual("ORDER BY Column1, Column2", orderByClauseCommand.CreateCommand());
        }

        [Test]
        public void ShouldBuildOrderByClauseToString()
        {
            var orderByClauseCommand = new OrderByClauseCommand(new List<string> { "Column1", "Column2", "COALESCE(Column3, 'N/A')" });
            Assert.AreEqual("ORDER BY Column1, Column2, COALESCE(Column3, 'N/A')", orderByClauseCommand.ToString());
        }

        [Test]
        public void ShouldBuildOrderByClauseValue()
        {
            var orderByClauseCommand = new OrderByClauseCommand(new List<string> { "Column1", "Column2" });
            Assert.AreEqual("Column1, Column2", orderByClauseCommand.GetCommandValue());
        }

        [TestCase(" TestColumn1 ")]
        [TestCase("TestColumn1, TestColumn2")]
        public void ShouldReturnValidCommandWhenOnlyOneExpressionAndIsValid(string expression)
        {
            var orderByClauseCommand = new OrderByClauseCommand();
            orderByClauseCommand.SetOrderByExpression(expression);
            Assert.IsTrue(orderByClauseCommand.IsValid);
        }

        [Test]
        public void ShouldReturnValidCommandWhenMoreThanOneExpression()
        {
            var orderByClauseCommand = new OrderByClauseCommand(new List<string> { "Column1", "SUM(Column2)" });
            Assert.IsTrue(orderByClauseCommand.IsValid);
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
            var orderByClauseCommand = new OrderByClauseCommand();
            orderByClauseCommand.SetOrderByExpression(expression);
            Assert.IsFalse(orderByClauseCommand.IsValid);
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
            var orderByClauseCommand = new OrderByClauseCommand(new List<string> { "Column1", "Column2" });
            orderByClauseCommand.SetOrderByExpression(expression);
            Assert.IsFalse(orderByClauseCommand.IsValid);
        }

        [Test]
        public void ShouldGetInvalidMessageAboutNoQueryExpressions()
        {
            var orderByClauseCommand = new OrderByClauseCommand();
            Assert.AreEqual("There are no ORDER BY expressions to build an ORDER BY clause with.", orderByClauseCommand.InvalidCommandMessage);
        }

        [Test]
        public void ShouldGetInvalidMessageAboutInvalidQueryExpressions()
        {
            var orderByClauseCommand = new OrderByClauseCommand(new List<string> { null, ",Column", "Column2" });
            Assert.AreEqual($"There are invalid ORDER BY expressions. Values are 'null', ',Column'.", orderByClauseCommand.InvalidCommandMessage);
        }
    }
}