using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMCode.Storage.UnitTests
{
    /// <summary>
    /// Mock implementation of DockerContainer for testing purposes
    /// </summary>
    public class MockDockerContainer
    {
        private readonly MockDockerClient dockerClient;

        public MockDockerContainer(MockDockerClient dockerClient)
        {
            this.dockerClient = dockerClient;
        }

        public async Task<MockContainerState> GetContainerStateAsync(MockGetContainerStateParams parameters)
        {
            // Mock implementation - return stopped state
            return await Task.FromResult(new MockContainerState { Running = false });
        }

        public async Task<bool> StopAsync(string containerName, bool force = false)
        {
            // Mock implementation - always return true
            return await Task.FromResult(true);
        }

        public async Task RunAsync(MockDockerRunParams parameters)
        {
            // Mock implementation - do nothing
            await Task.CompletedTask;
        }

        public async Task<string> GetIdAsync(string containerName)
        {
            // Mock implementation - return a fake container ID
            return await Task.FromResult("mock-container-id");
        }

        public async Task RemoveAsync(MockContainerRemoveParams parameters)
        {
            // Mock implementation - do nothing
            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Mock implementation of DockerVolume for testing purposes
    /// </summary>
    public class MockDockerVolume
    {
        private readonly MockDockerClient dockerClient;

        public MockDockerVolume(MockDockerClient dockerClient)
        {
            this.dockerClient = dockerClient;
        }

        public async Task DeleteVolumeAsync(string volumeName, bool force = false)
        {
            // Mock implementation - do nothing
            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Mock implementation of IDockerClient for testing purposes
    /// </summary>
    public class MockDockerClient
    {
        // Mock implementation - no methods needed for basic testing
    }

    /// <summary>
    /// Mock implementation of ContainerState for testing purposes
    /// </summary>
    public class MockContainerState
    {
        public bool Running { get; set; }
    }

    /// <summary>
    /// Mock implementation of GetContainerStateParams for testing purposes
    /// </summary>
    public class MockGetContainerStateParams
    {
        public string ContainerName { get; set; }
    }

    /// <summary>
    /// Mock implementation of DockerRunParams for testing purposes
    /// </summary>
    public class MockDockerRunParams
    {
        public MockDockerRunAlreadyRunningAction ActionForWhenAlreadyRunning { get; set; }
        public string ContainerName { get; set; }
        public string ImageName { get; set; }
        public string ImageTag { get; set; }
        public Dictionary<string, string> Ports { get; set; }
        public Dictionary<string, string> Volumes { get; set; }
        public List<string> Cmd { get; set; }
    }

    /// <summary>
    /// Mock implementation of ContainerRemoveParams for testing purposes
    /// </summary>
    public class MockContainerRemoveParams
    {
        public string ContainerName { get; set; }
        public bool Force { get; set; }
        public bool RemoveVolumes { get; set; }
    }

    /// <summary>
    /// Mock implementation of ContainerLogsParameters for testing purposes
    /// </summary>
    public class MockContainerLogsParameters
    {
        public bool ShowStdout { get; set; }
    }

    /// <summary>
    /// Mock implementation of DockerRunAlreadyRunningAction enum for testing purposes
    /// </summary>
    public enum MockDockerRunAlreadyRunningAction
    {
        RunIfExists
    }
}
