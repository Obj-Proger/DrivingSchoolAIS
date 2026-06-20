namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Represents the current state of a payment record.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// The payment has been registered but not yet confirmed by a manager.
    /// </summary>
    Pending = 1,

    /// <summary>The payment has been confirmed and applied to the contract balance.</summary>
    Completed = 2,

    /// <summary>The payment was refunded to the student.</summary>
    Refunded = 3
}