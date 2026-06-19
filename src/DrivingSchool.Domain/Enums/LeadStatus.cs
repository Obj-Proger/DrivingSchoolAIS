namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Represents the current stage of a lead in the sales pipeline.
/// </summary>
public enum LeadStatus
{
    /// <summary>The lead has just been registered and is awaiting assignment.</summary>
    New = 1,

    /// <summary>A manager is actively working with the lead.</summary>
    InProgress = 2,

    /// <summary>The lead has been successfully converted into a contract. Terminal state.</summary>
    ConvertedToContract = 3,

    /// <summary>The lead did not result in a contract. Terminal state.</summary>
    Lost = 4
}