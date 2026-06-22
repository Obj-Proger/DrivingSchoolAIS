namespace DrivingSchool.Application.Features.Practice.Commands.CreateSlot;

internal sealed class CreateSlotCommandValidator : AbstractValidator<CreateSlotCommand>
{
    public CreateSlotCommandValidator()
    {
        RuleFor(x => x.StartDateTime)
            .NotEmpty().WithMessage("Start time is required.")
            .GreaterThan(DateTime.UtcNow)
                .WithMessage("Slot must be scheduled in the future.");

        RuleFor(x => x.EndDateTime)
            .NotEmpty().WithMessage("End time is required.")
            .GreaterThan(x => x.StartDateTime)
                .WithMessage("End time must be after start time.");

        RuleFor(x => x)
            .Must(x => (x.EndDateTime - x.StartDateTime).TotalMinutes >= 30)
            .WithMessage("Slot duration must be at least 30 minutes.")
            .Must(x => (x.EndDateTime - x.StartDateTime).TotalHours <= 8)
            .WithMessage("Slot duration must not exceed 8 hours.");

        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Note must not exceed 500 characters.")
            .When(x => x.Note is not null);
    }
}