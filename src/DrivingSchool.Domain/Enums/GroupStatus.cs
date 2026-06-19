namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Represents the lifecycle stage of a training group.
/// </summary>
public enum GroupStatus
{
    /// <summary>The group is being assembled and accepts new members.</summary>
    Forming = 1,

    /// <summary>The group is actively attending lessons.</summary>
    Active = 2,

    /// <summary>All lessons have been completed.</summary>
    Completed = 3,

    /// <summary>The group has been archived and is no longer visible by default.</summary>
    Archived = 4
}