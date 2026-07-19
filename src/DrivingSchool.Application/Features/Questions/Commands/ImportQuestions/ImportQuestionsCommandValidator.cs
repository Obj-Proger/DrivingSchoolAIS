namespace DrivingSchool.Application.Features.Questions.Commands.ImportQuestions;

internal sealed class ImportQuestionsCommandValidator : AbstractValidator<ImportQuestionsCommand>
{
    public ImportQuestionsCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Target course is required.");

        RuleFor(x => x.QuestionsJson)
            .NotEmpty().WithMessage("Questions payload must not be empty.");
    }
}