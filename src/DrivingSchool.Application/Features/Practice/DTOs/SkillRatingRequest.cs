namespace DrivingSchool.Application.Features.Practice.DTOs;

/// <summary>
/// Represents a single skill assessment submitted by the instructor
/// when completing a booking.
/// </summary>
public sealed record SkillRatingRequest(
    string SkillName,
    int Score);