using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Auth.Commands.ConfirmEmail;

/// <summary>
/// Handles <see cref="ConfirmEmailCommand"/>.
/// Finds the user by their confirmation token via the repository
/// and delegates confirmation logic to the domain aggregate.
/// </summary>
internal sealed class ConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmEmailCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        ConfirmEmailCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users
            .GetByEmailConfirmationTokenAsync(command.Token, cancellationToken);

        if (user is null)
            return Result.Failure(DomainErrors.Auth.InvalidToken);

        var confirmResult = user.ConfirmEmail(command.Token);
        if (confirmResult.IsFailure) return confirmResult;

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}