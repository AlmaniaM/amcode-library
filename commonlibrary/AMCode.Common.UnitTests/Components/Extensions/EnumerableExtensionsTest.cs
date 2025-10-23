using System.Collections.Generic;
using AMCode.Common.Extensions.Enumerables;
using NUnit.Framework;

namespace AMCode.Common.UnitTests.Extensions
{
    [TestFixture]
    public class EnumerableExtensionsTest
    {
        [Test]
        public void ShouldRunThroughSimpleForLoopInIEnumerable()
        {
            var count = 0;
            var range = (IEnumerable<int>)new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            range.ForEach(value => count++);
            Assert.AreEqual(10, count);
        }

        [Test]
        public void ShouldRunThroughForLoopWithIndexInIEnumerable()
        {
            var range = (IEnumerable<int>)new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            range.ForEach((int value, int index) => Assert.AreEqual(value, index));
        }

        [Test]
        public void ShouldRunThroughSimpleForLoopInIList()
        {
            var count = 0;
            var range = (IList<int>)new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            range.ForEach(value => count++);
            Assert.AreEqual(10, count);
        }

        [Test]
        public void ShouldRunThroughForLoopWithIndexInIList()
        {
            var range = (IList<int>)new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            range.ForEach((int value, int index) => Assert.AreEqual(value, index));
        }

        [Test]
        public void ShouldRunThroughSimpleForLoopInArray()
        {
            var count = 0;
            var range = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            range.ForEach(value => count++);
            Assert.AreEqual(10, count);
        }

        [Test]
        public void ShouldRunThroughForLoopWithIndexInArray()
        {
            var range = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            range.ForEach((int value, int index) => Assert.AreEqual(value, index));
        }
    }
}