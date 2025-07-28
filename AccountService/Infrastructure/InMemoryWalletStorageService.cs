using AccountService.Infrastructure.Interface;
using Common.Src;

namespace AccountService.Infrastructure;

public class InMemoryWalletStorageService : IWalletStorageService
{
    private readonly List<Transaction> _transactions = new();
    private readonly List<Wallet> _wallets = new();

    public Task<Wallet> CreateAsync(Wallet wallet, CancellationToken ct)
    {
        _wallets.Add(wallet);
        return Task.FromResult(wallet);
    }

    public Task<Wallet?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return Task.FromResult(_wallets.FirstOrDefault(w => w.Id == id));
    }

    public Task<IEnumerable<Wallet>> GetAllAsync(CancellationToken ct)
    {
        return Task.FromResult<IEnumerable<Wallet>>(_wallets.ToList());
    }

    public Task<IEnumerable<Wallet>> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct)
    {
        return Task.FromResult(_wallets.Where(w => w.OwnerId == ownerId));
    }

    public Task UpdateAsync(Wallet wallet, CancellationToken ct)
    {
        var existing = _wallets.FirstOrDefault(w => w.Id == wallet.Id);
        if (existing != null)
        {
            _wallets.Remove(existing);
            _wallets.Add(wallet);
        }

        return Task.CompletedTask;
    }
  
    public Task AddTransactionAsync(Transaction transaction, CancellationToken ct)
    {
        _transactions.Add(transaction);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(Guid accountId, CancellationToken ct)
    {
        return Task.FromResult<IEnumerable<Transaction>>(
            _transactions.Where(t => t.AccountId == accountId).OrderByDescending(t => t.Date)
        );
    }
}