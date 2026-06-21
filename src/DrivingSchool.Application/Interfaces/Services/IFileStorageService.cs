namespace DrivingSchool.Application.Interfaces.Services;

/// <summary>
/// Stores and retrieves files uploaded by users or the system.
/// The concrete implementation switches between local disk and S3-compatible
/// object storage depending on configuration.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Uploads a file stream to the specified logical folder.
    /// </summary>
    /// <param name="stream">The file content stream.</param>
    /// <param name="fileName">The original file name.</param>
    /// <param name="contentType">The MIME type of the file.</param>
    /// <param name="folder">
    /// The logical storage folder (e.g. <c>"avatars"</c>, <c>"materials"</c>, <c>"documents"</c>).
    /// </param>
    /// <returns>A <see cref="FileUploadResult"/> containing the storage URL and metadata.</returns>
    Task<FileUploadResult> UploadAsync(
        Stream stream,
        string fileName,
        string contentType,
        string folder,
        CancellationToken ct = default);

    /// <summary>Deletes the file at the specified storage URL.</summary>
    Task DeleteAsync(string fileUrl, CancellationToken ct = default);

    /// <summary>Returns the publicly accessible URL for the specified relative path.</summary>
    string GetPublicUrl(string relativePath);
}

/// <summary>Carries the result of a successful file upload operation.</summary>
/// <param name="FileUrl">The publicly accessible URL of the uploaded file.</param>
/// <param name="FileName">The sanitised file name used for storage.</param>
/// <param name="SizeBytes">The size of the stored file in bytes.</param>
/// <param name="ContentType">The MIME type of the stored file.</param>
public sealed record FileUploadResult(
    string FileUrl,
    string FileName,
    long SizeBytes,
    string ContentType);