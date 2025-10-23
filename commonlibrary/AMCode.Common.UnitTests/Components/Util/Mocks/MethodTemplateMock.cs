using System.Collections.Generic;

namespace AMCode.Common.UnitTests.Util.Mocks
{
    public class MethodTemplateMock
    {
        public int TestVoidMethodResult { get; set; }

        public void TestVoidMethod()
            => TestVoidMethodResult = 0;
        public void TestVoidMethod(int p1)
            => TestVoidMethodResult = p1 + p1;
        public void TestVoidMethod(int p1, int p2)
            => TestVoidMethodResult = p1 + p1 + p2;
        public void TestVoidMethod(int p1, int p2, int p3)
            => TestVoidMethodResult = p1 + p1 + p2 + p3;
        public void TestVoidMethod(int p1, int p2, int p3, int p4)
            => TestVoidMethodResult = p1 + p1 + p2 + p3 + p4;
        public void TestVoidMethod(int p1, int p2, int p3, int p4, int p5)
            => TestVoidMethodResult = p1 + p1 + p2 + p3 + p4 + p5;
        public void TestVoidMethod(int p1, int p2, int p3, int p4, int p5, int p6)
            => TestVoidMethodResult = p1 + p1 + p2 + p3 + p4 + p5 + p6;
        public void TestVoidMethod(int p1, int p2, int p3, int p4, int p5, int p6, int p7)
            => TestVoidMethodResult = p1 + p1 + p2 + p3 + p4 + p5 + p6 + p7;
        public void TestVoidMethod(int p1, int p2, int p3, int p4, int p5, int p6, int p7, int p8)
            => TestVoidMethodResult = p1 + p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8;

        public string TestMethod()
            => string.Empty;
        public string TestMethod(string p1)
            => p1;
        public string TestMethod(string p1, string p2)
            => $"{p1}, {p2}";
        public string TestMethod(string p1, string p2, string p3)
            => $"{p1}, {p2}, {p3}";
        public string TestMethod(string p1, string p2, string p3, string p4)
            => $"{p1}, {p2}, {p3}, {p4}";
        public string TestMethod(string p1, string p2, string p3, string p4, string p5)
            => $"{p1}, {p2}, {p3}, {p4}, {p5}";
        public string TestMethod(string p1, string p2, string p3, string p4, string p5, string p6)
            => $"{p1}, {p2}, {p3}, {p4}, {p5}, {p6}";
        public string TestMethod(string p1, string p2, string p3, string p4, string p5, string p6, string p7)
            => $"{p1}, {p2}, {p3}, {p4}, {p5}, {p6}, {p7}";
        public string TestMethod(string p1, string p2, string p3, string p4, string p5, string p6, string p7, string p8)
            => $"{p1}, {p2}, {p3}, {p4}, {p5}, {p6}, {p7}, {p8}";
    }

    public class ConstructorTemplateMock
    {
        public ConstructorTemplateMock()
        {
            Values = new List<string> { };
        }

        public ConstructorTemplateMock(string p1)
        {
            Values = new List<string> { p1 };
        }

        public ConstructorTemplateMock(string p1, string p2)
        {
            Values = new List<string> { p1, p2 };
        }

        public ConstructorTemplateMock(string p1, string p2, string p3)
        {
            Values = new List<string> { p1, p2, p3 };
        }

        public ConstructorTemplateMock(string p1, string p2, string p3, string p4)
        {
            Values = new List<string> { p1, p2, p3, p4 };
        }

        public ConstructorTemplateMock(string p1, string p2, string p3, string p4, string p5)
        {
            Values = new List<string> { p1, p2, p3, p4, p5 };
        }

        public ConstructorTemplateMock(string p1, string p2, string p3, string p4, string p5, string p6)
        {
            Values = new List<string> { p1, p2, p3, p4, p5, p6 };
        }

        public ConstructorTemplateMock(string p1, string p2, string p3, string p4, string p5, string p6, string p7)
        {
            Values = new List<string> { p1, p2, p3, p4, p5, p6, p7 };
        }

        public IList<string> Values { get; }
    }
}