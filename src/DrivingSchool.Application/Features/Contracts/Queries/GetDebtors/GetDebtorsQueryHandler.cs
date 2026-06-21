using DrivingSchool.Application.Features.Contracts.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Contracts.Queries.GetDebtors;

/// <summary>
/// Handles <see cref="GetDebtorsQuery"/>.
/// </summary>
internal sealed class GetDebtorsQueryHandler
    : IQueryHandler<GetDebtorsQuery, IReadOnlyList<ContractDebtDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDebtorsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<ContractDebtDto>>> Handle(
        GetDebtorsQuery query,
        CancellationToken cancellationToken)
    {
        var contracts = await _unitOfWork.Contracts.GetDebtorsAsync(cancellationToken);

        var dtos = new List<ContractDebtDto>();

        foreach (var contract in contracts)
        {
            var student = await _unitOfWork.Users
                .GetByIdAsync(contract.StudentId, cancellationToken);

            var payments = await _unitOfWork.Payments
                .GetByContractIdAsync(contract.Id, cancellationToken);

            var lastPaymentDate = payments
                .Where(p => p.Status == PaymentStatus.Completed && p.PaidAt.HasValue)
                .OrderByDescending(p => p.PaidAt)
                .FirstOrDefault()
                ?.PaidAt;

            dtos.Add(new ContractDebtDto(
                contract.Id,
                contract.Number,
                contract.StudentId,
                student?.FullName.DisplayName ?? "Unknown",
                student?.Phone.Value ?? string.Empty,
                contract.DebtAmount.Amount,
                lastPaymentDate));
        }

        return Result.Success<IReadOnlyList<ContractDebtDto>>(
            dtos.OrderByDescending(d => d.DebtAmount).ToList());
    }
}