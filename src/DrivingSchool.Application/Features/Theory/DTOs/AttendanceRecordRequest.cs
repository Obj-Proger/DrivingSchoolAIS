namespace DrivingSchool.Application.Features.Theory.DTOs;

/// <summary>
/// Represents a single attendance entry submitted by the teacher.
/// </summary>
public sealed record AttendanceRecordRequest(
    Guid ContractId,
    bool IsPresent,
    string? Note = null);