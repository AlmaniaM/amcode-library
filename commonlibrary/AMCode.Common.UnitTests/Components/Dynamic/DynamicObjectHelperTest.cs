using System.Collections.Generic;
using System.Dynamic;
using AMCode.Common.Dynamic;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.UnitTests.Dynamic.Models;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Dynamic.DynamicObjectHelperTests
{
    [TestFixture]
    public class DynamicObjectHelperTest
    {
        private TestHelper testHelper;

        [SetUp]
        public void SetUp() => testHelper = new TestHelper();

        [Test]
        public void ShouldCreateExpandoObjectFromIDataReader()
        {
            var filePath = testHelper.GetFilePath("DynamicObjectHelperTest_One_Row_No_Space_In_Headers.csv", TestContext.CurrentContext);
            using var dataReader = new CSVDataReader(filePath);

            dataReader.Read();

            var expando = DynamicObjectHelper.BuildExpandoFromRow(dataReader);

            Assert.AreSame(typeof(ExpandoObject), expando.GetType());

            Assert.AreEqual(3, expando.Keys().Count);
            Assert.AreEqual(3, expando.Values().Count);

            Assert.AreEqual(1, expando.GetValue<int>("HeaderOne"));
            Assert.AreEqual(2, expando.GetValue<int>("HeaderTwo"));
            Assert.AreEqual(3, expando.GetValue<int>("HeaderThree"));
        }

        [Test]
        public void ShouldCreateExpandoObjectFromIDataReaderWithColumns()
        {
            var columnNames = new string[] { "ColumnOne", "ColumnTwo", "ColumnThree" };
            var filePath = testHelper.GetFilePath("DynamicObjectHelperTest_One_Row_No_Space_In_Headers.csv", TestContext.CurrentContext);
            using var dataReader = new CSVDataReader(filePath);

            dataReader.Read();

            var expando = DynamicObjectHelper.BuildExpandoFromRow(dataReader, columnNames);

            Assert.AreSame(typeof(ExpandoObject), expando.GetType());

            Assert.AreEqual(3, expando.Keys().Count);
            Assert.AreEqual(3, expando.Values().Count);

            Assert.AreEqual(1, expando.GetValue<int>("ColumnOne"));
            Assert.AreEqual(2, expando.GetValue<int>("ColumnTwo"));
            Assert.AreEqual(3, expando.GetValue<int>("ColumnThree"));
        }

        [Test]
        public void ShouldCreateExpandoObjectFromValues()
        {
            var stringExpando = DynamicObjectHelper.CreateExpandoObject("TestProperty", "Test Value");
            Assert.AreSame(typeof(ExpandoObject), stringExpando.GetType());
            Assert.AreEqual("Test Value", ((dynamic)stringExpando).TestProperty);

            var intExpando = DynamicObjectHelper.CreateExpandoObject("TestProperty", int.MaxValue);
            Assert.AreSame(typeof(ExpandoObject), intExpando.GetType());
            Assert.AreEqual(int.MaxValue, ((dynamic)intExpando).TestProperty);

            var doubleExpando = DynamicObjectHelper.CreateExpandoObject("TestProperty", double.MaxValue);
            Assert.AreSame(typeof(ExpandoObject), doubleExpando.GetType());
            Assert.AreEqual(double.MaxValue, ((dynamic)doubleExpando).TestProperty);

            var expandoExpando = DynamicObjectHelper.CreateExpandoObject("Test Property", stringExpando);
            Assert.AreSame(typeof(ExpandoObject), expandoExpando.GetType());
            Assert.AreSame(stringExpando, expandoExpando.GetValue<ExpandoObject>("Test Property"));
        }

        [Test]
        public void ShouldConvertExpandoObjectToDictionary()
        {
            var expando = DynamicObjectHelper.CreateExpandoObject("TestProperty", 10);
            expando.AddOrUpdatePropertyWithValue("TestProperty2", 20);

            var dictionary = DynamicObjectHelper.ExpandoToDictionary(expando);

            Assert.AreSame(typeof(Dictionary<string, object>), dictionary.GetType());
            Assert.AreEqual(2, dictionary.Keys.Count);
            dictionary.TryGetValue("TestProperty", out var value);
            Assert.AreEqual(10, (int)value);
        }
    }
}