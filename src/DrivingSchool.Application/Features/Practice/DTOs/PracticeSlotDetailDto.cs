namespace DrivingSchool.Application.Features.Practice.DTOs;

/// <summary>
/// Full slot view including the booking record if the slot is booked.
/// Used in the instructor's schedule grid.
/// </summary>
public sealed record PracticeSlotDetailDto(
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
    string? Note,
    BookingBriefDto? Booking);

/// <summary>
/// Lightweight booking summary embedded in slot detail views.
/// </summary>
public sealed record BookingBriefDto(
    Guid BookingId,
    Guid ContractId,
    Guid StudentId,
    string StudentName,
    string StudentPhone,
    BookingStatus Status);