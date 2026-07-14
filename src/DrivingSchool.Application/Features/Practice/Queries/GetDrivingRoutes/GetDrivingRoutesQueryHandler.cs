using DrivingSchool.Application.Features.Practice.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Queries.GetDrivingRoutes;

/// <summary>Handles <see cref="GetDrivingRoutesQuery"/>.</summary>
internal sealed class GetDrivingRoutesQueryHandler
    : IQueryHandler<GetDrivingRoutesQuery, IReadOnlyList<DrivingRouteDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDrivingRoutesQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<DrivingRouteDto>>> Handle(
        GetDrivingRoutesQuery query,
        CancellationToken cancellationToken)
    {
        var instructor = await _unitOfWork.Users
            .GetByIdAsync(query.InstructorId, cancellationToken);

        if (instructor is null)
            return Result.Failure<IReadOnlyList<DrivingRouteDto>>(DomainErrors.User.NotFound);

        var routes = await _unitOfWork.DrivingRoutes
            .GetByInstructorIdAsync(query.InstructorId, cancellationToken);

        var dtos = routes
            .Select(r => new DrivingRouteDto(
                r.Id,
                r.InstructorId,
                instructor.FullName.DisplayName,
                r.Name,
                r.Description,
                r.MapData,
                r.CreatedAt))
            .ToList();

        return Result.Success<IReadOnlyList<DrivingRouteDto>>(dtos);
    }
}