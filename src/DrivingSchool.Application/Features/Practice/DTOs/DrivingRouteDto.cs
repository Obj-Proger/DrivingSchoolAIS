namespace DrivingSchool.Application.Features.Practice.DTOs;

/// <summary>
/// Represents a predefined driving route.
/// </summary>
public sealed record DrivingRouteDto(
    Guid Id,
    Guid InstructorId,
    string InstructorName,
    string Name,
    string Description,
    string? MapData,
    DateTime CreatedAt);