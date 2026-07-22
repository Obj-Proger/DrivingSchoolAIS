using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Practice.Commands.UpdateDrivingRoute;

/// <summary>Handles <see cref="UpdateDrivingRouteCommand"/>.</summary>
internal sealed class UpdateDrivingRouteCommandHandler
    : ICommandHandler<UpdateDrivingRouteCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public UpdateDrivingRouteCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        UpdateDrivingRouteCommand command,
        CancellationToken cancellationToken)
    {
        var route = await _unitOfWork.DrivingRoutes
            .GetByIdAsync(command.RouteId, cancellationToken);

        if (route is null)
            return Result.Failure(DomainErrors.DrivingRoute.NotFound);

        if (route.InstructorId != _currentUser.UserId)
            return Result.Failure(DomainErrors.DrivingRoute.CannotModifyOthers);

        route.Update(command.Name, command.Description, command.MapData);

        _unitOfWork.DrivingRoutes.Update(route);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}