using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Groups.Commands.AddMemberToGroup;

/// <summary>Enrolls a student (via their contract) in a training group.</summary>
public sealed record AddMemberToGroupCommand(
    Guid GroupId,
    Guid ContractId) : ICommand;