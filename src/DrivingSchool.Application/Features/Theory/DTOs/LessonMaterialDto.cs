namespace DrivingSchool.Application.Features.Theory.DTOs;

/// <summary>
/// Represents a file or resource attached to a lesson or course topic.
/// </summary>
public sealed record LessonMaterialDto(
    Guid Id,
    Guid? LessonId,
    Guid? TopicId,
    string Title,
    string FileUrl,
    string ContentType,
    long SizeBytes,
    bool IsPublic,
    DateTime CreatedAt);