namespace DrivingSchool.Application.Features.Users.DTOs;

/// <summary>
/// Aggregated skill assessment for a student, shown in the instructor's student card view.
/// </summary>
public sealed record SkillSummaryDto(
    string SkillName,

    /// <summary>Average score across all recorded assessments (1.0–5.0).</summary>
    double AverageScore,

    /// <summary>Number of times this skill has been assessed.</summary>
    int AssessmentCount);