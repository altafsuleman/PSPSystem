using WebApiTemplate.Core.Models.Transaction;
using WebApiTemplate.Infrastructure.Caching;

namespace WebApiTemplate.Application.Payments.Commands;

public abstract class PaymentCommandHandlerBase
{
    public readonly ICacheService<string, TransactionResult> _cache;

    protected PaymentCommandHandlerBase(ICacheService<string, TransactionResult> cache
    )
    {
        _cache = cache;
    }
}
