using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Leads.Commands.MarkLeadAsLost;

/// <summary>Marks a lead as lost with an optional reason.</summary>
public sealed record MarkLeadAsLostCommand(
    Guid LeadId,
    Guid AuthorId,
    string? Reason = null) : ICommand;