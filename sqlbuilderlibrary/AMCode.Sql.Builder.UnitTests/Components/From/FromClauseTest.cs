using AMCode.Sql.Commands;
using AMCode.Sql.From;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.From.FromClauseTests
{
    [TestFixture]
    public class FromClauseTests
    {
        [Test]
        public void ShouldCreateAFromClauseWithTableName()
        {
            Assert.AreEqual(
                typeof(FromClauseCommand),
                new FromClause().CreateClause("TestSchema", "TestTableName").GetType()
            );
        }

        [Test]
        public void ShouldCreateAFromClauseWithSchemaTableNameAndAlias()
        {
            Assert.AreEqual(
                typeof(FromClauseCommand),
                new FromClause().CreateClause("TestSchema", "TestTableName", "TestAlias").GetType()
            );
        }

        [Test]
        public void ShouldCreateAFromClauseWithASelectCommand()
        {
            var selectCommandMoq = new Mock<ISelectCommand>();
            selectCommandMoq.SetupGet(moq => moq.IsValid).Returns(() => true);
            selectCommandMoq.SetupGet(moq => moq.CommandType).Returns(() => "SELECT");
            selectCommandMoq.Setup(moq => moq.CreateCommand()).Returns(() => "SELECT * FROM TestTable");

            Assert.AreEqual(
                typeof(FromClauseCommand),
                new FromClause().CreateClause(selectCommandMoq.Object, "TestAlias").GetType()
            );
        }
    }
}