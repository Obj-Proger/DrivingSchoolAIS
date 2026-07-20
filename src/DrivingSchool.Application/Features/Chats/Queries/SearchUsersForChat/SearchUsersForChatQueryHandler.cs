using DrivingSchool.Application.Features.Chats.DTOs;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Chats.Queries.SearchUsersForChat;

/// <summary>Handles <see cref="SearchUsersForChatQuery"/>.</summary>
internal sealed class SearchUsersForChatQueryHandler
    : IQueryHandler<SearchUsersForChatQuery, IReadOnlyList<UserSearchResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public SearchUsersForChatQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<IReadOnlyList<UserSearchResultDto>>> Handle(
        SearchUsersForChatQuery query,
        CancellationToken cancellationToken)
    {
        var paginated = await _unitOfWork.Users.GetPaginatedAsync(
            page: 1,
            pageSize: 20,
            search: query.Search,
            role: null,
            isActive: true,
            cancellationToken);

        var dtos = paginated.Items
            .Where(u => u.Id != _currentUser.UserId)
            .Select(u => new UserSearchResultDto(
                u.Id,
                u.FullName.DisplayName,
                u.PhotoUrl,
                u.Role))
            .ToList();

        return Result.Success<IReadOnlyList<UserSearchResultDto>>(dtos);
    }
}