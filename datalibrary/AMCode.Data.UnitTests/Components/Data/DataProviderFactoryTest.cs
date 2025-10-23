using AMCode.Data;
using AMCode.Data.Exceptions;
using Moq;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Data.DataProviderFactoryTests
{
    [TestFixture]
    public class DataProviderFactoryTest
    {
        [Test]
        public void ShouldCreateDefaultIDataProvider()
        {
            var dbExecuteFactoryMoq = Mock.Of<IDbExecuteFactory>();
            var expandoFactoryMoq = Mock.Of<IExpandoObjectDataProviderFactory>();
            var genericFactoryMoq = Mock.Of<IGenericDataProviderFactory>();

            var factory = new DataProviderFactory(dbExecuteFactoryMoq, expandoFactoryMoq, genericFactoryMoq);

            Assert.AreSame(typeof(DataProvider), factory.Create().GetType());
        }

        [Test]
        public void ShouldThrowExceptionWhenSomeParamsAreNullForDefault()
        {
            var dbExecuteFactoryMoq = Mock.Of<IDbExecuteFactory>();
            var expandoFactoryMoq = Mock.Of<IExpandoObjectDataProviderFactory>();
            var genericFactoryMoq = Mock.Of<IGenericDataProviderFactory>();

            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new DataProviderFactory(null, expandoFactoryMoq, genericFactoryMoq).Create());
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new DataProviderFactory(dbExecuteFactoryMoq, null, genericFactoryMoq).Create());
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new DataProviderFactory(dbExecuteFactoryMoq, expandoFactoryMoq, null).Create());
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new DataProviderFactory(null, null, null).Create());
        }

        [Test]
        public void ShouldCreateCustomParamsIGenericDataProvider()
        {
            var dbExecuteFactoryMoq = Mock.Of<IDbExecuteFactory>();
            var expandoFactoryMoq = Mock.Of<IExpandoObjectDataProviderFactory>();
            var genericFactoryMoq = Mock.Of<IGenericDataProviderFactory>();

            var factory = new DataProviderFactory();

            Assert.AreSame(typeof(DataProvider), factory.Create(dbExecuteFactoryMoq, expandoFactoryMoq, genericFactoryMoq).GetType());
        }

        [Test]
        public void ShouldThrowExceptionWhenSomeParamsAreNullForCustomparams()
        {
            var dbExecuteFactoryMoq = Mock.Of<IDbExecuteFactory>();
            var expandoFactoryMoq = Mock.Of<IExpandoObjectDataProviderFactory>();
            var genericFactoryMoq = Mock.Of<IGenericDataProviderFactory>();

            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new DataProviderFactory().Create(null, expandoFactoryMoq, genericFactoryMoq));
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new DataProviderFactory().Create(dbExecuteFactoryMoq, null, genericFactoryMoq));
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new DataProviderFactory().Create(dbExecuteFactoryMoq, expandoFactoryMoq, null));
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new DataProviderFactory().Create(null, null, null));
        }
    }
}