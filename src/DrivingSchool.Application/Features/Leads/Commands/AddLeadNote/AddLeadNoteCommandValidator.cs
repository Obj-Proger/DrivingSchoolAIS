namespace DrivingSchool.Application.Features.Leads.Commands.AddLeadNote;

internal sealed class AddLeadNoteCommandValidator : AbstractValidator<AddLeadNoteCommand>
{
    public AddLeadNoteCommandValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Note text is required.")
            .MaximumLength(2000).WithMessage("Note must not exceed 2000 characters.");
    }
}