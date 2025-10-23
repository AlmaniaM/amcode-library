using System.Collections.Generic;
using AMCode.Sql.Commands;
using AMCode.Sql.From;
using AMCode.Sql.GroupBy;
using AMCode.Sql.OrderBy;
using AMCode.Sql.Select;
using AMCode.Sql.Where;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Commands.SelectCommandBuilderTests
{
    [TestFixture]
    public class SelectCommandBuilderTest
    {
        [Test]
        public void ShouldAddFrom()
        {
            var commandBaseMoq = new Mock<ISelectCommand>();
            var commandBuilder = new SelectCommandBuilder(commandBaseMoq.Object)
                .From("Test", "TableName");

            commandBaseMoq.VerifySet(moq => moq.From = It.Is<IFromClauseCommand>(from => from.GetCommandValue().Equals("Test.TableName")));
        }

        [Test]
        public void ShouldAddGroupBy()
        {
            var commandBaseMoq = new Mock<ISelectCommand>();
            var groupByClauseCommandMoq = new Mock<IGroupByClauseCommand>();

            var commandBuilder = new SelectCommandBuilder(commandBaseMoq.Object)
                .GroupBy(groupByClauseCommandMoq.Object);

            commandBaseMoq.VerifySet(moq => moq.GroupBy = It.Is<IGroupByClauseCommand>(value => value == groupByClauseCommandMoq.Object));
        }

        [Test]
        public void ShouldAddLimit()
        {
            var commandBaseMoq = new Mock<ISelectCommand>();
            var commandBuilder = new SelectCommandBuilder(commandBaseMoq.Object)
                .Limit(20);

            commandBaseMoq.VerifySet(moq => moq.Limit = It.Is<int?>(value => value == 20));
        }

        [Test]
        public void ShouldAddLimitNull()
        {
            var commandBaseMoq = new Mock<ISelectCommand>();
            var commandBuilder = new SelectCommandBuilder(commandBaseMoq.Object)
                .Limit(null);

            commandBaseMoq.VerifySet(moq => moq.Limit = It.Is<int?>(value => value == null));
        }

        [Test]
        public void ShouldAddOffset()
        {
            var commandBaseMoq = new Mock<ISelectCommand>();
            var commandBuilder = new SelectCommandBuilder(commandBaseMoq.Object)
                .Offset(10);

            commandBaseMoq.VerifySet(moq => moq.Offset = It.Is<int?>(value => value == 10));
        }

        [Test]
        public void ShouldAddOffsetNull()
        {
            var commandBaseMoq = new Mock<ISelectCommand>();
            var commandBuilder = new SelectCommandBuilder(commandBaseMoq.Object)
                .Offset(null);

            commandBaseMoq.VerifySet(moq => moq.Offset = It.Is<int?>(value => value == null));
        }

        [Test]
        public void ShouldAddOrderBy()
        {
            var commandBaseMoq = new Mock<ISelectCommand>();
            var orderByClauseCommandMoq = new Mock<IOrderByClauseCommand>();

            var commandBuilder = new SelectCommandBuilder(commandBaseMoq.Object)
                .OrderBy(orderByClauseCommandMoq.Object);

            commandBaseMoq.VerifySet(moq => moq.OrderBy = It.Is<IOrderByClauseCommand>(value => value == orderByClauseCommandMoq.Object));
        }

        [Test]
        public void ShouldAddSelect()
        {
            var commandBaseMoq = new Mock<ISelectCommand>();
            var selectClauseCommandMoq = new Mock<ISelectClauseCommand>();

            var commandBuilder = new SelectCommandBuilder(commandBaseMoq.Object)
                .Select(selectClauseCommandMoq.Object);

            commandBaseMoq.VerifySet(moq => moq.Select = It.Is<ISelectClauseCommand>(value => value == selectClauseCommandMoq.Object));
        }

        [Test]
        public void ShouldAddSelectUsingQueryExpressions()
        {
            var commandBaseMoq = new Mock<ISelectCommand>();

            var queryExpressions = new List<string> { "Column1", "Column2" };

            var commandBuilder = new SelectCommandBuilder(commandBaseMoq.Object)
                .Select(queryExpressions);

            commandBaseMoq.VerifySet(moq => moq.Select = It.Is<ISelectClauseCommand>(selectCommand => selectCommand.GetCommandValue().Equals(string.Join(", ", queryExpressions))));
        }

        [Test]
        public void ShouldAddWhere()
        {
            var commandBaseMoq = new Mock<ISelectCommand>();
            var whereClauseCommandMoq = new Mock<IWhereClauseCommand>();

            var commandBuilder = new SelectCommandBuilder(commandBaseMoq.Object)
                .Where(whereClauseCommandMoq.Object);

            commandBaseMoq.VerifySet(moq => moq.Where = It.Is<IWhereClauseCommand>(value => value == whereClauseCommandMoq.Object));
        }

        [Test]
        public void ShouldBuildISelectCommand()
        {
            var groupByClauseCommandMoq = new Mock<IGroupByClauseCommand>();
            var orderByClauseCommandMoq = new Mock<IOrderByClauseCommand>();
            var selectClauseCommandMoq = new Mock<ISelectClauseCommand>();
            var whereClauseCommandMoq = new Mock<IWhereClauseCommand>();

            var expectedSelectCommand = new SelectCommand
            {
                GroupBy = groupByClauseCommandMoq.Object,
                Limit = 300,
                Offset = 20,
                OrderBy = orderByClauseCommandMoq.Object,
                Select = selectClauseCommandMoq.Object,
                From = new FromClauseCommand("Test", "TableName"),
                Where = whereClauseCommandMoq.Object
            };

            var resultSelectCommand = new SelectCommandBuilder(expectedSelectCommand)
                .Build();

            Assert.AreEqual(expectedSelectCommand.GroupBy, resultSelectCommand.GroupBy);
            Assert.AreEqual(expectedSelectCommand.Limit, resultSelectCommand.Limit);
            Assert.AreEqual(expectedSelectCommand.Offset, resultSelectCommand.Offset);
            Assert.AreEqual(expectedSelectCommand.OrderBy, resultSelectCommand.OrderBy);
            Assert.AreEqual(expectedSelectCommand.Select, resultSelectCommand.Select);
            Assert.AreEqual(expectedSelectCommand.From, resultSelectCommand.From);
            Assert.AreEqual(expectedSelectCommand.Where, resultSelectCommand.Where);
        }
    }
}