using System.Collections.Generic;
using System.Threading.Tasks;
using AMCode.Common.Testing.Containers;
using DlCommonTestingLibrary.UnitTests.Containers.Models;
using DlCommonTestingLibrary.UnitTests.Globals;
using Docker.DotNet;
using Docker.DotNet.Models;
using NUnit.Framework;

namespace DlCommonTestingLibrary.UnitTests.Containers.DockerImageTests
{
    [TestFixture]
    public class DockerImageTest
    {
        private IDockerClient dockerClient;
        private LocalTestFixture testFixture;

        [OneTimeSetUp]
        public void OneTimeSetup() => testFixture = new LocalTestFixture();

        [OneTimeTearDown]
        public void OneTimeTearDown() => testFixture.Dispose();

        [SetUp]
        public void Setup() => dockerClient = testFixture.DockerClient;

        [TearDown]
        public void TearDown() => dockerClient.Dispose();

        [Test]
        public async Task ShouldCreateImageFromPrivateRepo()
        {
            var dockerImage = new DockerImage(dockerClient);
            var imageName = $"{TestContainerResources.NoDbImageName}:{TestContainerResources.NoDbImageTag}";

            async Task<int> getImageCount() => (await dockerImage.ListImagesAsync(new List<string> { imageName })).Count;

            Assert.AreEqual(1, await getImageCount());

            await dockerImage.DeleteImageAsync(imageName);

            Assert.AreEqual(0, await getImageCount());

            await dockerImage.CreateAsync(imageName, new AuthConfig
            {
                Username = TestContainerResources.UserName,
                Password = TestContainerResources.AccessToken
            });

            Assert.AreEqual(1, await getImageCount());

            await dockerImage.DeleteImageAsync(imageName);
        }

        [Test]
        public async Task ShouldCreateImageFromPublicRepo()
        {
            var dockerImage = new DockerImage(dockerClient);
            var imageName = $"{LocalTestFixture.BaseImageName}:{LocalTestFixture.BaseImageTag}";

            async Task<int> getImageCount() => (await dockerImage.ListImagesAsync(new List<string> { imageName })).Count;

            Assert.AreEqual(1, await getImageCount());

            await dockerImage.DeleteImageAsync(imageName);

            Assert.AreEqual(0, await getImageCount());

            await dockerImage.CreateAsync(imageName, null);

            Assert.AreEqual(1, await getImageCount());
        }
    }
}