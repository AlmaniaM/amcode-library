using System.Text;
using AMCode.Sql.Commands;
using AMCode.Sql.From;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.IntegrationTests.From.FromClauseTests
{
    [TestFixture]
    public class FromClauseTests
    {
        [Test]
        public void ShouldCreateAFromClauseWithTableName()
        {
            Assert.AreEqual(
                "FROM TestSchema.TestTableName",
                new FromClause().CreateClause("TestSchema", "TestTableName").CreateCommand()
            );
        }

        [Test]
        public void ShouldCreateAFromClauseWithSchemaTableNameAndAlias()
        {
            Assert.AreEqual(
                "FROM TestSchema.TestTableName AS TestAlias",
                new FromClause().CreateClause("TestSchema", "TestTableName", "TestAlias").CreateCommand()
            );
        }

        [Test]
        public void ShouldCreateAFromClauseWithASelectCommand()
        {
            var selectCommandMoq = new Mock<ISelectCommand>();
            selectCommandMoq.SetupGet(moq => moq.IsValid).Returns(() => true);
            selectCommandMoq.SetupGet(moq => moq.CommandType).Returns(() => "SELECT");
            selectCommandMoq.Setup(moq => moq.CreateCommand()).Returns(() => "SELECT * FROM TestTable");

            var expectedString = new StringBuilder()
                .AppendLine("FROM (")
                .AppendLine("SELECT * FROM TestTable")
                .Append(") AS TestAlias")
                .ToString();

            Assert.AreEqual(
                expectedString,
                new FromClause().CreateClause(selectCommandMoq.Object, "TestAlias").CreateCommand()
            );
        }
    }
}