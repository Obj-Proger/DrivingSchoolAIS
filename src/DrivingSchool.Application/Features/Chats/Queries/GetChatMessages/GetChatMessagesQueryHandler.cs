using DrivingSchool.Application.Features.Chats.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace DrivingSchool.Application.Features.Chats.Queries.GetChatMessages;

/// <summary>Handles <see cref="GetChatMessagesQuery"/>.</summary>
internal sealed class GetChatMessagesQueryHandler
    : IQueryHandler<GetChatMessagesQuery, IReadOnlyList<MessageDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IEncryptionService _encryptionService;
    private readonly IConfiguration _configuration;

    public GetChatMessagesQueryHandler(
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

    public async Task<Result<IReadOnlyList<MessageDto>>> Handle(
        GetChatMessagesQuery query,
        CancellationToken cancellationToken)
    {
        var isParticipant = await _unitOfWork.Chats
            .IsParticipantAsync(query.ChatId, _currentUser.UserId, cancellationToken);

        if (!isParticipant)
            return Result.Failure<IReadOnlyList<MessageDto>>(DomainErrors.Chat.CannotWrite);

        var messages = await _unitOfWork.Chats.GetMessagesAsync(
            query.ChatId, query.Before, query.Limit, cancellationToken);

        var masterKey = _configuration["ENCRYPTION_MASTER_KEY"]
            ?? throw new InvalidOperationException(
                "ENCRYPTION_MASTER_KEY environment variable is not set.");

        var key = _encryptionService.DeriveKey(masterKey, query.ChatId);

        var senderCache = new Dictionary<Guid, User?>();
        var dtos = new List<MessageDto>();

        foreach (var message in messages)
        {
            User? sender = null;
            if (message.SenderId.HasValue)
            {
                if (!senderCache.TryGetValue(message.SenderId.Value, out sender))
                {
                    sender = await _unitOfWork.Users
                        .GetByIdAsync(message.SenderId.Value, cancellationToken);
                    senderCache[message.SenderId.Value] = sender;
                }
            }

            var content = message.IsDeleted
                ? "Message deleted"
                : DecryptContent(message, key);

            dtos.Add(new MessageDto(
                message.Id,
                message.ChatId,
                message.SenderId,
                sender?.FullName.DisplayName,
                sender?.PhotoUrl,
                content,
                message.ContentType,
                message.IsDeleted ? null : message.FileUrl,
                message.IsDeleted ? null : message.FileName,
                message.FileSizeBytes,
                message.SystemAction,
                message.ActionPayload,
                message.SentAt,
                message.IsDelivered,
                message.ReadBy.Any(r => r.UserId == _currentUser.UserId)));
        }

        return Result.Success<IReadOnlyList<MessageDto>>(dtos);
    }

    /// <summary>
    /// Decrypts a message's content, returning a placeholder instead of throwing
    /// if the ciphertext cannot be authenticated (e.g. corrupted or legacy data).
    /// </summary>
    private string DecryptContent(Message message, byte[] key)
    {
        try
        {
            return _encryptionService.Decrypt(
                new EncryptedData(message.EncryptedContent, message.IV, message.AuthTag), key);
        }
        catch (System.Security.Cryptography.CryptographicException)
        {
            return "[Unable to decrypt message]";
        }
    }
}