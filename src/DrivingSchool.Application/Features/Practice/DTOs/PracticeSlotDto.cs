namespace DrivingSchool.Application.Features.Practice.DTOs;

/// <summary>
/// Summary view of a practice slot used in the schedule grid and booking list.
/// </summary>
public sealed record PracticeSlotDto(
    Guid Id,
    Guid InstructorId,
    string InstructorName,
    string? InstructorPhotoUrl,
    Guid? VehicleId,
    string? VehicleModel,
    string? VehiclePlate,
    Guid? DefaultTrainingGroundId,
    string? DefaultGroundName,
    DateTime StartDateTime,
    DateTime EndDateTime,
    int DurationMinutes,
    SlotType Type,
    SlotStatus Status,
    bool IsOpenForStudentGroundChoice,
    string? Note);