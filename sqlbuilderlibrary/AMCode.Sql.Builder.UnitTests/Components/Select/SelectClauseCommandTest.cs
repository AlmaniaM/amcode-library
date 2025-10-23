using System.Collections.Generic;
using AMCode.Sql.Select;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Select.SelectClauseCommandTests
{
    [TestFixture]
    public class SelectClauseCommandTest
    {
        [Test]
        public void ShouldBuildSelectClauseCommand()
        {
            var selectClauseCommand = new SelectClauseCommand(new List<string> { "Column1", "Column2" });
            Assert.AreEqual("SELECT Column1, Column2", selectClauseCommand.CreateCommand());
        }

        [Test]
        public void ShouldBuildSelectClauseToString()
        {
            var selectClauseCommand = new SelectClauseCommand(new List<string> { "Column1", "Column2" });
            Assert.AreEqual("SELECT Column1, Column2", selectClauseCommand.ToString());
        }

        [Test]
        public void ShouldBuildSelectClauseValue()
        {
            var selectClauseCommand = new SelectClauseCommand(new List<string> { "Column1", "Column2" });
            Assert.AreEqual("Column1, Column2", selectClauseCommand.GetCommandValue());
        }

        [TestCase(" TestColumn1 ")]
        [TestCase("TestColumn1, TestColumn2")]
        public void ShouldReturnValidCommandWhenOnlyOneExpressionAndIsValid(string expression)
        {
            var selectClauseCommand = new SelectClauseCommand();
            selectClauseCommand.AddQueryExpression(expression);
            Assert.IsTrue(selectClauseCommand.IsValid);
        }

        [Test]
        public void ShouldReturnValidCommandWhenMoreThanOneExpression()
        {
            var selectClauseCommand = new SelectClauseCommand(new List<string> { "Column1", "Column2" });
            Assert.IsTrue(selectClauseCommand.IsValid);
        }

        [TestCase(" TestColumn1,")]
        [TestCase(null)]
        [TestCase("  , TestColumn1, TestColumn2")]
        [TestCase("    ")]
        [TestCase("")]
        [TestCase(",")]
        [TestCase(",,,")]
        public void ShouldReturnInvalidCommandWhenOnlyOneExpressionAndIsInvalid(string expression)
        {
            var selectClauseCommand = new SelectClauseCommand();
            selectClauseCommand.AddQueryExpression(expression);
            Assert.IsFalse(selectClauseCommand.IsValid);
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
            var selectClauseCommand = new SelectClauseCommand(new List<string> { "Column1", "Column2" });
            selectClauseCommand.AddQueryExpression(expression);
            Assert.IsFalse(selectClauseCommand.IsValid);
        }

        [Test]
        public void ShouldGetInvalidMessageAboutNoQueryExpressions()
        {
            var selectClauseCommand = new SelectClauseCommand();
            Assert.AreEqual("There are no query expressions to build a SELECT clause with.", selectClauseCommand.InvalidCommandMessage);
        }

        [Test]
        public void ShouldGetInvalidMessageAboutInvalidQueryExpressions()
        {
            var selectClauseCommand = new SelectClauseCommand(new List<string> { null, ",Column", "Column2" });
            Assert.AreEqual($"There are invalid query expressions. Values are 'null', ',Column'.", selectClauseCommand.InvalidCommandMessage);
        }
    }
}