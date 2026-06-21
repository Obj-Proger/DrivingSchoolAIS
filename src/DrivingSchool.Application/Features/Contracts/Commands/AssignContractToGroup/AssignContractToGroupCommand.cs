using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Contracts.Commands.AssignContractToGroup;

/// <summary>
/// Assigns a contract (student) to a training group.
/// Adds the contract as a member of the group and links the group to the contract.
/// </summary>
public sealed record AssignContractToGroupCommand(
    Guid ContractId,
    Guid GroupId) : ICommand;