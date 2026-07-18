namespace DrivingSchool.Application.Features.Courses.DTOs;

/// <summary>
/// Full detail view of a course including its ordered curriculum.
/// </summary>
public sealed record CourseDetailDto(
    Guid Id,
    string Name,
    string? Description,
    LicenseCategory Category,
    int TheoryHoursTotal,
    int PracticeHoursTotal,
    decimal Price,
    bool IsActive,
    IReadOnlyList<CourseTopicDto> Topics);