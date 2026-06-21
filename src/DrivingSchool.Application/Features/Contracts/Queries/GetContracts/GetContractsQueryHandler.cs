using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Contracts.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Contracts.Queries.GetContracts;

/// <summary>
/// Handles <see cref="GetContractsQuery"/>.
/// </summary>
internal sealed class GetContractsQueryHandler
    : IQueryHandler<GetContractsQuery, PaginatedResult<ContractDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetContractsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<PaginatedResult<ContractDto>>> Handle(
        GetContractsQuery query,
        CancellationToken cancellationToken)
    {
        var paginated = await _unitOfWork.Contracts.GetPaginatedAsync(
            query.Page,
            query.PageSize,
            query.Status,
            query.StudentId,
            query.HasDebt,
            cancellationToken);

        var dtos = new List<ContractDto>();

        foreach (var contract in paginated.Items)
        {
            var dto = await MapToDtoAsync(contract, cancellationToken);
            dtos.Add(dto);
        }

        return Result.Success(new PaginatedResult<ContractDto>(
            dtos, paginated.TotalCount, paginated.Page, paginated.PageSize));
    }

    internal async Task<ContractDto> MapToDtoAsync(
        Contract contract,
        CancellationToken ct)
    {
        var student = await _unitOfWork.Users.GetByIdAsync(contract.StudentId, ct);

        string? groupName = null;
        if (contract.GroupId.HasValue)
        {
            var group = await _unitOfWork.Groups.GetByIdAsync(contract.GroupId.Value, ct);
            groupName = group?.Name;
        }

        // Course name requires a Course repository — for now we use the CourseId label
        // Full implementation resolves via ICourseRepository in Infrastructure
        return new ContractDto(
            contract.Id,
            contract.Number,
            contract.StudentId,
            student?.FullName.DisplayName ?? "Unknown",
            student?.Phone.Value ?? string.Empty,
            contract.CourseId,
            string.Empty, // resolved in Infrastructure
            LicenseCategory.B, // resolved in Infrastructure
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
            contract.TerminatedAt);
    }
}