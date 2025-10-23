using System.Collections.Generic;

namespace AMCode.Storage.Recipes.Models
{
    /// <summary>
    /// Options for recipe image storage
    /// </summary>
    public class RecipeImageStorageOptions
    {
        public string BasePath { get; set; } = "recipe-storage";
        public List<string> SupportedImageExtensions { get; set; } = new List<string>
        {
            ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp"
        };
        public List<string> SupportedDocumentExtensions { get; set; } = new List<string>
        {
            ".pdf", ".docx", ".txt"
        };
        public List<string> SupportedExportExtensions { get; set; } = new List<string>
        {
            ".xlsx", ".csv", ".pdf"
        };
        public int MaxImageSizeMB { get; set; } = 10;
        public int MaxDocumentSizeMB { get; set; } = 50;
        public int MaxExportSizeMB { get; set; } = 100;
        public bool GenerateThumbnails { get; set; } = true;
        public int ThumbnailWidth { get; set; } = 300;
        public int ThumbnailHeight { get; set; } = 300;
        public bool CompressImages { get; set; } = true;
        public int ImageQuality { get; set; } = 85;
    }
}
