using System.Collections.Concurrent;

namespace AMCode.AI.RAG;

/// <summary>
/// In-memory vector store for development and testing.
/// Uses brute-force cosine similarity search.
/// </summary>
public class InMemoryVectorStore : IVectorStore
{
    private readonly ConcurrentDictionary<string, VectorEntry> _store = new();

    public Task UpsertAsync(string id, float[] embedding, Dictionary<string, object>? metadata = null, CancellationToken cancellationToken = default)
    {
        var entry = new VectorEntry(id, embedding, metadata);
        _store.AddOrUpdate(id, entry, (_, _) => entry);
        return Task.CompletedTask;
    }

    public Task UpsertBatchAsync(IEnumerable<VectorEntry> entries, CancellationToken cancellationToken = default)
    {
        foreach (var entry in entries)
        {
            _store.AddOrUpdate(entry.Id, entry, (_, _) => entry);
        }
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<VectorSearchResult>> SearchAsync(float[] queryEmbedding, int topK = 10, Dictionary<string, object>? filter = null, CancellationToken cancellationToken = default)
    {
        var results = _store.Values
            .Where(entry => MatchesFilter(entry.Metadata, filter))
            .Select(entry => new VectorSearchResult(entry.Id, CosineSimilarity(queryEmbedding, entry.Embedding), entry.Metadata))
            .OrderByDescending(r => r.Score)
            .Take(topK)
            .ToList();

        return Task.FromResult<IReadOnlyList<VectorSearchResult>>(results);
    }

    public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        _store.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    private static float CosineSimilarity(float[] a, float[] b)
    {
        if (a.Length != b.Length)
            return 0f;

        float dot = 0f, normA = 0f, normB = 0f;
        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            normA += a[i] * a[i];
            normB += b[i] * b[i];
        }

        var denominator = MathF.Sqrt(normA) * MathF.Sqrt(normB);
        return denominator == 0 ? 0f : dot / denominator;
    }

    private static bool MatchesFilter(Dictionary<string, object>? metadata, Dictionary<string, object>? filter)
    {
        if (filter == null || filter.Count == 0)
            return true;

        if (metadata == null)
            return false;

        foreach (var kvp in filter)
        {
            if (!metadata.TryGetValue(kvp.Key, out var value) || !Equals(value, kvp.Value))
                return false;
        }

        return true;
    }
}
