using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Common.Testing.Containers;
using AMCode.Common.Testing.Containers.Models;
using DlCommonTestingLibrary.UnitTests.Globals;
using Docker.DotNet;
using Docker.DotNet.Models;
using NUnit.Framework;

namespace DlCommonTestingLibrary.UnitTests.Containers.DockerContainerTests
{
    [TestFixture]
    public class DockerContainerTest
    {
        private IDockerClient dockerClient;
        private LocalTestFixture testFixture;
        private DockerRunParams runParams;

        [OneTimeSetUp]
        public void OneTimeSetup() => testFixture = new LocalTestFixture();

        [OneTimeTearDown]
        public void OneTimeTearDown() => testFixture.Dispose();

        [SetUp]
        public void Setup()
        {
            dockerClient = testFixture.DockerClient;
            runParams = new DockerRunParams
            {
                ActionForWhenAlreadyRunning = DockerRunAlreadyRunningAction.StopExistingAndRunNew,
                AutoRemoveContainer = false,
                ContainerName = Guid.NewGuid().ToString(),
                ImageName = testFixture.RepositoryName,
                ImageId = testFixture.ImageId,
                ImageTag = testFixture.Tag
            };
        }

        [TearDown]
        public void TearDown()
        {
            var dockerContainer = new DockerContainer(dockerClient);
            if (dockerContainer.IsRunningAsync(runParams.ContainerName).GetAwaiter().GetResult())
            {
                dockerContainer.StopAsync(runParams.ContainerName).GetAwaiter().GetResult();
            }

            dockerClient.Dispose();
        }

        [Test]
        public async Task ShouldRunDockerContainer()
        {
            var runResult = await new DockerContainer(dockerClient).RunAsync(runParams);
            Assert.IsTrue(runResult);
        }

        [Test]
        public async Task ShouldRunDockerContainerWithCommand()
        {
            runParams.Cmd = new List<string> { "-p", "4223" };

            var container = new DockerContainer(dockerClient);
            await container.RunAsync(runParams);

            var containerInspectResponse = await container.InspectContainer(runParams.ContainerName);

            Assert.Contains("-p", containerInspectResponse.Config.Cmd.ToList());
            Assert.Contains("4223", containerInspectResponse.Config.Cmd.ToList());
        }

        [Test]
        public async Task ShouldInspectDockerContainer()
        {
            var container = new DockerContainer(dockerClient);
            await container.RunAsync(runParams);

            var containerInspectResponse = await container.InspectContainer(runParams.ContainerName);
            Assert.IsNotNull(containerInspectResponse);
        }

        [Test]
        public async Task ShouldFindContainerId()
        {
            var container = new DockerContainer(dockerClient);

            var runResult = await container.RunAsync(runParams);

            Assert.IsTrue(runResult);

            var containerId = await container.GetIdAsync(runParams.ContainerName);

            Assert.IsNotEmpty(containerId);
        }

        [Test]
        public async Task ShouldCheckIfContainerIsRunning()
        {
            var container = new DockerContainer(dockerClient);

            var runResult = await container.RunAsync(runParams);

            Assert.IsTrue(runResult);

            var isContainerRunning = await container.IsRunningAsync(runParams.ContainerName);

            Assert.IsTrue(isContainerRunning);
        }

        [Test]
        public async Task ShouldStopContainer()
        {
            var container = new DockerContainer(dockerClient);

            var runResult = await container.RunAsync(runParams);

            Assert.IsTrue(runResult);

            var isContainerStopped = await container.StopAsync(runParams.ContainerName, true);

            Assert.IsTrue(isContainerStopped);

            var isContainerRunning = await container.IsRunningAsync(runParams.ContainerName);

            Assert.IsFalse(isContainerRunning);
        }

        [Test]
        public async Task ShouldRemoveContainer()
        {
            var container = new DockerContainer(dockerClient);

            await container.RemoveAsync(runParams.ContainerName);

            Assert.IsNull(await container.GetContainerAsync(runParams.ContainerName));
        }

        [Test]
        public async Task ShouldWaitForContainerToStop()
        {
            var container = new DockerContainer(dockerClient);

            runParams.AutoRemoveContainer = false;

            var isRunning = await container.RunAsync(runParams);

            Assert.IsTrue(isRunning);

            var runningContainer = await container.GetContainerAsync(runParams.ContainerName);

            Assert.IsNotNull(runningContainer);

            var task = dockerClient.Containers.StopContainerAsync(runningContainer.ID, new ContainerStopParameters
            {
                WaitBeforeKillSeconds = 1
            });

            await container.WaitForContainerToStop(runParams.ContainerName);

            Assert.IsFalse(await container.IsRunningAsync(runParams.ContainerName));

            await task;
        }

        [Test]
        public async Task ShouldGetContainerStatus()
        {
            var container = new DockerContainer(dockerClient);

            runParams.AutoRemoveContainer = false;

            var isRunning = await container.RunAsync(runParams);

            Assert.IsTrue(isRunning);

            var containerState = await container.GetContainerStateAsync(new GetContainerStateParams { ContainerName = runParams.ContainerName });

            Assert.AreEqual(true, containerState.Running);

            var stopped = await container.StopAsync(runParams.ContainerName, true);

            Assert.IsTrue(stopped);

            var containerStateAfterStop = await container.GetContainerStateAsync(new GetContainerStateParams { ContainerName = runParams.ContainerName });

            Assert.IsFalse(containerStateAfterStop.Running);
        }

        [Test]
        public async Task ShouldGetContainerStatusById()
        {
            var container = new DockerContainer(dockerClient);

            runParams.AutoRemoveContainer = false;

            var isRunning = await container.RunAsync(runParams);

            Assert.IsTrue(isRunning);

            var containerId = await container.GetIdAsync(runParams.ContainerName);

            var containerState = await container.GetContainerStateAsync(new GetContainerStateParams { Id = containerId });

            Assert.AreEqual(true, containerState.Running);

            var stopped = await container.StopAsync(runParams.ContainerName, true);

            Assert.IsTrue(stopped);

            var containerStateAfterStop = await container.GetContainerStateAsync(new GetContainerStateParams { Id = containerId });

            Assert.IsFalse(containerStateAfterStop.Running);
        }

        [Test]
        public void ShouldThrowExceptionWhenNoNameOrId()
        {
            var container = new DockerContainer(dockerClient);

            Assert.Throws<Exception>(() => container.GetContainerStateAsync(new GetContainerStateParams()).GetAwaiter().GetResult());
        }

        [Test]
        public async Task ShouldStartContainerAndOnlyExposeOnePort()
        {
            runParams.Ports = new Dictionary<string, string>
            {
                ["10000"] = "4222/tcp"
            };
            var container = new DockerContainer(dockerClient);
            await container.RunAsync(runParams);

            var containerInspectResponse = await container.InspectContainer(runParams.ContainerName);

            var networkSettings = containerInspectResponse.NetworkSettings;
            var boundPorts = networkSettings.Ports.Where(portConfig => portConfig.Value != null).ToList();

            Assert.AreEqual(1, boundPorts.Count);
            Assert.AreEqual("4222/tcp", boundPorts.First().Key);
            Assert.AreEqual("10000", boundPorts.First().Value.First().HostPort);
        }

        [Test]
        public async Task ShouldStartContainerAndPublishAllPorts()
        {
            runParams.PublishAllPorts = true;
            var container = new DockerContainer(dockerClient);
            await container.RunAsync(runParams);

            var containerInspectResponse = await container.InspectContainer(runParams.ContainerName);

            var networkSettings = containerInspectResponse.NetworkSettings;
            var boundPorts = networkSettings.Ports.Where(portConfig => portConfig.Value != null).ToList();

            var containerPorts = boundPorts.Select(portConfig => portConfig.Key).ToList();

            Assert.AreEqual(3, boundPorts.Count);
            Assert.Contains("4222/tcp", containerPorts);
            Assert.Contains("6222/tcp", containerPorts);
            Assert.Contains("8222/tcp", containerPorts);
        }

        [Test]
        public async Task ShouldStartContainerAndPublishAllPortsMapped()
        {
            runParams.Ports = new Dictionary<string, string>
            {
                ["10000"] = "4222/tcp",
                ["10001"] = "6222/tcp",
                ["10002"] = "8222/tcp"
            };
            var container = new DockerContainer(dockerClient);
            await container.RunAsync(runParams);

            var containerInspectResponse = await container.InspectContainer(runParams.ContainerName);

            var networkSettings = containerInspectResponse.NetworkSettings;
            var boundPorts = networkSettings.Ports.Where(portConfig => portConfig.Value != null).ToList();

            var containerPorts = boundPorts.Select(portConfig => portConfig.Key).ToList();
            var hostPorts = boundPorts.Select(portConfig => portConfig.Value?.First()?.HostPort).ToList();

            Assert.AreEqual(3, boundPorts.Count);
            Assert.Contains("4222/tcp", containerPorts);
            Assert.Contains("4222/tcp", containerPorts);
            Assert.Contains("4222/tcp", containerPorts);
            Assert.Contains("10000", hostPorts);
            Assert.Contains("10001", hostPorts);
            Assert.Contains("10002", hostPorts);
        }
    }
}