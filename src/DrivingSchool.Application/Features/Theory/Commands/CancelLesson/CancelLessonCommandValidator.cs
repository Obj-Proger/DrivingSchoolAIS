namespace DrivingSchool.Application.Features.Theory.Commands.CancelLesson;

internal sealed class CancelLessonCommandValidator : AbstractValidator<CancelLessonCommand>
{
    public CancelLessonCommandValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Cancellation reason is required.")
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.");
    }
}