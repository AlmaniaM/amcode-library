using AMCode.Sql.GroupBy;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.GroupBy.GroupByClauseFactoryTests
{
    [TestFixture]
    public class GroupByClauseFactoryTest
    {
        [Test]
        public void ShouldCreateInstanceOfGroupByClause()
            => Assert.AreSame(typeof(GroupByClause), new GroupByClauseFactory().Create().GetType());
    }
}