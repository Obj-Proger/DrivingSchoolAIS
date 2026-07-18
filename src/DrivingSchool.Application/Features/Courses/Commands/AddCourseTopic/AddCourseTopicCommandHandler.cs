using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Courses.Commands.AddCourseTopic;

/// <summary>Handles <see cref="AddCourseTopicCommand"/>.</summary>
internal sealed class AddCourseTopicCommandHandler : ICommandHandler<AddCourseTopicCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddCourseTopicCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(
        AddCourseTopicCommand command,
        CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Courses
            .GetByIdWithTopicsAsync(command.CourseId, cancellationToken);

        if (course is null)
            return Result.Failure<Guid>(DomainErrors.Course.NotFound);

        var topic = CourseTopic.Create(command.CourseId, command.Title, command.Description);
        course.AddTopic(topic);

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(topic.Id);
    }
}