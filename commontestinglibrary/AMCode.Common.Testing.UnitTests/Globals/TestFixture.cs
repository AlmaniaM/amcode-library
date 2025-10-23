using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using DlCommonTestingLibrary.UnitTests.Containers.Models;
using Docker.DotNet;
using Docker.DotNet.BasicAuth;
using Docker.DotNet.Models;
using Newtonsoft.Json;

namespace DlCommonTestingLibrary.UnitTests.Globals
{
    public class LocalTestFixture : IDisposable
    {
        public readonly static string BaseImageName = "nats";
        public readonly static string BaseImageTag = "latest";

        public LocalTestFixture() : this(BaseImageName, BaseImageTag) { }

        public LocalTestFixture(string baseImageName, string baseImageTag)
        {
            // Do not wait forever in case it gets stuck
            Cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            Cts.Token.Register(() => throw new TimeoutException("AMCode.Commont.Testing test timeout exception"));

            DockerClientConfiguration = new DockerClientConfiguration(
                credentials: new BasicAuthCredentials(TestContainerResources.UserName, TestContainerResources.AccessToken)
            );
            DockerClient = DockerClientConfiguration.CreateClient();

            // Create image
            DockerClient.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = baseImageName,
                    Tag = baseImageTag
                },
                null,
                new Progress<JSONMessage>((m) =>
                {
                    Console.WriteLine(JsonConvert.SerializeObject(m));
                    Debug.WriteLine(JsonConvert.SerializeObject(m));
                }),
                Cts.Token).GetAwaiter().GetResult();

            // Create local image tag to reuse
            var existingImagesResponse = DockerClient.Images.ListImagesAsync(
               new ImagesListParameters
               {
                   Filters = new Dictionary<string, IDictionary<string, bool>>
                   {
                       ["reference"] = new Dictionary<string, bool>
                       {
                           [baseImageName] = true
                       }
                   }
               },
               Cts.Token
           ).GetAwaiter().GetResult();

            ImageId = existingImagesResponse[0].ID;

            DockerClient.Images.TagImageAsync(
                ImageId,
                new ImageTagParameters
                {
                    RepositoryName = RepositoryName,
                    Tag = Tag
                },
                Cts.Token
            ).GetAwaiter().GetResult();
        }

        public CancellationTokenSource Cts { get; }
        public DockerClient DockerClient { get; }
        public DockerClientConfiguration DockerClientConfiguration { get; }
        public string RepositoryName { get; } = Guid.NewGuid().ToString();
        public string Tag { get; } = Guid.NewGuid().ToString();
        public string ImageId { get; }

        public void Dispose()
        {
            var containerList = DockerClient.Containers.ListContainersAsync(
                new ContainersListParameters
                {
                    Filters = new Dictionary<string, IDictionary<string, bool>>
                    {
                        ["ancestor"] = new Dictionary<string, bool>
                        {
                            [$"{RepositoryName}:{Tag}"] = true
                        }
                    },
                    All = true,
                },
                Cts.Token
                ).GetAwaiter().GetResult();

            foreach (var container in containerList)
            {
                DockerClient.Containers.RemoveContainerAsync(
                    container.ID,
                    new ContainerRemoveParameters
                    {
                        Force = true
                    },
                    Cts.Token
                ).GetAwaiter().GetResult();
            }

            foreach (var container in containerList)
            {
                var volumes = container.Mounts.Where(mountPoint => mountPoint.Type.Equals("volume")).Select(mountPoint => mountPoint.Name).ToList();
                foreach (var volume in volumes)
                {
                    DockerClient.Volumes.RemoveAsync(volume, true, Cts.Token);
                }
            }

            var imageList = DockerClient.Images.ListImagesAsync(
                new ImagesListParameters
                {
                    Filters = new Dictionary<string, IDictionary<string, bool>>
                    {
                        ["reference"] = new Dictionary<string, bool>
                        {
                            [RepositoryName] = true
                        }
                    },
                    All = true
                },
                Cts.Token
            ).GetAwaiter().GetResult();

            foreach (var image in imageList)
            {
                DockerClient.Images.DeleteImageAsync(
                    image.ID,
                    new ImageDeleteParameters { Force = true },
                    Cts.Token
                ).GetAwaiter().GetResult();
            }

            DockerClient.Dispose();
            DockerClientConfiguration.Dispose();
            Cts.Dispose();
        }
    }
}