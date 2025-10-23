using System;
using System.Collections.Generic;
using AMCode.Common.Extensions.Dictionary;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Extensions.DictionaryTests
{
    [TestFixture]
    public class DictionaryExtensionsTest
    {
        [Test]
        public void ShouleGetValue()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>()
            {
                ["key1"] = "value1"
            };

            Assert.AreEqual("value1", dictionary.GetValue("key1"));
            Assert.AreEqual(default(string), dictionary.GetValue("key2"));
        }

        [Test]
        public void ShouleThrowNullReferenceException()
        {
            IDictionary<string, string> dictionary = null;
            Assert.Throws<NullReferenceException>(() => dictionary.GetValue("key2"));
        }
    }
}