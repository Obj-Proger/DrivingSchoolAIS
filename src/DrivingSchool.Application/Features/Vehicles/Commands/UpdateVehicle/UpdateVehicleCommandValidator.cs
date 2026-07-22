namespace DrivingSchool.Application.Features.Vehicles.Commands.UpdateVehicle;

internal sealed class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
{
    public UpdateVehicleCommandValidator()
    {
        RuleFor(x => x.PlateNumber)
            .NotEmpty().WithMessage("Plate number is required.")
            .MaximumLength(20).WithMessage("Plate number must not exceed 20 characters.");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required.")
            .MaximumLength(100).WithMessage("Model must not exceed 100 characters.");

        RuleFor(x => x.Year)
            .GreaterThan(1970).WithMessage("Year must be later than 1970.")
            .LessThanOrEqualTo(DateTime.UtcNow.Year + 1)
                .WithMessage("Year cannot be in the future.");
    }
}