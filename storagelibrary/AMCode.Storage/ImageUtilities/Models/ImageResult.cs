using System.IO;

namespace AMCode.Storage.ImageUtilities.Models
{
    /// <summary>
    /// Result of an image processing operation containing the processed image data
    /// </summary>
    public class ImageResult
    {
        /// <summary>
        /// The processed image data stream
        /// </summary>
        public Stream Data { get; set; }

        /// <summary>
        /// The format of the output image
        /// </summary>
        public ImageFormat Format { get; set; }

        /// <summary>
        /// Width in pixels of the output image
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height in pixels of the output image
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// File size in bytes of the output image
        /// </summary>
        public long FileSizeBytes { get; set; }

        /// <summary>
        /// Optional suffix identifying this variant (e.g., "thumb_sm", "thumb_md")
        /// </summary>
        public string? VariantSuffix { get; set; }
    }
}
