namespace DrivingSchool.Application.Features.Documents.DTOs;

/// <summary>Represents a file attached to another entity (contract, student, group, etc.).</summary>
public sealed record DocumentDto(
    Guid Id,
    string OwnerType,
    Guid OwnerId,
    string FileName,
    string FileUrl,
    string ContentType,
    long SizeBytes,
    Guid UploadedById,
    string UploadedByName,
    DateTime CreatedAt);