namespace DrivingSchool.Application.Features.Courses.Commands.UpdateCourseTopic;

internal sealed class UpdateCourseTopicCommandValidator : AbstractValidator<UpdateCourseTopicCommand>
{
    public UpdateCourseTopicCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Topic title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");
    }
}