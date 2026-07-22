using DrivingSchool.Application.Features.Analytics.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Analytics.Queries.GetStaffActivityReport;

/// <summary>Handles <see cref="GetStaffActivityReportQuery"/>.</summary>
internal sealed class GetStaffActivityReportQueryHandler
    : IQueryHandler<GetStaffActivityReportQuery, IReadOnlyList<StaffActivityItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStaffActivityReportQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<StaffActivityItemDto>>> Handle(
        GetStaffActivityReportQuery query,
        CancellationToken cancellationToken)
    {
        var stats = await _unitOfWork.StaffTasks
            .GetActivityStatsAsync(query.From, query.To, cancellationToken);

        var dtos = new List<StaffActivityItemDto>();
        foreach (var stat in stats)
        {
            var user = await _unitOfWork.Users
                .GetByIdAsync(stat.AssignedToId, cancellationToken);

            dtos.Add(new StaffActivityItemDto(
                stat.AssignedToId,
                user?.FullName.DisplayName ?? "Unknown",
                user?.Role ?? default,
                stat.TotalCount,
                stat.CompletedCount,
                stat.OverdueCount));
        }

        return Result.Success<IReadOnlyList<StaffActivityItemDto>>(
            dtos.OrderByDescending(d => d.TasksAssigned).ToList());
    }
}