using FluentValidation;

namespace WebApiTemplate.Application.Payments.Queries;

public sealed class GetPaymentByIdQueryValidator : AbstractValidator<GetPaymentByIdQuery>
{
    public GetPaymentByIdQueryValidator()
    {
        RuleFor(x => x.TransactionId)
            .NotEmpty().WithMessage("Value cannot be empty.")
            .Matches("^[a-zA-Z0-9_]*$").WithMessage("InCorrect Transaction ID.");
    }
}
