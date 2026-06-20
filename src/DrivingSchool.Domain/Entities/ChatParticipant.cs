using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a user's membership in a <see cref="Chat"/>.
/// Uses a composite key of (<see cref="ChatId"/>, <see cref="UserId"/>).
/// </summary>
public sealed class ChatParticipant
{
    private ChatParticipant() { } // Required by EF Core

    /// <summary>Gets the identifier of the chat.</summary>
    public Guid ChatId { get; private set; }

    /// <summary>Gets the identifier of the participating user.</summary>
    public Guid UserId { get; private set; }

    /// <summary>Gets the participant's role, which determines their write permissions.</summary>
    public ParticipantRole Role { get; private set; }

    /// <summary>Gets the UTC timestamp when the user joined the chat.</summary>
    public DateTime JoinedAt { get; private set; }

    /// <summary>
    /// Gets a value indicating whether push notifications are suppressed for this participant.
    /// </summary>
    public bool IsMuted { get; private set; }

    /// <summary>Creates a new participant record.</summary>
    /// <param name="chatId">The chat identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="role">The role to assign.</param>
    public static ChatParticipant Create(
        Guid chatId,
        Guid userId,
        ParticipantRole role = ParticipantRole.Member)
        => new()
        {
            ChatId = chatId,
            UserId = userId,
            Role = role,
            JoinedAt = DateTime.UtcNow,
            IsMuted = false
        };

    /// <summary>Suppresses push notifications for this participant.</summary>
    public void Mute() => IsMuted = true;

    /// <summary>Restores push notifications for this participant.</summary>
    public void Unmute() => IsMuted = false;

    /// <summary>Changes the participant's role.</summary>
    /// <param name="role">The new role.</param>
    public void ChangeRole(ParticipantRole role) => Role = role;
}