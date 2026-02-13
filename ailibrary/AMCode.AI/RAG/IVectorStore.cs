namespace AMCode.AI.RAG;

/// <summary>
/// Abstraction for vector storage and similarity search
/// </summary>
public interface IVectorStore
{
    /// <summary>
    /// Insert or update a single vector entry
    /// </summary>
    Task UpsertAsync(string id, float[] embedding, Dictionary<string, object>? metadata = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Insert or update multiple vector entries in batch
    /// </summary>
    Task UpsertBatchAsync(IEnumerable<VectorEntry> entries, CancellationToken cancellationToken = default);

    /// <summary>
    /// Find the most similar vectors to a query embedding
    /// </summary>
    Task<IReadOnlyList<VectorSearchResult>> SearchAsync(float[] queryEmbedding, int topK = 10, Dictionary<string, object>? filter = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a vector entry by ID
    /// </summary>
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}

/// <summary>
/// A single entry in a vector store
/// </summary>
public record VectorEntry(string Id, float[] Embedding, Dictionary<string, object>? Metadata = null);

/// <summary>
/// Result from a vector similarity search
/// </summary>
public record VectorSearchResult(string Id, float Score, Dictionary<string, object>? Metadata = null);
