namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Represents the lifecycle stage of a <see cref="DrivingSchool.Domain.Entities.StaffTask"/>.
/// </summary>
public enum TaskItemStatus
{
    /// <summary>The task has been created but work has not yet begun.</summary>
    New = 1,

    /// <summary>The task is actively being worked on.</summary>
    InProgress = 2,

    /// <summary>The task has been completed.</summary>
    Done = 3
}