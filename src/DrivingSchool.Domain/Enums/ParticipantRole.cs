namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Defines the role of a user within a chat.
/// </summary>
public enum ParticipantRole
{
    /// <summary>
    /// May post in any chat type, manage participants, and delete any message.
    /// </summary>
    Admin = 1,

    /// <summary>
    /// May post in <see cref="ChatType.Direct"/> and <see cref="ChatType.Group"/> chats.
    /// </summary>
    Member = 2
}