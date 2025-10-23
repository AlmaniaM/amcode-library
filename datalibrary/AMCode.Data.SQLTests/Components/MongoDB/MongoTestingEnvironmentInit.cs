using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Testing.Containers;
using AMCode.Common.Util;
using Docker.DotNet;
using Docker.DotNet.Models;
using NUnit.Framework;

namespace AMCode.Data.SQLTests.MongoDB
{
    /// <summary>
    /// MongoDB integration test environment setup using Docker containers
    /// </summary>
    [SetUpFixture]
    public class MongoTestingEnvironmentInit
    {
        private readonly string containerName = "dl-data-library-mongodb";
        private readonly string databaseName = "testdb";
        private readonly string username = "testuser";
        private readonly string password = "testpassword";

        [OneTimeSetUp]
        public async Task InitializeEnvironment()
        {
            var dockerClient = new DockerClientConfiguration().CreateClient();

            if (dockerClient == null)
            {
                throw new Exception("Could not create DockerClient for MongoDB testing");
            }

            var runSetupParams = new ContainerSetupParams
            {
                ActionForWhenAlreadyRunning = DockerRunAlreadyRunningAction.RunIfExists,
                ContainerName = containerName,
                DbName = new KeyValuePair<string, string>("MONGO_INITDB_DATABASE", databaseName),
                DbPassword = new KeyValuePair<string, string>("MONGO_INITDB_ROOT_PASSWORD", password),
                ImageName = "mongo",
                ImageTag = "7.0",
                PortMappings = new Dictionary<string, string>
                {
                    ["27017"] = "27017/tcp"
                },
                EnvironmentVariables = new Dictionary<string, string>
                {
                    ["MONGO_INITDB_ROOT_USERNAME"] = username,
                    ["MONGO_INITDB_ROOT_PASSWORD"] = password,
                    ["MONGO_INITDB_DATABASE"] = databaseName
                }
            };

            var dbContainer = new DbContainer(dockerClient);
            await dbContainer.SetupAsync(runSetupParams);

            await WaitForMongoDbReady(dockerClient, containerName);
        }

        [OneTimeTearDown]
        public async Task TeardownEnvironment()
        {
            var dockerClient = new DockerClientConfiguration().CreateClient();

            if (dockerClient == null)
            {
                throw new Exception("Could not create DockerClient for MongoDB teardown");
            }

            var dbContainerSetup = new DbContainer(dockerClient);
            var stopped = await dbContainerSetup.TeardownAsync(containerName);

            if (!stopped)
            {
                throw new Exception($"Could not shutdown Docker container with name {containerName}.");
            }
        }

        /// <summary>
        /// Gets the MongoDB connection string for integration tests
        /// </summary>
        public static string GetConnectionString()
        {
            return $"mongodb://testuser:testpassword@localhost:27017/testdb?authSource=admin";
        }

        /// <summary>
        /// Gets the MongoDB database name for integration tests
        /// </summary>
        public static string GetDatabaseName()
        {
            return "testdb";
        }

        /// <summary>
        /// Waits for MongoDB to be ready by checking container logs
        /// </summary>
        private async Task WaitForMongoDbReady(IDockerClient dockerClient, string containerName, double maxTimeToWait = 30)
        {
            var containerId = await new DockerContainer((Docker.DotNet.DockerClient)dockerClient).GetIdAsync(containerName);
            var MAX_WAIT_TIME = maxTimeToWait * 60000;
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            while (stopWatch.ElapsedMilliseconds < MAX_WAIT_TIME)
            {
                var logs = await dockerClient.Containers.GetContainerLogsAsync(containerId, false, new ContainerLogsParameters
                {
                    ShowStdout = true
                });

                var output = await logs.ReadOutputToEndAsync(new CancellationToken());
                var stdout = output.stdout;
                var stderr = output.stderr;

                if (IsMongoDbReady(stdout))
                {
                    stopWatch.Stop();
                    break;
                }

                await Task.Run(() => Thread.Sleep(1000));
            }

            stopWatch.Stop();

            if (stopWatch.ElapsedMilliseconds >= MAX_WAIT_TIME)
            {
                throw new Exception($"MongoDB container {containerName} did not become ready within {maxTimeToWait} minutes");
            }
        }

        /// <summary>
        /// Checks if MongoDB is ready by looking for specific log messages
        /// </summary>
        private bool IsMongoDbReady(string logs)
        {
            var readyMessages = new[]
            {
                "Waiting for connections",
                "MongoDB starting",
                "MongoDB init process complete"
            };

            foreach (var message in readyMessages)
            {
                if (logs.Contains(message))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
