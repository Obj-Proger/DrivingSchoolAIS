namespace DrivingSchool.Application.Features.Groups.DTOs;

/// <summary>
/// Summary representation of a training group used in list views.
/// </summary>
public sealed record GroupDto(
    Guid Id,
    string Name,
    Guid CourseId,
    string CourseName,
    LicenseCategory Category,
    Guid TeacherId,
    string TeacherName,
    GroupStatus Status,
    int MembersCount,
    int MaxStudents,
    DateTime StartDate,
    DateTime? EndDate);