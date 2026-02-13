using AMCode.AI.Models;

namespace AMCode.AI.RAG;

/// <summary>
/// Retrieval-Augmented Generation service combining embedding, retrieval, and generation
/// </summary>
public interface IRAGService
{
    /// <summary>
    /// Query the RAG system with a natural language question
    /// </summary>
    Task<RAGResult> QueryAsync(string question, RAGOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Index a document for later retrieval
    /// </summary>
    Task IndexDocumentAsync(string id, string content, Dictionary<string, object>? metadata = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a document from the index
    /// </summary>
    Task RemoveDocumentAsync(string id, CancellationToken cancellationToken = default);
}

/// <summary>
/// Options for RAG queries
/// </summary>
public class RAGOptions
{
    /// <summary>
    /// Number of relevant documents to retrieve
    /// </summary>
    public int TopK { get; set; } = 5;

    /// <summary>
    /// Minimum similarity score threshold (0.0 to 1.0)
    /// </summary>
    public float MinScore { get; set; } = 0.0f;

    /// <summary>
    /// Optional metadata filter for retrieval
    /// </summary>
    public Dictionary<string, object>? Filter { get; set; }

    /// <summary>
    /// System instruction for the generation step
    /// </summary>
    public string? SystemInstruction { get; set; }

    /// <summary>
    /// Maximum tokens for the generated answer
    /// </summary>
    public int? MaxTokens { get; set; }

    /// <summary>
    /// Temperature for the generation step
    /// </summary>
    public float? Temperature { get; set; }
}

/// <summary>
/// Result from a RAG query
/// </summary>
public class RAGResult
{
    /// <summary>
    /// The generated answer
    /// </summary>
    public string Answer { get; set; } = string.Empty;

    /// <summary>
    /// Source documents used to generate the answer
    /// </summary>
    public List<RAGSource> Sources { get; set; } = new();

    /// <summary>
    /// Whether the query was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Usage statistics for the generation step
    /// </summary>
    public AIUsageStats? Usage { get; set; }

    public static RAGResult Ok(string answer, List<RAGSource> sources, AIUsageStats? usage = null) => new()
    {
        Answer = answer,
        Sources = sources,
        Success = true,
        Usage = usage
    };

    public static RAGResult Fail(string error) => new()
    {
        Success = false,
        ErrorMessage = error
    };
}

/// <summary>
/// A source document retrieved during RAG
/// </summary>
public record RAGSource(string Id, float Score, string? Snippet = null);
