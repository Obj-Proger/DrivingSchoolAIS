namespace DrivingSchool.Application.Features.Theory.Commands.MarkAttendance;

internal sealed class MarkAttendanceCommandValidator
    : AbstractValidator<MarkAttendanceCommand>
{
    public MarkAttendanceCommandValidator()
    {
        RuleFor(x => x.Records)
            .NotEmpty().WithMessage("At least one attendance record is required.");

        RuleForEach(x => x.Records)
            .ChildRules(record =>
            {
                record.RuleFor(r => r.ContractId)
                    .NotEmpty().WithMessage("Contract ID is required.");

                record.RuleFor(r => r.Note)
                    .MaximumLength(500)
                        .WithMessage("Note must not exceed 500 characters.")
                    .When(r => r.Note is not null);
            });
    }
}