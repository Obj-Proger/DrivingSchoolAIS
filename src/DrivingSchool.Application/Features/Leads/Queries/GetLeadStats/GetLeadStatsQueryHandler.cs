using DrivingSchool.Application.Features.Leads.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Leads.Queries.GetLeadStats;

/// <summary>
/// Handles <see cref="GetLeadStatsQuery"/>.
/// Loads all leads and computes groupings in memory.
/// For large datasets this should be replaced with a dedicated SQL aggregation query.
/// </summary>
internal sealed class GetLeadStatsQueryHandler
    : IQueryHandler<GetLeadStatsQuery, LeadStatsDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLeadStatsQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<LeadStatsDto>> Handle(
        GetLeadStatsQuery query,
        CancellationToken cancellationToken)
    {
        var allLeads = await _unitOfWork.Leads
            .GetPaginatedAsync(1, int.MaxValue, ct: cancellationToken);

        var leads = allLeads.Items;
        var total = leads.Count;

        var bySource = leads
            .GroupBy(l => l.Source)
            .Select(g => new LeadCountBySource(g.Key, g.Count()))
            .OrderByDescending(x => x.Count)
            .ToList();

        var byStatus = leads
            .GroupBy(l => l.Status)
            .Select(g => new LeadCountByStatus(g.Key, g.Count()))
            .ToList();

        var converted = leads.Count(l => l.Status == LeadStatus.ConvertedToContract);
        var conversionRate = total > 0
            ? Math.Round((decimal)converted / total * 100, 1)
            : 0m;

        return Result.Success(new LeadStatsDto(
            total,
            bySource,
            byStatus,
            conversionRate));
    }
}