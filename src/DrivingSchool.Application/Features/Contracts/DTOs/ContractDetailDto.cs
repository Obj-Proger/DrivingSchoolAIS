using DrivingSchool.Application.Features.Payments.DTOs;

namespace DrivingSchool.Application.Features.Contracts.DTOs;

/// <summary>
/// Full detail view of a contract including payment history.
/// </summary>
public sealed record ContractDetailDto(
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
    DateTime? TerminatedAt,
    Guid? BranchId,
    IReadOnlyList<PaymentDto> Payments);