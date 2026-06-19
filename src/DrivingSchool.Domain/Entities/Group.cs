using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a training group — a cohort of students studying the same course
/// under the same teacher with a shared schedule.
/// </summary>
public sealed class Group : BaseEntity
{
    private readonly List<GroupMember> _members = [];

    private Group() { } // Required by EF Core

    /// <summary>Gets the display name of the group (e.g. "B-2024-03").</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the course this group is studying.</summary>
    public Guid CourseId { get; private set; }

    /// <summary>Gets the identifier of the teacher responsible for this group's theory lessons.</summary>
    public Guid TeacherId { get; private set; }

    /// <summary>Gets the UTC date when the group's training begins.</summary>
    public DateTime StartDate { get; private set; }

    /// <summary>Gets the UTC date when the group's training ends, or <c>null</c> if not scheduled.</summary>
    public DateTime? EndDate { get; private set; }

    /// <summary>Gets the maximum number of students allowed in the group.</summary>
    public int MaxStudents { get; private set; }

    /// <summary>Gets the current lifecycle status of the group.</summary>
    public GroupStatus Status { get; private set; }

    /// <summary>Gets the current members of the group.</summary>
    public IReadOnlyList<GroupMember> Members => _members.AsReadOnly();

    /// <summary>
    /// Gets a value indicating whether a new member can be added.
    /// Requires the group to be in a recruiting status and below maximum capacity.
    /// </summary>
    public bool CanAddMember =>
        _members.Count < MaxStudents &&
        Status is GroupStatus.Forming or GroupStatus.Active;

    // Factory

    /// <summary>
    /// Creates a new group in the <see cref="GroupStatus.Forming"/> state.
    /// </summary>
    /// <param name="name">The group's display name.</param>
    /// <param name="courseId">The course the group will study.</param>
    /// <param name="teacherId">The responsible teacher.</param>
    /// <param name="startDate">The training start date.</param>
    /// <param name="maxStudents">The maximum number of members.</param>
    /// <param name="endDate">The optional scheduled end date.</param>
    /// <returns>
    /// A successful <see cref="Result{Group}"/>,
    /// or a failure with <see cref="DomainErrors.Group.InvalidCapacity"/>.
    /// </returns>
    public static Result<Group> Create(
        string name,
        Guid courseId,
        Guid teacherId,
        DateTime startDate,
        int maxStudents,
        DateTime? endDate = null)
    {
        if (maxStudents <= 0)
            return Result.Failure<Group>(DomainErrors.Group.InvalidCapacity);

        return Result.Success(new Group
        {
            Name = name,
            CourseId = courseId,
            TeacherId = teacherId,
            StartDate = startDate,
            EndDate = endDate,
            MaxStudents = maxStudents,
            Status = GroupStatus.Forming
        });
    }

    // Behaviour

    /// <summary>
    /// Enrolls a student (identified by their contract) in the group.
    /// </summary>
    /// <param name="contractId">The contract identifier of the student to add.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Group.Full"/>,
    /// or <see cref="DomainErrors.Group.AlreadyMember"/>.
    /// </returns>
    public Result AddMember(Guid contractId)
    {
        if (!CanAddMember)
            return Result.Failure(DomainErrors.Group.Full);

        if (_members.Any(m => m.ContractId == contractId))
            return Result.Failure(DomainErrors.Group.AlreadyMember);

        _members.Add(GroupMember.Create(Id, contractId));
        return Result.Success();
    }

    /// <summary>
    /// Removes a student from the group.
    /// </summary>
    /// <param name="contractId">The contract identifier of the student to remove.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Group.MemberNotFound"/>.
    /// </returns>
    public Result RemoveMember(Guid contractId)
    {
        var member = _members.FirstOrDefault(m => m.ContractId == contractId);

        if (member is null)
            return Result.Failure(DomainErrors.Group.MemberNotFound);

        _members.Remove(member);
        return Result.Success();
    }

    /// <summary>Updates the group's name, responsible teacher, and capacity.</summary>
    public void Update(string name, Guid teacherId, int maxStudents, DateTime? endDate)
    {
        Name = name;
        TeacherId = teacherId;
        MaxStudents = maxStudents;
        EndDate = endDate;
    }

    /// <summary>Transitions the group to <see cref="GroupStatus.Active"/>.</summary>
    public void Activate() => Status = GroupStatus.Active;

    /// <summary>
    /// Archives the group, recording the end date if not already set.
    /// </summary>
    public void Archive()
    {
        Status = GroupStatus.Archived;
        EndDate ??= DateTime.UtcNow;
    }
}