using System.Collections.Generic;
using AMCode.Common.Extensions.Objects;
using AMCode.Common.UnitTests.Extensions.Mocks;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Extensions.ObjectExtensionsTests
{
    [TestFixture]
    public class ObjectExtensionsTest
    {
        [Test]
        public void ShouldConvertSingleValuePropertyClass()
        {
            IEmptyInterface emptyInterface = new SingleValueProperty
            {
                Value = "Test Value"
            };

            Assert.IsTrue(emptyInterface.Is(out ISingleValueProperty singleValueProperty));
            Assert.IsNotNull(singleValueProperty);
        }

        [Test]
        public void ShouldNotConvertDoubleValuePropertyClass()
        {
            IEmptyInterface emptyInterface = new SingleValueProperty
            {
                Value = "Test Value"
            };

            Assert.IsFalse(emptyInterface.Is(out IDoubleValueProperty doubleValueProperty));
            Assert.IsNull(doubleValueProperty);
        }

        [Test]
        public void ShouldDeepCopyObjectAndRetainAllInformationAndFunctionalityOfObject()
        {
            var testObject = new CloneObjectTestMain
            {
                AdditionObject = new CloneObjectTestAddition
                {
                    Value1 = 1,
                    Value2 = 2
                },
                SubtractValue1 = 20,
                SubtractValue2 = 10,
                IntValue = 100,
                StringValue = "StringTestValues"
            };

            var newTestObject = testObject.DeepCopy();

            Assert.AreEqual("100 StringTestValues", newTestObject.ToString());
            Assert.AreEqual(3, newTestObject.AdditionObject.AddValues());
            Assert.AreEqual(10, newTestObject.Subtract());
        }

        [Test]
        public void ShouldDeepCopyObjectAndRetainAllInformationAndFunctionalityOfObjectUsingInterface()
        {
            var testObject = new CloneObjectTestMain
            {
                AdditionObject = new CloneObjectTestAddition
                {
                    Value1 = 1,
                    Value2 = 2
                },
                SubtractValue1 = 20,
                SubtractValue2 = 10,
                IntValue = 100,
                StringValue = "StringTestValues"
            };

            var newTestObject = testObject.DeepCopy<ICloneObjectTestMain, CloneObjectTestMain>();

            Assert.AreEqual("100 StringTestValues", newTestObject.ToString());
            Assert.AreEqual(3, newTestObject.AdditionObject.AddValues());
            Assert.AreEqual(10, newTestObject.Subtract());
        }

        [Test]
        public void ShouldDeepCopyCollection()
        {
            var testCollection = new List<string>
            {
                "Test",
                "Value"
            };

            var newTestCollection = testCollection.DeepCopy();

            Assert.AreEqual(2, newTestCollection.Count);
            Assert.AreEqual("Test", newTestCollection[0]);
            Assert.AreEqual("Value", newTestCollection[1]);
        }

        [Test]
        public void ShouldDeepCopyWithInterfaceCollection()
        {
            var testCollection = new List<string>
            {
                "Test",
                "Value"
            };

            var newTestCollection = testCollection.DeepCopy<IList<string>, List<string>>();

            Assert.AreEqual(2, newTestCollection.Count);
            Assert.AreEqual("Test", newTestCollection[0]);
            Assert.AreEqual("Value", newTestCollection[1]);
        }

        [TestCase("TestValue")]
        [TestCase(100)]
        [TestCase(200.50)]
        [TestCase(true)]
        public void ShouldDeepCopyObjects(object value) => Assert.AreEqual(value, value.DeepCopy());

        [Test]
        public void ShouldDeepCopyChar() => Assert.AreEqual('a', 'a'.DeepCopy());
    }
}