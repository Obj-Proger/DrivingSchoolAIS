using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;
using DrivingSchool.Application.Interfaces.Services;

namespace DrivingSchool.Application.Features.Theory.Commands.CreateLesson;

/// <summary>
/// Handles <see cref="CreateLessonCommand"/>.
/// Validates group and teacher existence before scheduling the lesson.
/// </summary>
internal sealed class CreateLessonCommandHandler : ICommandHandler<CreateLessonCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateLessonCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<Guid>> Handle(
        CreateLessonCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Validate group
        var group = await _unitOfWork.Groups
            .GetByIdAsync(command.GroupId, cancellationToken);

        if (group is null)
            return Result.Failure<Guid>(DomainErrors.Group.NotFound);

        //2. Create lesson aggregate
        var lessonResult = TheoryLesson.Create(
            command.GroupId,
            _currentUser.UserId,
            command.TopicId,
            command.Title,
            command.ScheduledAt,
            command.DurationMinutes,
            command.RoomOrLink,
            command.Description);

        if (lessonResult.IsFailure)
            return Result.Failure<Guid>(lessonResult.Error);

        // 3. Persist
        await _unitOfWork.TheoryLessons.AddAsync(lessonResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(lessonResult.Value.Id);
    }
}