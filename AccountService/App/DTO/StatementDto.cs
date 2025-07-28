namespace AccountService.App.DTO;

public class StatementDto
{
    public Guid WalletId { get; set; }
    public decimal StartingBalance { get; set; }
    public decimal EndingBalance { get; set; }
    public List<TransactionDto> Transactions { get; set; } = new();
}