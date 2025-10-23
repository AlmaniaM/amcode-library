using System.Collections.Generic;
using System.Linq;
using AMCode.Sql.Commands.Models;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Sql.Select;
using AMCode.Sql.Select.Extensions;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Select.SelectClauseTests
{
    [TestFixture]
    public class SelectClauseTests
    {
        [Test]
        public void ShouldCreateASelectClause()
        {
            var queriesMoq = new List<Mock<IGetQueryExpression>> { new(), new(), new() };
            queriesMoq.ForEach((queryMoq, i) =>
            {
                queryMoq.Setup(moq => moq.GetExpression()).Returns($"TestColumn{i + 1}");
                queryMoq.Setup(moq => moq.IsVisible).Returns(true);
            });

            Assert.AreEqual(
                "SELECT TestColumn1, TestColumn2, TestColumn3",
                new SelectClause().CreateClause(queriesMoq.Select(moq => moq.Object)).CreateCommand()
            );
        }

        [Test]
        public void ShouldCreateASelectClauseWithCustomQueryName()
        {
            var queriesMoq = new List<Mock<IGetQueryExpression>> { new(), new(), new() };
            queriesMoq.ForEach((queryMoq, i) =>
            {
                queryMoq.Setup(moq => moq.GetExpression()).Returns($"TestColumn{i + 1}");
                queryMoq.Setup(moq => moq.GetExpression("Custom")).Returns($"Custom(TestColumn{i + 1})");
                queryMoq.Setup(moq => moq.IsVisible).Returns(true);
            });

            Assert.AreEqual(
                "SELECT Custom(TestColumn1), Custom(TestColumn2), Custom(TestColumn3)",
                new SelectClause().CreateClause(queriesMoq.Select(moq => moq.Object), queryProvider => "Custom").CreateCommand()
            );
        }

        [Test]
        public void ShouldGetAnEmptyStringWhenAnEmptyQueryProviderListIsProvided()
        {
            var queriesMoq = new List<Mock<IGetQueryExpression>> { };
            queriesMoq.ForEach((queryMoq, i) =>
            {
                queryMoq.Setup(moq => moq.GetExpression()).Returns($"TestColumn{i + 1}");
                queryMoq.Setup(moq => moq.IsVisible).Returns(true);
            });

            Assert.AreEqual(string.Empty, new SelectClause().CreateClause(queriesMoq.Select(moq => moq.Object)).CreateCommand());
        }

        [Test]
        public void ShouldGetAnEmptyStringWhenAnEmptyQueryProviderListIsProvidedWithFunction()
        {
            var queriesMoq = new List<Mock<IGetQueryExpression>> { };
            queriesMoq.ForEach((queryMoq, i) =>
            {
                queryMoq.Setup(moq => moq.GetExpression("Custom")).Returns($"Custom(TestColumn{i + 1})");
                queryMoq.Setup(moq => moq.IsVisible).Returns(true);
            });

            Assert.AreEqual(string.Empty, new SelectClause().CreateClause(queriesMoq.Select(moq => moq.Object), queryProvider => "Custom").CreateCommand());
        }

        [Test]
        public void ShouldThrowNoGetQueryExpressionNameFunctionProvidedExceptionForCreateClause()
        {
            var queriesMoq = new List<Mock<IGetQueryExpression>> { new() };
            queriesMoq.ForEach((queryMoq, i) =>
            {
                queryMoq.Setup(moq => moq.GetExpression()).Returns(string.Empty);
                queryMoq.Setup(moq => moq.IsVisible).Returns(true);
            });

            Assert.Throws<NoGetQueryExpressionNameFunctionProvidedException>(() => new SelectClause().CreateClause(queriesMoq.Select(moq => moq.Object), null));
        }
    }
}