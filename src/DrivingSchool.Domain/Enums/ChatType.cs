namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Defines the structure and write-access rules of a chat.
/// </summary>
public enum ChatType
{
    /// <summary>
    /// A private conversation between exactly two users.
    /// Both participants may send messages.
    /// </summary>
    Direct = 1,

    /// <summary>
    /// A multi-participant conversation in which all members may send messages.
    /// </summary>
    Group = 2,

    /// <summary>
    /// A channel where only administrators may post.
    /// Replies from members are redirected to the administrator's direct messages.
    /// Typically used for school-wide announcements.
    /// </summary>
    BroadcastChannel = 3,

    /// <summary>
    /// A read-only channel. No participant may send messages directly.
    /// Used for automated system notifications.
    /// </summary>
    Channel = 4
}