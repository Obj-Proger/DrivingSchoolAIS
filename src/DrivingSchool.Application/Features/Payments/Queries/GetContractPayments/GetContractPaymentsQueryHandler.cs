using DrivingSchool.Application.Features.Payments.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Payments.Queries.GetContractPayments;

/// <summary>Handles <see cref="GetContractPaymentsQuery"/>.</summary>
internal sealed class GetContractPaymentsQueryHandler
    : IQueryHandler<GetContractPaymentsQuery, IReadOnlyList<PaymentDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetContractPaymentsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<PaymentDto>>> Handle(
        GetContractPaymentsQuery query,
        CancellationToken cancellationToken)
    {
        var payments = await _unitOfWork.Payments
            .GetByContractIdAsync(query.ContractId, cancellationToken);

        var dtos = new List<PaymentDto>();
        foreach (var payment in payments)
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

        return Result.Success<IReadOnlyList<PaymentDto>>(dtos);
    }
}