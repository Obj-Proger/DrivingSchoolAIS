using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Vehicles.Commands.DeactivateVehicle;

/// <summary>Deactivates a vehicle, removing it from availability for practice slots.</summary>
public sealed record DeactivateVehicleCommand(Guid VehicleId) : ICommand;