using DrivingSchool.Application.Features.Theory.DTOs;

namespace DrivingSchool.Application.Features.Theory.Queries.GetGroupLessons;

/// <summary>
/// Returns all theory lessons for the specified group
/// within an optional date range, ordered by scheduled time.
/// </summary>
public sealed record GetGroupLessonsQuery(
    Guid GroupId,
    DateTime? From = null,
    DateTime? To = null) : IQuery<IReadOnlyList<LessonDto>>;