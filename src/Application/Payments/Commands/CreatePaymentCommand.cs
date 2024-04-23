using HumbleMediator;
using WebApiTemplate.Core.Models.Transaction;

namespace WebApiTemplate.Application.Payments.Commands;

public record CreatePaymentCommand : ICommand<TransactionResult>
{
    public string MerchantId { get; set; }
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string CVV { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}
