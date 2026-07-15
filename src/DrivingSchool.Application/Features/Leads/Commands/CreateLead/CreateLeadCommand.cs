using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Leads.Commands.CreateLead;

/// <summary>
/// Registers a new prospective student in the CRM pipeline.
/// </summary>
public sealed record CreateLeadCommand(
    string FirstName,
    string LastName,
    string? MiddleName,
    string Phone,
    string? Email,
    LeadSource Source,
    LicenseCategory? CourseInterest = null,
    string? Comment = null,
    Guid? ResponsibleManagerId = null,
    Guid? BranchId = null) : ICommand<Guid>;