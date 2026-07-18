using DrivingSchool.Application.Features.Courses.DTOs;

namespace DrivingSchool.Application.Features.Courses.Queries.GetCourses;

/// <summary>
/// Returns all courses. Used to populate course selection dropdowns
/// (contracts, groups, test templates) and the course catalog screen.
/// </summary>
/// <param name="ActiveOnly">When <c>true</c>, excludes deactivated courses.</param>
public sealed record GetCoursesQuery(bool ActiveOnly = false)
    : IQuery<IReadOnlyList<CourseDto>>;