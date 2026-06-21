using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Leads.Commands.AddLeadNote;

/// <summary>Appends a note to the lead's activity history.</summary>
public sealed record AddLeadNoteCommand(
    Guid LeadId,
    string Text,
    Guid AuthorId) : ICommand;