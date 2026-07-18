using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Courses.Commands.CreateCourse;

/// <summary>
/// Creates a new course (training programme) offered by the school.
/// Restricted to administrators.
/// </summary>
public sealed record CreateCourseCommand(
    string Name,
    LicenseCategory Category,
    int TheoryHoursTotal,
    int PracticeHoursTotal,
    decimal Price,
    string? Description = null) : ICommand<Guid>;