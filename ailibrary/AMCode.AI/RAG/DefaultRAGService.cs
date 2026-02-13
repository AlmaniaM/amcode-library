using AMCode.AI.Models;
using Microsoft.Extensions.Logging;

namespace AMCode.AI.RAG;

/// <summary>
/// Default RAG service that composes embedding, vector store, and AI provider
/// for retrieval-augmented generation
/// </summary>
public class DefaultRAGService : IRAGService
{
    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorStore _vectorStore;
    private readonly IAIProvider _provider;
    private readonly ILogger<DefaultRAGService> _logger;

    public DefaultRAGService(
        IEmbeddingService embeddingService,
        IVectorStore vectorStore,
        IAIProvider provider,
        ILogger<DefaultRAGService> logger)
    {
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
        _provider = provider;
        _logger = logger;
    }

    public async Task<RAGResult> QueryAsync(string question, RAGOptions? options = null, CancellationToken cancellationToken = default)
    {
        options ??= new RAGOptions();

        try
        {
            // 1. Embed the query
            var queryEmbedding = await _embeddingService.EmbedAsync(question, cancellationToken);

            // 2. Retrieve relevant documents
            var searchResults = await _vectorStore.SearchAsync(
                queryEmbedding, options.TopK, options.Filter, cancellationToken);

            // Filter by minimum score
            var relevantResults = searchResults
                .Where(r => r.Score >= options.MinScore)
                .ToList();

            if (relevantResults.Count == 0)
            {
                _logger.LogInformation("No relevant documents found for query: {Query}", question);
                return RAGResult.Ok("I couldn't find any relevant information to answer your question.", new List<RAGSource>());
            }

            // 3. Build context from retrieved documents
            var context = BuildContext(relevantResults);

            // 4. Generate answer using AI provider
            var systemPrompt = options.SystemInstruction ??
                "You are a helpful assistant. Answer the user's question based on the provided context. " +
                "If the context doesn't contain enough information to answer, say so. " +
                "Always cite which sources you used.";

            var chatRequest = new AIChatRequest
            {
                SystemInstruction = systemPrompt,
                Messages = new List<AIChatMessage>
                {
                    AIChatMessage.User($"Context:\n{context}\n\nQuestion: {question}")
                },
                MaxTokens = options.MaxTokens,
                Temperature = options.Temperature
            };

            var chatResult = await _provider.ChatAsync(chatRequest, cancellationToken);

            if (!chatResult.Success)
            {
                return RAGResult.Fail($"Generation failed: {chatResult.ErrorMessage}");
            }

            var sources = relevantResults.Select(r =>
            {
                string? snippet = null;
                if (r.Metadata?.TryGetValue("content", out var content) == true)
                {
                    var text = content.ToString() ?? string.Empty;
                    snippet = text.Length > 200 ? text[..200] + "..." : text;
                }
                return new RAGSource(r.Id, r.Score, snippet);
            }).ToList();

            return RAGResult.Ok(chatResult.Message.Content, sources, chatResult.Usage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RAG query failed for: {Query}", question);
            return RAGResult.Fail($"RAG query failed: {ex.Message}");
        }
    }

    public async Task IndexDocumentAsync(string id, string content, Dictionary<string, object>? metadata = null, CancellationToken cancellationToken = default)
    {
        var embedding = await _embeddingService.EmbedAsync(content, cancellationToken);

        // Store the content in metadata for retrieval
        metadata ??= new Dictionary<string, object>();
        metadata["content"] = content;

        await _vectorStore.UpsertAsync(id, embedding, metadata, cancellationToken);

        _logger.LogDebug("Indexed document {Id}", id);
    }

    public async Task RemoveDocumentAsync(string id, CancellationToken cancellationToken = default)
    {
        await _vectorStore.DeleteAsync(id, cancellationToken);
        _logger.LogDebug("Removed document {Id}", id);
    }

    private static string BuildContext(List<VectorSearchResult> results)
    {
        var parts = new List<string>();
        for (int i = 0; i < results.Count; i++)
        {
            var result = results[i];
            if (result.Metadata?.TryGetValue("content", out var content) == true)
            {
                parts.Add($"[Source {i + 1}] (relevance: {result.Score:F2})\n{content}");
            }
        }
        return string.Join("\n\n", parts);
    }
}
