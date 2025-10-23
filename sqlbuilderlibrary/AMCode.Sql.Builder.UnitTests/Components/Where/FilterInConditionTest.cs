using AMCode.Sql.Where.Internal;
using Moq;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Where.FilterInConditionTests
{
    [TestFixture]
    public class FilterInConditionTest
    {
        [Test]
        public void ShouldCreateAValidFilterIsCondition()
        {
            var filterInCondition = new FilterInCondition("TestFilter1", createValueBuilderMoq("1, 2, 3").Object, string.Empty);
            Assert.AreEqual("TestFilter1 IN (1, 2, 3)", filterInCondition.CreateFilterCondition());
        }

        [Test]
        public void ShouldCloneFilterInCondition()
        {
            var filterInCondition = new FilterInCondition("TestFilter1", createValueBuilderMoq("1, 2, 3").Object, string.Empty);
            var clone = filterInCondition.Clone();

            Assert.AreEqual("TestFilter1 IN (1, 2, 3)", filterInCondition.CreateFilterCondition());
            Assert.AreEqual("TestFilter1 IN (1, 2, 3)", clone.CreateFilterCondition());
        }

        [Test]
        public void ShouldCreateAValidFilterInConditionWithAlias()
        {
            var filterInCondition = new FilterInCondition("TestFilter1", createValueBuilderMoq("1, 2, 3").Object, "TestTable");
            Assert.AreEqual("TestTable.TestFilter1 IN (1, 2, 3)", filterInCondition.CreateFilterCondition());
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("  ")]
        public void ShouldCreateEmptyStringWhenInvalid(string invalidValue)
        {
            var filterInCondition = new FilterInCondition("TestFilter1", createValueBuilderMoq(invalidValue).Object, string.Empty);
            Assert.AreEqual(string.Empty, filterInCondition.CreateFilterCondition());
        }

        [Test]
        public void ShouldBeAValidCommand()
        {
            var filterInCondition = new FilterInCondition("TestFilter1", createValueBuilderMoq("1, 2, 3").Object, string.Empty);
            Assert.IsTrue(filterInCondition.IsValid);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("  ")]
        public void ShouldBeAnInvalidCommand(string invalidValue)
        {
            var filterInCondition = new FilterInCondition("TestFilter1", createValueBuilderMoq(invalidValue).Object, string.Empty);
            Assert.IsFalse(filterInCondition.IsValid);
        }

        [Test]
        public void ShouldBeAnEmptyInvalidMessageStringForValidCommand()
        {
            var filterInCondition = new FilterInCondition("TestFilter1", createValueBuilderMoq("1, 2, 3").Object, string.Empty);
            Assert.AreEqual(string.Empty, filterInCondition.InvalidCommandMessage);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("  ")]
        public void ShouldCorrectInvalidCommandMessage(string invalidValue)
        {
            var filterInCondition = new FilterInCondition("TestFilter1", createValueBuilderMoq(invalidValue).Object, string.Empty);
            Assert.AreEqual($"No values provided for evaluation. Values are '{invalidValue}'.", filterInCondition.InvalidCommandMessage);
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