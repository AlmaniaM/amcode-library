using System;
using System.Threading.Tasks;
using AMCode.Common.Testing.Containers;
using AMCode.Common.Testing.Containers.Models;
using DlCommonTestingLibrary.UnitTests.Containers.DbContainerTests.Models;
using DlCommonTestingLibrary.UnitTests.Containers.Models;
using Docker.DotNet;
using NUnit.Framework;

namespace DlCommonTestingLibrary.UnitTests.Containers.DbContainerTests
{
    [TestFixture]
    public class DbContainerTearDownTest
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

        [Test]
        public async Task ShouldTearDownDbContainerAndRemove()
        {
            var dockerClient = new DockerClientConfiguration().CreateClient();

            var dbContainer = new DbContainer(dockerClient);

            await dbContainer.SetupAsync(runSetupParams);

            var success = await DbContainerHelpers.WaitFor(
                dockerClient,
                runSetupParams.ContainerName,
                (string logs) => logs.Contains("NoDb container is now running", StringComparison.CurrentCulture)
            );

            Assert.IsTrue(success);

            await dbContainer.TeardownAndRemoveAsync(runSetupParams.ContainerName);

            var container = new DockerContainer(dockerClient);

            var state = await container.GetContainerStateAsync(new GetContainerStateParams
            {
                ContainerName = runSetupParams.ContainerName
            });

            Assert.AreEqual("none", state.Status);
        }
    }
}