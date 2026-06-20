using DrivingSchool.Domain.Common;
using DrivingSchool.Domain.Enums;

namespace DrivingSchool.Domain.Entities;

/// <summary>
/// Represents a message sent within a <see cref="Chat"/>.
/// </summary>
/// <remarks>
/// <para>
/// All text content is stored encrypted using AES-256-GCM.
/// The plain text never touches the database.
/// The per-message encryption key is derived via HKDF from the master key and the chat identifier.
/// </para>
/// <para>
/// Messages are not included in the <see cref="Chat"/> aggregate's in-memory collection;
/// they are retrieved independently via the repository using cursor-based pagination.
/// </para>
/// </remarks>
public sealed class Message : BaseEntity
{
    private readonly List<MessageRead> _readBy = [];

    private Message() { } // Required by EF Core

    /// <summary>Gets the identifier of the chat this message belongs to.</summary>
    public Guid ChatId { get; private set; }

    /// <summary>
    /// Gets the identifier of the user who sent the message,
    /// or <c>null</c> for automated system messages.
    /// </summary>
    public Guid? SenderId { get; private set; }

    // Encrypted payload

    /// <summary>
    /// Gets the AES-256-GCM encrypted message text, encoded as a Base64 string.
    /// For file messages this field contains the encrypted file name.
    /// </summary>
    public string EncryptedContent { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the 96-bit (12-byte) random initialisation vector used for encryption,
    /// encoded as a Base64 string. Unique per message.
    /// </summary>
    public string IV { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the 128-bit GCM authentication tag that ensures ciphertext integrity,
    /// encoded as a Base64 string.
    /// </summary>
    public string AuthTag { get; private set; } = string.Empty;

    // Content metadata

    /// <summary>Gets the type of content this message carries.</summary>
    public MessageContentType ContentType { get; private set; }

    /// <summary>Gets the storage URL of the attached file, or <c>null</c> for text messages.</summary>
    public string? FileUrl { get; private set; }

    /// <summary>Gets the original file name, or <c>null</c> for text messages.</summary>
    public string? FileName { get; private set; }

    /// <summary>Gets the file size in bytes, or <c>null</c> for text messages.</summary>
    public long? FileSizeBytes { get; private set; }

    // System message fields

    /// <summary>
    /// Gets the domain action this system message represents,
    /// used by the client to render the appropriate action button.
    /// <c>null</c> for user messages.
    /// </summary>
    public SystemAction? SystemAction { get; private set; }

    /// <summary>
    /// Gets a JSON payload containing the entity identifiers required
    /// to navigate to the relevant screen when the action button is tapped.
    /// <c>null</c> for user messages.
    /// </summary>
    public string? ActionPayload { get; private set; }

    // Delivery and state

    /// <summary>Gets the UTC timestamp when the message was sent.</summary>
    public DateTime SentAt { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the message has been delivered
    /// to at least one recipient.
    /// </summary>
    public bool IsDelivered { get; private set; }

    /// <summary>Gets a value indicating whether the message has been soft-deleted.</summary>
    public bool IsDeleted { get; private set; }

    /// <summary>Gets the list of users who have read this message.</summary>
    public IReadOnlyList<MessageRead> ReadBy => _readBy.AsReadOnly();

    // Factories

    /// <summary>
    /// Creates an encrypted text message sent by a user.
    /// </summary>
    /// <param name="chatId">The target chat identifier.</param>
    /// <param name="senderId">The sender's user identifier.</param>
    /// <param name="encryptedContent">The AES-256-GCM encrypted text (Base64).</param>
    /// <param name="iv">The initialisation vector (Base64, 12 bytes).</param>
    /// <param name="authTag">The GCM authentication tag (Base64, 16 bytes).</param>
    public static Message CreateText(
        Guid chatId,
        Guid senderId,
        string encryptedContent,
        string iv,
        string authTag)
        => new()
        {
            ChatId = chatId,
            SenderId = senderId,
            EncryptedContent = encryptedContent,
            IV = iv,
            AuthTag = authTag,
            ContentType = MessageContentType.Text,
            SentAt = DateTime.UtcNow
        };

    /// <summary>
    /// Creates an encrypted file or image message sent by a user.
    /// </summary>
    /// <param name="chatId">The target chat identifier.</param>
    /// <param name="senderId">The sender's user identifier.</param>
    /// <param name="encryptedFileName">The AES-256-GCM encrypted original file name (Base64).</param>
    /// <param name="iv">The initialisation vector (Base64, 12 bytes).</param>
    /// <param name="authTag">The GCM authentication tag (Base64, 16 bytes).</param>
    /// <param name="fileUrl">The storage URL where the file can be downloaded.</param>
    /// <param name="originalFileName">The plain-text original file name for display purposes.</param>
    /// <param name="fileSizeBytes">The file size in bytes.</param>
    /// <param name="contentType">
    /// <see cref="MessageContentType.Image"/> or <see cref="MessageContentType.File"/>.
    /// </param>
    public static Message CreateFile(
        Guid chatId,
        Guid senderId,
        string encryptedFileName,
        string iv,
        string authTag,
        string fileUrl,
        string originalFileName,
        long fileSizeBytes,
        MessageContentType contentType)
        => new()
        {
            ChatId = chatId,
            SenderId = senderId,
            EncryptedContent = encryptedFileName,
            IV = iv,
            AuthTag = authTag,
            ContentType = contentType,
            FileUrl = fileUrl,
            FileName = originalFileName,
            FileSizeBytes = fileSizeBytes,
            SentAt = DateTime.UtcNow
        };

    /// <summary>
    /// Creates an automated system message with an optional interactive action payload.
    /// System messages have no sender and are generated by domain event handlers.
    /// </summary>
    /// <param name="chatId">The target chat identifier.</param>
    /// <param name="action">The domain action this message represents.</param>
    /// <param name="actionPayload">
    /// A JSON string containing the entity identifiers needed to build the action button deep link.
    /// </param>
    /// <param name="encryptedContent">An AES-256-GCM encrypted summary text (Base64).</param>
    /// <param name="iv">The initialisation vector (Base64, 12 bytes).</param>
    /// <param name="authTag">The GCM authentication tag (Base64, 16 bytes).</param>
    public static Message CreateSystem(
        Guid chatId,
        SystemAction action,
        string actionPayload,
        string encryptedContent,
        string iv,
        string authTag)
        => new()
        {
            ChatId = chatId,
            SenderId = null,
            EncryptedContent = encryptedContent,
            IV = iv,
            AuthTag = authTag,
            ContentType = MessageContentType.System,
            SystemAction = action,
            ActionPayload = actionPayload,
            SentAt = DateTime.UtcNow
        };

    // Behaviour

    /// <summary>
    /// Records that the specified user has read this message.
    /// Marks the message as delivered on the first read receipt.
    /// Silently ignores duplicate read receipts for the same user.
    /// </summary>
    /// <param name="userId">The user who read the message.</param>
    public void MarkReadBy(Guid userId)
    {
        if (_readBy.Any(r => r.UserId == userId))
            return;

        _readBy.Add(MessageRead.Create(Id, userId));
        IsDelivered = true;
    }

    /// <summary>
    /// Soft-deletes the message. The record is retained for audit purposes
    /// but the content is no longer returned by the repository.
    /// </summary>
    public void SoftDelete() => IsDeleted = true;
}