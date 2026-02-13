# AMCode.OCR — Agent Guide

## What This Is

Multi-cloud OCR abstraction library (.NET 8) supporting Azure Computer Vision, AWS Textract, Google Cloud Vision, Anthropic Vision, OpenAI Vision, and PaddleOCR. Includes smart provider selection, cost estimation, and batch processing.

## When to Use

- Text extraction from images (OCR)
- Multi-provider OCR with fallback
- OCR provider health checks and cost estimation
- Batch image processing

## When NOT to Use

- AI-powered recipe parsing → Use AI pipelines (`AMCode.AI`)
- Image storage → `AMCode.Storage`
- Document generation → `AMCode.Documents`

## Key Interfaces

| Interface | Purpose |
|-----------|---------|
| `IOCRProvider` | Unified OCR interface (ProcessImageAsync, CheckHealthAsync, GetCostEstimateAsync) |
| `OCRProviderRegistry` | Provider registration and selection |
| `IOCRConfiguration` | Provider-specific configuration (Azure, AWS, Google, etc.) |

## Supported Providers

| Provider | Config Key | Capabilities |
|----------|-----------|-------------|
| Azure Computer Vision | `OCR:Azure` | High accuracy, handwriting support |
| AWS Textract | `OCR:AWS` | Document/table extraction |
| Google Cloud Vision | `OCR:GCP` | Multi-language, document AI |
| PaddleOCR (Python) | `OCR:PaddleOCR` | Free, local, via Python microservice |
| OpenAI Vision | `OCR:OpenAI` | GPT-4V based |
| Anthropic Vision | `OCR:Anthropic` | Claude Vision based |

## Verification

```bash
cd repos/amcode-library
dotnet build ocrlibrary/AMCode.OCR/AMCode.OCR.csproj
```
