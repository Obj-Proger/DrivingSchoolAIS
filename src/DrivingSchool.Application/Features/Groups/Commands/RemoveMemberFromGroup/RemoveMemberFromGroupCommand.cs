using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Groups.Commands.RemoveMemberFromGroup;

/// <summary>Removes a student from a training group.</summary>
public sealed record RemoveMemberFromGroupCommand(
    Guid GroupId,
    Guid ContractId) : ICommand;