using AMCode.Common.FilterStructures;
using AMCode.Sql.Where.Internal;
using AMCode.Sql.Where.Models;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Where.DefaultFilterConditionOrganizerTests
{
    [TestFixture]
    public class DefaultFilterConditionOrganizerTest
    {
        private Mock<IFilterConditionSection> filterConditionSectionMoq;
        private Mock<IFilterConditionBuilder> filterConditionBuilderMoq;
        private DefaultFilterConditionOrganizer filterConditionOrganizer;
        private Mock<IWhereClauseBuilder> whereClauseBuilderMoq;

        [SetUp]
        public void SetUp()
        {
            filterConditionSectionMoq = new();
            filterConditionBuilderMoq = new();
            filterConditionBuilderMoq.Setup(moq => moq.CreateFilterClause()).Returns(filterConditionSectionMoq.Object);
            filterConditionBuilderMoq.Setup(moq => moq.GetFilterName(It.IsAny<IFilter>())).Returns("TestFilter");

            whereClauseBuilderMoq = new();
        }

        [Test]
        public void ShouldAddFilterConditionSectionOnce()
        {
            filterConditionOrganizer = new DefaultFilterConditionOrganizer(filterConditionBuilderMoq.Object);
            filterConditionOrganizer.AddFilterCondition(whereClauseBuilderMoq.Object);

            whereClauseBuilderMoq.Verify(
                moq => moq.AddFilterCondition(
                    It.IsAny<IFilterConditionSection>(),
                    It.IsAny<FilterConditionSectionType>()
                ),
                Times.Once()
            );

            whereClauseBuilderMoq.Verify(
                moq => moq.AddFilterCondition(
                    It.IsAny<IFilterConditionSection>(),
                    It.Is<FilterConditionSectionType>(t => t == FilterConditionSectionType.Default)
                ),
                Times.Once()
            );
        }
    }
}