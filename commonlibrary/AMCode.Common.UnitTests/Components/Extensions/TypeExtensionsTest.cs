using System;
using AMCode.Common.Extensions.Types;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Extensions.TypeTests
{
    public enum TestTypeEnum
    {
        Testing
    };

    [TestFixture]
    public class TypeExtensionsTest
    {
        [Test]
        public void ShouldPassDateTypeTest()
        {
            Assert.IsTrue(DateTime.Now.GetType().IsDate());

            DateTime? date = DateTime.Now;
            Assert.IsTrue(date.GetType().IsDate());
        }

        [Test]
        public void ShouldNotRecognizeTypesAsDate()
        {
            Assert.IsFalse(typeof(byte).IsDate());
            Assert.IsFalse(typeof(sbyte).IsDate());
            Assert.IsFalse(typeof(int).IsDate());
            Assert.IsFalse(typeof(long).IsDate());
            Assert.IsFalse(typeof(double).IsDate());
            Assert.IsFalse(typeof(decimal).IsDate());
            Assert.IsFalse(typeof(string).IsDate());
            Assert.IsFalse(typeof(char).IsDate());
            Assert.IsFalse(typeof(bool).IsDate());
            Assert.IsFalse(typeof(float).IsDate());
            Assert.IsFalse(typeof(object).IsDate());
            Assert.IsFalse(typeof(short).IsDate());
            Assert.IsFalse(typeof(uint).IsDate());
            Assert.IsFalse(typeof(ulong).IsDate());
            Assert.IsFalse(typeof(ushort).IsDate());

            Assert.IsFalse(typeof(byte?).IsDate());
            Assert.IsFalse(typeof(sbyte?).IsDate());
            Assert.IsFalse(typeof(int?).IsDate());
            Assert.IsFalse(typeof(long?).IsDate());
            Assert.IsFalse(typeof(double?).IsDate());
            Assert.IsFalse(typeof(decimal?).IsDate());
            Assert.IsFalse(typeof(char?).IsDate());
            Assert.IsFalse(typeof(bool?).IsDate());
            Assert.IsFalse(typeof(float?).IsDate());
            Assert.IsFalse(typeof(short?).IsDate());
            Assert.IsFalse(typeof(uint?).IsDate());
            Assert.IsFalse(typeof(ulong?).IsDate());
            Assert.IsFalse(typeof(ushort?).IsDate());
        }

        [Test]
        public void ShouldDetectAllPrimitiveTypes()
        {
            Assert.IsTrue(typeof(byte).IsSimple());
            Assert.IsTrue(typeof(sbyte).IsSimple());
            Assert.IsTrue(typeof(int).IsSimple());
            Assert.IsTrue(typeof(long).IsSimple());
            Assert.IsTrue(typeof(double).IsSimple());
            Assert.IsTrue(typeof(decimal).IsSimple());
            Assert.IsTrue(typeof(string).IsSimple());
            Assert.IsTrue(typeof(char).IsSimple());
            Assert.IsTrue(typeof(bool).IsSimple());
            Assert.IsTrue(typeof(float).IsSimple());
            Assert.IsTrue(typeof(short).IsSimple());
            Assert.IsTrue(typeof(uint).IsSimple());
            Assert.IsTrue(typeof(ulong).IsSimple());
            Assert.IsTrue(typeof(ushort).IsSimple());
            Assert.IsTrue(typeof(byte?).IsSimple());
            Assert.IsTrue(typeof(sbyte?).IsSimple());
            Assert.IsTrue(typeof(int?).IsSimple());
            Assert.IsTrue(typeof(long?).IsSimple());
            Assert.IsTrue(typeof(double?).IsSimple());
            Assert.IsTrue(typeof(decimal?).IsSimple());
            Assert.IsTrue(typeof(char?).IsSimple());
            Assert.IsTrue(typeof(bool?).IsSimple());
            Assert.IsTrue(typeof(float?).IsSimple());
            Assert.IsTrue(typeof(short?).IsSimple());
            Assert.IsTrue(typeof(uint?).IsSimple());
            Assert.IsTrue(typeof(ulong?).IsSimple());
            Assert.IsTrue(typeof(ushort?).IsSimple());
            Assert.IsTrue(TestTypeEnum.Testing.GetType().IsSimple());
        }

        [Test]
        public void ShouldDetectNonPrimitiveTypes()
        {
            Assert.IsFalse(typeof(object).IsSimple());
            Assert.IsFalse(typeof(Func<string, int>).IsSimple());
            Assert.IsFalse(typeof(Action).IsSimple());
            Assert.IsFalse(typeof(Action<string>).IsSimple());
        }
    }
}