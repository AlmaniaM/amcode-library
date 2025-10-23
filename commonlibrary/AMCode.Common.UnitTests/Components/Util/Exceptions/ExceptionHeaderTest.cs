using System.Linq;
using AMCode.Common.Util;
using AMCode.Common.UnitTests.Util.Mocks;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Util.ExceptionTests
{
    [TestFixture]
    public class ExceptionHeaderTest
    {
        [Test]
        public void ShouldCreateExceptionHeaderForVoidMethod()
        {
            var headerVoidResult = $"[{nameof(MethodTemplateMock)}][{nameof(MethodTemplateMock.TestVoidMethod)}]";

            string createExpectedHeader(int parameterCount)
            {
                var parameters = Enumerable.Range(1, parameterCount).Select(index => $"Int32 p{index}");
                return $"{headerVoidResult}({string.Join(", ", parameters)})";
            }

            var methodTemplate = new MethodTemplateMock();
            Assert.AreEqual(createExpectedHeader(0), ExceptionUtil.CreateExceptionHeader(methodTemplate.TestVoidMethod));
            Assert.AreEqual(createExpectedHeader(1), ExceptionUtil.CreateExceptionHeader<int>(methodTemplate.TestVoidMethod));
            Assert.AreEqual(createExpectedHeader(2), ExceptionUtil.CreateExceptionHeader<int, int>(methodTemplate.TestVoidMethod));
            Assert.AreEqual(createExpectedHeader(3), ExceptionUtil.CreateExceptionHeader<int, int, int>(methodTemplate.TestVoidMethod));
            Assert.AreEqual(createExpectedHeader(4), ExceptionUtil.CreateExceptionHeader<int, int, int, int>(methodTemplate.TestVoidMethod));
            Assert.AreEqual(createExpectedHeader(5), ExceptionUtil.CreateExceptionHeader<int, int, int, int, int>(methodTemplate.TestVoidMethod));
            Assert.AreEqual(createExpectedHeader(6), ExceptionUtil.CreateExceptionHeader<int, int, int, int, int, int>(methodTemplate.TestVoidMethod));
            Assert.AreEqual(createExpectedHeader(7), ExceptionUtil.CreateExceptionHeader<int, int, int, int, int, int, int>(methodTemplate.TestVoidMethod));
            Assert.AreEqual(createExpectedHeader(8), ExceptionUtil.CreateExceptionHeader<int, int, int, int, int, int, int, int>(methodTemplate.TestVoidMethod));
        }

        [Test]
        public void ShouldCreateExceptionHeaderForReturnTypeMethod()
        {
            var headerStringResult = $"[{nameof(MethodTemplateMock)}][{nameof(MethodTemplateMock.TestMethod)}]";

            string createExpectedHeader(int parameterCount)
            {
                var parameters = Enumerable.Range(1, parameterCount).Select(index => $"String p{index}");
                return $"{headerStringResult}({string.Join(", ", parameters)})";
            }

            var methodTemplate = new MethodTemplateMock();
            Assert.AreEqual(createExpectedHeader(0), ExceptionUtil.CreateExceptionHeader(methodTemplate.TestMethod));
            Assert.AreEqual(createExpectedHeader(0), ExceptionUtil.CreateExceptionHeader<string>(methodTemplate.TestMethod));
            Assert.AreEqual(createExpectedHeader(1), ExceptionUtil.CreateExceptionHeader<string, string>(methodTemplate.TestMethod));
            Assert.AreEqual(createExpectedHeader(2), ExceptionUtil.CreateExceptionHeader<string, string, string>(methodTemplate.TestMethod));
            Assert.AreEqual(createExpectedHeader(3), ExceptionUtil.CreateExceptionHeader<string, string, string, string>(methodTemplate.TestMethod));
            Assert.AreEqual(createExpectedHeader(4), ExceptionUtil.CreateExceptionHeader<string, string, string, string, string>(methodTemplate.TestMethod));
            Assert.AreEqual(createExpectedHeader(5), ExceptionUtil.CreateExceptionHeader<string, string, string, string, string, string>(methodTemplate.TestMethod));
            Assert.AreEqual(createExpectedHeader(6), ExceptionUtil.CreateExceptionHeader<string, string, string, string, string, string, string>(methodTemplate.TestMethod));
            Assert.AreEqual(createExpectedHeader(7), ExceptionUtil.CreateExceptionHeader<string, string, string, string, string, string, string, string>(methodTemplate.TestMethod));
            Assert.AreEqual(createExpectedHeader(8), ExceptionUtil.CreateExceptionHeader<string, string, string, string, string, string, string, string, string>(methodTemplate.TestMethod));
        }

        [Test]
        public void ShouldCreateExceptionHeaderForConstructor()
        {
            var headerStringResult = $"[{nameof(ConstructorTemplateMock)}][{nameof(ConstructorTemplateMock)}]";

            string createExpectedHeader(int parameterCount)
            {
                var parameters = Enumerable.Range(1, parameterCount).Select(index => $"String p{index}");
                return $"{headerStringResult}({string.Join(", ", parameters)})";
            }

            Assert.AreEqual(createExpectedHeader(0), ExceptionUtil.CreateConstructorExceptionHeader<ConstructorTemplateMock>());
            Assert.AreEqual(createExpectedHeader(1), ExceptionUtil.CreateConstructorExceptionHeader<ConstructorTemplateMock, string>());
            Assert.AreEqual(createExpectedHeader(2), ExceptionUtil.CreateConstructorExceptionHeader<ConstructorTemplateMock, string, string>());
            Assert.AreEqual(createExpectedHeader(3), ExceptionUtil.CreateConstructorExceptionHeader<ConstructorTemplateMock, string, string, string>());
            Assert.AreEqual(createExpectedHeader(4), ExceptionUtil.CreateConstructorExceptionHeader<ConstructorTemplateMock, string, string, string, string>());
            Assert.AreEqual(createExpectedHeader(5), ExceptionUtil.CreateConstructorExceptionHeader<ConstructorTemplateMock, string, string, string, string, string>());
            Assert.AreEqual(createExpectedHeader(6), ExceptionUtil.CreateConstructorExceptionHeader<ConstructorTemplateMock, string, string, string, string, string, string>());
            Assert.AreEqual(createExpectedHeader(7), ExceptionUtil.CreateConstructorExceptionHeader<ConstructorTemplateMock, string, string, string, string, string, string, string>());
        }
    }
}