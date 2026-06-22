using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Theory.Commands.UpdateLesson;

/// <summary>
/// Handles <see cref="UpdateLessonCommand"/>.
/// </summary>
internal sealed class UpdateLessonCommandHandler : ICommandHandler<UpdateLessonCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLessonCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateLessonCommand command,
        CancellationToken cancellationToken)
    {
        var lesson = await _unitOfWork.TheoryLessons
            .GetByIdAsync(command.LessonId, cancellationToken);

        if (lesson is null)
            return Result.Failure(DomainErrors.TheoryLesson.NotFound);

        var result = lesson.Update(
            command.Title,
            command.Description,
            command.ScheduledAt,
            command.DurationMinutes,
            command.RoomOrLink);

        if (result.IsFailure) return result;

        _unitOfWork.TheoryLessons.Update(lesson);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}