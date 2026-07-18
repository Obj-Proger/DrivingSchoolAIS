namespace DrivingSchool.Application.Features.Courses.DTOs;

/// <summary>
/// Summary representation of a course used in list views and as a reference
/// (e.g. course name lookups from Contracts and Groups).
/// </summary>
public sealed record CourseDto(
    Guid Id,
    string Name,
    string? Description,
    LicenseCategory Category,
    int TheoryHoursTotal,
    int PracticeHoursTotal,
    decimal Price,
    bool IsActive,
    int TopicsCount);