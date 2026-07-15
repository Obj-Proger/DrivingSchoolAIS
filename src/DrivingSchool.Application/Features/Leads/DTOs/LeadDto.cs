namespace DrivingSchool.Application.Features.Leads.DTOs;

/// <summary>
/// Summary representation of a lead used in list views.
/// </summary>
public sealed record LeadDto(
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
    Guid? BranchId,
    DateTime CreatedAt,
    DateTime UpdatedAt);