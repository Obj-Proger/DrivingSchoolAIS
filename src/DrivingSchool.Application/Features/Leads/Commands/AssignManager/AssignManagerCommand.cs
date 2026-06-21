using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Leads.Commands.AssignManager;

/// <summary>Assigns a manager to a lead and transitions it to InProgress.</summary>
public sealed record AssignManagerCommand(
    Guid LeadId,
    Guid ManagerId) : ICommand;