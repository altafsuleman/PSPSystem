using HumbleMediator;
using PaymentAcquirer;
using WebApiTemplate.Core.Enum;
using WebApiTemplate.Core.Models.Payments;
using WebApiTemplate.Core.Models.Transaction;
using WebApiTemplate.Infrastructure.Caching;

namespace WebApiTemplate.Application.Payments.Commands;

public class CreatePaymentCommandHandler
    : PaymentCommandHandlerBase,
        ICommandHandler<CreatePaymentCommand, TransactionResult>
{
    public CreatePaymentCommandHandler(ICacheService<string, TransactionResult> cache
    ) : base(cache)
    {
    }

    public async Task<TransactionResult> Handle(
        CreatePaymentCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var payment = new PaymentRequest
        {
            CardNumber = command.CardNumber,
            ExpiryMonth = command.ExpiryMonth,
            ExpiryYear = command.ExpiryYear,
            MerchantType = (MerchantType)Enum.Parse(typeof(MerchantType), command.MerchantId, true),
            CVV = command.CVV,
            Amount = command.Amount,
            Currency = command.Currency
        };


        var result = new PaymentProcessor().ProcessPayment(payment);


        _cache.Add(result.TransactionId, result);

        return result;
    }
}
