using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Contracts.Commands.TerminateContract;

/// <summary>
/// Terminates a training contract before its scheduled end date.
/// Requires a reason to be recorded for audit purposes.
/// </summary>
public sealed record TerminateContractCommand(
    Guid ContractId,
    string Reason) : ICommand;