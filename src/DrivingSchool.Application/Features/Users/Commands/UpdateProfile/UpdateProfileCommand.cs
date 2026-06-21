using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Users.Commands.UpdateProfile;

/// <summary>
/// Updates the profile information of the specified user.
/// A user may only update their own profile; admins may update any profile.
/// </summary>
public sealed record UpdateProfileCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string? MiddleName,
    string Phone) : ICommand;