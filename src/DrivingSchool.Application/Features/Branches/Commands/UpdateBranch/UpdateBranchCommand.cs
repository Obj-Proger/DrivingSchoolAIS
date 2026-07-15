using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Branches.Commands.UpdateBranch;

/// <summary>
/// Updates the display information of an existing branch.
/// Restricted to administrators.
/// </summary>
public sealed record UpdateBranchCommand(
    Guid BranchId,
    string Name,
    string City,
    string Address,
    string Phone) : ICommand;