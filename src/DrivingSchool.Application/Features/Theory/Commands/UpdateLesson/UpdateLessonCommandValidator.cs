namespace DrivingSchool.Application.Features.Theory.Commands.UpdateLesson;

internal sealed class UpdateLessonCommandValidator : AbstractValidator<UpdateLessonCommand>
{
    public UpdateLessonCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Lesson title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.ScheduledAt)
            .NotEmpty().WithMessage("Scheduled time is required.")
            .GreaterThan(DateTime.UtcNow)
                .WithMessage("Lesson must be scheduled in the future.");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Duration must be greater than zero.")
            .LessThanOrEqualTo(480).WithMessage("Duration must not exceed 8 hours.");

        RuleFor(x => x.RoomOrLink)
            .NotEmpty().WithMessage("Room or meeting link is required.")
            .MaximumLength(500).WithMessage("Room or link must not exceed 500 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
            .When(x => x.Description is not null);
    }
}