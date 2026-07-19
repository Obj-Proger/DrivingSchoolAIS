namespace DrivingSchool.Application.Features.Questions.Commands.UpdateQuestion;

internal sealed class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty().WithMessage("Question is required.");

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Question text is required.")
            .MaximumLength(2000).WithMessage("Question text must not exceed 2000 characters.");

        RuleFor(x => x.CorrectOptionId)
            .NotEmpty().WithMessage("Correct option is required.");

        RuleFor(x => x.Explanation)
            .MaximumLength(2000).WithMessage("Explanation must not exceed 2000 characters.");
    }
}