using DrivingSchool.Application.Features.Vehicles.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Vehicles.Queries.GetVehicles;

/// <summary>Handles <see cref="GetVehiclesQuery"/>.</summary>
internal sealed class GetVehiclesQueryHandler
    : IQueryHandler<GetVehiclesQuery, IReadOnlyList<VehicleDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetVehiclesQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<VehicleDto>>> Handle(
        GetVehiclesQuery query,
        CancellationToken cancellationToken)
    {
        var vehicles = await _unitOfWork.Vehicles
            .GetAllAsync(query.ActiveOnly, cancellationToken);

        var dtos = vehicles
            .Select(v => new VehicleDto(
                v.Id,
                v.PlateNumber,
                v.Model,
                v.Year,
                v.Category,
                v.IsActive))
            .OrderBy(v => v.PlateNumber)
            .ToList();

        return Result.Success<IReadOnlyList<VehicleDto>>(dtos);
    }
}