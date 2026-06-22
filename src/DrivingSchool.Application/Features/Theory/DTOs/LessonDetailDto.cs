namespace DrivingSchool.Application.Features.Theory.DTOs;

/// <summary>
/// Full detail view of a theory lesson including materials and attendance records.
/// </summary>
public sealed record LessonDetailDto(
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
    IReadOnlyList<LessonMaterialDto> Materials,
    IReadOnlyList<AttendanceDto> AttendanceRecords,
    DateTime CreatedAt);