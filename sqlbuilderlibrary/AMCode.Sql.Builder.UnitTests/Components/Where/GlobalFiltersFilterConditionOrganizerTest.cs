using AMCode.Common.FilterStructures;
using AMCode.Sql.Where.Internal;
using AMCode.Sql.Where.Models;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Where.GlobalFiltersFilterConditionOrganizerTests
{
    [TestFixture]
    public class GlobalFiltersFilterConditionOrganizerTest
    {
        private Mock<IFilter> filterMoq;
        private Mock<IFilterConditionSection> filterConditionSectionMoq;
        private Mock<IFilterConditionBuilder> filterConditionBuilderMoq;
        private GlobalFiltersFilterConditionOrganizer filterConditionOrganizer;
        private Mock<IWhereClauseBuilder> whereClauseBuilderMoq;

        [SetUp]
        public void SetUp()
        {
            filterMoq = new();
            filterConditionSectionMoq = new();
            filterConditionBuilderMoq = new();
            filterConditionBuilderMoq.Setup(moq => moq.CreateFilterClause()).Returns(filterConditionSectionMoq.Object);
            filterConditionBuilderMoq.Setup(moq => moq.GetFilterName(It.IsAny<IFilter>())).Returns("TestFilter");

            whereClauseBuilderMoq = new();
        }

        [Test]
        public void ShouldAddFilterConditionSectionOnceIfNotLastSelectedFilter()
        {
            filterConditionOrganizer = new GlobalFiltersFilterConditionOrganizer(filterConditionBuilderMoq.Object, filterMoq.Object, "TestFilter1");
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

        [Test]
        public void ShouldAddFilterConditionSectionTwiceIfLastSelectedFilter()
        {
            filterConditionOrganizer = new GlobalFiltersFilterConditionOrganizer(filterConditionBuilderMoq.Object, filterMoq.Object, "TestFilter");
            filterConditionOrganizer.AddFilterCondition(whereClauseBuilderMoq.Object);

            whereClauseBuilderMoq.Verify(
                moq => moq.AddFilterCondition(
                    It.IsAny<IFilterConditionSection>(),
                    It.IsAny<FilterConditionSectionType>()
                ),
                Times.Exactly(2)
            );

            whereClauseBuilderMoq.Verify(
                moq => moq.AddFilterCondition(
                    It.IsAny<IFilterConditionSection>(),
                    It.Is<FilterConditionSectionType>(t => t == FilterConditionSectionType.Default)
                ),
                Times.Once()
            );

            whereClauseBuilderMoq.Verify(
                moq => moq.AddFilterCondition(
                    It.IsAny<IFilterConditionSection>(),
                    It.Is<FilterConditionSectionType>(t => t == FilterConditionSectionType.LastSelected)
                ),
                Times.Once()
            );
        }
    }
}