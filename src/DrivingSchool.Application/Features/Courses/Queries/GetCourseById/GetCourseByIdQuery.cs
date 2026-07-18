using DrivingSchool.Application.Features.Courses.DTOs;

namespace DrivingSchool.Application.Features.Courses.Queries.GetCourseById;

/// <summary>Returns full course details, including its ordered topic list.</summary>
public sealed record GetCourseByIdQuery(Guid CourseId) : IQuery<CourseDetailDto>;