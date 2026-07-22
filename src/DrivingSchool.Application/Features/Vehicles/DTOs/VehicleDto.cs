namespace DrivingSchool.Application.Features.Vehicles.DTOs;

/// <summary>Represents a training vehicle.</summary>
public sealed record VehicleDto(
    Guid Id,
    string PlateNumber,
    string Model,
    int Year,
    LicenseCategory Category,
    bool IsActive);