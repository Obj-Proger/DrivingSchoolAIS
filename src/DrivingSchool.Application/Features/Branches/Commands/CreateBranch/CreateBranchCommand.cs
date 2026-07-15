using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Branches.Commands.CreateBranch;

/// <summary>
/// Creates a new branch (physical office) of the driving school.
/// Restricted to administrators.
/// </summary>
public sealed record CreateBranchCommand(
    string Name,
    string City,
    string Address,
    string Phone) : ICommand<Guid>;