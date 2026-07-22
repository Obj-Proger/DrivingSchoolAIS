using DrivingSchool.Application.Features.Vehicles.DTOs;

namespace DrivingSchool.Application.Features.Vehicles.Queries.GetVehicles;

/// <summary>Returns all training vehicles, used to populate vehicle selection dropdowns.</summary>
public sealed record GetVehiclesQuery(bool ActiveOnly = false)
    : IQuery<IReadOnlyList<VehicleDto>>;