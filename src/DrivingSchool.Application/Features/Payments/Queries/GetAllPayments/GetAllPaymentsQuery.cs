using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Payments.DTOs;

namespace DrivingSchool.Application.Features.Payments.Queries.GetAllPayments;

/// <summary>Returns a paginated, filterable list of all payments school-wide.</summary>
public sealed record GetAllPaymentsQuery(
    int Page = 1,
    int PageSize = 20,
    PaymentMethod? Method = null,
    PaymentStatus? Status = null,
    DateTime? From = null,
    DateTime? To = null,
    Guid? ManagerId = null) : IQuery<PaginatedResult<PaymentDto>>;