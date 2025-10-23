using AMCode.Common.Extensions.ReflectionMethodInfo;
using AMCode.Common.UnitTests.Extensions.MethodInfoTests.Mocks;
using NUnit.Framework;
using static AMCode.Common.Util.MethodInfoUtil;

namespace AMCode.Common.UnitTests.Extensions.MethodInfoTests
{
    [TestFixture]
    public class MethodInfoExtensionsTest
    {
        [Test]
        public void ShouldBuildExceptionHeaderNoParam()
        {
            var headerVoidResult = $"[{nameof(MethodInfoTestClass)}][{nameof(MethodInfoTestClass.TestVoidMethod)}]()";
            var headerStringResult = $"[{nameof(MethodInfoTestClass)}][{nameof(MethodInfoTestClass.TestMethod)}]()";

            Assert.AreEqual(headerVoidResult, GetMethodInfo(new MethodInfoTestClass().TestVoidMethod).CreateExceptionHeader());
            Assert.AreEqual(headerStringResult, GetMethodInfo(new MethodInfoTestClass().TestMethod).CreateExceptionHeader());
        }

        [Test]
        public void ShouldBuildExceptionHeaderWithParams()
        {
            var headerVoidResult = $"[{nameof(MethodInfoTestClass)}][{nameof(MethodInfoTestClass.TestVoidMethod)}](String p1, String p2)";
            var headerStringResult = $"[{nameof(MethodInfoTestClass)}][{nameof(MethodInfoTestClass.TestMethod)}](Int32 s1, Int32 s2, Int32 s3)";

            Assert.AreEqual(headerVoidResult, GetMethodInfo<string, string>(new MethodInfoTestClass().TestVoidMethod).CreateExceptionHeader());
            Assert.AreEqual(headerStringResult, GetMethodInfo<int, int, int, string>(new MethodInfoTestClass().TestMethod).CreateExceptionHeader());
        }
    }
}