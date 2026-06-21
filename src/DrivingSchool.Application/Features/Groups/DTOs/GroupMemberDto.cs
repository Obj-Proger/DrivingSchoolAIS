namespace DrivingSchool.Application.Features.Groups.DTOs;

/// <summary>
/// Represents a student member within a training group.
/// </summary>
public sealed record GroupMemberDto(
    Guid ContractId,
    Guid StudentId,
    string StudentName,
    string StudentPhone,
    int PracticeHoursCompleted,
    int TheoryLessonsAttended,
    decimal DebtAmount,
    int? QualityIndicator,
    DateTime JoinedAt);