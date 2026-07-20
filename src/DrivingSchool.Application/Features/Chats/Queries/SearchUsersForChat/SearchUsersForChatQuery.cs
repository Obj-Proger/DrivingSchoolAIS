using DrivingSchool.Application.Features.Chats.DTOs;

namespace DrivingSchool.Application.Features.Chats.Queries.SearchUsersForChat;

/// <summary>
/// Searches active users by name, email, or phone to pick a recipient for a
/// new chat. Excludes the current user from the results.
/// </summary>
public sealed record SearchUsersForChatQuery(string Search)
    : IQuery<IReadOnlyList<UserSearchResultDto>>;