namespace DrivingSchool.Application.Features.Branches.DTOs;

/// <summary>
/// Represents a physical office (location) of the driving school.
/// </summary>
public sealed record BranchDto(
    Guid Id,
    string Name,
    string City,
    string Address,
    string Phone,
    bool IsActive);