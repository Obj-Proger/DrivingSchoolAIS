namespace DrivingSchool.Application.Features.Practice.DTOs;

/// <summary>
/// Represents a training ground available for practice bookings.
/// </summary>
public sealed record TrainingGroundDto(
    Guid Id,
    string Name,
    string Address,
    string? Description,
    bool IsActive,
    Guid? BranchId);