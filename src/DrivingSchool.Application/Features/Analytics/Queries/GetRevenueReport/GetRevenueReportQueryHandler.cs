using DrivingSchool.Application.Features.Analytics.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Analytics.Queries.GetRevenueReport;

/// <summary>Handles <see cref="GetRevenueReportQuery"/>.</summary>
internal sealed class GetRevenueReportQueryHandler
    : IQueryHandler<GetRevenueReportQuery, RevenueReportDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRevenueReportQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<RevenueReportDto>> Handle(
        GetRevenueReportQuery query,
        CancellationToken cancellationToken)
    {
        var byMonth = await _unitOfWork.Payments
            .GetRevenueByMonthAsync(query.From, query.To, cancellationToken);

        var monthDtos = byMonth
            .OrderBy(m => m.Year).ThenBy(m => m.Month)
            .Select(m => new MonthlyRevenueDto(m.Year, m.Month, m.Total))
            .ToList();

        return Result.Success(new RevenueReportDto(
            monthDtos.Sum(m => m.Total),
            monthDtos));
    }
}