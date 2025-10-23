using AMCode.Data;
using AMCode.Data.Exceptions;
using Moq;
using NUnit.Framework;

namespace AMCode.Data.UnitTests.Data.GenericDataProviderFactoryTests
{
    [TestFixture]
    public class GenericDataProviderFactoryTest
    {
        [Test]
        public void ShouldCreateDefaultIGenericDataProvider()
        {
            var dataReaderFactoryMoq = Mock.Of<IDbBridgeProviderFactory>();
            var expandoFactoryMoq = Mock.Of<IExpandoObjectDataProviderFactory>();
            var queryCancelMoq = Mock.Of<IQueryCancellation>();

            var factory = new GenericDataProviderFactory(dataReaderFactoryMoq, expandoFactoryMoq, queryCancelMoq);

            Assert.AreSame(typeof(GenericDataProvider), factory.Create().GetType());
        }

        [Test]
        public void ShouldThrowExceptionWhenSomeParamsAreNullForDefault()
        {
            var dataReaderFactoryMoq = Mock.Of<IDbBridgeProviderFactory>();
            var expandoFactoryMoq = Mock.Of<IExpandoObjectDataProviderFactory>();
            var queryCancelMoq = Mock.Of<IQueryCancellation>();

            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new GenericDataProviderFactory(null, expandoFactoryMoq, queryCancelMoq).Create());
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new GenericDataProviderFactory(dataReaderFactoryMoq, null, queryCancelMoq).Create());
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new GenericDataProviderFactory(dataReaderFactoryMoq, expandoFactoryMoq, null).Create());
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new GenericDataProviderFactory(null, null, null).Create());
        }

        [Test]
        public void ShouldCreateCustomParamsIGenericDataProvider()
        {
            var dataReaderFactoryMoq = Mock.Of<IDbBridgeProviderFactory>();
            var expandoFactoryMoq = Mock.Of<IExpandoObjectDataProviderFactory>();
            var queryCancelMoq = Mock.Of<IQueryCancellation>();

            var factory = new GenericDataProviderFactory();

            Assert.AreSame(typeof(GenericDataProvider), factory.Create(dataReaderFactoryMoq, expandoFactoryMoq, queryCancelMoq).GetType());
        }

        [Test]
        public void ShouldThrowExceptionWhenSomeParamsAreNullForCustomparams()
        {
            var dataReaderFactoryMoq = Mock.Of<IDbBridgeProviderFactory>();
            var expandoFactoryMoq = Mock.Of<IExpandoObjectDataProviderFactory>();
            var queryCancelMoq = Mock.Of<IQueryCancellation>();

            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new GenericDataProviderFactory().Create(null, expandoFactoryMoq, queryCancelMoq));
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new GenericDataProviderFactory().Create(dataReaderFactoryMoq, null, queryCancelMoq));
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new GenericDataProviderFactory().Create(dataReaderFactoryMoq, expandoFactoryMoq, null));
            Assert.Throws<DefaultFactoryMethodParametersMissingException>(() => new GenericDataProviderFactory().Create(null, null, null));
        }
    }
}