namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="Document"/> entity.
/// </summary>
public interface IDocumentRepository
{
    /// <summary>Returns the document with the specified identifier, or <c>null</c> if not found.</summary>
    Task<Document?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns all documents attached to the specified owner entity
    /// (see <see cref="Document.OwnerType"/> / <see cref="Document.OwnerId"/>),
    /// most recently uploaded first.
    /// </summary>
    Task<IReadOnlyList<Document>> GetByOwnerAsync(
        string ownerType,
        Guid ownerId,
        CancellationToken ct = default);

    /// <summary>Adds a new document record to the repository.</summary>
    Task AddAsync(Document document, CancellationToken ct = default);

    /// <summary>Removes a document record from the repository.</summary>
    void Delete(Document document);
}