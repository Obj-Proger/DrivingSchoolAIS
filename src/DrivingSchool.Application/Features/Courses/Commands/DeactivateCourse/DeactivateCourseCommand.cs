using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Courses.Commands.DeactivateCourse;

/// <summary>
/// Deactivates a course, preventing it from being selected for new contracts
/// or groups. Existing active contracts are not affected.
/// Restricted to administrators.
/// </summary>
public sealed record DeactivateCourseCommand(Guid CourseId) : ICommand;