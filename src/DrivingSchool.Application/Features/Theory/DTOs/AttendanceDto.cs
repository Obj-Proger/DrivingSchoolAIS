namespace DrivingSchool.Application.Features.Theory.DTOs;

/// <summary>
/// Represents the attendance record of a single student at a theory lesson.
/// </summary>
public sealed record AttendanceDto(
    Guid ContractId,
    Guid StudentId,
    string StudentName,
    bool IsPresent,
    string? Note);