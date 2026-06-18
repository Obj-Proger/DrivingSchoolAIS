using System.Text.RegularExpressions;
using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.ValueObjects;

/// <summary>
/// Represents a validated and normalised email address.
/// </summary>
public sealed class Email : ValueObject
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        matchTimeout: TimeSpan.FromMilliseconds(250));

    private Email(string value) => Value = value;

    /// <summary>Gets the normalised (lower-case, trimmed) email address value.</summary>
    public string Value { get; }

    /// <summary>
    /// Creates a validated <see cref="Email"/> instance.
    /// The value is trimmed and converted to lower-case on success.
    /// </summary>
    /// <param name="value">The raw email address string.</param>
    /// <returns>
    /// A successful <see cref="Result{Email}"/> or a failure with
    /// <see cref="DomainErrors.Email.Empty"/>, <see cref="DomainErrors.Email.TooLong"/>,
    /// or <see cref="DomainErrors.Email.InvalidFormat"/>.
    /// </returns>
    public static Result<Email> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Email>(DomainErrors.Email.Empty);

        if (value.Length > 320)
            return Result.Failure<Email>(DomainErrors.Email.TooLong);

        if (!EmailRegex.IsMatch(value.Trim()))
            return Result.Failure<Email>(DomainErrors.Email.InvalidFormat);

        return Result.Success(new Email(value.Trim().ToLowerInvariant()));
    }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>Implicitly converts an <see cref="Email"/> to its string value.</summary>
    public static implicit operator string(Email email) => email.Value;

    /// <inheritdoc />
    public override string ToString() => Value;
}