using HumbleMediator;
using WebApiTemplate.Core.Models.Transaction;

namespace WebApiTemplate.Application.Payments.Queries;

public sealed record GetPaymentByIdQuery(string Id) : IQuery<TransactionResult>
{
    public string TransactionId { get; set; } = Id;
}
