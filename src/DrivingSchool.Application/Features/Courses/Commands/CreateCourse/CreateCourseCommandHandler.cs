using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Courses.Commands.CreateCourse;

/// <summary>Handles <see cref="CreateCourseCommand"/>.</summary>
internal sealed class CreateCourseCommandHandler : ICommandHandler<CreateCourseCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(
        CreateCourseCommand command,
        CancellationToken cancellationToken)
    {
        var priceResult = Money.Create(command.Price);
        if (priceResult.IsFailure)
            return Result.Failure<Guid>(priceResult.Error);

        var courseResult = Course.Create(
            command.Name,
            command.Category,
            command.TheoryHoursTotal,
            command.PracticeHoursTotal,
            priceResult.Value,
            command.Description);

        if (courseResult.IsFailure)
            return Result.Failure<Guid>(courseResult.Error);

        await _unitOfWork.Courses.AddAsync(courseResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(courseResult.Value.Id);
    }
}