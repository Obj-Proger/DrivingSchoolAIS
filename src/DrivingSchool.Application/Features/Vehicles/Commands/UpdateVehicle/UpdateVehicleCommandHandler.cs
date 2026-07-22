using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Vehicles.Commands.UpdateVehicle;

/// <summary>Handles <see cref="UpdateVehicleCommand"/>.</summary>
internal sealed class UpdateVehicleCommandHandler : ICommandHandler<UpdateVehicleCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateVehicleCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateVehicleCommand command,
        CancellationToken cancellationToken)
    {
        var vehicle = await _unitOfWork.Vehicles
            .GetByIdAsync(command.VehicleId, cancellationToken);

        if (vehicle is null)
            return Result.Failure(DomainErrors.Vehicle.NotFound);

        var plateExists = await _unitOfWork.Vehicles.ExistsByPlateNumberAsync(
            command.PlateNumber, excludeVehicleId: command.VehicleId, ct: cancellationToken);

        if (plateExists)
            return Result.Failure(DomainErrors.Vehicle.DuplicatePlateNumber);

        vehicle.Update(command.PlateNumber, command.Model, command.Year);

        _unitOfWork.Vehicles.Update(vehicle);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}