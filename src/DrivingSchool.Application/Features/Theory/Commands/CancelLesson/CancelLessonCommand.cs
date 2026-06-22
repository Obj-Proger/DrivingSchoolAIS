using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Theory.Commands.CancelLesson;

/// <summary>
/// Cancels a scheduled theory lesson with a mandatory reason.
/// Raises a domain event that notifies all students in the group.
/// </summary>
public sealed record CancelLessonCommand(
    Guid LessonId,
    string Reason) : ICommand;