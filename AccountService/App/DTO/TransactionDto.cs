using Common.Src;

namespace AccountService.App.DTO;

public class TransactionDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid? CounterPartyAccountId { get; set; }
    public decimal TransferAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public TransactionType TransactionType { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}