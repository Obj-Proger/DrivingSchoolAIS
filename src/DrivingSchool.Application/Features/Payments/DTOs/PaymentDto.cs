namespace DrivingSchool.Application.Features.Payments.DTOs;

/// <summary>
/// Represents a single payment record.
/// </summary>
public sealed record PaymentDto(
    Guid Id,
    Guid ContractId,
    Guid ManagerId,
    string ManagerName,
    decimal Amount,
    string Purpose,
    PaymentMethod Method,
    PaymentStatus Status,
    string? ReceiptNumber,
    DateTime? PaidAt,
    DateTime CreatedAt);