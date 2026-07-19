namespace DrivingSchool.Application.Features.Questions.Commands.CreateQuestion;

internal sealed class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(x => x.TopicId)
            .NotEmpty().WithMessage("Topic is required.");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Invalid licence category.");

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Question text is required.")
            .MaximumLength(2000).WithMessage("Question text must not exceed 2000 characters.");

        RuleFor(x => x.Options)
            .Must(o => o.Count >= 2).WithMessage("At least two answer options are required.")
            .Must(o => o.All(t => !string.IsNullOrWhiteSpace(t)))
                .WithMessage("Answer options must not be empty.");

        RuleFor(x => x.CorrectOptionIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Correct option index must not be negative.")
            .Must((command, index) => index < command.Options.Count)
                .WithMessage("Correct option index is out of range.");

        RuleFor(x => x.Explanation)
            .MaximumLength(2000).WithMessage("Explanation must not exceed 2000 characters.");
    }
}