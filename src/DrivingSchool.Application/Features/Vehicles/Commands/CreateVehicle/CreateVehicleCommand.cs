using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Vehicles.Commands.CreateVehicle;

/// <summary>Registers a new training vehicle. Restricted to administrators.</summary>
public sealed record CreateVehicleCommand(
    string PlateNumber,
    string Model,
    int Year,
    LicenseCategory Category) : ICommand<Guid>;