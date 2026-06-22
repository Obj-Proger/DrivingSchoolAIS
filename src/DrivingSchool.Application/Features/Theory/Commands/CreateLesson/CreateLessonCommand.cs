using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Theory.Commands.CreateLesson;

/// <summary>
/// Schedules a new theory lesson for a training group.
/// Accessible to teachers.
/// </summary>
public sealed record CreateLessonCommand(
    Guid GroupId,
    Guid TopicId,
    string Title,
    DateTime ScheduledAt,
    int DurationMinutes,
    string RoomOrLink,
    string? Description = null) : ICommand<Guid>;