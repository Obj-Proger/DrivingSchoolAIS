using DrivingSchool.Application.Features.Chats.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace DrivingSchool.Application.Features.Chats.Queries.GetUserChats;

/// <summary>Handles <see cref="GetUserChatsQuery"/>.</summary>
internal sealed class GetUserChatsQueryHandler
    : IQueryHandler<GetUserChatsQuery, IReadOnlyList<ChatSummaryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IEncryptionService _encryptionService;
    private readonly IConfiguration _configuration;

    public GetUserChatsQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IEncryptionService encryptionService,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _encryptionService = encryptionService;
        _configuration = configuration;
    }

    public async Task<Result<IReadOnlyList<ChatSummaryDto>>> Handle(
        GetUserChatsQuery query,
        CancellationToken cancellationToken)
    {
        var chats = await _unitOfWork.Chats
            .GetUserChatsAsync(_currentUser.UserId, cancellationToken);

        var masterKey = _configuration["ENCRYPTION_MASTER_KEY"]
            ?? throw new InvalidOperationException(
                "ENCRYPTION_MASTER_KEY environment variable is not set.");

        var dtos = new List<ChatSummaryDto>();

        foreach (var chat in chats)
        {
            var participant = chat.Participants
                .FirstOrDefault(p => p.UserId == _currentUser.UserId);

            var name = chat.Name ?? "Chat";
            string? photoUrl = null;

            if (chat.Type == ChatType.Direct)
            {
                var otherUserId = chat.Participants
                    .Select(p => p.UserId)
                    .FirstOrDefault(id => id != _currentUser.UserId);

                var other = await _unitOfWork.Users.GetByIdAsync(otherUserId, cancellationToken);
                name = other?.FullName.DisplayName ?? "Unknown";
                photoUrl = other?.PhotoUrl;
            }

            var unreadCount = await _unitOfWork.Chats
                .GetUnreadCountAsync(chat.Id, _currentUser.UserId, cancellationToken);

            var lastMessages = await _unitOfWork.Chats
                .GetMessagesAsync(chat.Id, before: null, limit: 1, cancellationToken);
            var lastMessage = lastMessages.FirstOrDefault();

            string? preview = null;
            if (lastMessage is not null)
            {
                var key = _encryptionService.DeriveKey(masterKey, chat.Id);
                preview = TryDecryptPreview(lastMessage, key);
            }

            dtos.Add(new ChatSummaryDto(
                chat.Id,
                chat.Type,
                name,
                photoUrl,
                chat.IsSystem,
                participant?.IsMuted ?? false,
                unreadCount,
                preview,
                lastMessage?.SentAt));
        }

        return Result.Success<IReadOnlyList<ChatSummaryDto>>(
            dtos.OrderByDescending(c => c.LastMessageAt ?? DateTime.MinValue).ToList());
    }

    /// <summary>
    /// Decrypts a message for use as a list preview, returning a placeholder
    /// instead of throwing if the ciphertext cannot be authenticated (e.g. it
    /// was encrypted under a since-rotated key).
    /// </summary>
    private string TryDecryptPreview(Message message, byte[] key)
    {
        if (message.ContentType != MessageContentType.Text)
            return message.FileName ?? "Attachment";

        try
        {
            return _encryptionService.Decrypt(
                new EncryptedData(message.EncryptedContent, message.IV, message.AuthTag), key);
        }
        catch (System.Security.Cryptography.CryptographicException)
        {
            return "Message";
        }
    }
}