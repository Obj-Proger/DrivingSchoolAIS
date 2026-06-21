using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Users.Commands.UploadAvatar;

/// <summary>
/// Handles <see cref="UploadAvatarCommand"/>.
/// Stores the image in the file storage service and updates the user's photo URL.
/// </summary>
internal sealed class UploadAvatarCommandHandler : ICommandHandler<UploadAvatarCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorage;

    public UploadAvatarCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorage)
    {
        _unitOfWork = unitOfWork;
        _fileStorage = fileStorage;
    }

    public async Task<Result<string>> Handle(
        UploadAvatarCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users
            .GetByIdAsync(command.UserId, cancellationToken);

        if (user is null)
            return Result.Failure<string>(DomainErrors.User.NotFound);

        var uploadResult = await _fileStorage.UploadAsync(
            command.FileStream,
            command.FileName,
            command.ContentType,
            folder: "avatars",
            cancellationToken);

        user.UpdateProfile(user.FullName, user.Phone, uploadResult.FileUrl);

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(uploadResult.FileUrl);
    }
}