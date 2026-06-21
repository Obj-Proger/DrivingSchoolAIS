namespace DrivingSchool.Application.Interfaces.Repositories;

/// <summary>
/// Provides data access operations for the <see cref="PracticeSlot"/> aggregate.
/// </summary>
public interface IPracticeSlotRepository
{
    /// <summary>Returns the slot with the specified identifier, or <c>null</c> if not found.</summary>
    Task<PracticeSlot?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Returns all available slots within the specified date range,
    /// with optional filtering by instructor and licence category.
    /// </summary>
    Task<IReadOnlyList<PracticeSlot>> GetAvailableAsync(
        DateTime from,
        DateTime to,
        Guid? instructorId = null,
        LicenseCategory? category = null,
        CancellationToken ct = default);

    /// <summary>
    /// Returns all slots created by the specified instructor
    /// within the given date range, ordered by start time ascending.
    /// </summary>
    Task<IReadOnlyList<PracticeSlot>> GetInstructorScheduleAsync(
        Guid instructorId,
        DateTime from,
        DateTime to,
        CancellationToken ct = default);

    /// <summary>
    /// Returns <c>true</c> if the instructor already has a slot that overlaps
    /// the specified time range, excluding the given slot identifier.
    /// Used to prevent double-booking.
    /// </summary>
    Task<bool> HasOverlapAsync(
        Guid instructorId,
        DateTime start,
        DateTime end,
        Guid? excludeSlotId = null,
        CancellationToken ct = default);

    /// <summary>Adds a new practice slot to the repository.</summary>
    Task AddAsync(PracticeSlot slot, CancellationToken ct = default);

    /// <summary>Adds multiple practice slots in a single operation.</summary>
    Task AddRangeAsync(IEnumerable<PracticeSlot> slots, CancellationToken ct = default);

    /// <summary>Marks an existing practice slot as modified.</summary>
    void Update(PracticeSlot slot);
}