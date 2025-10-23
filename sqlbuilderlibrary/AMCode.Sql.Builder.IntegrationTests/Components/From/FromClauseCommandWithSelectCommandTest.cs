using System;
using System.Collections.Generic;
using System.Text;
using AMCode.Sql.Commands;
using AMCode.Sql.From;
using AMCode.Sql.Select;
using NUnit.Framework;

namespace AMCode.Sql.Builder.IntegrationTests.From.FromClauseCommandTests
{
    [TestFixture]
    public class FromClauseCommandWithSelectCommandTest
    {
        private readonly string aliasName = "TestAlias";
        private string expectedString;
        private readonly string schemaName = "TestSchema";
        private readonly string tableName = "TestTable";

        private ISelectCommand selectCommand;

        [SetUp]
        public void SetUp()
        {
            var selectCommandString = $"SELECT Column1, Column2, Column3{Environment.NewLine}FROM {schemaName}.{tableName} AS Table";

            selectCommand = new SelectCommandBuilder()
                .Select(new SelectClauseCommand(new List<string> { "Column1", "Column2", "Column3" }))
                .From(schemaName, tableName, "Table")
                .Build();

            expectedString = new StringBuilder()
                .Append("FROM ")
                .AppendLine("(")
                .AppendLine(selectCommandString)
                .Append($") AS {aliasName}")
                .ToString();
        }

        [Test]
        public void ShouldBuildFromClauseCommandWithSubQuery()
            => Assert.AreEqual(expectedString, new FromClauseCommand(selectCommand, aliasName).CreateCommand());

        [Test]
        public void ShouldBuildFromClauseCommandWithSubQueryToString()
            => Assert.AreEqual(expectedString, new FromClauseCommand(selectCommand, aliasName).ToString());

        [Test]
        public void ShouldBuildFromClauseCommandWithSubQueryGetFrom()
            => Assert.AreEqual(expectedString, new FromClauseCommand(selectCommand, aliasName).GetFrom());
    }
}