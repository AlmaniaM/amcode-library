using AMCode.Sql.Where.Internal;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Where.FilterBetweenConditionTests
{
    [TestFixture]
    public class FilterBetweenConditionTest
    {
        [Test]
        public void ShouldCreateAValidFilterBetweenCondition()
        {
            var filterBetweenCondition = new FilterBetweenCondition("TestFilter1", createValueBuilderMoq("1 AND 3").Object, string.Empty);
            Assert.AreEqual("TestFilter1 BETWEEN 1 AND 3", filterBetweenCondition.CreateFilterCondition());
        }

        [Test]
        public void ShouldCloneFilterBetweenCondition()
        {
            var filterBetweenCondition = new FilterBetweenCondition("TestFilter1", createValueBuilderMoq("1 AND 2").Object, string.Empty);
            var clone = filterBetweenCondition.Clone();

            Assert.AreEqual("TestFilter1 BETWEEN 1 AND 2", filterBetweenCondition.CreateFilterCondition());
            Assert.AreEqual("TestFilter1 BETWEEN 1 AND 2", clone.CreateFilterCondition());
        }

        [TestCase(true, ">=")]
        [TestCase(false, "<=")]
        public void ShouldCloneAndCreateAValidFilterBetweenConditionWithAlias(bool greaterThanOrEqualTo, string expectedComparitor)
        {
            var builderMoq = createValueBuilderMoq("1");
            builderMoq.SetupGet(moq => moq.Count).Returns(1);

            var filterBetweenCondition = new FilterBetweenCondition("TestFilter1", builderMoq.Object, "TestTable", greaterThanOrEqualTo);
            var clone = filterBetweenCondition.Clone();
            Assert.AreEqual($"TestTable.TestFilter1 {expectedComparitor} 1", clone.CreateFilterCondition());
        }

        [TestCase(true, ">=")]
        [TestCase(false, "<=")]
        public void ShouldCreateAValidFilterBetweenConditionWithAlias(bool greaterThanOrEqualTo, string expectedComparitor)
        {
            var builderMoq = createValueBuilderMoq("1");
            builderMoq.SetupGet(moq => moq.Count).Returns(1);

            var filterBetweenCondition = new FilterBetweenCondition("TestFilter1", builderMoq.Object, "TestTable", greaterThanOrEqualTo);
            Assert.AreEqual($"TestTable.TestFilter1 {expectedComparitor} 1", filterBetweenCondition.CreateFilterCondition());
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("  ")]
        public void ShouldCreateEmptyStringWhenInvalid(string invalidValue)
        {
            var filterBetweenCondition = new FilterBetweenCondition("TestFilter1", createValueBuilderMoq(invalidValue).Object, string.Empty);
            Assert.AreEqual(string.Empty, filterBetweenCondition.CreateFilterCondition());
        }

        [Test]
        public void ShouldBeAValidCommand()
        {
            var filterBetweenCondition = new FilterBetweenCondition("TestFilter1", createValueBuilderMoq("3").Object, string.Empty);
            Assert.IsTrue(filterBetweenCondition.IsValid);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("  ")]
        public void ShouldBeAnInvalidCommand(string invalidValue)
        {
            var filterBetweenCondition = new FilterBetweenCondition("TestFilter1", createValueBuilderMoq(invalidValue).Object, string.Empty);
            Assert.IsFalse(filterBetweenCondition.IsValid);
        }

        [Test]
        public void ShouldBeAnEmptyInvalidMessageStringForValidCommand()
        {
            var filterBetweenCondition = new FilterBetweenCondition("TestFilter1", createValueBuilderMoq("'2022-03-20'").Object, string.Empty);
            Assert.AreEqual(string.Empty, filterBetweenCondition.InvalidCommandMessage);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("  ")]
        public void ShouldCorrectInvalidCommandMessage(string invalidValue)
        {
            var filterBetweenCondition = new FilterBetweenCondition("TestFilter1", createValueBuilderMoq(invalidValue).Object, string.Empty);
            Assert.AreEqual($"No values provided for evaluation. Values are '{invalidValue}'.", filterBetweenCondition.InvalidCommandMessage);
        }

        /// <summary>
        /// Creates a default mock of the <see cref="IFilterConditionValueBuilder"/> interface.
        /// </summary>
        /// <param name="values">The value to return as the Filter IN (...values) section.</param>
        /// <returns>A <see cref="Mock{T}"/> of <see cref="IFilterConditionValueBuilder"/>.</returns>
        private Mock<IFilterConditionValueBuilder> createValueBuilderMoq(string values)
        {
            var valueBuilderMoq = new Mock<IFilterConditionValueBuilder>();
            valueBuilderMoq.Setup(moq => moq.CreateFilterConditionValue()).Returns(() => values);
            return valueBuilderMoq;
        }
    }
}