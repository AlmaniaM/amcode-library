using System;
using System.Threading.Tasks;
using AMCode.Common.Testing.Containers;
using DlCommonTestingLibrary.UnitTests.Globals;
using Docker.DotNet;
using NUnit.Framework;

namespace DlCommonTestingLibrary.UnitTests.Containers.DockerVolumeTests
{
    [TestFixture]
    public class DockerVolumeTest
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
        public async Task ShouldCreateVolume()
        {
            var volumeName = Guid.NewGuid().ToString();
            var dockerVolume = new DockerVolume(dockerClient);

            await dockerVolume.CreateAsync(volumeName);

            Assert.IsTrue(await dockerVolume.ExistsAsync(volumeName));

            await dockerVolume.DeleteVolumeAsync(volumeName);

            Assert.IsFalse(await dockerVolume.ExistsAsync(volumeName));
        }

        [Test]
        public async Task ShouldDeleteVolume()
        {
            var volumeName = Guid.NewGuid().ToString();
            var dockerVolume = new DockerVolume(dockerClient);

            await dockerVolume.CreateAsync(volumeName);

            Assert.IsTrue(await dockerVolume.ExistsAsync(volumeName));

            await dockerVolume.DeleteVolumeAsync(volumeName);

            Assert.IsFalse(await dockerVolume.ExistsAsync(volumeName));
        }

        [Test]
        public async Task ShouldCheckIfVolumeExists()
        {
            var volumeName = Guid.NewGuid().ToString();
            var dockerVolume = new DockerVolume(dockerClient);

            Assert.IsFalse(await dockerVolume.ExistsAsync(volumeName));

            await dockerVolume.CreateAsync(volumeName);

            Assert.IsTrue(await dockerVolume.ExistsAsync(volumeName));

            await dockerVolume.DeleteVolumeAsync(volumeName);

            Assert.IsFalse(await dockerVolume.ExistsAsync(volumeName));
        }
    }
}