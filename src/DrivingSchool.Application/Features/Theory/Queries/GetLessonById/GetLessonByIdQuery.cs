using DrivingSchool.Application.Features.Theory.DTOs;

namespace DrivingSchool.Application.Features.Theory.Queries.GetLessonById;

/// <summary>
/// Returns the full detail view of a single theory lesson
/// including materials and attendance records.
/// </summary>
public sealed record GetLessonByIdQuery(Guid LessonId) : IQuery<LessonDetailDto>;