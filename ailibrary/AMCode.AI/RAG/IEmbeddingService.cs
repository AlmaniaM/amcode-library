namespace AMCode.AI.RAG;

/// <summary>
/// Service for generating text embeddings for semantic search and RAG
/// </summary>
public interface IEmbeddingService
{
    /// <summary>
    /// Generate an embedding vector for a single text
    /// </summary>
    Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate embedding vectors for multiple texts in a single batch
    /// </summary>
    Task<float[][]> EmbedBatchAsync(IEnumerable<string> texts, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dimensions of the embedding vectors produced by this service
    /// </summary>
    int Dimensions { get; }
}
