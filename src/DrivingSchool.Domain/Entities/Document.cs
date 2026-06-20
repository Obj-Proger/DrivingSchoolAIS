using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a file uploaded to the system and associated with a domain entity
/// such as a contract or student profile.
/// </summary>
/// <remarks>
/// Documents use a loosely-typed owner reference (<see cref="OwnerType"/> +
/// <see cref="OwnerId"/>) rather than a strongly-typed foreign key so that
/// a single table can store files for any entity type without requiring
/// a separate join table per entity.
/// </remarks>
public sealed class Document : BaseEntity
{
    private Document() { } // Required by EF Core

    /// <summary>
    /// Gets the type name of the entity this document is attached to
    /// (e.g. <c>"Contract"</c>, <c>"Student"</c>, <c>"Group"</c>).
    /// </summary>
    public string OwnerType { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the owning entity.</summary>
    public Guid OwnerId { get; private set; }

    /// <summary>Gets the original file name as uploaded by the user.</summary>
    public string FileName { get; private set; } = string.Empty;

    /// <summary>Gets the storage URL where the file can be retrieved.</summary>
    public string FileUrl { get; private set; } = string.Empty;

    /// <summary>Gets the MIME content type of the file (e.g. <c>application/pdf</c>).</summary>
    public string ContentType { get; private set; } = string.Empty;

    /// <summary>Gets the file size in bytes.</summary>
    public long SizeBytes { get; private set; }

    /// <summary>Gets the identifier of the user who uploaded the document.</summary>
    public Guid UploadedById { get; private set; }

    // Factory

    /// <summary>
    /// Creates a new document record after a file has been uploaded to storage.
    /// </summary>
    /// <param name="ownerType">The type name of the owning entity.</param>
    /// <param name="ownerId">The identifier of the owning entity.</param>
    /// <param name="fileName">The original file name.</param>
    /// <param name="fileUrl">The storage URL of the uploaded file.</param>
    /// <param name="contentType">The MIME type of the file.</param>
    /// <param name="sizeBytes">The file size in bytes.</param>
    /// <param name="uploadedById">The uploader's user identifier.</param>
    public static Document Create(
        string ownerType,
        Guid ownerId,
        string fileName,
        string fileUrl,
        string contentType,
        long sizeBytes,
        Guid uploadedById)
        => new()
        {
            OwnerType = ownerType.Trim(),
            OwnerId = ownerId,
            FileName = fileName.Trim(),
            FileUrl = fileUrl.Trim(),
            ContentType = contentType.Trim(),
            SizeBytes = sizeBytes,
            UploadedById = uploadedById
        };
}