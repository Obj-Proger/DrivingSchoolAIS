using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.ValueObjects;

/// <summary>
/// Represents a validated and normalised Russian phone number
/// stored in the canonical format <c>+7XXXXXXXXXX</c>.
/// </summary>
public sealed class PhoneNumber : ValueObject
{
    private PhoneNumber(string value) => Value = value;

    /// <summary>
    /// Gets the normalised phone number in the format <c>+7XXXXXXXXXX</c>.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a validated <see cref="PhoneNumber"/> instance.
    /// Accepted input formats: <c>+7XXXXXXXXXX</c>, <c>8XXXXXXXXXX</c>,
    /// <c>7XXXXXXXXXX</c>, and <c>XXXXXXXXXX</c> (10 digits).
    /// All non-digit characters are stripped before validation.
    /// </summary>
    /// <param name="value">The raw phone number string.</param>
    /// <returns>
    /// A successful <see cref="Result{PhoneNumber}"/> carrying the normalised value,
    /// or a failure with <see cref="DomainErrors.PhoneNumber.Empty"/>
    /// or <see cref="DomainErrors.PhoneNumber.InvalidFormat"/>.
    /// </returns>
    public static Result<PhoneNumber> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<PhoneNumber>(DomainErrors.PhoneNumber.Empty);

        var normalised = Normalise(value);

        if (normalised is null)
            return Result.Failure<PhoneNumber>(DomainErrors.PhoneNumber.InvalidFormat);

        return Result.Success(new PhoneNumber(normalised));
    }

    /// <summary>
    /// Strips non-digit characters and converts the number to <c>+7XXXXXXXXXX</c>.
    /// Returns <c>null</c> when normalisation is not possible.
    /// </summary>
    private static string? Normalise(string input)
    {
        var digits = new string(input.Where(char.IsDigit).ToArray());

        return digits.Length switch
        {
            10 => "+7" + digits,
            11 when digits[0] is '8' => "+7" + digits[1..],
            11 when digits[0] is '7' => "+" + digits,
            _ => null
        };
    }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>Implicitly converts a <see cref="PhoneNumber"/> to its string value.</summary>
    public static implicit operator string(PhoneNumber phone) => phone.Value;

    /// <inheritdoc />
    public override string ToString() => Value;
}