namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="PracticeBooking"/> aggregate.
/// </summary>
public interface IPracticeBookingRepository
{
    /// <summary>Returns the booking with the specified identifier, or <c>null</c> if not found.</summary>
    Task<PracticeBooking?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns the confirmed booking associated with the specified slot,
    /// or <c>null</c> if the slot has no active booking.
    /// </summary>
    Task<PracticeBooking?> GetBySlotIdAsync(Guid slotId, CancellationToken ct = default);

    /// <summary>
    /// Returns all bookings for the specified contract,
    /// ordered by slot start time descending.
    /// </summary>
    Task<IReadOnlyList<PracticeBooking>> GetByContractIdAsync(
        Guid contractId,
        BookingStatus? status = null,
        CancellationToken ct = default);

    /// <summary>
    /// Returns all bookings for slots owned by the specified instructor
    /// within the given date range.
    /// </summary>
    Task<IReadOnlyList<PracticeBooking>> GetByInstructorAsync(
        Guid instructorId,
        DateTime from,
        DateTime to,
        CancellationToken ct = default);

    /// <summary>
    /// Returns <c>true</c> if the student (contract) already has a confirmed booking
    /// that overlaps the specified time range.
    /// </summary>
    Task<bool> HasOverlapAsync(
        Guid contractId,
        DateTime start,
        DateTime end,
        CancellationToken ct = default);

    /// <summary>Adds a new booking to the repository.</summary>
    Task AddAsync(PracticeBooking booking, CancellationToken ct = default);

    /// <summary>Marks an existing booking as modified.</summary>
    void Update(PracticeBooking booking);

    /// <summary>
    /// Returns per-instructor workload statistics for bookings whose slot
    /// starts within the specified date range.
    /// </summary>
    Task<IReadOnlyList<InstructorWorkloadStats>> GetInstructorWorkloadAsync(
        DateTime from,
        DateTime to,
        CancellationToken ct = default);
}

/// <summary>Aggregate workload statistics for a single instructor.</summary>
public sealed record InstructorWorkloadStats(
    Guid InstructorId,
    int TotalBookings,
    int CompletedBookings,
    int CancelledBookings,
    int TotalHoursLogged,
    double? AverageRating);