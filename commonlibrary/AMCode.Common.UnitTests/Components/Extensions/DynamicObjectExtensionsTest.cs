using System.Collections.Generic;
using AMCode.Common.Dynamic;
using AMCode.Common.Extensions.ExpandoObjects;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Extensions.DynamicObjectExtensionsTests
{
    [TestFixture]
    public class DynamicObjectExtensionsTest
    {
        [Test]
        public void ShouldGetBooleanValue()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestProperty", true);
            expando.AddOrUpdatePropertyWithValue("TestStringProperty", "true");
            expando.AddOrUpdatePropertyWithValue("TestNullProperty", null);

            Assert.AreEqual(true, expando.GetValue<bool>("TestProperty"));
            Assert.AreEqual(true, expando.GetValue<bool?>("TestProperty"));
            Assert.AreEqual(true, expando.GetValue<bool>("TestStringProperty"));
            Assert.AreEqual(true, expando.GetValue<bool?>("TestStringProperty"));
            Assert.AreEqual(null, expando.GetValue<bool?>("TestNullProperty"));
        }

        [Test]
        public void ShouldGetCharValue()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestProperty", 'a');
            expando.AddOrUpdatePropertyWithValue("TestStringProperty", "b");
            expando.AddOrUpdatePropertyWithValue("TestNullProperty", null);

            Assert.AreEqual('a', expando.GetValue<char>("TestProperty"));
            Assert.AreEqual('a', expando.GetValue<char?>("TestProperty"));
            Assert.AreEqual('b', expando.GetValue<char>("TestStringProperty"));
            Assert.AreEqual('b', expando.GetValue<char?>("TestStringProperty"));
            Assert.AreEqual(null, expando.GetValue<char?>("TestNullProperty"));
        }

        [Test]
        public void ShouldGetDoubleValue()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestProperty", 100.10);
            expando.AddOrUpdatePropertyWithValue("TestStringProperty", "1.20");
            expando.AddOrUpdatePropertyWithValue("TestNullProperty", null);

            Assert.AreEqual(100.10, expando.GetValue<double>("TestProperty"));
            Assert.AreEqual(100.10, expando.GetValue<double?>("TestProperty"));
            Assert.AreEqual(1.20, expando.GetValue<double>("TestStringProperty"));
            Assert.AreEqual(1.20, expando.GetValue<double?>("TestStringProperty"));
            Assert.AreEqual(null, expando.GetValue<double?>("TestNullProperty"));
        }

        [Test]
        public void ShouldGetFloatValue()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestProperty", 100f);
            expando.AddOrUpdatePropertyWithValue("TestStringProperty", "1");
            expando.AddOrUpdatePropertyWithValue("TestNullProperty", null);

            Assert.AreEqual(100f, expando.GetValue<float>("TestProperty"));
            Assert.AreEqual(100f, expando.GetValue<float?>("TestProperty"));
            Assert.AreEqual(1f, expando.GetValue<float>("TestStringProperty"));
            Assert.AreEqual(1f, expando.GetValue<float?>("TestStringProperty"));
            Assert.AreEqual(null, expando.GetValue<float?>("TestNullProperty"));
        }

        [Test]
        public void ShouldGetIntValue()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestProperty", 100);
            expando.AddOrUpdatePropertyWithValue("TestStringProperty", "1");
            expando.AddOrUpdatePropertyWithValue("TestNullProperty", null);

            Assert.AreEqual(100, expando.GetValue<int>("TestProperty"));
            Assert.AreEqual(100, expando.GetValue<int?>("TestProperty"));
            Assert.AreEqual(1, expando.GetValue<int>("TestStringProperty"));
            Assert.AreEqual(1, expando.GetValue<int?>("TestStringProperty"));
            Assert.AreEqual(null, expando.GetValue<int?>("TestNullProperty"));
        }

        [Test]
        public void ShouldGetStringValue()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestProperty", "Test Value");
            expando.AddOrUpdatePropertyWithValue("TestStringProperty", "Test Value 2");
            expando.AddOrUpdatePropertyWithValue("TestNullProperty", null);

            Assert.AreEqual("Test Value", expando.GetValue<string>("TestProperty"));
            Assert.AreEqual("Test Value 2", expando.GetValue<string>("TestStringProperty"));
            Assert.AreEqual(null, expando.GetValue<string>("TestNullProperty"));
        }

        [Test]
        public void ShouldTryGetBooleanValue()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestProperty", true);
            expando.AddOrUpdatePropertyWithValue("TestStringProperty", "true");
            expando.AddOrUpdatePropertyWithValue("TestNullProperty", null);

            Assert.IsTrue(expando.TryGetValue("TestProperty", out bool testValue));
            Assert.AreEqual(true, testValue);

            Assert.IsTrue(expando.TryGetValue("TestProperty", out bool? testNullableValue));
            Assert.AreEqual(true, testNullableValue);

            Assert.IsTrue(expando.TryGetValue("TestStringProperty", out bool testStrValue));
            Assert.AreEqual(true, testStrValue);

            Assert.IsTrue(expando.TryGetValue("TestStringProperty", out bool? testStrNullable));
            Assert.AreEqual(true, testStrNullable);

            Assert.IsFalse(expando.TryGetValue("TestNullProperty", out bool? testNullValue));
            Assert.AreEqual(null, testNullValue);
        }

        [Test]
        public void ShouldTryGetCharValue()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestProperty", 'a');
            expando.AddOrUpdatePropertyWithValue("TestStringProperty", "b");
            expando.AddOrUpdatePropertyWithValue("TestNullProperty", null);

            Assert.IsTrue(expando.TryGetValue("TestProperty", out char testValue));
            Assert.AreEqual('a', testValue);

            Assert.IsTrue(expando.TryGetValue("TestProperty", out char? testNullableValue));
            Assert.AreEqual('a', testNullableValue);

            Assert.IsTrue(expando.TryGetValue("TestStringProperty", out char testStrValue));
            Assert.AreEqual('b', testStrValue);

            Assert.IsTrue(expando.TryGetValue("TestStringProperty", out char? testStrNullable));
            Assert.AreEqual('b', testStrNullable);

            Assert.IsFalse(expando.TryGetValue("TestNullProperty", out char? testNullValue));
            Assert.AreEqual(null, testNullValue);
        }

        [Test]
        public void ShouldTryGetDoubleValue()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestProperty", 100.10);
            expando.AddOrUpdatePropertyWithValue("TestStringProperty", "1.20");
            expando.AddOrUpdatePropertyWithValue("TestNullProperty", null);

            Assert.IsTrue(expando.TryGetValue("TestProperty", out double testValue));
            Assert.AreEqual(100.10, testValue);

            Assert.IsTrue(expando.TryGetValue("TestProperty", out double? testNullableValue));
            Assert.AreEqual(100.10, testNullableValue);

            Assert.IsTrue(expando.TryGetValue("TestStringProperty", out double testStrValue));
            Assert.AreEqual(1.20, testStrValue);

            Assert.IsTrue(expando.TryGetValue("TestStringProperty", out double? testStrNullable));
            Assert.AreEqual(1.20, testStrNullable);

            Assert.IsFalse(expando.TryGetValue("TestNullProperty", out double? testNullValue));
            Assert.AreEqual(null, testNullValue);
        }

        [Test]
        public void ShouldTryGetFloatValue()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestProperty", 100f);
            expando.AddOrUpdatePropertyWithValue("TestStringProperty", "1");
            expando.AddOrUpdatePropertyWithValue("TestNullProperty", null);

            Assert.IsTrue(expando.TryGetValue("TestProperty", out float testValue));
            Assert.AreEqual(100f, testValue);

            Assert.IsTrue(expando.TryGetValue("TestProperty", out float? testNullableValue));
            Assert.AreEqual(100f, testNullableValue);

            Assert.IsTrue(expando.TryGetValue("TestStringProperty", out float testStrValue));
            Assert.AreEqual(1f, testStrValue);

            Assert.IsTrue(expando.TryGetValue("TestStringProperty", out float? testStrNullable));
            Assert.AreEqual(1f, testStrNullable);

            Assert.IsFalse(expando.TryGetValue("TestNullProperty", out float? testNullValue));
            Assert.AreEqual(null, testNullValue);
        }

        [Test]
        public void ShouldTryGetIntValue()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestProperty", 100);
            expando.AddOrUpdatePropertyWithValue("TestStringProperty", "1");
            expando.AddOrUpdatePropertyWithValue("TestNullProperty", null);

            Assert.IsTrue(expando.TryGetValue("TestProperty", out int testValue));
            Assert.AreEqual(100, testValue);

            Assert.IsTrue(expando.TryGetValue("TestProperty", out int? testNullableValue));
            Assert.AreEqual(100, testNullableValue);

            Assert.IsTrue(expando.TryGetValue("TestStringProperty", out int testStrValue));
            Assert.AreEqual(1, testStrValue);

            Assert.IsTrue(expando.TryGetValue("TestStringProperty", out int? testStrNullable));
            Assert.AreEqual(1, testStrNullable);

            Assert.IsFalse(expando.TryGetValue("TestNullProperty", out int? testNullValue));
            Assert.AreEqual(null, testNullValue);
        }

        [Test]
        public void ShouldTryGetStringValue()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestProperty", "Test Value");
            expando.AddOrUpdatePropertyWithValue("TestStringProperty", "Test Value 2");
            expando.AddOrUpdatePropertyWithValue("TestNullProperty", null);

            Assert.IsTrue(expando.TryGetValue("TestProperty", out string testPropertyValue));
            Assert.AreEqual("Test Value", testPropertyValue);

            Assert.IsTrue(expando.TryGetValue("TestStringProperty", out string testStringPropertyValue));
            Assert.AreEqual("Test Value 2", testStringPropertyValue);

            Assert.IsFalse(expando.TryGetValue("TestNullProperty", out string testNullValue));
            Assert.AreEqual(null, testNullValue);
        }

        [Test]
        public void ShouldGetValuesFromExpandoObject()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestValue1", "Test Value 1");
            expando.AddOrUpdatePropertyWithValue("TestValue2", "Test Value 2");

            Assert.AreEqual(2, expando.Values().Count);
            Assert.AreEqual("Test Value 1", expando.Values()[0]);
            Assert.AreEqual("Test Value 2", expando.Values()[1]);
        }

        [Test]
        public void ShouldGetKeysFromExpandoObject()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestValue1", "Test Value 1");
            expando.AddOrUpdatePropertyWithValue("TestValue2", "Test Value 2");

            Assert.AreEqual(2, expando.Keys().Count);
            Assert.AreEqual("TestValue1", expando.Keys()[0]);
            Assert.AreEqual("TestValue2", expando.Keys()[1]);
        }

        [Test]
        public void ShouldRemoveKeyFromExpandoObject()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestValue1", "Test Value 1");
            expando.AddOrUpdatePropertyWithValue("TestValue2", "Test Value 2");

            Assert.AreEqual(2, expando.Keys().Count);
            expando.Remove("TestValue1");

            Assert.AreEqual(1, expando.Keys().Count);
            Assert.IsNull(expando.GetValue<string>("TestValue1"));
        }

        [Test]
        public void ShouldCopyExpandoObject()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestValue1", "Test Value 1");
            expando.AddOrUpdatePropertyWithValue("TestValue2", "Test Value 2");

            Assert.AreEqual(2, expando.Values().Count);
            Assert.AreEqual("Test Value 1", expando.Values()[0]);
            Assert.AreEqual("Test Value 2", expando.Values()[1]);

            var expandoCopy = expando.Copy();
            Assert.AreEqual(2, expandoCopy.Values().Count);
            Assert.AreEqual("Test Value 1", expandoCopy.Values()[0]);
            Assert.AreEqual("Test Value 2", expandoCopy.Values()[1]);
            Assert.AreEqual(2, expandoCopy.Keys().Count);
            Assert.AreEqual("TestValue1", expandoCopy.Keys()[0]);
            Assert.AreEqual("TestValue2", expandoCopy.Keys()[1]);
        }

        [Test]
        public void ShouldConvertExpandoObjectToDictionary()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestProperty", 10);
            expando.AddOrUpdatePropertyWithValue("TestProperty2", 20);

            var dictionary = expando.ToDictionary();

            Assert.AreSame(typeof(Dictionary<string, object>), dictionary.GetType());
            Assert.AreEqual(2, dictionary.Keys.Count);
            dictionary.TryGetValue("TestProperty", out var value);
            Assert.AreEqual(10, (int)value);
        }
    }
}