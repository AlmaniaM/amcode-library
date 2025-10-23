using System;
using AMCode.Common.UnitTests.Util.Mocks;
using NUnit.Framework;
using static AMCode.Common.Util.MethodInfoUtil;

namespace AMCode.Common.UnitTests.Util.MethodInfoTests
{
    [TestFixture]
    public class MethodInfoTest
    {
        private readonly string classAndMethod = $"[{nameof(MethodTemplateMock)}][{nameof(MethodTemplateMock.TestMethod)}]";
        private readonly MethodTemplateMock testClass = new();

        [Test]
        public void ShouldGetMethodInfoUsingFunc()
        {
            var methodInfo = typeof(MethodTemplateMock).GetMethod("TestMethod", new Type[] { typeof(string) });

            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo(testClass.TestMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<string, string>(testClass.TestMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<string, string, string>(testClass.TestMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<string, string, string, string>(testClass.TestMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<string, string, string, string, string>(testClass.TestMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<string, string, string, string, string, string>(testClass.TestMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<string, string, string, string, string, string, string>(testClass.TestMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<string, string, string, string, string, string, string, string>(testClass.TestMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<string, string, string, string, string, string, string, string, string>(testClass.TestMethod).GetType());
        }

        [Test]
        public void ShouldGetMethodInfoUsingAction()
        {
            var methodInfo = typeof(MethodTemplateMock).GetMethod("TestVoidMethod", new Type[] { typeof(int) });

            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo(testClass.TestVoidMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<int>(testClass.TestVoidMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<int, int>(testClass.TestVoidMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<int, int, int>(testClass.TestVoidMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<int, int, int, int>(testClass.TestVoidMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<int, int, int, int, int>(testClass.TestVoidMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<int, int, int, int, int, int>(testClass.TestVoidMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<int, int, int, int, int, int, int>(testClass.TestVoidMethod).GetType());
            Assert.AreEqual(methodInfo.GetType(), GetMethodInfo<int, int, int, int, int, int, int, int>(testClass.TestVoidMethod).GetType());
        }
    }
}