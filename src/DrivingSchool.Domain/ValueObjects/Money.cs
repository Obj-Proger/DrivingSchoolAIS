using DrivingSchool.Domain.Common;

namespace DrivingSchool.Domain.ValueObjects;

/// <summary>
/// Represents a monetary value with an amount and an ISO 4217 currency code.
/// The amount is rounded to two decimal places on creation.
/// </summary>
public sealed class Money : ValueObject
{
    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>Gets the monetary amount rounded to two decimal places.</summary>
    public decimal Amount { get; }

    /// <summary>Gets the ISO 4217 currency code (e.g. <c>RUB</c>, <c>USD</c>).</summary>
    public string Currency { get; }

    /// <summary>Gets a value indicating whether the amount is greater than zero.</summary>
    public bool IsPositive => Amount > 0;

    /// <summary>Gets a value indicating whether the amount equals zero.</summary>
    public bool IsZero => Amount == 0;

    /// <summary>Returns a zero-amount instance for the specified currency.</summary>
    /// <param name="currency">The ISO 4217 currency code. Defaults to <c>RUB</c>.</param>
    public static Money Zero(string currency = "RUB") => new(0m, currency);

    /// <summary>
    /// Creates a validated <see cref="Money"/> instance.
    /// The amount is rounded to two decimal places using mid-point rounding away from zero.
    /// The currency code is trimmed and upper-cased.
    /// </summary>
    /// <param name="amount">The monetary amount. Must be non-negative.</param>
    /// <param name="currency">The ISO 4217 currency code. Defaults to <c>RUB</c>.</param>
    /// <returns>
    /// A successful <see cref="Result{Money}"/> or a failure with
    /// <see cref="DomainErrors.Money.NegativeAmount"/>
    /// or <see cref="DomainErrors.Money.InvalidCurrency"/>.
    /// </returns>
    public static Result<Money> Create(decimal amount, string currency = "RUB")
    {
        if (amount < 0)
            return Result.Failure<Money>(DomainErrors.Money.NegativeAmount);

        if (string.IsNullOrWhiteSpace(currency))
            return Result.Failure<Money>(DomainErrors.Money.InvalidCurrency);

        return Result.Success(new Money(
            Math.Round(amount, 2, MidpointRounding.AwayFromZero),
            currency.Trim().ToUpperInvariant()));
    }

    /// <summary>Returns a new <see cref="Money"/> that is the sum of this and <paramref name="other"/>.</summary>
    /// <exception cref="InvalidOperationException">Thrown when currencies differ.</exception>
    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    /// <summary>Returns a new <see cref="Money"/> that is the difference of this and <paramref name="other"/>.</summary>
    /// <exception cref="InvalidOperationException">Thrown when currencies differ.</exception>
    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount - other.Amount, Currency);
    }

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException(
                $"Cannot perform arithmetic between '{Currency}' and '{other.Currency}'.");
    }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    /// <inheritdoc />
    public override string ToString() => $"{Amount:F2} {Currency}";
}