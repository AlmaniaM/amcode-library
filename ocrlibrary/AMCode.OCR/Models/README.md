# Models

**Location:** `AMCode.OCR/Models/`  
**Last Updated:** 2025-01-27  
**Purpose:** OCR data models, request/response structures, and result wrappers

---

## Overview

The Models subfolder contains all data structures used throughout the OCR library, including OCR requests, results, text blocks, bounding boxes, provider health information, and capabilities.

## Responsibilities

- Define OCR request structure with all processing options
- Define OCR result structure with extracted text and metadata
- Provide text block models with position and confidence information
- Define provider health and capability models
- Provide result wrapper for consistent error handling

## Class Catalog

### Models

#### OCRRequest

**File:** `OCRRequest.cs`

**Purpose:** Configuration options for OCR processing requests.

**Key Properties:**
- `ImageStream` / `ImagePath` / `ImageBytes` - Image input (one required)
- `ConfidenceThreshold` - Minimum confidence threshold (0.0 to 1.0, default: 0.5)
- `RequiresLanguageDetection` - Enable automatic language detection
- `RequiresHandwritingSupport` - Enable handwriting recognition
- `RequiresTableDetection` - Enable table detection
- `RequiresFormDetection` - Enable form detection
- `ExpectedLanguage` - Expected language code (e.g., "en", "es")
- `MaxRetries` - Maximum retry attempts (default: 3)
- `Timeout` - Request timeout (default: 5 minutes)
- `PreprocessImage` - Enable image preprocessing (default: true)
- `PreprocessingOptions` - Image preprocessing settings
- `ReturnDetailedTextBlocks` - Return detailed text block information
- `ReturnBoundingBoxes` - Return bounding box coordinates
- `ReturnConfidenceScores` - Return confidence scores
- `MaxImageSizeMB` - Maximum image size in MB (default: 50)
- `MaxCostPerRequest` - Maximum cost per request in USD (default: 1.0)

**Usage:**
```csharp
var request = new OCRRequest
{
    ImagePath = "image.jpg",
    ConfidenceThreshold = 0.8,
    RequiresLanguageDetection = true,
    RequiresHandwritingSupport = true,
    PreprocessingOptions = new ImagePreprocessingOptions
    {
        EnhanceContrast = true,
        DetectSkew = true
    }
};
var result = await ocrService.ExtractTextAsync(imageStream, request);
```

---

#### OCRResult

**File:** `OCRResult.cs`

**Purpose:** Result of OCR processing containing extracted text and metadata.

**Key Properties:**
- `Text` - Extracted text content (string)
- `TextBlocks` - Individual text blocks with positions (List<TextBlock>)
- `Confidence` - Overall confidence score (0.0 to 1.0)
- `Language` - Detected language code (e.g., "en", "es")
- `Provider` - Provider that processed the image
- `ProcessingTime` - Processing duration (TimeSpan)
- `Cost` - Processing cost in USD (decimal)
- `ProcessedAt` - Timestamp when OCR was performed (DateTime)
- `Metadata` - Additional metadata dictionary
- `ContainsHandwriting` - Whether result contains handwritten text
- `ContainsPrintedText` - Whether result contains printed text
- `WordCount` - Number of words detected (computed)
- `LineCount` - Number of lines detected (computed)
- `Error` - Error message if operation failed

**Usage:**
```csharp
var result = await ocrService.ExtractTextAsync(imageStream);
if (result.IsSuccess)
{
    Console.WriteLine($"Text: {result.Value.Text}");
    Console.WriteLine($"Confidence: {result.Value.Confidence}");
    Console.WriteLine($"Provider: {result.Value.Provider}");
    Console.WriteLine($"Cost: ${result.Value.Cost}");
    Console.WriteLine($"Processing Time: {result.Value.ProcessingTime}");
}
```

---

#### TextBlock

**File:** `TextBlock.cs`

**Purpose:** Represents a block of text with its position and properties.

**Key Properties:**
- `Text` - Text content of this block
- `Confidence` - Confidence score for this block (0.0 to 1.0)
- `BoundingBox` - Bounding box coordinates (BoundingBox)
- `IsHandwritten` - Whether text is handwritten
- `IsPrinted` - Whether text is printed
- `Language` - Language of this text block
- `FontSize` - Font size (if available)
- `FontFamily` - Font family (if available)
- `ReadingOrder` - Reading order index
- `IsTableContent` - Whether block is part of a table
- `TableCell` - Table cell information (if part of table)
- `Properties` - Additional properties dictionary

**Usage:**
```csharp
foreach (var block in result.TextBlocks)
{
    Console.WriteLine($"Text: {block.Text}");
    Console.WriteLine($"Position: {block.BoundingBox.X}, {block.BoundingBox.Y}");
    Console.WriteLine($"Confidence: {block.Confidence}");
}
```

---

#### BoundingBox

**File:** `BoundingBox.cs`

**Purpose:** Represents bounding box coordinates for text blocks.

**Key Properties:**
- `X` - X coordinate of top-left corner
- `Y` - Y coordinate of top-left corner
- `Width` - Width of bounding box
- `Height` - Height of bounding box

**Usage:**
```csharp
var boundingBox = new BoundingBox
{
    X = 100,
    Y = 200,
    Width = 300,
    Height = 50
};
```

---

#### OCRProviderHealth

**File:** `OCRProviderHealth.cs`

**Purpose:** Health status information for an OCR provider.

**Key Properties:**
- `Status` - Health status (Healthy, Unhealthy, Degraded)
- `SuccessRate` - Success rate percentage (0-100)
- `AverageResponseTime` - Average response time (TimeSpan)
- `LastCheckTime` - Last health check timestamp
- `ErrorCount` - Number of errors
- `TotalRequests` - Total number of requests
- `IsAvailable` - Whether provider is currently available

**Usage:**
```csharp
var health = await provider.CheckHealthAsync();
Console.WriteLine($"Status: {health.Status}");
Console.WriteLine($"Success Rate: {health.SuccessRate}%");
```

---

#### OCRProviderCapabilities

**File:** `OCRProviderCapabilities.cs`

**Purpose:** Capabilities and features supported by an OCR provider.

**Key Properties:**
- `SupportsLanguageDetection` - Whether provider supports language detection
- `SupportsHandwriting` - Whether provider supports handwriting recognition
- `SupportsTableDetection` - Whether provider supports table detection
- `SupportsFormDetection` - Whether provider supports form detection
- `SupportedLanguages` - List of supported language codes
- `MaxImageSizeMB` - Maximum image size in MB
- `MaxBatchSize` - Maximum batch size for batch processing
- `CostPerRequest` - Cost per request in USD

**Usage:**
```csharp
var capabilities = provider.Capabilities;
if (capabilities.SupportsHandwriting)
{
    // Use handwriting-specific features
}
```

---

#### Result<T>

**File:** `Result.cs`

**Purpose:** Generic result wrapper for consistent error handling using Result pattern.

**Key Properties:**
- `IsSuccess` - Whether operation was successful
- `Value` - Result value (if successful)
- `Error` - Error message (if failed)

**Usage:**
```csharp
Result<OCRResult> result = await ocrService.ExtractTextAsync(imageStream);
if (result.IsSuccess)
{
    var text = result.Value.Text;
}
else
{
    Console.WriteLine($"Error: {result.Error}");
}
```

---

## Architecture Patterns

- **Result Pattern**: `Result<T>` for consistent error handling
- **Value Objects**: Immutable data structures (BoundingBox, TextBlock)
- **Data Transfer Objects**: Request/Response models (OCRRequest, OCRResult)

## Usage Patterns

### Pattern 1: Basic OCR Request

```csharp
var request = new OCRRequest
{
    ImagePath = "image.jpg",
    ConfidenceThreshold = 0.7
};
var result = await ocrService.ExtractTextAsync(imageStream, request);
```

### Pattern 2: Advanced OCR Request with Preprocessing

```csharp
var request = new OCRRequest
{
    ImagePath = "image.jpg",
    RequiresLanguageDetection = true,
    RequiresHandwritingSupport = true,
    PreprocessingOptions = new ImagePreprocessingOptions
    {
        EnhanceContrast = true,
        DetectSkew = true,
        RemoveNoise = true
    }
};
```

### Pattern 3: Result Handling

```csharp
var result = await ocrService.ExtractTextAsync(imageStream);
if (result.IsSuccess)
{
    ProcessText(result.Value);
}
else
{
    HandleError(result.Error);
}
```

## Dependencies

### Internal Dependencies

None - Models are standalone data structures.

### External Dependencies

- `System.Text.Json.Serialization` - JSON serialization attributes

## Related Components

### Within Same Library

- [Services](../Services/README.md) - Uses models for requests and results
- [Providers](../Providers/README.md) - Returns models as results
- [Configurations](../Configurations/README.md) - Configuration models

### In Other Libraries

- None

## Testing

### Test Coverage

- Unit tests: `AMCode.OCR.Tests/Models/`

### Example Test

```csharp
[Test]
public void OCRRequest_WithPreprocessing_IsValid()
{
    var request = new OCRRequest
    {
        ImagePath = "test.jpg",
        PreprocessImage = true,
        PreprocessingOptions = new ImagePreprocessingOptions
        {
            EnhanceContrast = true
        }
    };
    
    Assert.IsTrue(request.PreprocessImage);
    Assert.IsTrue(request.PreprocessingOptions.EnhanceContrast);
}
```

## Notes

- All models use nullable reference types for optional properties
- `Result<T>` pattern provides consistent error handling across the library
- Models are serializable for API communication
- Text blocks include detailed position and formatting information

---

**See Also:**
- [Library README](../README.md) - Library overview
- [Root README](../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

