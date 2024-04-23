using WebApiTemplate.Core.Enum;
using WebApiTemplate.Core.Interfaces;
using WebApiTemplate.Core.Models.Payments;

namespace PaymentAcquirer;

public static class GatewayFactory
{
    public static IPaymentGateway CreateGateway(MerchantType gatewayType)
    {
        switch (gatewayType)
        {
            case MerchantType.Stripe:
                return new StripePaymentGateway();
            case MerchantType.Paypal:
                return new PayPalPaymentGateway();
            default:
                throw new ArgumentException("Unsupported gateway type");
        }
    }
}
