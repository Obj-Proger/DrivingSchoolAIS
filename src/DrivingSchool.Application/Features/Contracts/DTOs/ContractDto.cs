namespace DrivingSchool.Application.Features.Contracts.DTOs;

/// <summary>
/// Summary representation of a contract used in list views.
/// </summary>
public sealed record ContractDto(
    Guid Id,
    string Number,
    Guid StudentId,
    string StudentName,
    string StudentPhone,
    Guid CourseId,
    string CourseName,
    LicenseCategory Category,
    Guid? GroupId,
    string? GroupName,
    ContractStatus Status,
    decimal TotalCost,
    decimal PaidAmount,
    decimal DebtAmount,
    int? QualityIndicator,
    int PracticeHoursCompleted,
    int TheoryLessonsAttended,
    DateTime SignedAt,
    DateTime StartDate,
    DateTime EndDate,
    string? TerminationReason,
    DateTime? TerminatedAt);