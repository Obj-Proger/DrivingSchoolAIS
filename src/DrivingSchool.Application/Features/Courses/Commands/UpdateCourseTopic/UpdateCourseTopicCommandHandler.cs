using DrivingSchool.Application.Common;
using DrivingSchool.Application.Interfaces.Repositories;

namespace DrivingSchool.Application.Features.Courses.Commands.UpdateCourseTopic;

/// <summary>Handles <see cref="UpdateCourseTopicCommand"/>.</summary>
internal sealed class UpdateCourseTopicCommandHandler : ICommandHandler<UpdateCourseTopicCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseTopicCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(
        UpdateCourseTopicCommand command,
        CancellationToken cancellationToken)
    {
        var course = await _unitOfWork.Courses
            .GetByIdWithTopicsAsync(command.CourseId, cancellationToken);

        if (course is null)
            return Result.Failure(DomainErrors.Course.NotFound);

        var topic = course.Topics.FirstOrDefault(t => t.Id == command.TopicId);
        if (topic is null)
            return Result.Failure(DomainErrors.Course.TopicNotFound);

        topic.Update(command.Title, command.Description);

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}