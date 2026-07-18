using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Courses.Commands.AddCourseTopic;

/// <summary>
/// Appends a new topic to the end of a course's curriculum.
/// Restricted to administrators.
/// </summary>
public sealed record AddCourseTopicCommand(
    Guid CourseId,
    string Title,
    string? Description = null) : ICommand<Guid>;