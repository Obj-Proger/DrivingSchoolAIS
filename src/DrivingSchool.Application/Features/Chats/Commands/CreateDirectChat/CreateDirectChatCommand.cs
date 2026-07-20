using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Chats.Commands.CreateDirectChat;

/// <summary>
/// Starts (or returns the existing) direct message chat between the current
/// user and the specified recipient. Idempotent — calling it again for the
/// same pair of users returns the same chat rather than creating a duplicate.
/// </summary>
public sealed record CreateDirectChatCommand(Guid RecipientId) : ICommand<Guid>;