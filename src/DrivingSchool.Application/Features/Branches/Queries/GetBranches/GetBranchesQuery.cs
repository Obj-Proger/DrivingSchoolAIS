using DrivingSchool.Application.Features.Branches.DTOs;

namespace DrivingSchool.Application.Features.Branches.Queries.GetBranches;

/// <summary>
/// Returns all branches. Used to populate branch selection dropdowns
/// and the branch management screen.
/// </summary>
/// <param name="ActiveOnly">When <c>true</c>, excludes deactivated branches.</param>
public sealed record GetBranchesQuery(bool ActiveOnly = false)
    : IQuery<IReadOnlyList<BranchDto>>;