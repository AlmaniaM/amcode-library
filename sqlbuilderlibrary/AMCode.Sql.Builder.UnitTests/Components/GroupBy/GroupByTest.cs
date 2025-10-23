using System.Collections.Generic;
using System.Linq;
using AMCode.Sql.Commands.Models;
using AMCode.Sql.GroupBy;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.GroupBy.GroupByClauseTests
{
    [TestFixture]
    public class GroupByTest
    {
        [Test]
        public void ShouldCreateGroupByClauseWithPrimaryColumnsOnly()
        {
            var primaryColumnOne = new TestDataQueryColumnDefinition("PrimaryColumnOne", true, true, true);
            var secondaryColumnOne = new TestDataQueryColumnDefinition("SecondaryColumnOne", false, true, false);
            var primaryColumnTwo = new TestDataQueryColumnDefinition("PrimaryColumnTwo", true, true, true);
            var secondaryColumnTwo = new TestDataQueryColumnDefinition("SecondaryColumnTwo", false, true, false);

            var listOfColumns = new List<IDataQueryColumnDefinition>
            {
                primaryColumnOne,
                secondaryColumnOne,
                primaryColumnTwo,
                secondaryColumnTwo
            };

            var groupByClause = new GroupByClause().CreateClause(listOfColumns.Cast<IGroupable>(), true).ToString();

            Assert.AreEqual("GROUP BY PrimaryColumnOne, PrimaryColumnTwo", groupByClause);
        }

        [Test]
        public void ShouldCreateGroupByClauseWithAllColumns()
        {
            var secondaryColumnOneMoq = new Mock<IDataQueryColumnDefinition>();
            var primaryColumnOneMoq = setUpPrimaryColumn(new Mock<IDataQueryColumnDefinition>(), "PrimaryColumnOne", secondaryColumnOneMoq, true);
            secondaryColumnOneMoq = setUpSecondaryColumn(secondaryColumnOneMoq, "SecondaryColumnOne", true);

            var secondaryColumnTwoMoq = new Mock<IDataQueryColumnDefinition>();
            var primaryColumnTwoMoq = setUpPrimaryColumn(new Mock<IDataQueryColumnDefinition>(), "PrimaryColumnTwo", secondaryColumnTwoMoq, true);
            secondaryColumnTwoMoq = setUpSecondaryColumn(secondaryColumnTwoMoq, "SecondaryColumnTwo", true);

            var listOfColumns = new List<IDataQueryColumnDefinition>
            {
                primaryColumnOneMoq.Object,
                secondaryColumnOneMoq.Object,
                primaryColumnTwoMoq.Object,
                secondaryColumnTwoMoq.Object
            };

            var groupByClause = new GroupByClause().CreateClause(listOfColumns, false).ToString();

            Assert.AreEqual("GROUP BY PrimaryColumnOne, SecondaryColumnOne, PrimaryColumnTwo, SecondaryColumnTwo", groupByClause);
        }

        [Test]
        public void ShouldNotCreateGroupByClauseWithAllColumnsWhenNotSelected()
        {
            var secondaryColumnOneMoq = new Mock<IDataQueryColumnDefinition>();
            var primaryColumnOneMoq = setUpPrimaryColumn(new Mock<IDataQueryColumnDefinition>(), "PrimaryColumnOne", secondaryColumnOneMoq, false);
            secondaryColumnOneMoq = setUpSecondaryColumn(secondaryColumnOneMoq, "SecondaryColumnOne", false);

            var secondaryColumnTwoMoq = new Mock<IDataQueryColumnDefinition>();
            var primaryColumnTwoMoq = setUpPrimaryColumn(new Mock<IDataQueryColumnDefinition>(), "PrimaryColumnTwo", secondaryColumnTwoMoq, false);
            secondaryColumnTwoMoq = setUpSecondaryColumn(secondaryColumnTwoMoq, "SecondaryColumnTwo", false);

            var listOfColumns = new List<IDataQueryColumnDefinition>
            {
                primaryColumnOneMoq.Object,
                secondaryColumnOneMoq.Object,
                primaryColumnTwoMoq.Object,
                secondaryColumnTwoMoq.Object
            };

            var groupByClause = new GroupByClause().CreateClause(listOfColumns, false).ToString();

            Assert.AreEqual(string.Empty, groupByClause);
        }

        private static Mock<IDataQueryColumnDefinition> setUpPrimaryColumn(Mock<IDataQueryColumnDefinition> columnMoq, string name, Mock<IDataQueryColumnDefinition> secondaryMoq, bool selected)
        {
            columnMoq.Setup(moq => moq.CurrentColumnDefinition()).Returns(() => columnMoq.Object);
            columnMoq.Setup(moq => moq.NextColumnDefinition()).Returns(() => secondaryMoq.Object);
            columnMoq.Setup(moq => moq.IsPrimaryColumn()).Returns(true);
            columnMoq.SetupGet(moq => moq.IsGroupable).Returns(true);
            columnMoq.SetupGet(moq => moq.IsVisible).Returns(selected);
            columnMoq.Setup(moq => moq.FieldName()).Returns(name);
            return columnMoq;
        }

        private static Mock<IDataQueryColumnDefinition> setUpSecondaryColumn(Mock<IDataQueryColumnDefinition> columnMoq, string name, bool selected)
        {
            columnMoq.Setup(moq => moq.CurrentColumnDefinition()).Returns(() => columnMoq.Object);
            columnMoq.Setup(moq => moq.NextColumnDefinition()).Returns(() => null);
            columnMoq.Setup(moq => moq.IsPrimaryColumn()).Returns(false);
            columnMoq.SetupGet(moq => moq.IsGroupable).Returns(true);
            columnMoq.SetupGet(moq => moq.IsVisible).Returns(selected);
            columnMoq.Setup(moq => moq.FieldName()).Returns(name);
            return columnMoq;
        }

        private static Mock<IDataQueryColumnDefinition> setUpNonGroupablePrimaryColumn(Mock<IDataQueryColumnDefinition> columnMoq, string name, Mock<IDataQueryColumnDefinition> secondaryMoq, bool selected)
        {
            columnMoq = setUpPrimaryColumn(columnMoq, name, secondaryMoq, selected);
            columnMoq.SetupGet(moq => moq.IsGroupable).Returns(() => false);
            return columnMoq;
        }
    }

    public class TestDataQueryColumnDefinition : IDataQueryColumnDefinition
    {
        private readonly string _fieldName;
        private readonly bool _isPrimaryColumn;
        private readonly bool _isGroupable;
        private readonly bool _isVisible;

        public TestDataQueryColumnDefinition(string fieldName, bool isPrimaryColumn, bool isGroupable, bool isVisible)
        {
            _fieldName = fieldName;
            _isPrimaryColumn = isPrimaryColumn;
            _isGroupable = isGroupable;
            _isVisible = isVisible;
        }

        public string FieldName() => _fieldName;
        public bool IsSelected() => true;
        public bool IsVisible => _isVisible;
        public IDataQueryColumnDefinition NextColumnDefinition() => null;
        public string DisplayName => _fieldName;
        public bool IsGroupable => _isGroupable;
        public bool IsPrimaryColumn() => _isPrimaryColumn;
        public IDataQueryColumnDefinition CurrentColumnDefinition() => this;
    }
}