using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Users.Commands.UpdateProfile;

/// <summary>
/// Handles <see cref="UpdateProfileCommand"/>.
/// </summary>
internal sealed class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateProfileCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users
            .GetByIdAsync(command.UserId, cancellationToken);

        if (user is null)
            return Result.Failure(DomainErrors.User.NotFound);

        var fullNameResult = FullName.Create(
            command.FirstName, command.LastName, command.MiddleName);

        if (fullNameResult.IsFailure)
            return Result.Failure(fullNameResult.Error);

        var phoneResult = PhoneNumber.Create(command.Phone);
        if (phoneResult.IsFailure)
            return Result.Failure(phoneResult.Error);

        user.UpdateProfile(fullNameResult.Value, phoneResult.Value, user.PhotoUrl);

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}