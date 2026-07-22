using DrivingSchool.Application.Features.Documents.DTOs;

namespace DrivingSchool.Application.Features.Documents.Queries.GetDocuments;

/// <summary>Returns all documents attached to the specified owner entity.</summary>
public sealed record GetDocumentsQuery(string OwnerType, Guid OwnerId)
    : IQuery<IReadOnlyList<DocumentDto>>;