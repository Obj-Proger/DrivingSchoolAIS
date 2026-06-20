namespace DrivingSchool.Domain.Enums;

/// <summary>
/// Defines the payment method used to settle a contract instalment.
/// </summary>
public enum PaymentMethod
{
    /// <summary>Payment made in cash at the school's office.</summary>
    Cash = 1,

    /// <summary>Payment made by debit or credit card at a terminal.</summary>
    Card = 2,

    /// <summary>Payment made through an online payment gateway.</summary>
    Online = 3,

    /// <summary>Payment made via a bank wire transfer.</summary>
    BankTransfer = 4
}