using WebApiTemplate.Core.Enum;

namespace WebApiTemplate.Core.Models.Payments;

public class PaymentRequest
{
    public string CardNumber { get; set; }
    public decimal Amount { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public MerchantType MerchantType { get; set; }
    public string CVV { get; set; }
    public string Currency { get; set; }
    public string Transactionid { get; set; }
}
