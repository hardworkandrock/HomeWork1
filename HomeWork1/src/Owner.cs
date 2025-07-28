namespace Common.Src
{
    public class Owner
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;

        public ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
    }
}
