using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Courses.Commands.UpdateCourseTopic;

/// <summary>
/// Updates the title and description of an existing course topic.
/// Restricted to administrators.
/// </summary>
public sealed record UpdateCourseTopicCommand(
    Guid CourseId,
    Guid TopicId,
    string Title,
    string? Description) : ICommand;