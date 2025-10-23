using System;
using System.Threading.Tasks;
using AMCode.Common.Testing.Containers;
using DlCommonTestingLibrary.UnitTests.Containers.DbContainerTests.Models;
using DlCommonTestingLibrary.UnitTests.Containers.Models;
using Docker.DotNet;
using NUnit.Framework;

namespace DlCommonTestingLibrary.UnitTests.Containers.DbContainerTests
{
    [TestFixture]
    public class DbContainerTest
    {
        private ContainerSetupParams runSetupParams;

        [SetUp]
        public void SetUp()
        {
            runSetupParams = new ContainerSetupParams
            {
                ActionForWhenAlreadyRunning = DockerRunAlreadyRunningAction.StopExistingAndRunNew,
                ContainerName = Guid.NewGuid().ToString(),
                ImageName = TestContainerResources.NoDbImageName,
                ImageTag = TestContainerResources.NoDbImageTag,
                Password = TestContainerResources.AccessToken,
                UserName = TestContainerResources.UserName
            };
        }

        [TearDown]
        public async Task TeardownEnvironment()
        {
            var dockerClient = new DockerClientConfiguration().CreateClient();

            if (dockerClient == null)
            {
                throw new Exception($"Could not create DockerClient with docker client name: ${runSetupParams.ContainerName}");
            }

            var dbContainerSetup = new DbContainer(dockerClient);
            var stopped = await dbContainerSetup.TeardownAsync(runSetupParams.ContainerName, true);

            await new DockerContainer(dockerClient).RemoveAsync(runSetupParams.ContainerName);

            if (!stopped)
            {
                throw new Exception($"Could not shutdown Docker container with name {runSetupParams.ContainerName}.");
            }
        }

        [Test]
        public async Task ShouldCreateDbContainerFromRemoteImage()
        {
            var dockerClient = new DockerClientConfiguration().CreateClient();

            var dbContainer = new DbContainer(dockerClient);
            await dbContainer.SetupAsync(runSetupParams);

            var success = await DbContainerHelpers.WaitFor(dockerClient, runSetupParams.ContainerName, (string logs) => logs.Contains("NoDb container is now running", StringComparison.CurrentCulture));

            Assert.IsTrue(success);
        }

        [Test]
        public async Task ShouldRunExistingContainerFromRemoteImage()
        {
            var dockerClient = new DockerClientConfiguration().CreateClient();

            var container = new DockerContainer(dockerClient);
            var dockerRunParams = new DockerRunParams
            {
                ActionForWhenAlreadyRunning = DockerRunAlreadyRunningAction.RunIfExists,
                AutoRemoveContainer = false,
                ContainerName = runSetupParams.ContainerName,
                ImageName = runSetupParams.ImageName,
                ImageTag = runSetupParams.ImageTag,
                UserName = runSetupParams.UserName,
                Password = runSetupParams.Password
            };

            var started = await container.RunAsync(dockerRunParams);

            Assert.IsTrue(started);

            var stopped = await container.StopAsync(dockerRunParams.ContainerName, true);

            Assert.IsTrue(stopped);

            var dbContainer = new DbContainer(dockerClient);

            runSetupParams.ActionForWhenAlreadyRunning = DockerRunAlreadyRunningAction.RunIfExists;

            await dbContainer.SetupAsync(runSetupParams);

            var success = await DbContainerHelpers.WaitFor(dockerClient, runSetupParams.ContainerName, (string logs) => logs.Contains("NoDb container is now running", StringComparison.CurrentCulture));

            Assert.IsTrue(success);
        }

        [Test]
        public async Task ShouldNotErrorWhenNoContainerExistsAndOptionIsToRunIfExists()
        {
            var dockerClient = new DockerClientConfiguration().CreateClient();

            var container = new DockerContainer(dockerClient);
            var dockerRunParams = new DockerRunParams
            {
                ActionForWhenAlreadyRunning = DockerRunAlreadyRunningAction.RunIfExists,
                AutoRemoveContainer = false,
                ContainerName = runSetupParams.ContainerName,
                ImageName = runSetupParams.ImageName,
                ImageTag = runSetupParams.ImageTag,
                UserName = runSetupParams.UserName,
                Password = runSetupParams.Password
            };

            var dbContainer = new DbContainer(dockerClient);

            runSetupParams.ActionForWhenAlreadyRunning = DockerRunAlreadyRunningAction.RunIfExists;

            await dbContainer.SetupAsync(runSetupParams);

            var success = await DbContainerHelpers.WaitFor(dockerClient, runSetupParams.ContainerName, (string logs) => logs.Contains("NoDb container is now running", StringComparison.CurrentCulture));

            Assert.IsTrue(success);
        }
    }
}