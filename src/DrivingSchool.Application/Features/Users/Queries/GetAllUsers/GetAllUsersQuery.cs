using DrivingSchool.Application.Common;
using DrivingSchool.Application.Features.Users.DTOs;

namespace DrivingSchool.Application.Features.Users.Queries.GetAllUsers;

/// <summary>
/// Returns a paginated list of users with optional search and filter parameters.
/// Accessible to managers and administrators.
/// </summary>
public sealed record GetAllUsersQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    UserRole? Role = null,
    bool? IsActive = null) : IQuery<PaginatedResult<UserDto>>;