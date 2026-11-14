using Fileforge.Domain.Entities;

namespace Fileforge.Application.Interfaces;

public interface IDocumentRepository
{
    Task<Document?> GetAsync(Guid documentId, CancellationToken ct = default);
    Task AddAsync(Document document, CancellationToken ct = default);
}
