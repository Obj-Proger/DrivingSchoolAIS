using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Vehicles.Commands.DeactivateVehicle;

/// <summary>Handles <see cref="DeactivateVehicleCommand"/>.</summary>
internal sealed class DeactivateVehicleCommandHandler : ICommandHandler<DeactivateVehicleCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateVehicleCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeactivateVehicleCommand command,
        CancellationToken cancellationToken)
    {
        var vehicle = await _unitOfWork.Vehicles
            .GetByIdAsync(command.VehicleId, cancellationToken);

        if (vehicle is null)
            return Result.Failure(DomainErrors.Vehicle.NotFound);

        vehicle.Deactivate();

        _unitOfWork.Vehicles.Update(vehicle);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}