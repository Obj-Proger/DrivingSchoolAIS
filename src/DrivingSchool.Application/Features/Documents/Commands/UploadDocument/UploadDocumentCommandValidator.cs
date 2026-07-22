namespace DrivingSchool.Application.Features.Documents.Commands.UploadDocument;

internal sealed class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
{
    public UploadDocumentCommandValidator()
    {
        RuleFor(x => x.OwnerType)
            .NotEmpty().WithMessage("Owner type is required.")
            .MaximumLength(100).WithMessage("Owner type must not exceed 100 characters.");

        RuleFor(x => x.OwnerId).NotEmpty();

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File name is required.")
            .MaximumLength(255).WithMessage("File name must not exceed 255 characters.");

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage("Content type is required.");
    }
}