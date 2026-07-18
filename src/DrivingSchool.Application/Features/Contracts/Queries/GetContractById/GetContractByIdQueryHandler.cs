using DrivingSchool.Application.Features.Contracts.DTOs;
using DrivingSchool.Application.Features.Contracts.Queries.GetContracts;
using DrivingSchool.Application.Features.Payments.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Contracts.Queries.GetContractById;

/// <summary>
/// Handles <see cref="GetContractByIdQuery"/>.
/// </summary>
internal sealed class GetContractByIdQueryHandler
    : IQueryHandler<GetContractByIdQuery, ContractDetailDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetContractByIdQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<ContractDetailDto>> Handle(
        GetContractByIdQuery query,
        CancellationToken cancellationToken)
    {
        var contract = await _unitOfWork.Contracts
            .GetByIdAsync(query.ContractId, cancellationToken);

        if (contract is null)
            return Result.Failure<ContractDetailDto>(DomainErrors.Contract.NotFound);

        var student = await _unitOfWork.Users
            .GetByIdAsync(contract.StudentId, cancellationToken);

        string? groupName = null;
        if (contract.GroupId.HasValue)
        {
            var group = await _unitOfWork.Groups
                .GetByIdAsync(contract.GroupId.Value, cancellationToken);
            groupName = group?.Name;
        }

        var payments = await _unitOfWork.Payments
            .GetByContractIdAsync(contract.Id, cancellationToken);

        var paymentDtos = new List<PaymentDto>();
        foreach (var payment in payments)
        {
            var manager = await _unitOfWork.Users
                .GetByIdAsync(payment.ManagerId, cancellationToken);

            paymentDtos.Add(new PaymentDto(
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

        var course = await _unitOfWork.Courses
            .GetByIdAsync(contract.CourseId, cancellationToken);

        return Result.Success(new ContractDetailDto(
            contract.Id,
            contract.Number,
            contract.StudentId,
            student?.FullName.DisplayName ?? "Unknown",
            student?.Phone.Value ?? string.Empty,
            contract.CourseId,
            course?.Name ?? "Unknown",
            course?.Category ?? LicenseCategory.B,
            contract.GroupId,
            groupName,
            contract.Status,
            contract.TotalCost.Amount,
            contract.PaidAmount.Amount,
            contract.DebtAmount.Amount,
            contract.QualityIndicator,
            contract.PracticeHoursCompleted,
            contract.TheoryLessonsAttended,
            contract.SignedAt,
            contract.StartDate,
            contract.EndDate,
            contract.TerminationReason,
            contract.TerminatedAt,
            contract.BranchId,
            paymentDtos));
    }
}