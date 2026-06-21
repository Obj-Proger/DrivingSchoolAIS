namespace DrivingSchool.Application.Features.Leads.DTOs;

/// <summary>
/// Full detail view of a lead including its note history.
/// </summary>
public sealed record LeadDetailDto(
    Guid Id,
    string FullName,
    string Phone,
    string? Email,
    LeadSource Source,
    LeadStatus Status,
    string? ResponsibleManagerName,
    Guid? ResponsibleManagerId,
    LicenseCategory? CourseInterest,
    string? Comment,
    Guid? ContractId,
    IReadOnlyList<LeadNoteDto> Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt);