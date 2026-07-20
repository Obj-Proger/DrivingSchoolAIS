using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Chats.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace DrivingSchool.Application.Features.Chats.Commands.SendMessage;

/// <summary>Handles <see cref="SendMessageCommand"/>.</summary>
internal sealed class SendMessageCommandHandler : ICommandHandler<SendMessageCommand, MessageDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IEncryptionService _encryptionService;
    private readonly IChatNotifier _chatNotifier;
    private readonly IConfiguration _configuration;

    public SendMessageCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser,
        IEncryptionService encryptionService,
        IChatNotifier chatNotifier,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _encryptionService = encryptionService;
        _chatNotifier = chatNotifier;
        _configuration = configuration;
    }

    public async Task<Result<MessageDto>> Handle(
        SendMessageCommand command,
        CancellationToken cancellationToken)
    {
        var chat = await _unitOfWork.Chats.GetByIdAsync(command.ChatId, cancellationToken);
        if (chat is null)
            return Result.Failure<MessageDto>(DomainErrors.Chat.NotFound);

        if (!chat.CanUserWrite(_currentUser.UserId))
            return Result.Failure<MessageDto>(DomainErrors.Chat.CannotWrite);

        var masterKey = _configuration["ENCRYPTION_MASTER_KEY"]
            ?? throw new InvalidOperationException(
                "ENCRYPTION_MASTER_KEY environment variable is not set.");

        var key = _encryptionService.DeriveKey(masterKey, command.ChatId);
        var encrypted = _encryptionService.Encrypt(command.Text, key);

        var message = Message.CreateText(
            command.ChatId,
            _currentUser.UserId,
            encrypted.CipherText,
            encrypted.IV,
            encrypted.AuthTag);

        await _unitOfWork.Chats.AddMessageAsync(message, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var sender = await _unitOfWork.Users.GetByIdAsync(_currentUser.UserId, cancellationToken);

        var payload = new ChatMessagePayload(
            message.Id,
            message.ChatId,
            message.SenderId,
            sender?.FullName.DisplayName,
            sender?.PhotoUrl,
            command.Text,
            message.ContentType,
            message.FileUrl,
            message.FileName,
            message.SystemAction,
            message.ActionPayload,
            message.SentAt);

        await _chatNotifier.NotifyMessageSentAsync(command.ChatId, payload, cancellationToken);

        return Result.Success(new MessageDto(
            message.Id,
            message.ChatId,
            message.SenderId,
            sender?.FullName.DisplayName,
            sender?.PhotoUrl,
            command.Text,
            message.ContentType,
            message.FileUrl,
            message.FileName,
            message.FileSizeBytes,
            message.SystemAction,
            message.ActionPayload,
            message.SentAt,
            message.IsDelivered,
            IsReadByMe: true));
    }
}