using AMCode.Sql.Select;
using NUnit.Framework;

namespace AMCode.Sql.Builder.UnitTests.Select.SelectClauseFactoryTests
{
    [TestFixture]
    public class SelectClauseFactoryTest
    {
        [Test]
        public void ShouldCreateInstanceOfSelectClause()
            => Assert.AreSame(typeof(SelectClause), new SelectClauseFactory().Create().GetType());
    }
}