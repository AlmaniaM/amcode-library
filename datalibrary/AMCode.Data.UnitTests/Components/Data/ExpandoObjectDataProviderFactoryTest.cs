using AMCode.Data;
using AMCode.Data.Exceptions;
using Moq;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Data.ExpandoObjectDataProviderFactoryTests
{
    [TestFixture]
    public class ExpandoObjectDataProviderFactoryTest
    {
        [Test]
        public void ShouldCreateDefaultIGenericDataProvider()
        {
            var dataReaderFactoryMoq = Mock.Of<IDbBridgeProviderFactory>();
            var queryCancelMoq = Mock.Of<IQueryCancellation>();

            var factory = new ExpandoObjectDataProviderFactory(dataReaderFactoryMoq, queryCancelMoq);

            Assert.AreSame(typeof(ExpandoObjectDataProvider), factory.Create().GetType());
        }

        [Test]
        public void ShouldThrowExceptionWhenSomeParamsAreNullForDefault()
        {
            var dataReaderFactoryMoq = Mock.Of<IDbBridgeProviderFactory>();
            var expandoFactoryMoq = Mock.Of<IExpandoObjectDataProviderFactory>();
            var queryCancelMoq = Mock.Of<IQueryCancellation>();

            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new ExpandoObjectDataProviderFactory(null, queryCancelMoq).Create());
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new ExpandoObjectDataProviderFactory(dataReaderFactoryMoq, null).Create());
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new ExpandoObjectDataProviderFactory(null, null).Create());
        }

        [Test]
        public void ShouldCreateCustomParamsIGenericDataProvider()
        {
            var dataReaderFactoryMoq = Mock.Of<IDbBridgeProviderFactory>();
            var queryCancelMoq = Mock.Of<IQueryCancellation>();

            var factory = new ExpandoObjectDataProviderFactory();

            Assert.AreSame(typeof(ExpandoObjectDataProvider), factory.Create(dataReaderFactoryMoq, queryCancelMoq).GetType());
        }

        [Test]
        public void ShouldThrowExceptionWhenSomeParamsAreNullForCustomparams()
        {
            var dataReaderFactoryMoq = Mock.Of<IDbBridgeProviderFactory>();
            var expandoFactoryMoq = Mock.Of<IExpandoObjectDataProviderFactory>();
            var queryCancelMoq = Mock.Of<IQueryCancellation>();

            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new ExpandoObjectDataProviderFactory().Create(null, queryCancelMoq));
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new ExpandoObjectDataProviderFactory().Create(dataReaderFactoryMoq, null));
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new ExpandoObjectDataProviderFactory().Create(null, null));
        }
    }
}