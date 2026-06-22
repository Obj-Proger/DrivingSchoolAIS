namespace DrivingSchool.Application.Features.Practice.DTOs;

/// <summary>
/// Represents a practice booking from the student's perspective.
/// </summary>
public sealed record BookingDto(
    Guid Id,
    Guid SlotId,
    Guid ContractId,
    Guid InstructorId,
    string InstructorName,
    string? InstructorPhotoUrl,
    DateTime StartDateTime,
    DateTime EndDateTime,
    Guid? SelectedTrainingGroundId,
    string? GroundName,
    string? VehicleModel,
    string? VehiclePlate,
    BookingStatus Status,
    int? StudentRating,
    string? StudentReview,
    Guid? RouteId,
    string? RouteName,
    string? InstructorNote,
    int? PracticeHoursLogged,
    IReadOnlyList<SkillRatingDto> SkillRatings,
    DateTime BookedAt);