using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AMCode.Storage.UnitTests
{
    [SetUpFixture]
    public class TestingEnvironmentInit
    {
        private readonly string containerName = "dl-storage-library-azurite";
        private readonly string volumeName = "amcode-volume-dl-storage-library";

        [OneTimeSetUp]
        public async Task InitializeEnvironment()
        {
            var dockerClient = new MockDockerClient();

            if (dockerClient == null)
            {
                throw new Exception($"Could not create DockerClient with docker client name: ");
            }

            var runSetupParams = new MockDockerRunParams
            {
                ActionForWhenAlreadyRunning = MockDockerRunAlreadyRunningAction.RunIfExists,
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

            var dockerContainer = new MockDockerContainer(dockerClient);

            var containerState = await dockerContainer.GetContainerStateAsync(new MockGetContainerStateParams
            {
                ContainerName = containerName,
            });

            if (containerState.Running)
            {
                await dockerContainer.StopAsync(containerName, true);
            }

            await dockerContainer.RunAsync(runSetupParams);

            // Mock implementation - just wait a bit instead of checking logs
            await Task.Delay(2000);
        }

        [OneTimeTearDown]
        public async Task TeardownEnvironment()
        {
            var dockerClient = new MockDockerClient();

            if (dockerClient == null)
            {
                throw new Exception($"Could not create DockerClient with docker client name: ");
            }

            var container = new MockDockerContainer(dockerClient);
            var stopped = await container.StopAsync(containerName, true);
            await container.RemoveAsync(new MockContainerRemoveParams
            {
                ContainerName = containerName,
                Force = true,
                RemoveVolumes = true
            });

            var volume = new MockDockerVolume(dockerClient);
            await volume.DeleteVolumeAsync(volumeName, true);

            if (!stopped)
            {
                throw new Exception($"Could not shutdown Docker container with name {containerName}.");
            }
        }
    }
}