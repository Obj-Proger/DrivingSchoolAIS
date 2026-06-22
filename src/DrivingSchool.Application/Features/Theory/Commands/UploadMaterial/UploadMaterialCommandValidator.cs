namespace DrivingSchool.Application.Features.Theory.Commands.UploadMaterial;

internal sealed class UploadMaterialCommandValidator
    : AbstractValidator<UploadMaterialCommand>
{
    private static readonly string[] AllowedContentTypes =
    [
        "application/pdf",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.ms-powerpoint",
        "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        "video/mp4",
        "image/jpeg",
        "image/png"
    ];

    public UploadMaterialCommandValidator()
    {
        RuleFor(x => x)
            .Must(x => x.LessonId.HasValue ^ x.TopicId.HasValue)
            .WithMessage("Exactly one of LessonId or TopicId must be provided.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Material title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File name is required.");

        RuleFor(x => x.ContentType)
            .Must(ct => AllowedContentTypes.Contains(ct.ToLowerInvariant()))
            .WithMessage("This file type is not permitted.");

        RuleFor(x => x.FileStream)
            .Must(s => s.Length <= 50 * 1024 * 1024)
            .WithMessage("File must not exceed 50 MB.");
    }
}