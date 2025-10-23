using System.Collections.Generic;
using AMCode.Sql.Where;
using AMCode.Sql.Where.Internal;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Where.WhereClauseCommandTests
{
    [TestFixture]
    public class WhereClauseCommandTest
    {
        [Test]
        public void ShouldBuildWhereClauseCommand()
        {
            var selectClauseCommand = new WhereClauseCommand(new List<IWhereClauseSection>
            {
                createWhereClauseSectionMoq(true, true, "Column1 IN (1, 2) OR ").Object,
                createWhereClauseSectionMoq(true, true, "Column2 IN (3, 4)").Object
            });

            Assert.AreEqual("WHERE Column1 IN (1, 2) OR Column2 IN (3, 4)", selectClauseCommand.CreateCommand());
            Assert.AreEqual("WHERE Column1 IN (1, 2) OR Column2 IN (3, 4)", selectClauseCommand.ToString());
        }

        [Test]
        public void ShouldBuildWhereClauseValue()
        {
            var selectClauseCommand = new WhereClauseCommand(new List<IWhereClauseSection>
            {
                createWhereClauseSectionMoq(true, true, "Column1 IN (1, 2) OR ").Object,
                createWhereClauseSectionMoq(true, true, "Column2 IN (3, 4)").Object
            });

            Assert.AreEqual("Column1 IN (1, 2) OR Column2 IN (3, 4)", selectClauseCommand.GetCommandValue());
        }

        [Test]
        public void ShouldReturnValidCommandWhenOnlyOneExpressionAndIsValid()
        {
            var selectClauseCommand = new WhereClauseCommand();

            var whereClauseSectionMoq = createWhereClauseSectionMoq(true, true, string.Empty);

            selectClauseCommand.AddWhereClauseSection(whereClauseSectionMoq.Object);

            Assert.IsTrue(selectClauseCommand.IsValid);
        }

        [Test]
        public void ShouldReturnValidCommandWhenMoreThanOneExpression()
        {
            var selectClauseCommand = new WhereClauseCommand(new List<IWhereClauseSection>
            {
                createWhereClauseSectionMoq(true, true, "Column1 IN (1, 2) OR ").Object,
                createWhereClauseSectionMoq(true, true, "Column2 IN (3, 4)").Object
            });

            Assert.IsTrue(selectClauseCommand.IsValid);
        }

        [Test]
        public void ShouldReturnInvalidCommandWhenOnlyOneExpressionAndIsInvalid()
        {
            var selectClauseCommand = new WhereClauseCommand();

            var whereClauseSectionMoq = createWhereClauseSectionMoq(false, true, string.Empty);

            selectClauseCommand.AddWhereClauseSection(whereClauseSectionMoq.Object);

            Assert.IsFalse(selectClauseCommand.IsValid);
        }

        [Test]
        public void ShouldReturnInvalidCommandWhenMoreThanOneExpressionAndOneIsInvalid()
        {
            var selectClauseCommand = new WhereClauseCommand(new List<IWhereClauseSection>
            {
                createWhereClauseSectionMoq(true, true, "Column1 IN (1, 2) OR ").Object,
                createWhereClauseSectionMoq(false, true, "Column2 IN (3, 4)").Object
            });

            Assert.IsFalse(selectClauseCommand.IsValid);
        }

        [Test]
        public void ShouldGetInvalidMessageAboutNoQueryExpressions()
        {
            var selectClauseCommand = new WhereClauseCommand();
            Assert.AreEqual("There are no where clause sections to build a WHERE clause with.", selectClauseCommand.InvalidCommandMessage);
        }

        [Test]
        public void ShouldGetInvalidMessageAboutInvalidQueryExpressions()
        {
            var invalidWhereClauseSectionMoq = createWhereClauseSectionMoq(false, false, "Column1 IN (1, 2) OR ");
            invalidWhereClauseSectionMoq.Setup(moq => moq.InvalidCommandMessage).Returns(() => "Invalid where clause section");
            var selectClauseCommand = new WhereClauseCommand(new List<IWhereClauseSection>
            {
                invalidWhereClauseSectionMoq.Object,
                null
            });

            Assert.AreEqual($"There are invalid where clause sections. Values are 'Invalid where clause section', 'null'.", selectClauseCommand.InvalidCommandMessage);
        }

        private Mock<IWhereClauseSection> createWhereClauseSectionMoq(bool isValid, bool hasAny, string section)
        {
            var sectionMoq = new Mock<IWhereClauseSection>();
            sectionMoq.Setup(moq => moq.IsValid).Returns(() => isValid);
            sectionMoq.Setup(moq => moq.HasAny).Returns(() => hasAny);
            sectionMoq.Setup(moq => moq.CreateWhereClauseSectionString(It.IsAny<bool>())).Returns(() => section);
            return sectionMoq;
        }
    }
}