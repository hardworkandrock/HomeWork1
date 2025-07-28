using AccountService.App.Commands;
using AccountService.Infrastructure.Interface;
using Common.Src;
using MediatR;

namespace AccountService.App.Handlers.Commands;

public class RegisterTransactionCommandHandler : IRequestHandler<RegisterTransactionCommand, Guid>
{
    private readonly IWalletStorageService _walletStorage;

    public RegisterTransactionCommandHandler(IWalletStorageService walletStorage)
    {
        _walletStorage = walletStorage;
    }

    public async Task<Guid> Handle(RegisterTransactionCommand request, CancellationToken ct)
    {
        // 1. Получаем счёт
        var account = await _walletStorage.GetByIdAsync(request.AccountId, ct);
        if (account == null)
            throw new InvalidOperationException("Счёт не найден.");

        // 2. Проверка валюты
        if (account.Currency != request.Currency)
            throw new InvalidOperationException("Валюта транзакции не совпадает с валютой счёта.");

        // 3. Создаём транзакцию
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            CounterPartyAccountId = request.CounterPartyAccountId,
            TransferAmount = request.Amount,
            Currency = request.Currency,
            TransactionType = request.Type,
            Description = request.Description,
            Date = DateTime.UtcNow
        };

        // 4. Обновление баланса
        if (request.Type == TransactionType.Credit)
        {
            account.Balance += request.Amount;
        }
        else if (request.Type == TransactionType.Debit)
        {
            if (account.Balance < request.Amount)
                throw new InvalidOperationException("Недостаточно средств.");
            account.Balance -= request.Amount;
        }

        // 5. Сохраняем изменения
        await _walletStorage.UpdateAsync(account, ct); // Обновляем счёт
        await _walletStorage.AddTransactionAsync(transaction, ct); // Добавляем транзакцию

        return transaction.Id;
    }
}