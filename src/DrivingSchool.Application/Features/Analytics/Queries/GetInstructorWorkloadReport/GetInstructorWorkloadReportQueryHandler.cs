using DrivingSchool.Application.Features.Analytics.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Analytics.Queries.GetInstructorWorkloadReport;

/// <summary>Handles <see cref="GetInstructorWorkloadReportQuery"/>.</summary>
internal sealed class GetInstructorWorkloadReportQueryHandler
    : IQueryHandler<GetInstructorWorkloadReportQuery, IReadOnlyList<InstructorWorkloadItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInstructorWorkloadReportQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<InstructorWorkloadItemDto>>> Handle(
        GetInstructorWorkloadReportQuery query,
        CancellationToken cancellationToken)
    {
        var stats = await _unitOfWork.PracticeBookings
            .GetInstructorWorkloadAsync(query.From, query.To, cancellationToken);

        var dtos = new List<InstructorWorkloadItemDto>();
        foreach (var stat in stats)
        {
            var instructor = await _unitOfWork.Users
                .GetByIdAsync(stat.InstructorId, cancellationToken);

            dtos.Add(new InstructorWorkloadItemDto(
                stat.InstructorId,
                instructor?.FullName.DisplayName ?? "Unknown",
                stat.TotalBookings,
                stat.CompletedBookings,
                stat.CancelledBookings,
                stat.TotalHoursLogged,
                stat.AverageRating));
        }

        return Result.Success<IReadOnlyList<InstructorWorkloadItemDto>>(
            dtos.OrderByDescending(d => d.TotalBookings).ToList());
    }
}