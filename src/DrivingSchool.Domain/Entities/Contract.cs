using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Events;
using DrivingSchool.Domain.ValueObjects;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a training contract between the driving school and a student.
/// The contract is the central entity of the student's learning journey —
/// group membership, lesson attendance, payments, and exam results are all
/// associated with a specific contract rather than the student account directly.
/// This allows a single student to hold multiple contracts for different licence categories.
/// </summary>
public sealed class Contract : BaseEntity
{
    private Contract() { } // Required by EF Core

    /// <summary>Gets the unique human-readable contract number (e.g. "ДГ-2024-001").</summary>
    public string Number { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the student party to this contract.</summary>
    public Guid StudentId { get; private set; }

    /// <summary>Gets the identifier of the course the student is enrolled in.</summary>
    public Guid CourseId { get; private set; }

    /// <summary>
    /// Gets the identifier of the training group the student has been assigned to,
    /// or <c>null</c> if not yet assigned.
    /// </summary>
    public Guid? GroupId { get; private set; }

    /// <summary>Gets the UTC timestamp when the contract was signed.</summary>
    public DateTime SignedAt { get; private set; }

    /// <summary>Gets the UTC date when training is scheduled to begin.</summary>
    public DateTime StartDate { get; private set; }

    /// <summary>Gets the UTC date when training is scheduled to end.</summary>
    public DateTime EndDate { get; private set; }

    /// <summary>Gets the current status of the contract.</summary>
    public ContractStatus Status { get; private set; }

    /// <summary>Gets the total cost of the training programme.</summary>
    public Money TotalCost { get; private set; } = null!;

    /// <summary>Gets the total amount paid by the student to date.</summary>
    public Money PaidAmount { get; private set; } = null!;

    /// <summary>
    /// Gets the outstanding debt (i.e. <see cref="TotalCost"/> minus <see cref="PaidAmount"/>).
    /// </summary>
    public Money DebtAmount { get; private set; } = null!;

    /// <summary>
    /// Gets the rolling quality indicator for this contract (1–5).
    /// Updated after each practice lesson rating. <c>null</c> before the first rating.
    /// </summary>
    public int? QualityIndicator { get; private set; }

    /// <summary>Gets the total number of practical driving hours completed.</summary>
    public int PracticeHoursCompleted { get; private set; }

    /// <summary>Gets the total number of theory lessons attended.</summary>
    public int TheoryLessonsAttended { get; private set; }

    /// <summary>Gets the reason for early termination, or <c>null</c> for active contracts.</summary>
    public string? TerminationReason { get; private set; }

    /// <summary>Gets the UTC timestamp when the contract was terminated, or <c>null</c>.</summary>
    public DateTime? TerminatedAt { get; private set; }

    /// <summary>
    /// Gets the identifier of the branch this contract was signed at,
    /// or <c>null</c> if not assigned to a specific branch.
    /// </summary>
    public Guid? BranchId { get; private set; }

    // Factory

    /// <summary>
    /// Creates a new active contract.
    /// </summary>
    /// <param name="number">The unique contract number.</param>
    /// <param name="studentId">The student's identifier.</param>
    /// <param name="courseId">The enrolled course identifier.</param>
    /// <param name="startDate">The training start date.</param>
    /// <param name="endDate">The scheduled training end date.</param>
    /// <param name="totalCost">The total cost of the programme.</param>
    /// <param name="branchId">The branch this contract was signed at (optional).</param>
    /// <returns>
    /// A successful <see cref="Result{Contract}"/>, or a failure describing
    /// which validation rule was violated.
    /// </returns>
    public static Result<Contract> Create(
        string number,
        Guid studentId,
        Guid courseId,
        DateTime startDate,
        DateTime endDate,
        Money totalCost,
        Guid? branchId = null)
    {
        if (string.IsNullOrWhiteSpace(number))
            return Result.Failure<Contract>(DomainErrors.Contract.NumberEmpty);

        if (endDate <= startDate)
            return Result.Failure<Contract>(DomainErrors.Contract.InvalidDateRange);

        return Result.Success(new Contract
        {
            Number = number.Trim(),
            StudentId = studentId,
            CourseId = courseId,
            StartDate = startDate,
            EndDate = endDate,
            TotalCost = totalCost,
            PaidAmount = Money.Zero(),
            DebtAmount = totalCost,
            Status = ContractStatus.Active,
            SignedAt = DateTime.UtcNow,
            PracticeHoursCompleted = 0,
            TheoryLessonsAttended = 0,
            BranchId = branchId
        });
    }

    // Financial

    /// <summary>
    /// Records a confirmed payment and recalculates the outstanding debt.
    /// </summary>
    /// <param name="paymentAmount">The confirmed payment amount.</param>
    public void RecordPayment(Money paymentAmount)
    {
        PaidAmount = PaidAmount.Add(paymentAmount);

        var remainingAmount = TotalCost.Amount - PaidAmount.Amount;
        DebtAmount = remainingAmount > 0
            ? Money.Create(remainingAmount).Value
            : Money.Zero();
    }

    // Training Progress

    /// <summary>
    /// Increments the practice hours counter after a completed driving lesson.
    /// </summary>
    /// <param name="hours">The number of hours to add. Defaults to 1.</param>
    public void RegisterPracticeHours(int hours = 1) =>
        PracticeHoursCompleted += hours;

    /// <summary>Increments the theory attendance counter after a completed theory lesson.</summary>
    public void RegisterTheoryAttendance() =>
        TheoryLessonsAttended++;

    /// <summary>
    /// Records a student's practice lesson rating and updates the rolling quality indicator.
    /// Raises <see cref="LowRatingReceivedEvent"/> when the rating is below 4.
    /// </summary>
    /// <param name="rating">The rating submitted by the student (1–5).</param>
    public void RecordRating(int rating)
    {
        QualityIndicator = QualityIndicator.HasValue
            ? (int)Math.Round((QualityIndicator.Value + rating) / 2.0)
            : rating;

        if (rating < 4)
            RaiseDomainEvent(new LowRatingReceivedEvent(Id, StudentId, rating));
    }

    // Group Assignment

    /// <summary>Assigns the student to a training group.</summary>
    /// <param name="groupId">The identifier of the group.</param>
    public void AssignToGroup(Guid groupId) => GroupId = groupId;

    /// <summary>Assigns the contract to a branch.</summary>
    /// <param name="branchId">The identifier of the branch.</param>
    public void AssignBranch(Guid branchId) => BranchId = branchId;

    // Status Management

    /// <summary>
    /// Marks the contract as successfully completed.
    /// </summary>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Contract.AlreadyClosed"/>.
    /// </returns>
    public Result Complete()
    {
        if (Status != ContractStatus.Active)
            return Result.Failure(DomainErrors.Contract.AlreadyClosed);

        Status = ContractStatus.Completed;
        return Result.Success();
    }

    /// <summary>
    /// Terminates the contract early with a recorded reason.
    /// </summary>
    /// <param name="reason">The reason for early termination.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Contract.AlreadyClosed"/>.
    /// </returns>
    public Result Terminate(string reason)
    {
        if (Status != ContractStatus.Active)
            return Result.Failure(DomainErrors.Contract.AlreadyClosed);

        Status = ContractStatus.Terminated;
        TerminationReason = reason;
        TerminatedAt = DateTime.UtcNow;

        return Result.Success();
    }
}