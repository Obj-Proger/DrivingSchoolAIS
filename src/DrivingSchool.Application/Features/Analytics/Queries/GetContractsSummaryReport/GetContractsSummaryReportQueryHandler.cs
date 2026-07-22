using DrivingSchool.Application.Features.Analytics.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Analytics.Queries.GetContractsSummaryReport;

/// <summary>Handles <see cref="GetContractsSummaryReportQuery"/>.</summary>
internal sealed class GetContractsSummaryReportQueryHandler
    : IQueryHandler<GetContractsSummaryReportQuery, ContractsSummaryReportDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetContractsSummaryReportQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<ContractsSummaryReportDto>> Handle(
        GetContractsSummaryReportQuery query,
        CancellationToken cancellationToken)
    {
        var stats = await _unitOfWork.Contracts
            .GetStatsAsync(query.From, query.To, cancellationToken);

        // Contracts still Active have no decided outcome yet, so the completion
        // rate is measured only across contracts that have actually concluded.
        var concludedCount = stats.CompletedCount + stats.TerminatedCount;
        var completionRate = concludedCount == 0
            ? 0
            : Math.Round(100.0 * stats.CompletedCount / concludedCount, 1);

        return Result.Success(new ContractsSummaryReportDto(
            stats.TotalCount,
            stats.ActiveCount,
            stats.CompletedCount,
            stats.TerminatedCount,
            completionRate,
            stats.AverageQualityIndicator));
    }
}