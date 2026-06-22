namespace DrivingSchool.Application.Features.Practice.Commands.CopySchedule;

internal sealed class CopyScheduleCommandValidator : AbstractValidator<CopyScheduleCommand>
{
    public CopyScheduleCommandValidator()
    {
        RuleFor(x => x.TargetDate)
            .GreaterThan(DateTime.UtcNow.Date)
                .WithMessage("Target date must be in the future.")
            .NotEqual(x => x.SourceDate.Date)
                .WithMessage("Source and target dates must differ.");
    }
}