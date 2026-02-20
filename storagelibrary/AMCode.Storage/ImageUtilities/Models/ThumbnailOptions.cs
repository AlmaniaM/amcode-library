using System.Collections.Generic;

namespace AMCode.Storage.ImageUtilities.Models
{
    /// <summary>
    /// A single thumbnail variant defined by its maximum dimension and naming suffix
    /// </summary>
    /// <param name="MaxDimension">Maximum side length in pixels</param>
    /// <param name="Suffix">Name suffix used to identify this variant (e.g., "thumb_sm")</param>
    public record ThumbnailVariant(int MaxDimension, string Suffix);

    /// <summary>
    /// Options for generating multiple thumbnail variants in a single pass
    /// </summary>
    public class ThumbnailOptions
    {
        /// <summary>
        /// List of thumbnail variants to generate. Defaults to SM (150), MD (300), LG (600).
        /// </summary>
        public List<ThumbnailVariant> Variants { get; set; } = new List<ThumbnailVariant>
        {
            new ThumbnailVariant(150, "thumb_sm"),
            new ThumbnailVariant(300, "thumb_md"),
            new ThumbnailVariant(600, "thumb_lg"),
        };

        /// <summary>
        /// Output format for all generated thumbnails. Default is WebP.
        /// </summary>
        public ImageFormat OutputFormat { get; set; } = ImageFormat.WebP;

        /// <summary>
        /// Quality for thumbnail encoding, 1-100. Default is 80.
        /// </summary>
        public int Quality { get; set; } = 80;

        /// <summary>
        /// When true (default), EXIF metadata is stripped from thumbnails.
        /// </summary>
        public bool StripMetadata { get; set; } = true;
    }
}
