using CreditCardValidator;
using FluentValidation;
using WebApiTemplate.Core.Enum;

namespace WebApiTemplate.Application.Payments.Commands;

public sealed class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.CardNumber)
            .CreditCard().WithMessage("Invalid card number.")
            .NotEmpty().WithMessage("Card number is required.");

        RuleFor(e => e.CardNumber).Must(IsCardNumberValid).WithMessage("Invalid Card Luhn.");

        RuleFor(x => x.ExpiryMonth)
            .InclusiveBetween(1, 12).WithMessage("Expiry month must be between 1 and 12");

        RuleFor(x => x.ExpiryYear)
            .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("Expiry year cannot be in the past")
            .LessThanOrEqualTo(DateTime.Now.Year + 20).WithMessage("Expiry year is too far in the future");

        RuleFor(x => x.CVV)
            .Matches(@"^\d{3,4}$").WithMessage("CVV is invalid.")
            .NotEmpty().WithMessage("CVV is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Currency)
            .Length(3).WithMessage("Currency must be a 3-letter ISO code.")
            .NotEmpty().WithMessage("Currency is required.");

        //Validate MerchantType is a defined enum value
        RuleFor(x => x.MerchantId)
            .NotEmpty().WithMessage("Merchant ID is required.")
            .Must(BeAValidMerchantType).WithMessage("Invalid merchant ID.");
    }

    private bool BeAValidMerchantType(string merchantId)
    {
        if (int.TryParse(merchantId, out var id) && Enum.IsDefined(typeof(MerchantType), id))
        {
            return true;
        }

        return false;
    }

    private bool IsCardNumberValid(string cardNumber)
    {
        try
        {
            return Luhn.CheckLuhn(cardNumber);
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
