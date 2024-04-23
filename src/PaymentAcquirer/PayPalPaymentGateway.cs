using WebApiTemplate.Core.Enum;
using WebApiTemplate.Core.Interfaces;
using WebApiTemplate.Core.Models.Payments;
using WebApiTemplate.Core.Models.Transaction;

namespace PaymentAcquirer;

public class PayPalPaymentGateway : IPaymentGateway
{
    public TransactionResult Capture(PaymentRequest details)
    {
        if (new Random().Next(100) < 20) // 10% chance of failure
        {
            throw new TimeoutException("PayPal gateway timeout");
        }

        var transactionId = "PayPal_" + new Random().Next(1000000, 9999999);

        var lastDigit = int.Parse(details.CardNumber[^1].ToString());
        var isEven = lastDigit % 2 == 0;

        var status = isEven ? TransactionStatus.Approved : TransactionStatus.Denied;
        return new TransactionResult
        {
            IsSuccessful = isEven,
            Message = isEven ? "Transaction approved" : "Transaction denied",
            TransactionId = transactionId,
            Status = status,
            TrasactionDate = DateTime.Now
        };
    }
}
