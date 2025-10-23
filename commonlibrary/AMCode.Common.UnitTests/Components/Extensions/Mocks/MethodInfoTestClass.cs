namespace AMCode.Common.UnitTests.Extensions.MethodInfoTests.Mocks
{
    public class MethodInfoTestClass
    {
        public string TestVoidMethodValue { get; set; }

        public void TestVoidMethod() => TestVoidMethodValue = string.Empty;
        public void TestVoidMethod(string p1, string p2) => TestVoidMethodValue = $"{p1}, {p2}";

        public string TestMethod() => string.Empty;
        public string TestMethod(int s1, int s2, int s3) => $"{s1}, {s2}, {s3}";
    }
}