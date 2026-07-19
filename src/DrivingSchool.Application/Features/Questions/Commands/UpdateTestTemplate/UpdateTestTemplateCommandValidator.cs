namespace DrivingSchool.Application.Features.Questions.Commands.UpdateTestTemplate;

internal sealed class UpdateTestTemplateCommandValidator
    : AbstractValidator<UpdateTestTemplateCommand>
{
    public UpdateTestTemplateCommandValidator()
    {
        RuleFor(x => x.TemplateId)
            .NotEmpty().WithMessage("Template is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Template name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.ErrorHandling)
            .IsInEnum().WithMessage("Invalid error-handling mode.");

        RuleFor(x => x.QuestionCount)
            .GreaterThan(0).WithMessage("Question count must be greater than zero.");

        RuleFor(x => x.TimeLimitMinutes)
            .GreaterThan(0).WithMessage("Time limit must be greater than zero minutes.");

        RuleFor(x => x.PassScore)
            .GreaterThan(0).WithMessage("Pass score must be greater than zero.")
            .LessThanOrEqualTo(x => x.QuestionCount)
                .WithMessage("Pass score must not exceed the question count.");

        RuleFor(x => x.AddQuestionsOnError)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.AddMinutesOnError)
            .GreaterThanOrEqualTo(0);
    }
}