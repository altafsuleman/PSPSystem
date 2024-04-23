using WebApiTemplate.Core.Enum;

namespace WebApiTemplate.Core.Models.Transaction;

public class TransactionResult
{
    public bool IsSuccessful { get; set; }
    public string Message { get; set; }
    public TransactionStatus Status { get; set; }
    public string TransactionId { get; set; }
    public DateTime TrasactionDate { get; set; }
}
