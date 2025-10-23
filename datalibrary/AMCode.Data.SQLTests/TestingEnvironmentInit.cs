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

namespace AMCode.Data
{
    [SetUpFixture]
    public class TestingEnvironmentInit
    {
        private readonly string containerName = "dl-data-library-vertica";

        [OneTimeSetUp]
        public async Task InitializeEnvironment()
        {
            var dockerClient = new DockerClientConfiguration().CreateClient();

            if (dockerClient == null)
            {
                throw new Exception($"Could not create DockerClient with docker client name: ");
            }

            var runSetupParams = new ContainerSetupParams
            {
                ActionForWhenAlreadyRunning = DockerRunAlreadyRunningAction.RunIfExists,
                ContainerName = containerName,
                DbName = new KeyValuePair<string, string>("DATABASE_NAME", "demandlink"),
                DbPassword = new KeyValuePair<string, string>("DATABASE_PASSWORD", "testpassword"),
                ImageName = "demandlink/dl-data-library-vertica",
                ImageTag = "1.0.0",
                Password = EnvironmentUtil.GetEnvironmentVariable("AMCODE_TEST_DOCKER_PAT"),
                PortMappings = new Dictionary<string, string>
                {
                    ["5433"] = "5433/tcp"
                },
                UserName = EnvironmentUtil.GetEnvironmentVariable("AMCODE_TEST_DOCKER_USERNAME"),
                VolumeMaps = new Dictionary<string, string>
                {
                    ["demandlink-volume-dl-data-library-vertica"] = "/home/dbadmin/docker"
                }
            };

            var dbContainer = new DbContainer(dockerClient);
            await dbContainer.SetupAsync(runSetupParams);

            await waitFor(dockerClient, containerName, (string logs) =>
            {
                var message = "Vertica is now running";

                var indexOfMessage = logs.LastIndexOf(message);

                // Make sure it's the last message in the container logs
                return indexOfMessage == logs.Length - message.Length - 1;
            });
        }

        [OneTimeTearDown]
        public async Task TeardownEnvironment()
        {
            var dockerClient = new DockerClientConfiguration().CreateClient();

            if (dockerClient == null)
            {
                throw new Exception($"Could not create DockerClient with docker client name: ");
            }

            var dbContainerSetup = new DbContainer(dockerClient);
            var stopped = await dbContainerSetup.TeardownAsync(containerName);

            if (!stopped)
            {
                throw new Exception($"Could not shutdown Docker container with name {containerName}.");
            }
        }

        private async Task waitFor(IDockerClient dockerClient, string containerName, Func<string, bool> canContinue, double maxTimeToWait = 10)
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

                if (canContinue(stdout))
                {
                    stopWatch.Stop();
                    break;
                }

                await Task.Run(() => Thread.Sleep(1000));
            }

            stopWatch.Stop();
        }
    }
}