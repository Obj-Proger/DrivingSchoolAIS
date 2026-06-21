namespace DrivingSchool.Application.Features.Users.DTOs;

/// <summary>
/// Instructor-facing summary of a student's training progress.
/// Shown in the instructor's student card view.
/// </summary>
public sealed record StudentCardDto(
    UserDto User,

    /// <summary>The active contract associated with the current training programme.</summary>
    Guid? ActiveContractId,

    /// <summary>Total practice hours completed across all contracts.</summary>
    int TotalPracticeHoursCompleted,

    /// <summary>Total theory lessons attended across all contracts.</summary>
    int TotalTheoryLessonsAttended,

    /// <summary>Outstanding debt across all active contracts.</summary>
    decimal TotalDebtAmount,

    /// <summary>Current quality indicator (1–5) from the most recent active contract.</summary>
    int? QualityIndicator,

    /// <summary>Aggregated skill assessments from all completed practice lessons.</summary>
    IReadOnlyList<SkillSummaryDto> SkillSummaries);