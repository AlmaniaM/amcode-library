using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DemandLink.Common.Testing.Containers;
using DemandLink.Common.Testing.Containers.Models;
using Docker.DotNet;
using Docker.DotNet.Models;
using NUnit.Framework;

namespace DlStorageLibrary
{
    [SetUpFixture]
    public class TestingEnvironmentInit
    {
        private readonly string containerName = "dl-storage-library-azurite";
        private readonly string volumeName = "demandlink-volume-dl-storage-library";

        [OneTimeSetUp]
        public async Task InitializeEnvironment()
        {
            var dockerClient = new DockerClientConfiguration().CreateClient();

            if (dockerClient == null)
            {
                throw new Exception($"Could not create DockerClient with docker client name: ");
            }

            var runSetupParams = new DockerRunParams
            {
                ActionForWhenAlreadyRunning = DockerRunAlreadyRunningAction.RunIfExists,
                ContainerName = containerName,
                ImageName = "mcr.microsoft.com/azure-storage/azurite",
                ImageTag = "latest",
                Ports = new Dictionary<string, string>
                {
                    ["10000"] = "10000"
                },
                Volumes = new Dictionary<string, string>
                {
                    [volumeName] = "/data"
                },
                Cmd = new List<string> { "azurite-blob", "--blobHost", "0.0.0.0" }
            };

            var dockerContainer = new DockerContainer(dockerClient);

            var containerState = await dockerContainer.GetContainerStateAsync(new GetContainerStateParams
            {
                ContainerName = containerName,
            });

            if (containerState.Running)
            {
                await dockerContainer.StopAsync(containerName, true);
            }

            await dockerContainer.RunAsync(runSetupParams);

            await waitFor(dockerClient, containerName, (string logs) =>
            {
                var message = "Azurite Blob service successfully listens on http://0.0.0.0:10000";

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

            var container = new DockerContainer(dockerClient);
            var stopped = await container.StopAsync(containerName, true);
            await container.RemoveAsync(new ContainerRemoveParams
            {
                ContainerName = containerName,
                Force = true,
                RemoveVolumes = true
            });

            var volume = new DockerVolume(dockerClient);
            await volume.DeleteVolumeAsync(volumeName, true);

            if (!stopped)
            {
                throw new Exception($"Could not shutdown Docker container with name {containerName}.");
            }
        }

        private async Task waitFor(IDockerClient dockerClient, string containerName, Func<string, bool> canContinue, double maxTimeToWait = 10)
        {
            var containerId = await new DockerContainer(dockerClient).GetIdAsync(containerName);
            var MAX_WAIT_TIME = maxTimeToWait * 60000;
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            while (stopWatch.ElapsedMilliseconds < MAX_WAIT_TIME)
            {
                var logs = await dockerClient.Containers.GetContainerLogsAsync(containerId, false, new ContainerLogsParameters
                {
                    ShowStdout = true
                });

                var (stdout, stderr) = await logs.ReadOutputToEndAsync(new CancellationToken());

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