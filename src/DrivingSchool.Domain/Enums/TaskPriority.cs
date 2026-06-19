namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Defines the urgency level of a <see cref="DrivingSchool.Domain.Entities.StaffTask"/>.
/// </summary>
public enum TaskPriority
{
    /// <summary>The task can be addressed whenever convenient.</summary>
    Low = 1,

    /// <summary>The task should be completed within the normal working schedule.</summary>
    Normal = 2,

    /// <summary>The task requires immediate attention.</summary>
    High = 3
}