using AMCode.Sql.OrderBy;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.OrderBy.OrderByClauseFactoryTests
{
    [TestFixture]
    public class OrderByClauseFactoryTest
    {
        [Test]
        public void ShouldCreateInstanceOfOrderByClause()
            => Assert.AreSame(typeof(OrderByClause), new OrderByClauseFactory().Create().GetType());
    }
}