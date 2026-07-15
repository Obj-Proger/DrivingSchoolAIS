using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Groups.Commands.CreateGroup;

/// <summary>Creates a new training group.</summary>
public sealed record CreateGroupCommand(
    string Name,
    Guid CourseId,
    Guid TeacherId,
    DateTime StartDate,
    int MaxStudents,
    DateTime? EndDate = null,
    Guid? BranchId = null) : ICommand<Guid>;