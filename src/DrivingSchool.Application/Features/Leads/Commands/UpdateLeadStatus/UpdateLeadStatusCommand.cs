using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Leads.Commands.UpdateLeadStatus;

/// <summary>Updates the pipeline status of a lead.</summary>
public sealed record UpdateLeadStatusCommand(
    Guid LeadId,
    LeadStatus NewStatus) : ICommand;