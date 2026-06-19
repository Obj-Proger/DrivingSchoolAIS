using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Events;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a student's booking of a practice slot.
/// The booking aggregate manages the lesson lifecycle from confirmation
/// through completion and student rating.
/// </summary>
public sealed class PracticeBooking : BaseEntity
{
    private readonly List<SkillRating> _skillRatings = [];

    private PracticeBooking() { } // Required by EF Core

    /// <summary>Gets the identifier of the booked practice slot.</summary>
    public Guid SlotId { get; private set; }

    /// <summary>
    /// Gets the identifier of the student's contract.
    /// Bookings are linked to contracts rather than user accounts directly.
    /// </summary>
    public Guid ContractId { get; private set; }

    /// <summary>Gets the UTC timestamp when the booking was created.</summary>
    public DateTime BookedAt { get; private set; }

    /// <summary>
    /// Gets the identifier of the training ground selected by the student,
    /// or <c>null</c> if the default ground is used.
    /// </summary>
    public Guid? SelectedTrainingGroundId { get; private set; }

    /// <summary>Gets the current status of the booking.</summary>
    public BookingStatus Status { get; private set; }

    // Post-completion fields (set by instructor)

    /// <summary>
    /// Gets the identifier of the driving route used during this lesson,
    /// or <c>null</c> if not recorded.
    /// </summary>
    public Guid? RouteId { get; private set; }

    /// <summary>Gets an optional note from the instructor about the lesson.</summary>
    public string? InstructorNote { get; private set; }

    /// <summary>Gets the number of practice hours logged for this lesson.</summary>
    public int? PracticeHoursLogged { get; private set; }

    /// <summary>Gets the skill assessments recorded by the instructor.</summary>
    public IReadOnlyList<SkillRating> SkillRatings => _skillRatings.AsReadOnly();

    // Post-rating fields (set by student)

    /// <summary>
    /// Gets the student's rating for this lesson (1–5),
    /// or <c>null</c> if not yet rated.
    /// </summary>
    public int? StudentRating { get; private set; }

    /// <summary>Gets the student's optional written review of the lesson.</summary>
    public string? StudentReview { get; private set; }

    // Factory

    /// <summary>
    /// Creates a confirmed booking for a practice slot.
    /// Raises <see cref="BookingCreatedEvent"/> to notify the instructor.
    /// </summary>
    /// <param name="slotId">The identifier of the slot being booked.</param>
    /// <param name="contractId">The student's contract identifier.</param>
    /// <param name="selectedTrainingGroundId">
    /// The student's preferred training ground, or <c>null</c> to use the slot default.
    /// </param>
    public static PracticeBooking Create(
        Guid slotId,
        Guid contractId,
        Guid? selectedTrainingGroundId = null)
    {
        var booking = new PracticeBooking
        {
            SlotId = slotId,
            ContractId = contractId,
            BookedAt = DateTime.UtcNow,
            SelectedTrainingGroundId = selectedTrainingGroundId,
            Status = BookingStatus.Confirmed
        };

        booking.RaiseDomainEvent(new BookingCreatedEvent(booking.Id, slotId, contractId));

        return booking;
    }

    // Cancellation

    /// <summary>
    /// Cancels the booking on behalf of the student.
    /// Raises <see cref="BookingCancelledEvent"/> to release the slot and notify the instructor.
    /// </summary>
    /// <param name="reason">An optional reason for cancellation.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.PracticeBooking.CannotCancel"/>.
    /// </returns>
    public Result CancelByStudent(string? reason = null)
    {
        if (Status != BookingStatus.Confirmed)
            return Result.Failure(DomainErrors.PracticeBooking.CannotCancel);

        Status = BookingStatus.CancelledByStudent;

        RaiseDomainEvent(new BookingCancelledEvent(
            Id, SlotId, ContractId, CancelledByStudent: true, reason));

        return Result.Success();
    }

    /// <summary>
    /// Cancels the booking on behalf of the instructor.
    /// Raises <see cref="BookingCancelledEvent"/> to release the slot and notify the student.
    /// </summary>
    /// <param name="reason">An optional reason for cancellation.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.PracticeBooking.CannotCancel"/>.
    /// </returns>
    public Result CancelByInstructor(string? reason = null)
    {
        if (Status != BookingStatus.Confirmed)
            return Result.Failure(DomainErrors.PracticeBooking.CannotCancel);

        Status = BookingStatus.CancelledByInstructor;

        RaiseDomainEvent(new BookingCancelledEvent(
            Id, SlotId, ContractId, CancelledByStudent: false, reason));

        return Result.Success();
    }

    // Completion

    /// <summary>
    /// Marks the booking as completed after the lesson has been delivered.
    /// Records skill ratings, route, instructor note, and hours logged.
    /// Raises <see cref="BookingCompletedEvent"/> to update the contract and
    /// send a rating request to the student.
    /// </summary>
    /// <param name="skillRatings">The instructor's skill assessments.</param>
    /// <param name="practiceHoursLogged">The number of hours driven.</param>
    /// <param name="routeId">The identifier of the route driven (optional).</param>
    /// <param name="instructorNote">An optional note for the student (optional).</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.PracticeBooking.CannotComplete"/>.
    /// </returns>
    public Result Complete(
        IEnumerable<SkillRating> skillRatings,
        int practiceHoursLogged,
        Guid? routeId = null,
        string? instructorNote = null)
    {
        if (Status != BookingStatus.Confirmed)
            return Result.Failure(DomainErrors.PracticeBooking.CannotComplete);

        Status = BookingStatus.Completed;
        RouteId = routeId;
        InstructorNote = instructorNote;
        PracticeHoursLogged = practiceHoursLogged;

        _skillRatings.AddRange(skillRatings);

        RaiseDomainEvent(new BookingCompletedEvent(Id, ContractId, practiceHoursLogged));

        return Result.Success();
    }

    // Rating

    /// <summary>
    /// Records the student's rating and optional review for the completed lesson.
    /// Raises <see cref="PracticeRatedEvent"/> to update the contract's quality indicator.
    /// </summary>
    /// <param name="rating">The student's rating (1–5).</param>
    /// <param name="review">An optional written review.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.PracticeBooking.CannotRate"/>,
    /// <see cref="DomainErrors.PracticeBooking.AlreadyRated"/>,
    /// or <see cref="DomainErrors.PracticeBooking.InvalidRating"/>.
    /// </returns>
    public Result Rate(int rating, string? review = null)
    {
        if (Status != BookingStatus.Completed)
            return Result.Failure(DomainErrors.PracticeBooking.CannotRate);

        if (StudentRating.HasValue)
            return Result.Failure(DomainErrors.PracticeBooking.AlreadyRated);

        if (rating is < 1 or > 5)
            return Result.Failure(DomainErrors.PracticeBooking.InvalidRating);

        StudentRating = rating;
        StudentReview = review;

        RaiseDomainEvent(new PracticeRatedEvent(Id, ContractId, rating));

        return Result.Success();
    }
}