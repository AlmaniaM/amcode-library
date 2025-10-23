using AMCode.Sql.Where.Internal;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Where.FilterIsConditionTests
{
    [TestFixture]
    public class FilterIsConditionTest
    {
        [Test]
        public void ShouldCreateAValidFilterIsCondition()
        {
            var filterIsCondition = new FilterIsCondition("TestFilter1", () => "NOT NULL", string.Empty);
            Assert.AreEqual("TestFilter1 IS NOT NULL", filterIsCondition.CreateFilterCondition());
        }

        [Test]
        public void ShouldCloneFilterIsCondition()
        {
            var filterIsCondition = new FilterIsCondition("TestFilter1", () => "NOT NULL", string.Empty);
            var clone = filterIsCondition.Clone();

            Assert.AreEqual("TestFilter1 IS NOT NULL", filterIsCondition.CreateFilterCondition());
            Assert.AreEqual("TestFilter1 IS NOT NULL", clone.CreateFilterCondition());
        }

        [Test]
        public void ShouldCreateAValidFilterIsConditionWithAlias()
        {
            var filterIsCondition = new FilterIsCondition("TestFilter1", () => "NOT NULL", "TestTable");
            Assert.AreEqual("TestTable.TestFilter1 IS NOT NULL", filterIsCondition.CreateFilterCondition());
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("  ")]
        public void ShouldCreateEmptyStringWhenInvalid(string invalidValue)
        {
            var filterIsCondition = new FilterIsCondition("TestFilter1", () => invalidValue, string.Empty);
            Assert.AreEqual(string.Empty, filterIsCondition.CreateFilterCondition());
        }

        [Test]
        public void ShouldBeAValidCommand()
        {
            var filterIsCondition = new FilterIsCondition("TestFilter1", () => "NOT NULL", string.Empty);
            Assert.IsTrue(filterIsCondition.IsValid);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("  ")]
        public void ShouldBeAnInvalidCommand(string invalidValue)
        {
            var filterIsCondition = new FilterIsCondition("TestFilter1", () => invalidValue, string.Empty);
            Assert.IsFalse(filterIsCondition.IsValid);
        }

        [Test]
        public void ShouldBeAnEmptyInvalidMessageStringForValidCommand()
        {
            var filterIsCondition = new FilterIsCondition("TestFilter1", () => "NOT NULL", string.Empty);
            Assert.AreEqual(string.Empty, filterIsCondition.InvalidCommandMessage);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("  ")]
        public void ShouldCorrectInvalidCommandMessage(string invalidValue)
        {
            var filterIsCondition = new FilterIsCondition("TestFilter1", () => invalidValue, string.Empty);
            Assert.AreEqual($"No value provided for evaluation. Value is '{invalidValue}'.", filterIsCondition.InvalidCommandMessage);
        }
    }
}