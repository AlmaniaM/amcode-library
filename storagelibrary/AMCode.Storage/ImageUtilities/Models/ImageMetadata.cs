using System;
using System.Collections.Generic;

namespace AMCode.Storage.ImageUtilities.Models
{
    /// <summary>
    /// Metadata extracted from an image including dimensions, format, and EXIF data
    /// </summary>
    public class ImageMetadata
    {
        /// <summary>
        /// Width of the image in pixels
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of the image in pixels
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Detected format of the image
        /// </summary>
        public ImageFormat Format { get; set; }

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSizeBytes { get; set; }

        /// <summary>
        /// EXIF metadata key-value pairs (e.g., camera model, GPS coordinates)
        /// </summary>
        public Dictionary<string, string> ExifData { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// EXIF orientation value (1-8), or null if not present
        /// </summary>
        public int? Orientation { get; set; }

        /// <summary>
        /// Date the photo was taken, or null if not available in EXIF data
        /// </summary>
        public DateTime? DateTaken { get; set; }
    }
}
