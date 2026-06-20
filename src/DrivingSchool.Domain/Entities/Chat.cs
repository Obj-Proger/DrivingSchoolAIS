using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a conversation between one or more users.
/// The chat aggregate manages participant membership and write-access rules.
/// </summary>
/// <remarks>
/// Messages are not held in memory as part of this aggregate.
/// They are retrieved independently via the repository using cursor-based pagination
/// to avoid loading an unbounded collection.
/// </remarks>
public sealed class Chat : BaseEntity
{
    private readonly List<ChatParticipant> _participants = [];

    private Chat() { } // Required by EF Core

    /// <summary>Gets the type of this chat, which defines its write-access rules.</summary>
    public ChatType Type { get; private set; }

    /// <summary>
    /// Gets the display name of the chat, or <c>null</c> for direct messages
    /// (where the name is derived from the other participant's name at the client).
    /// </summary>
    public string? Name { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this chat was created by the system
    /// (e.g. a notification channel) rather than by a user.
    /// </summary>
    public bool IsSystem { get; private set; }

    /// <summary>
    /// Gets the identifier of the user who created this chat,
    /// or <c>null</c> for system-generated chats.
    /// </summary>
    public Guid? CreatedById { get; private set; }

    /// <summary>Gets the current participants of this chat.</summary>
    public IReadOnlyList<ChatParticipant> Participants => _participants.AsReadOnly();

    // Factories

    /// <summary>
    /// Creates a direct message chat between exactly two users.
    /// Both users are added as admins.
    /// </summary>
    /// <param name="initiatorId">The user who initiated the conversation.</param>
    /// <param name="recipientId">The other participant.</param>
    public static Chat CreateDirect(Guid initiatorId, Guid recipientId)
    {
        var chat = new Chat
        {
            Type = ChatType.Direct,
            CreatedById = initiatorId,
            IsSystem = false
        };

        chat._participants.Add(ChatParticipant.Create(chat.Id, initiatorId, ParticipantRole.Admin));
        chat._participants.Add(ChatParticipant.Create(chat.Id, recipientId, ParticipantRole.Admin));

        return chat;
    }

    /// <summary>
    /// Creates a group, broadcast channel, or read-only channel.
    /// </summary>
    /// <param name="type">Must be <see cref="ChatType.Group"/>, <see cref="ChatType.BroadcastChannel"/>, or <see cref="ChatType.Channel"/>.</param>
    /// <param name="name">The display name of the chat.</param>
    /// <param name="createdById">The creator's user identifier.</param>
    /// <param name="participantIds">Additional member identifiers (excluding the creator).</param>
    /// <returns>
    /// A successful <see cref="Result{Chat}"/>,
    /// or a failure with <see cref="DomainErrors.Chat.InvalidTypeForCreate"/>.
    /// </returns>
    public static Result<Chat> Create(
        ChatType type,
        string name,
        Guid createdById,
        IEnumerable<Guid> participantIds)
    {
        if (type == ChatType.Direct)
            return Result.Failure<Chat>(DomainErrors.Chat.InvalidTypeForCreate);

        var chat = new Chat
        {
            Type = type,
            Name = name.Trim(),
            CreatedById = createdById,
            IsSystem = false
        };

        chat._participants.Add(ChatParticipant.Create(chat.Id, createdById, ParticipantRole.Admin));

        foreach (var userId in participantIds.Where(id => id != createdById))
            chat._participants.Add(ChatParticipant.Create(chat.Id, userId, ParticipantRole.Member));

        return Result.Success(chat);
    }

    /// <summary>Creates a system-generated notification channel.</summary>
    /// <param name="name">The channel name.</param>
    public static Chat CreateSystem(string name)
        => new()
        {
            Type = ChatType.Channel,
            Name = name.Trim(),
            IsSystem = true
        };

    // Write access

    /// <summary>
    /// Determines whether the specified user is permitted to send messages in this chat.
    /// </summary>
    /// <param name="userId">The user requesting write access.</param>
    public bool CanUserWrite(Guid userId)
    {
        var participant = _participants.FirstOrDefault(p => p.UserId == userId);
        if (participant is null) return false;

        return Type switch
        {
            ChatType.Direct => true,
            ChatType.Group => true,
            ChatType.BroadcastChannel => participant.Role == ParticipantRole.Admin,
            ChatType.Channel => false,
            _ => false
        };
    }

    // Participant management

    /// <summary>
    /// Adds a user to the chat.
    /// </summary>
    /// <param name="userId">The user to add.</param>
    /// <param name="role">The role to assign. Defaults to <see cref="ParticipantRole.Member"/>.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Chat.AlreadyParticipant"/>.
    /// </returns>
    public Result AddParticipant(Guid userId, ParticipantRole role = ParticipantRole.Member)
    {
        if (_participants.Any(p => p.UserId == userId))
            return Result.Failure(DomainErrors.Chat.AlreadyParticipant);

        _participants.Add(ChatParticipant.Create(Id, userId, role));
        return Result.Success();
    }

    /// <summary>
    /// Removes a user from the chat.
    /// The last administrator cannot be removed to ensure the chat remains manageable.
    /// </summary>
    /// <param name="userId">The user to remove.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Chat.ParticipantNotFound"/>
    /// or <see cref="DomainErrors.Chat.CannotRemoveLastAdmin"/>.
    /// </returns>
    public Result RemoveParticipant(Guid userId)
    {
        var participant = _participants.FirstOrDefault(p => p.UserId == userId);

        if (participant is null)
            return Result.Failure(DomainErrors.Chat.ParticipantNotFound);

        var isLastAdmin =
            participant.Role == ParticipantRole.Admin &&
            _participants.Count(p => p.Role == ParticipantRole.Admin) == 1;

        if (isLastAdmin)
            return Result.Failure(DomainErrors.Chat.CannotRemoveLastAdmin);

        _participants.Remove(participant);
        return Result.Success();
    }

    /// <summary>Changes the role of an existing participant.</summary>
    /// <param name="userId">The participant whose role will be changed.</param>
    /// <param name="newRole">The new role to assign.</param>
    /// <returns>
    /// Success, or a failure with <see cref="DomainErrors.Chat.ParticipantNotFound"/>
    /// or <see cref="DomainErrors.Chat.CannotRemoveLastAdmin"/>.
    /// </returns>
    public Result ChangeParticipantRole(Guid userId, ParticipantRole newRole)
    {
        var participant = _participants.FirstOrDefault(p => p.UserId == userId);

        if (participant is null)
            return Result.Failure(DomainErrors.Chat.ParticipantNotFound);

        if (participant.Role == ParticipantRole.Admin &&
            newRole == ParticipantRole.Member &&
            _participants.Count(p => p.Role == ParticipantRole.Admin) == 1)
        {
            return Result.Failure(DomainErrors.Chat.CannotRemoveLastAdmin);
        }

        participant.ChangeRole(newRole);
        return Result.Success();
    }

    /// <summary>Updates the chat display name.</summary>
    /// <param name="name">The new name.</param>
    public void Rename(string name) => Name = name.Trim();
}