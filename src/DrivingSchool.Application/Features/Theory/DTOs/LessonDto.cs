namespace DrivingSchool.Application.Features.Theory.DTOs;

/// <summary>
/// Summary view of a theory lesson used in schedule and list views.
/// </summary>
public sealed record LessonDto(
    Guid Id,
    Guid GroupId,
    string GroupName,
    Guid TeacherId,
    string TeacherName,
    Guid TopicId,
    string TopicTitle,
    string Title,
    string? Description,
    DateTime ScheduledAt,
    int DurationMinutes,
    string RoomOrLink,
    LessonStatus Status,
    string? CancellationReason,
    int MaterialsCount,
    int AttendanceCount,
    DateTime CreatedAt);