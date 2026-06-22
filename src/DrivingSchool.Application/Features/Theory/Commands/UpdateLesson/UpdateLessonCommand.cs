using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Theory.Commands.UpdateLesson;

/// <summary>
/// Updates the scheduling details of an existing theory lesson.
/// Only available while the lesson is in the Scheduled state.
/// </summary>
public sealed record UpdateLessonCommand(
    Guid LessonId,
    string Title,
    string? Description,
    DateTime ScheduledAt,
    int DurationMinutes,
    string RoomOrLink) : ICommand;