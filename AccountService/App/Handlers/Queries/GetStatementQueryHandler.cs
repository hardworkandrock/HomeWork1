using AccountService.App.DTO;
using AccountService.App.Queries;
using AccountService.Infrastructure.Interface;
using Common.Src;
using MediatR;

namespace AccountService.App.Handlers.Queries;

public class GetStatementQueryHandler : IRequestHandler<GetStatementQuery, StatementDto>
{
    private readonly IWalletStorageService _walletStorage;

    public GetStatementQueryHandler(IWalletStorageService walletStorage)
    {
        _walletStorage = walletStorage;
    }

    public async Task<StatementDto> Handle(GetStatementQuery request, CancellationToken ct)
    {
        // 1. Получаем счёт
        var wallet = await _walletStorage.GetByIdAsync(request.WalletId, ct);
        if (wallet == null)
            throw new InvalidOperationException("Счёт не найден.");

        // 2. Получаем все транзакции счёта
        var allTransactions = await _walletStorage.GetTransactionsByAccountIdAsync(request.WalletId, ct);

        // 3. Фильтруем по диапазону дат
        var filteredTransactions = allTransactions
            .Where(t => t.Date >= request.From && t.Date <= request.To)
            .OrderBy(t => t.Date)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                AccountId = t.AccountId,
                CounterPartyAccountId = t.CounterPartyAccountId,
                TransferAmount = t.TransferAmount,
                Currency = t.Currency,
                TransactionType = t.TransactionType,
                Description = t.Description,
                Date = t.Date
            })
            .ToList();

        // 4. Рассчитываем начальный баланс
        // Предполагаем, что баланс на момент запроса — это текущий Balance
        // Начальный баланс = текущий баланс - все изменения в периоде
        var netChange = filteredTransactions.Sum(t =>
            t.TransactionType == TransactionType.Credit ? t.TransferAmount : -t.TransferAmount);

        var startingBalance = wallet.Balance - netChange;
        var endingBalance = wallet.Balance; // потому что Balance — актуальный

        return new StatementDto
        {
            WalletId = request.WalletId,
            StartingBalance = startingBalance,
            EndingBalance = endingBalance,
            Transactions = filteredTransactions
        };
    }
}