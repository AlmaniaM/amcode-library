using AMCode.OCR.Providers;

namespace AMCode.OCR.Configurations;

/// <summary>
/// Centralized registry for OCR provider names and configuration key mappings.
/// </summary>
public static class OCRProviderRegistry
{
    /// <summary>
    /// Provider name constants matching actual ProviderName property values.
    /// </summary>
    public static class Names
    {
        public const string PaddleOCR = "PaddleOCR";
        public const string AzureComputerVision = "Azure Computer Vision";
        public const string AWSTextract = "AWS Textract";
        public const string GoogleCloudVision = "Google Cloud Vision";
    }

    /// <summary>
    /// Maps configuration keys to provider types for factory instantiation.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, Type> ProviderTypeMap =
        new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            { "paddle-ocr", typeof(PaddleOCRProvider) },
            { "azure-cognitive-services", typeof(AzureComputerVisionOCRService) },
            { "aws-textract", typeof(AWSTextractOCRService) },
            { "google-cloud-vision", typeof(GoogleCloudVisionOCRService) }
        };

    /// <summary>
    /// Maps configuration keys to provider name aliases for backward compatibility.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, string[]> ProviderNameAliases =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            { "paddle-ocr", new[] { Names.PaddleOCR, "PaddleOCR", "Paddle" } },
            { "paddleocr", new[] { Names.PaddleOCR } },
            { "paddle", new[] { Names.PaddleOCR } },
            { "azure-cognitive-services", new[] { Names.AzureComputerVision, "Azure", "AzureComputerVision" } },
            { "azure", new[] { Names.AzureComputerVision } },
            { "azurecomputervision", new[] { Names.AzureComputerVision } },
            { "aws-textract", new[] { Names.AWSTextract, "AWS", "Textract", "Amazon" } },
            { "aws", new[] { Names.AWSTextract } },
            { "textract", new[] { Names.AWSTextract } },
            { "amazon", new[] { Names.AWSTextract } },
            { "google-cloud-vision", new[] { Names.GoogleCloudVision, "Google", "GCP", "GCPVision" } },
            { "google", new[] { Names.GoogleCloudVision } },
            { "gcpvision", new[] { Names.GoogleCloudVision } },
            { "gcp", new[] { Names.GoogleCloudVision } },
            { "googlecloudvision", new[] { Names.GoogleCloudVision } }
        };
}
