using WebApiTemplate.Core.Enum;
using WebApiTemplate.Core.Models.Payments;
using WebApiTemplate.Core.Models.Transaction;

namespace PaymentAcquirer;

public class PaymentProcessor
{
    public TransactionResult ProcessPayment(PaymentRequest paymentRequest)
    {
        var gatewayPreferences = MerchantPriority(paymentRequest);

        foreach (var gatewayType in gatewayPreferences)
        {
            try
            {
                var gateway = GatewayFactory.CreateGateway(gatewayType);
                return gateway.Capture(paymentRequest);
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Error with {gatewayType} gateway: {ex.Message}. Trying next gateway.");
                // Log error or handle it appropriately
            }
        }

        return new TransactionResult
        {
            IsSuccessful = false, Message = "All gateways failed", Status = TransactionStatus.Denied
        };
    }

    private static List<MerchantType> MerchantPriority(PaymentRequest paymentRequest)
    {
        var gatewayPreferences = new List<MerchantType>();
        if (paymentRequest.MerchantType == MerchantType.Paypal)
        {
            gatewayPreferences.Add(MerchantType.Paypal);
            gatewayPreferences.Add(MerchantType.Stripe);
        }
        else
        {
            gatewayPreferences.Add(MerchantType.Stripe);
            gatewayPreferences.Add(MerchantType.Paypal);
        }

        return gatewayPreferences;
    }
}
