using DrivingSchool.Application.Features.Users.DTOs;

namespace DrivingSchool.Application.Features.Users.Queries.GetUserById;

/// <summary>
/// Returns the full profile of a single user by their identifier.
/// </summary>
public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserDto>;