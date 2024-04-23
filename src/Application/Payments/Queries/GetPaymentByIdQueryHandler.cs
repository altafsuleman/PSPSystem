using HumbleMediator;
using WebApiTemplate.Core.Models.Transaction;
using WebApiTemplate.Infrastructure.Caching;

namespace WebApiTemplate.Application.Payments.Queries;

public class GetPaymentByIdQueryHandler : IQueryHandler<GetPaymentByIdQuery, TransactionResult>
{
    private readonly ICacheService<string, TransactionResult> _cache;

    public GetPaymentByIdQueryHandler(ICacheService<string, TransactionResult> cache
    )
    {
        _cache = cache;
    }

    public Task<TransactionResult> Handle(
        GetPaymentByIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        if (_cache.TryGetValue(query.TransactionId, out var Transaction))
        {
            return Task.FromResult(Transaction);
        }

        return Task.FromResult(new TransactionResult()
        {
            IsSuccessful = false
        });
    }
}
