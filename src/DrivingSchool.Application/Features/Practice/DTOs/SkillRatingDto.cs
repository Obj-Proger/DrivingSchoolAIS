namespace DrivingSchool.Application.Features.Practice.DTOs;

/// <summary>
/// Represents a single instructor skill assessment for a practice lesson.
/// </summary>
public sealed record SkillRatingDto(
    string SkillName,
    int Score);