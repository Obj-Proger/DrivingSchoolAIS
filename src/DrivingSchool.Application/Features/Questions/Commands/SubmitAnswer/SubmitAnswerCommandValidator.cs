namespace DrivingSchool.Application.Features.Questions.Commands.SubmitAnswer;

internal sealed class SubmitAnswerCommandValidator : AbstractValidator<SubmitAnswerCommand>
{
    public SubmitAnswerCommandValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty();
        RuleFor(x => x.QuestionId).NotEmpty();
        RuleFor(x => x.SelectedOptionId).NotEmpty();
    }
}