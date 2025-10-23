using System;
using AMCode.Common.Util;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Util.EnvironmentUtilTests
{
    [TestFixture]
    public class EnvironmentUtilTest
    {
        [TestCase(EnvironmentVariableTarget.Process)]
        public void ShouldReadEnvironmentVariable(EnvironmentVariableTarget target)
        {
            Environment.SetEnvironmentVariable("VAR_ONE", "Value_1", target);

            Assert.AreEqual("Value_1", EnvironmentUtil.GetEnvironmentVariable("VAR_ONE"));

            Environment.SetEnvironmentVariable("VAR_ONE", "", target);
        }

        [Test]
        public void ShouldGetMachineEnvironmentVariable()
            => Assert.IsNotNull(EnvironmentUtil.GetEnvironmentVariable("PATH"));

        [Test]
        public void ShouldGetCorrectEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("VAR_TEST_PRECEDENCE", "Process_Value", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("VAR_TEST_PRECEDENCE", "User_Value", EnvironmentVariableTarget.User);

            Assert.AreEqual("Process_Value", EnvironmentUtil.GetEnvironmentVariable("VAR_TEST_PRECEDENCE"));

            Environment.SetEnvironmentVariable("VAR_TEST_PRECEDENCE", "");
        }
    }
}