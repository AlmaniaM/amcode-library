using AMCode.AI.Models;
using Microsoft.Extensions.Logging;

namespace AMCode.AI.RAG;

/// <summary>
/// Default embedding service that wraps an IAIProvider's embedding capabilities
/// </summary>
public class DefaultEmbeddingService : IEmbeddingService
{
    private readonly IAIProvider _provider;
    private readonly ILogger<DefaultEmbeddingService> _logger;
    private readonly string? _model;
    private int? _dimensions;

    public DefaultEmbeddingService(IAIProvider provider, ILogger<DefaultEmbeddingService> logger, string? model = null, int? dimensions = null)
    {
        _provider = provider;
        _logger = logger;
        _model = model;
        _dimensions = dimensions;
    }

    public int Dimensions => _dimensions ?? 1536; // Default OpenAI embedding dimensions

    public async Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken = default)
    {
        var request = new AIEmbeddingRequest
        {
            Texts = new List<string> { text },
            Model = _model,
            Dimensions = _dimensions
        };

        var result = await _provider.GetEmbeddingsAsync(request, cancellationToken);

        if (!result.Success || result.Embeddings.Count == 0)
        {
            _logger.LogError("Embedding failed: {Error}", result.ErrorMessage);
            throw new InvalidOperationException($"Embedding failed: {result.ErrorMessage}");
        }

        if (_dimensions == null && result.Dimensions > 0)
        {
            _dimensions = result.Dimensions;
        }

        return result.Embeddings[0];
    }

    public async Task<float[][]> EmbedBatchAsync(IEnumerable<string> texts, CancellationToken cancellationToken = default)
    {
        var textList = texts.ToList();
        if (textList.Count == 0)
            return Array.Empty<float[]>();

        var request = new AIEmbeddingRequest
        {
            Texts = textList,
            Model = _model,
            Dimensions = _dimensions
        };

        var result = await _provider.GetEmbeddingsAsync(request, cancellationToken);

        if (!result.Success)
        {
            _logger.LogError("Batch embedding failed: {Error}", result.ErrorMessage);
            throw new InvalidOperationException($"Batch embedding failed: {result.ErrorMessage}");
        }

        if (_dimensions == null && result.Dimensions > 0)
        {
            _dimensions = result.Dimensions;
        }

        return result.Embeddings.ToArray();
    }
}
