using Fileforge.Application.Interfaces;
using Fileforge.Domain.Entities;


namespace Fileforge.IntegrationTests.FileForge.ApplicationTests;

internal sealed class FailingChunkRepository(IChunkRepository inner) : IChunkRepository
{
    public Task DeleteByDocumentIdAsync(
        Guid id,
        CancellationToken ct)
    {
        return inner.DeleteByDocumentIdAsync(id, ct);
    }

    public Task AddRangeAsync(
        IEnumerable<DocumentChunk> chunks,
        CancellationToken ct)
    {
        throw new InvalidOperationException(
            "Simulated failure while adding replacement chunks.");
    }

    public Task<IReadOnlyList<ScoredChunk>> SearchAsync(
        ReadOnlyMemory<float> queryEmbedding,
        IEnumerable<Guid> documentIds,
        int topK,
        CancellationToken ct)
    {
        return inner.SearchAsync(
            queryEmbedding,
            documentIds,
            topK,
            ct);
    }

    public Task<IReadOnlyList<ScoredChunk>> SearchAllAsync(ReadOnlyMemory<float> queryEmbedding, int topK, CancellationToken ct = default) 
    {
        return null;
    }
}
