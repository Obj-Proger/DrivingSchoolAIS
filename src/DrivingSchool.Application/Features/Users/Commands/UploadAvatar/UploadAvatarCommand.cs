using DrivingSchool.Application.Common;

namespace DrivingSchool.Application.Features.Users.Commands.UploadAvatar;

/// <summary>
/// Uploads a new profile photo for the specified user and updates their profile.
/// Returns the public URL of the stored photo.
/// </summary>
public sealed record UploadAvatarCommand(
    Guid UserId,
    Stream FileStream,
    string FileName,
    string ContentType) : ICommand<string>;