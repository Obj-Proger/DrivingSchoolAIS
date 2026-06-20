namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Records that a specific user has read a specific message.
/// Uses a composite key of (<see cref="MessageId"/>, <see cref="UserId"/>).
/// </summary>
public sealed class MessageRead
{
    private MessageRead() { } // Required by EF Core

    /// <summary>Gets the identifier of the message that was read.</summary>
    public Guid MessageId { get; private set; }

    /// <summary>Gets the identifier of the user who read the message.</summary>
    public Guid UserId { get; private set; }

    /// <summary>Gets the UTC timestamp when the message was read.</summary>
    public DateTime ReadAt { get; private set; }

    /// <summary>Creates a new read receipt.</summary>
    /// <param name="messageId">The read message identifier.</param>
    /// <param name="userId">The reading user identifier.</param>
    public static MessageRead Create(Guid messageId, Guid userId)
        => new()
        {
            MessageId = messageId,
            UserId = userId,
            ReadAt = DateTime.UtcNow
        };
}