namespace DrivingSchool.Application.Features.Courses.DTOs;

/// <summary>Represents a single topic within a course's curriculum.</summary>
public sealed record CourseTopicDto(
    Guid Id,
    Guid CourseId,
    int OrderIndex,
    string Title,
    string? Description);