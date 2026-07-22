using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Vehicles.Commands.CreateVehicle;

/// <summary>Handles <see cref="CreateVehicleCommand"/>.</summary>
internal sealed class CreateVehicleCommandHandler : ICommandHandler<CreateVehicleCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateVehicleCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(
        CreateVehicleCommand command,
        CancellationToken cancellationToken)
    {
        var plateExists = await _unitOfWork.Vehicles
            .ExistsByPlateNumberAsync(command.PlateNumber, ct: cancellationToken);

        if (plateExists)
            return Result.Failure<Guid>(DomainErrors.Vehicle.DuplicatePlateNumber);

        var vehicle = Vehicle.Create(
            command.PlateNumber,
            command.Model,
            command.Year,
            command.Category);

        await _unitOfWork.Vehicles.AddAsync(vehicle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(vehicle.Id);
    }
}