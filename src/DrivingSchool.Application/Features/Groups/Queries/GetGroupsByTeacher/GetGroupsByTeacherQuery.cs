using DrivingSchool.Application.Features.Groups.DTOs;

namespace DrivingSchool.Application.Features.Groups.Queries.GetGroupsByTeacher;

/// <summary>
/// Returns all groups assigned to the specified teacher.
/// </summary>
public sealed record GetGroupsByTeacherQuery(Guid TeacherId)
    : IQuery<IReadOnlyList<GroupDto>>;