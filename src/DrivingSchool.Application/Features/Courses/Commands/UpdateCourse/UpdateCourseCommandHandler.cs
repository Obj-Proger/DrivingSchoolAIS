using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Courses.Commands.UpdateCourse;

/// <summary>Handles <see cref="UpdateCourseCommand"/>.</summary>
internal sealed class UpdateCourseCommandHandler : ICommandHandler<UpdateCourseCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateCourseCommand command,
        CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Courses
            .GetByIdAsync(command.CourseId, cancellationToken);

        if (course is null)
            return Result.Failure(DomainErrors.Course.NotFound);

        var priceResult = Money.Create(command.Price);
        if (priceResult.IsFailure)
            return Result.Failure(priceResult.Error);

        course.Update(command.Name, command.Description);
        course.UpdatePrice(priceResult.Value);

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}