using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Vehicles.Commands.UpdateVehicle;

/// <summary>Updates a vehicle's registration and model information.</summary>
public sealed record UpdateVehicleCommand(
    Guid VehicleId,
    string PlateNumber,
    string Model,
    int Year) : ICommand;