using AMCode.Sql.From;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.From.FromClauseFactoryTests
{
    [TestFixture]
    public class FromClauseFactoryTest
    {
        [Test]
        public void ShouldCreateInstanceOfFromClause()
            => Assert.AreSame(typeof(FromClause), new FromClauseFactory().Create().GetType());
    }
}