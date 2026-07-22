using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Practice.Commands.DeleteDrivingRoute;

/// <summary>Handles <see cref="DeleteDrivingRouteCommand"/>.</summary>
internal sealed class DeleteDrivingRouteCommandHandler
    : ICommandHandler<DeleteDrivingRouteCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public DeleteDrivingRouteCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        DeleteDrivingRouteCommand command,
        CancellationToken cancellationToken)
    {
        var route = await _unitOfWork.DrivingRoutes
            .GetByIdAsync(command.RouteId, cancellationToken);

        if (route is null)
            return Result.Failure(DomainErrors.DrivingRoute.NotFound);

        if (route.InstructorId != _currentUser.UserId)
            return Result.Failure(DomainErrors.DrivingRoute.CannotModifyOthers);

        _unitOfWork.DrivingRoutes.Delete(route);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}