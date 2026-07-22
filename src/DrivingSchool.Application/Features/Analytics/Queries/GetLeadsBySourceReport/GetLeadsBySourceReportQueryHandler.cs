using DrivingSchool.Application.Features.Analytics.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Analytics.Queries.GetLeadsBySourceReport;

/// <summary>Handles <see cref="GetLeadsBySourceReportQuery"/>.</summary>
internal sealed class GetLeadsBySourceReportQueryHandler
    : IQueryHandler<GetLeadsBySourceReportQuery, IReadOnlyList<LeadSourceReportItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLeadsBySourceReportQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<LeadSourceReportItemDto>>> Handle(
        GetLeadsBySourceReportQuery query,
        CancellationToken cancellationToken)
    {
        var stats = await _unitOfWork.Leads
            .GetStatsBySourceAsync(query.From, query.To, cancellationToken);

        var dtos = stats
            .Select(s => new LeadSourceReportItemDto(
                s.Source,
                s.TotalCount,
                s.ConvertedCount,
                s.TotalCount == 0
                    ? 0
                    : Math.Round(100.0 * s.ConvertedCount / s.TotalCount, 1)))
            .OrderByDescending(s => s.TotalCount)
            .ToList();

        return Result.Success<IReadOnlyList<LeadSourceReportItemDto>>(dtos);
    }
}