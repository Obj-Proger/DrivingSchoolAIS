using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Courses.Commands.UpdateCourse;

/// <summary>
/// Updates a course's display information and price.
/// Restricted to administrators.
/// </summary>
public sealed record UpdateCourseCommand(
    Guid CourseId,
    string Name,
    string? Description,
    decimal Price) : ICommand;