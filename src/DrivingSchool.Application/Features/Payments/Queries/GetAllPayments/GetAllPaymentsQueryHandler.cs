using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Payments.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Payments.Queries.GetAllPayments;

/// <summary>Handles <see cref="GetAllPaymentsQuery"/>.</summary>
internal sealed class GetAllPaymentsQueryHandler
    : IQueryHandler<GetAllPaymentsQuery, PaginatedResult<PaymentDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllPaymentsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<PaginatedResult<PaymentDto>>> Handle(
        GetAllPaymentsQuery query,
        CancellationToken cancellationToken)
    {
        var paginated = await _unitOfWork.Payments.GetPaginatedAsync(
            query.Page,
            query.PageSize,
            query.Method,
            query.Status,
            query.From,
            query.To,
            query.ManagerId,
            cancellationToken);

        var dtos = new List<PaymentDto>();
        foreach (var payment in paginated.Items)
        {
            var manager = await _unitOfWork.Users
                .GetByIdAsync(payment.ManagerId, cancellationToken);

            dtos.Add(new PaymentDto(
                payment.Id,
                payment.ContractId,
                payment.ManagerId,
                manager?.FullName.ShortName ?? "Unknown",
                payment.Amount.Amount,
                payment.Purpose,
                payment.Method,
                payment.Status,
                payment.ReceiptNumber,
                payment.PaidAt,
                payment.CreatedAt));
        }

        return Result.Success(new PaginatedResult<PaymentDto>(
            dtos, paginated.TotalCount, paginated.Page, paginated.PageSize));
    }
}