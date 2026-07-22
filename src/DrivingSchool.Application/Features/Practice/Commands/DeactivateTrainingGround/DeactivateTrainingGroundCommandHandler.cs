using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Commands.DeactivateTrainingGround;

/// <summary>Handles <see cref="DeactivateTrainingGroundCommand"/>.</summary>
internal sealed class DeactivateTrainingGroundCommandHandler
    : ICommandHandler<DeactivateTrainingGroundCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateTrainingGroundCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeactivateTrainingGroundCommand command,
        CancellationToken cancellationToken)
    {
        var ground = await _unitOfWork.TrainingGrounds
            .GetByIdAsync(command.GroundId, cancellationToken);

        if (ground is null)
            return Result.Failure(DomainErrors.TrainingGround.NotFound);

        ground.Deactivate();

        _unitOfWork.TrainingGrounds.Update(ground);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}