namespace DrivingSchool.Application.Features.Users.Commands.UploadAvatar;

internal sealed class UploadAvatarCommandValidator : AbstractValidator<UploadAvatarCommand>
{
    private static readonly string[] AllowedContentTypes =
        ["image/jpeg", "image/png", "image/webp"];

    public UploadAvatarCommandValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("File name is required.");

        RuleFor(x => x.ContentType)
            .Must(ct => AllowedContentTypes.Contains(ct.ToLowerInvariant()))
            .WithMessage("Only JPEG, PNG, and WebP images are accepted.");

        RuleFor(x => x.FileStream)
            .Must(s => s.Length <= 5 * 1024 * 1024)
            .WithMessage("Avatar image must not exceed 5 MB.");
    }
}