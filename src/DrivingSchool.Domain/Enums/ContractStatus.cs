namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Represents the current state of a student's training contract.
/// </summary>
public enum ContractStatus
{
    /// <summary>The contract is active and the student is currently enrolled.</summary>
    Active = 1,

    /// <summary>The student has successfully completed all training requirements.</summary>
    Completed = 2,

    /// <summary>The contract was terminated before completion.</summary>
    Terminated = 3
}