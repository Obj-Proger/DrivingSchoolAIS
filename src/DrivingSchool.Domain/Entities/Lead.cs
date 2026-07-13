using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.ValueObjects;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a prospective student (lead) in the CRM pipeline.
/// Tracks the lead's origin, current status, and progression towards a contract.
/// </summary>
public sealed class Lead : BaseEntity
{
    private readonly List<LeadNote> _notes = [];

    private Lead() { } // Required by EF Core

    /// <summary>Gets the lead's full name.</summary>
    public FullName FullName { get; private set; } = null!;

    /// <summary>Gets the lead's phone number.</summary>
    public PhoneNumber Phone { get; private set; } = null!;

    /// <summary>Gets the lead's email address, or <c>null</c> if not provided.</summary>
    public Email? Email { get; private set; }

    /// <summary>Gets the channel through which the lead was acquired.</summary>
    public LeadSource Source { get; private set; }

    /// <summary>Gets the lead's current position in the sales pipeline.</summary>
    public LeadStatus Status { get; private set; }

    /// <summary>
    /// Gets the identifier of the manager responsible for this lead,
    /// or <c>null</c> if not yet assigned.
    /// </summary>
    public Guid? ResponsibleManagerId { get; private set; }

    /// <summary>
    /// Gets the identifier of the contract created from this lead,
    /// or <c>null</c> if not yet converted.
    /// </summary>
    public Guid? ContractId { get; private set; }

    /// <summary>Gets the driving licence category the lead is interested in, or <c>null</c>.</summary>
    public LicenseCategory? CourseInterest { get; private set; }

    /// <summary>Gets an optional free-text comment about the lead.</summary>
    public string? Comment { get; private set; }

    /// <summary>
    /// Gets the identifier of the branch this lead was acquired for,
    /// or <c>null</c> if not yet assigned to a specific branch.
    /// </summary>
    public Guid? BranchId { get; private set; }

    /// <summary>Gets the chronological list of notes added by staff members.</summary>
    public IReadOnlyList<LeadNote> Notes => _notes.AsReadOnly();

    // Factory

    /// <summary>
    /// Registers a new lead in the CRM system.
    /// The initial status is always <see cref="LeadStatus.New"/>.
    /// </summary>
    /// <param name="fullName">The lead's full name.</param>
    /// <param name="phone">The lead's phone number.</param>
    /// <param name="source">The acquisition channel.</param>
    /// <param name="email">The lead's email address (optional).</param>
    /// <param name="courseInterest">The licence category the lead is interested in (optional).</param>
    /// <param name="comment">An optional free-text comment.</param>
    /// <param name="responsibleManagerId">The manager to assign immediately (optional).</param>
    /// <param name="branchId">The branch this lead was acquired for (optional).</param>
    public static Lead Create(
        FullName fullName,
        PhoneNumber phone,
        LeadSource source,
        Email? email = null,
        LicenseCategory? courseInterest = null,
        string? comment = null,
        Guid? responsibleManagerId = null,
        Guid? branchId = null)
        => new()
        {
            FullName = fullName,
            Phone = phone,
            Source = source,
            Email = email,
            CourseInterest = courseInterest,
            Comment = comment,
            ResponsibleManagerId = responsibleManagerId,
            BranchId = branchId,
            Status = LeadStatus.New
        };

    // Behaviour

    /// <summary>
    /// Updates the lead's contact and interest information.
    /// </summary>
    public void Update(
        FullName fullName,
        PhoneNumber phone,
        Email? email,
        LicenseCategory? courseInterest,
        string? comment)
    {
        FullName = fullName;
        Phone = phone;
        Email = email;
        CourseInterest = courseInterest;
        Comment = comment;
    }

    /// <summary>Assigns the lead to a branch.</summary>
    /// <param name="branchId">The identifier of the branch.</param>
    public void AssignBranch(Guid branchId) => BranchId = branchId;

    /// <summary>
    /// Assigns the lead to a manager.
    /// </summary>
    /// <param name="managerId">The identifier of the responsible manager.</param>
    public void AssignManager(Guid managerId)
    {
        ResponsibleManagerId = managerId;

        if (Status == LeadStatus.New)
            Status = LeadStatus.InProgress;
    }

    /// <summary>
    /// Updates the lead's pipeline status.
    /// Cannot change the status of a lead that has already been converted or lost.
    /// </summary>
    /// <param name="newStatus">The target status.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Lead.AlreadyInTerminalState"/>.
    /// </returns>
    public Result UpdateStatus(LeadStatus newStatus)
    {
        if (Status is LeadStatus.ConvertedToContract or LeadStatus.Lost)
            return Result.Failure(DomainErrors.Lead.AlreadyInTerminalState);

        Status = newStatus;
        return Result.Success();
    }

    /// <summary>
    /// Marks the lead as converted and records the resulting contract identifier.
    /// This is a terminal operation — a converted lead cannot change status again.
    /// </summary>
    /// <param name="contractId">The identifier of the contract created from this lead.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Lead.AlreadyInTerminalState"/>.
    /// </returns>
    public Result ConvertToContract(Guid contractId)
    {
        if (Status is LeadStatus.ConvertedToContract or LeadStatus.Lost)
            return Result.Failure(DomainErrors.Lead.AlreadyInTerminalState);

        ContractId = contractId;
        Status = LeadStatus.ConvertedToContract;
        return Result.Success();
    }

    /// <summary>Marks the lead as lost with an optional reason recorded as a note.</summary>
    /// <param name="reason">The reason for losing the lead (optional).</param>
    /// <param name="authorId">The identifier of the staff member marking the lead as lost.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Lead.AlreadyInTerminalState"/>.
    /// </returns>
    public Result MarkAsLost(string? reason, Guid authorId)
    {
        if (Status is LeadStatus.ConvertedToContract or LeadStatus.Lost)
            return Result.Failure(DomainErrors.Lead.AlreadyInTerminalState);

        Status = LeadStatus.Lost;

        if (!string.IsNullOrWhiteSpace(reason))
            _notes.Add(LeadNote.Create(Id, $"[Lost] {reason}", authorId));

        return Result.Success();
    }

    /// <summary>Adds a note to the lead's history.</summary>
    /// <param name="text">The note content.</param>
    /// <param name="authorId">The identifier of the note's author.</param>
    public void AddNote(string text, Guid authorId)
        => _notes.Add(LeadNote.Create(Id, text, authorId));
}