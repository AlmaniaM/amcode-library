using System.Collections.Generic;
using System.Linq;
using AMCode.Sql.Commands.Models;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Sql.OrderBy;
using AMCode.Sql.OrderBy.Exceptions;
using AMCode.Sql.OrderBy.Models;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.OrderBy.OrderByClauseTests
{
    [TestFixture]
    public class OrderByClauseTest
    {
        [Test]
        public void ShouldBuildOrderByClause()
        {
            var sortProviders = new List<Mock<ISortProvider>> { new(), new(), new() };
            sortProviders.ForEach((Mock<ISortProvider> sortProviderMoq, int i) =>
            {
                sortProviderMoq.SetupGet(moq => moq.IsVisible).Returns(true);
                sortProviderMoq.Setup(moq => moq.SortIndex).Returns(i + 1);
                sortProviderMoq.Setup(moq => moq.GetSort()).Returns(() => $"Test Column {i + 1} ASC");
            });

            Assert.AreEqual(
                "ORDER BY Test Column 1 ASC, Test Column 2 ASC, Test Column 3 ASC",
                new OrderByClause().CreateClause(sortProviders.Select(sortProvider => sortProvider.Object)).ToString()
            );
        }

        [Test]
        public void ShouldBuildOrderByClauseInUnOrderedList()
        {
            Mock<ISortProvider> createSortProviderMoq(int index, int columnNumber)
            {
                var sortProviderMoq = new Mock<ISortProvider>();
                sortProviderMoq.SetupGet(moq => moq.IsVisible).Returns(true);
                sortProviderMoq.Setup(moq => moq.SortIndex).Returns(index);
                sortProviderMoq.Setup(moq => moq.GetSort()).Returns(() => $"TestColumn{columnNumber} ASC");
                return sortProviderMoq;
            }

            var sortProviders = new List<Mock<ISortProvider>>
            {
                createSortProviderMoq(2, 1),
                createSortProviderMoq(3, 2),
                createSortProviderMoq(1, 3)
            };

            Assert.AreEqual(
                "ORDER BY TestColumn3 ASC, TestColumn1 ASC, TestColumn2 ASC",
                new OrderByClause().CreateClause(sortProviders.Select(sortProvider => sortProvider.Object)).ToString()
            );
        }

        [Test]
        public void ShouldNotBuildOrderByClauseWhenSortProvidersAreEmpty()
        {
            var sortProviders = new List<Mock<ISortProvider>> { };
            sortProviders.ForEach((Mock<ISortProvider> sortProviderMoq, int i) => sortProviderMoq.Setup(moq => moq.GetSort()).Returns($"Test Column {i + 1} ASC"));

            Assert.AreEqual(string.Empty, new OrderByClause().CreateClause(sortProviders.Select(sortProvider => sortProvider.Object)).ToString());
        }

        [Test]
        public void ShouldBuildOrderByClauseWithNamedSort()
        {
            var sortProviders = new List<Mock<ISortProvider>> { new(), new(), new() };
            sortProviders.ForEach((Mock<ISortProvider> sortProviderMoq, int i) =>
            {
                sortProviderMoq.SetupGet(moq => moq.IsVisible).Returns(true);
                sortProviderMoq.Setup(moq => moq.SortIndex).Returns(i + 1);
                sortProviderMoq.Setup(moq => moq.GetSort(It.IsAny<bool?>())).Returns($"Test Column {i + 1} ASC");
                sortProviderMoq.Setup(moq => moq.GetSort("Custom")).Returns($"Custom(TestColumn{i + 1}) DESC");
            });

            Assert.AreEqual(
                "ORDER BY Custom(TestColumn1) DESC, Custom(TestColumn2) DESC, Custom(TestColumn3) DESC",
                new OrderByClause().CreateClause(sortProviders.Select(sortProvider => sortProvider.Object), column => "Custom").ToString()
            );
        }

        [Test]
        public void ShouldGetAnEmptyListFromCreateClauseWhenSortProvidersIsEmpty()
            => Assert.IsNull(new OrderByClause().CreateClause(null));

        [Test]
        public void ShouldGetAnEmptyListFromCreateClauseWhenSortProvidersIsEmptyWithFunction()
            => Assert.IsNull(new OrderByClause().CreateClause(null, column => "Custom"));

        [Test, Description("Should throw a " + nameof(NoGetSortFormatterNameFunctionProvidedException) + " when provided a null " + nameof(GetSortFormatterNameFunction) + ".")]
        public void ShouldThrowNoGetSortFormatterNameFunctionProvidedException()
            => Assert.Throws<NoGetSortFormatterNameFunctionProvidedException>(() => new OrderByClause().CreateClause(new List<ISortProvider>(), null));
    }
}