namespace DrivingSchool.Application.Features.Groups.DTOs;

/// <summary>
/// Full detail view of a group including its member list.
/// </summary>
public sealed record GroupDetailDto(
    Guid Id,
    string Name,
    Guid CourseId,
    string CourseName,
    LicenseCategory Category,
    Guid TeacherId,
    string TeacherName,
    GroupStatus Status,
    int MaxStudents,
    DateTime StartDate,
    DateTime? EndDate,
    IReadOnlyList<GroupMemberDto> Members);