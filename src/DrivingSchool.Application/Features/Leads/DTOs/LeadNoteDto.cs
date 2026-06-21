namespace DrivingSchool.Application.Features.Leads.DTOs;

/// <summary>
/// Represents a single note on a lead's activity history.
/// </summary>
public sealed record LeadNoteDto(
    Guid Id,
    string Text,
    Guid AuthorId,
    string AuthorName,
    DateTime CreatedAt);