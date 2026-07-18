using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Courses.Commands.DeactivateCourse;

/// <summary>Handles <see cref="DeactivateCourseCommand"/>.</summary>
internal sealed class DeactivateCourseCommandHandler : ICommandHandler<DeactivateCourseCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateCourseCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        DeactivateCourseCommand command,
        CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Courses
            .GetByIdAsync(command.CourseId, cancellationToken);

        if (course is null)
            return Result.Failure(DomainErrors.Course.NotFound);

        course.Deactivate();

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}