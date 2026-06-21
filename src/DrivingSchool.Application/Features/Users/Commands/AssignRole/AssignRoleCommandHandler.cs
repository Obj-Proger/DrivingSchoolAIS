using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Users.Commands.AssignRole;

/// <summary>
/// Handles <see cref="AssignRoleCommand"/>.
/// </summary>
internal sealed class AssignRoleCommandHandler : ICommandHandler<AssignRoleCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignRoleCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        AssignRoleCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users
            .GetByIdAsync(command.UserId, cancellationToken);

        if (user is null)
            return Result.Failure(DomainErrors.User.NotFound);

        user.AssignRole(command.NewRole);

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}