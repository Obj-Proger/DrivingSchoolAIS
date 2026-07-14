using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Practice.Commands.CreateDrivingRoute;

/// <summary>Handles <see cref="CreateDrivingRouteCommand"/>.</summary>
internal sealed class CreateDrivingRouteCommandHandler
    : ICommandHandler<CreateDrivingRouteCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateDrivingRouteCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(
        CreateDrivingRouteCommand command,
        CancellationToken cancellationToken)
    {
        var route = DrivingRoute.Create(
            _currentUser.UserId,
            command.Name,
            command.Description,
            command.MapData);

        await _unitOfWork.DrivingRoutes.AddAsync(route, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(route.Id);
    }
}