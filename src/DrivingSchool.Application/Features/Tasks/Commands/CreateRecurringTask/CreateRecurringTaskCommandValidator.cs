namespace DrivingSchool.Application.Features.Tasks.Commands.CreateRecurringTask;

internal sealed class CreateRecurringTaskCommandValidator
    : AbstractValidator<CreateRecurringTaskCommand>
{
    public CreateRecurringTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Task title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.RecurringDays)
            .NotEmpty().WithMessage("At least one recurring day must be specified.")
            .Must(days => days.Distinct().Count() == days.Count)
                .WithMessage("Recurring days must not contain duplicates.");
    }
}