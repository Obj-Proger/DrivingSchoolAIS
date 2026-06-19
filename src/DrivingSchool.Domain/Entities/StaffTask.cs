using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a task assigned by a manager to a staff member.
/// Tasks can optionally be linked to a domain entity such as a lead or contract,
/// and can be configured to recur on specific days of the week.
/// </summary>
public sealed class StaffTask : BaseEntity
{
    private StaffTask() { } // Required by EF Core

    /// <summary>Gets the short title of the task.</summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>Gets the optional detailed description of the task.</summary>
    public string? Description { get; private set; }

    /// <summary>Gets the identifier of the staff member responsible for completing the task.</summary>
    public Guid AssignedToId { get; private set; }

    /// <summary>Gets the identifier of the manager who created the task.</summary>
    public Guid CreatedById { get; private set; }

    /// <summary>Gets the optional UTC deadline for the task.</summary>
    public DateTime? DueDate { get; private set; }

    /// <summary>Gets the current status of the task.</summary>
    public TaskItemStatus Status { get; private set; }

    /// <summary>Gets the priority level of the task.</summary>
    public TaskPriority Priority { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this task repeats automatically
    /// on the days specified in <see cref="RecurringDays"/>.
    /// </summary>
    public bool IsRecurring { get; private set; }

    /// <summary>
    /// Gets the days of the week on which a recurring task is automatically recreated.
    /// Empty when <see cref="IsRecurring"/> is <c>false</c>.
    /// </summary>
    public List<DayOfWeek> RecurringDays { get; private set; } = [];

    /// <summary>
    /// Gets the type name of the entity this task is linked to
    /// (e.g. <c>"Lead"</c>, <c>"Contract"</c>), or <c>null</c> if unlinked.
    /// </summary>
    public string? LinkedEntityType { get; private set; }

    /// <summary>
    /// Gets the identifier of the linked entity, or <c>null</c> if unlinked.
    /// </summary>
    public Guid? LinkedEntityId { get; private set; }

    // Factory

    /// <summary>
    /// Creates a new one-time task assigned to a staff member.
    /// </summary>
    public static StaffTask Create(
        string title,
        string? description,
        Guid assignedToId,
        Guid createdById,
        TaskPriority priority,
        DateTime? dueDate = null,
        string? linkedEntityType = null,
        Guid? linkedEntityId = null)
        => new()
        {
            Title = title,
            Description = description,
            AssignedToId = assignedToId,
            CreatedById = createdById,
            Priority = priority,
            DueDate = dueDate,
            Status = TaskItemStatus.New,
            IsRecurring = false,
            LinkedEntityType = linkedEntityType,
            LinkedEntityId = linkedEntityId
        };

    /// <summary>
    /// Creates a new recurring task that repeats on the specified days of the week.
    /// </summary>
    public static StaffTask CreateRecurring(
        string title,
        string? description,
        Guid assignedToId,
        Guid createdById,
        TaskPriority priority,
        IEnumerable<DayOfWeek> recurringDays)
        => new()
        {
            Title = title,
            Description = description,
            AssignedToId = assignedToId,
            CreatedById = createdById,
            Priority = priority,
            Status = TaskItemStatus.New,
            IsRecurring = true,
            RecurringDays = recurringDays.Distinct().ToList()
        };

    // Behaviour

    /// <summary>Transitions the task to <see cref="TaskItemStatus.InProgress"/>.</summary>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.StaffTask.AlreadyCompleted"/>.
    /// </returns>
    public Result Start()
    {
        if (Status == TaskItemStatus.Done)
            return Result.Failure(DomainErrors.StaffTask.AlreadyCompleted);

        Status = TaskItemStatus.InProgress;
        return Result.Success();
    }

    /// <summary>Marks the task as completed.</summary>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.StaffTask.AlreadyCompleted"/>.
    /// </returns>
    public Result Complete()
    {
        if (Status == TaskItemStatus.Done)
            return Result.Failure(DomainErrors.StaffTask.AlreadyCompleted);

        Status = TaskItemStatus.Done;
        return Result.Success();
    }

    /// <summary>Reopens a completed task, resetting its status to <see cref="TaskItemStatus.New"/>.</summary>
    public void Reopen() => Status = TaskItemStatus.New;

    /// <summary>Updates the task's priority.</summary>
    /// <param name="priority">The new priority level.</param>
    public void UpdatePriority(TaskPriority priority) => Priority = priority;

    /// <summary>Updates the task's deadline.</summary>
    /// <param name="dueDate">The new deadline, or <c>null</c> to remove it.</param>
    public void UpdateDueDate(DateTime? dueDate) => DueDate = dueDate;

    /// <summary>Updates the task's title and description.</summary>
    public void Update(string title, string? description)
    {
        Title = title;
        Description = description;
    }
}