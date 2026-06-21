namespace DrivingSchool.Application.Interfaces.Services;

/// <summary>
/// Sends transactional email messages using pre-defined HTML templates.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends a raw email to the specified recipient.
    /// </summary>
    /// <param name="to">The recipient's email address.</param>
    /// <param name="subject">The email subject line.</param>
    /// <param name="htmlBody">The HTML content of the email body.</param>
    Task SendAsync(
        string to,
        string subject,
        string htmlBody,
        CancellationToken ct = default);

    /// <summary>
    /// Renders and sends a named HTML template with the provided variables.
    /// </summary>
    /// <param name="to">The recipient's email address.</param>
    /// <param name="templateName">
    /// The template identifier (e.g. <c>"EmailConfirmation"</c>, <c>"LessonReminder"</c>).
    /// </param>
    /// <param name="variables">
    /// Key-value pairs substituted into the template placeholders.
    /// </param>
    Task SendTemplatedAsync(
        string to,
        string templateName,
        Dictionary<string, string> variables,
        CancellationToken ct = default);
}