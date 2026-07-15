using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Groups.Commands.CreateGroup;

/// <summary>Handles <see cref="CreateGroupCommand"/>.</summary>
internal sealed class CreateGroupCommandHandler : ICommandHandler<CreateGroupCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateGroupCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(
        CreateGroupCommand command,
        CancellationToken cancellationToken)
    {
        var teacher = await _unitOfWork.Users
            .GetByIdAsync(command.TeacherId, cancellationToken);

        if (teacher is null)
            return Result.Failure<Guid>(DomainErrors.User.NotFound);

        var groupResult = Group.Create(
            command.Name,
            command.CourseId,
            command.TeacherId,
            command.StartDate,
            command.MaxStudents,
            command.EndDate,
            command.BranchId);

        if (groupResult.IsFailure)
            return Result.Failure<Guid>(groupResult.Error);

        await _unitOfWork.Groups.AddAsync(groupResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(groupResult.Value.Id);
    }
}