using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Commands.UpdateTrainingGround;

/// <summary>Handles <see cref="UpdateTrainingGroundCommand"/>.</summary>
internal sealed class UpdateTrainingGroundCommandHandler
    : ICommandHandler<UpdateTrainingGroundCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTrainingGroundCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateTrainingGroundCommand command,
        CancellationToken cancellationToken)
    {
        var ground = await _unitOfWork.TrainingGrounds
            .GetByIdAsync(command.GroundId, cancellationToken);

        if (ground is null)
            return Result.Failure(DomainErrors.TrainingGround.NotFound);

        ground.Update(command.Name, command.Address, command.Description);

        _unitOfWork.TrainingGrounds.Update(ground);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}