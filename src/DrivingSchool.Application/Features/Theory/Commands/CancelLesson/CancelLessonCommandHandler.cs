using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Theory.Commands.CancelLesson;

/// <summary>
/// Handles <see cref="CancelLessonCommand"/>.
/// Delegates to the domain aggregate and dispatches
/// the <see cref="LessonCancelledEvent"/> via the saved changes pipeline.
/// </summary>
internal sealed class CancelLessonCommandHandler : ICommandHandler<CancelLessonCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelLessonCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        CancelLessonCommand command,
        CancellationToken cancellationToken)
    {
        var lesson = await _unitOfWork.TheoryLessons
            .GetByIdAsync(command.LessonId, cancellationToken);

        if (lesson is null)
            return Result.Failure(DomainErrors.TheoryLesson.NotFound);

        var result = lesson.Cancel(command.Reason);
        if (result.IsFailure) return result;

        _unitOfWork.TheoryLessons.Update(lesson);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}