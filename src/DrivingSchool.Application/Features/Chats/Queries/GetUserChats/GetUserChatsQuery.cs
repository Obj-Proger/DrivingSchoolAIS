using DrivingSchool.Application.Features.Chats.DTOs;

namespace DrivingSchool.Application.Features.Chats.Queries.GetUserChats;

/// <summary>
/// Returns the current user's conversation list, most recently active first.
/// </summary>
public sealed record GetUserChatsQuery : IQuery<IReadOnlyList<ChatSummaryDto>>;