using System;
using System.Data;
using System.Data.Odbc;
using AMCode.Common.Util;
using AMCode.Data;
using AMCode.Data.Vertica;
using Moq;
using Vertica.Data.VerticaClient;

namespace AMCode.Data.SQLTests.Globals
{
    public class MoqProvider
    {
        private readonly static string connectionString = EnvironmentUtil.GetEnvironmentVariable("TEST_CONTAINER_VERTICA_CONNECTION_STRING");

        public static IDbBridge CreateDbBridge()
        {
            var connectionFactoryMoq = CreateDbConnectionFactoryMoq();
            var commandFactoryMoq = CreateDbCommandFactoryMoq();

            return new DbBridge(connectionFactoryMoq.Object, commandFactoryMoq.Object);
        }

        public static IDbConnection CreateDbConnection()
        {
            // Use Vertica .NET driver on all platforms for better compatibility
            return new VerticaConnection(connectionString);
        }

        public static IDbExecute CreateDbExecute() => new DbExecute(CreateDataReaderProviderFactoryMoq().Object, CreateQueryCancellation());

        public static Mock<IDbBridgeProviderFactory> CreateDataReaderProviderFactoryMoq()
        {
            var dataReaderProviderFactoryMoq = new Mock<IDbBridgeProviderFactory>();
            dataReaderProviderFactoryMoq.Setup(factory => factory.Create()).Returns(() => CreateDbBridge());

            return dataReaderProviderFactoryMoq;
        }

        public static Mock<IDbCommandFactory> CreateDbCommandFactoryMoq()
        {
            var factoryMoq = new Mock<IDbCommandFactory>();
            var createSequence = factoryMoq.Setup(factory => factory.Create(It.IsAny<IDbConnection>())).Returns((IDbConnection connection) => connection.CreateCommand());
            factoryMoq.Setup(factory => factory.Create(It.IsAny<IDbConnection>(), It.IsAny<string>())).Returns((IDbConnection connection, string command) =>
            {
                // Use VerticaCommand for all platforms for better compatibility
                return new VerticaCommand(command, (VerticaConnection)connection);
            });

            return factoryMoq;
        }

        public static Mock<IDbConnectionFactory> CreateDbConnectionFactoryMoq()
        {
            var factoryMoq = new Mock<IDbConnectionFactory>();
            factoryMoq.Setup(factory => factory.Create()).Returns(() => CreateDbConnection());
            factoryMoq.Setup(factory => factory.Create(It.IsAny<string>())).Returns((string connStr) =>
            {
                if (isWindowsOs())
                {
                    return new VerticaConnection(connStr);
                }
                else
                {
                    return new OdbcConnection(connStr);
                }
            });

            return factoryMoq;
        }

        public static IExpandoObjectDataProvider CreateExpandoObjectDataProvider() => new ExpandoObjectDataProvider(CreateDataReaderProviderFactoryMoq().Object, CreateQueryCancellation());

        public static Mock<IExpandoObjectDataProviderFactory> CreateExpandoObjectDataProviderFactory()
        {
            var factoryMoq = new Mock<IExpandoObjectDataProviderFactory>();
            factoryMoq.Setup(factory => factory.Create()).Returns(() => new ExpandoObjectDataProvider(CreateDataReaderProviderFactoryMoq().Object, CreateQueryCancellation()));
            factoryMoq.Setup(factory => factory.Create(It.IsAny<IDbBridgeProviderFactory>(), It.IsAny<IQueryCancellation>()))
                .Returns((IDbBridgeProviderFactory factory, IQueryCancellation queryCancellation) => new ExpandoObjectDataProvider(factory, queryCancellation));

            return factoryMoq;
        }

        public static IGenericDataProvider CreateGenericDataProvider() => new GenericDataProvider(CreateDataReaderProviderFactoryMoq().Object, CreateExpandoObjectDataProviderFactory().Object, CreateQueryCancellation());

        public static IQueryCancellation CreateQueryCancellation() => new VerticaQueryCancellation(CreateDataReaderProviderFactoryMoq().Object);

        private static bool isWindowsOs() => Environment.OSVersion.Platform == PlatformID.Win32NT;
    }
}