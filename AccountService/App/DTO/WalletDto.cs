using Common.Src;

namespace AccountService.App.DTO
{
    public class WalletDto
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public TypeWallet Type { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public decimal? InterestRate { get; set; }
        public DateTime DateOpen { get; set; }
        public DateTime? DateClosed { get; set; }
    }
}
