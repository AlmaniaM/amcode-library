namespace AMCode.Storage.ImageUtilities.Models
{
    /// <summary>
    /// Options controlling image compression behavior
    /// </summary>
    public class CompressionOptions
    {
        /// <summary>
        /// Compression quality from 1 (lowest) to 100 (highest). Default is 85.
        /// </summary>
        public int Quality { get; set; } = 85;

        /// <summary>
        /// Output format. When null, uses the same format as the input image.
        /// </summary>
        public ImageFormat? OutputFormat { get; set; }

        /// <summary>
        /// Target maximum file size in bytes. When set, quality is reduced iteratively
        /// until the output is under this size. Takes precedence over Quality.
        /// </summary>
        public long? TargetFileSizeBytes { get; set; }
    }
}
