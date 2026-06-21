using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Users.Commands.DeactivateUser;

/// <summary>
/// Handles <see cref="DeactivateUserCommand"/>.
/// </summary>
internal sealed class DeactivateUserCommandHandler : ICommandHandler<DeactivateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateUserCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeactivateUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users
            .GetByIdAsync(command.UserId, cancellationToken);

        if (user is null)
            return Result.Failure(DomainErrors.User.NotFound);

        var result = user.Deactivate();
        if (result.IsFailure) return result;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}