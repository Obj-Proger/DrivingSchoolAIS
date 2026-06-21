using DrivingSchool.Application.Features.Users.DTOs;

namespace DrivingSchool.Application.Features.Users.Queries.GetMyProfile;

/// <summary>
/// Returns the extended profile of the currently authenticated user,
/// including identifiers of all their contracts.
/// </summary>
public sealed record GetMyProfileQuery : IQuery<UserProfileDto>;