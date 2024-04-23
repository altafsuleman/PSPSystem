using WebApiTemplate.Core.Models.Payments;
using WebApiTemplate.Core.Models.Transaction;

namespace WebApiTemplate.Core.Interfaces;

public interface IPaymentGateway
{
    TransactionResult Capture(PaymentRequest paymentsDetails);
}
