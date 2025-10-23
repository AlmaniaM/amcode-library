using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Testing.Containers.Models;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace AMCode.Common.Testing.Containers
{
    /// <summary>
    /// A class that represents a Docker image with some simple functions.
    /// </summary>
    public class DockerImage : IDockerImage
    {
        private readonly IDockerClient dockerClient;

        /// <summary>
        /// Create an instance of the <see cref="DockerImage"/> class.
        /// </summary>
        /// <param name="dockerClient">Provide a <see cref="IDockerClient"/> to communicate with.</param>
        public DockerImage(IDockerClient dockerClient)
        {
            this.dockerClient = dockerClient;
        }

        /// <summary>
        /// Currently, this pulls down an image from our private Docker account. A more general
        /// Create function was not needed at the moment.
        /// </summary>
        /// <param name="imageNameWithTag"></param>
        /// <param name="authConfig"></param>
        /// <returns></returns>
        public async Task CreateAsync(string imageNameWithTag, AuthConfig authConfig)
        {
            await dockerClient.Images.CreateImageAsync(new ImagesCreateParameters
            {
                FromImage = imageNameWithTag
            },
            authConfig,
            new Progress<JSONMessage>((m) => Console.WriteLine(m)),
            default);
        }

        /// <summary>
        /// Delete an existing image.
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public async Task<IList<IDictionary<string, string>>> DeleteImageAsync(string imageName)
        {
            return await dockerClient.Images.DeleteImageAsync(imageName, new ImageDeleteParameters
            {
                Force = true
            });
        }

        /// <summary>
        /// Get a <see cref="IList{T}"/> of <see cref="ImagesListResponse"/> based on the <see cref="IList{T}"/>
        /// of <see cref="string"/> image names.
        /// </summary>
        /// <param name="imageNames"></param>
        /// <returns></returns>
        public async Task<IList<ImagesListResponse>> ListImagesAsync(IList<string> imageNames)
        {
            var filters = imageNames.ToDictionary((imageName) => imageName, (imageName) => true);

            return await ListImagesAsync(new ImagesListParameters
            {
                Filters = new Dictionary<string, IDictionary<string, bool>>
                {
                    ["reference"] = filters
                }
            });
        }

        /// <summary>
        /// Lists images based on the provided parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IList<ImagesListResponse>> ListImagesAsync(ImagesListParameters parameters, CancellationToken cancellationToken = default) => await dockerClient.Images.ListImagesAsync(parameters, cancellationToken);
    }
}