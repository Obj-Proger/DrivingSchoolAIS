using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Practice.Commands.CreateTrainingGround;

/// <summary>Handles <see cref="CreateTrainingGroundCommand"/>.</summary>
internal sealed class CreateTrainingGroundCommandHandler
    : ICommandHandler<CreateTrainingGroundCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTrainingGroundCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(
        CreateTrainingGroundCommand command,
        CancellationToken cancellationToken)
    {
        var ground = TrainingGround.Create(
            command.Name,
            command.Address,
            command.Description);

        await _unitOfWork.TrainingGrounds.AddAsync(ground, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(ground.Id);
    }
}