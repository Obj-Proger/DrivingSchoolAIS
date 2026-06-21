using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Leads.Commands.UpdateLead;

/// <summary>Updates the contact and interest information of an existing lead.</summary>
public sealed record UpdateLeadCommand(
    Guid LeadId,
    string FirstName,
    string LastName,
    string? MiddleName,
    string Phone,
    string? Email,
    LicenseCategory? CourseInterest,
    string? Comment) : ICommand;