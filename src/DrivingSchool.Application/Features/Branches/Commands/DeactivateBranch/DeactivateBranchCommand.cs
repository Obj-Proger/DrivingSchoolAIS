using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Branches.Commands.DeactivateBranch;

/// <summary>
/// Deactivates a branch, hiding it from selection when creating new
/// leads, contracts, groups, or staff assignments.
/// Restricted to administrators.
/// </summary>
public sealed record DeactivateBranchCommand(Guid BranchId) : ICommand;