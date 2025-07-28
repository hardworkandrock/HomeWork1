using AccountService.App.Commands;
using AccountService.Infrastructure.Interface;
using Common.Src;
using MediatR;

namespace AccountService.App.Handlers.Commands;

public class UpdateWalletCommandHandler : IRequestHandler<UpdateWalletCommand>
{
    private readonly IWalletStorageService _walletStorage;

    public UpdateWalletCommandHandler(IWalletStorageService walletStorage)
    {
        _walletStorage = walletStorage;
    }

    public async Task Handle(UpdateWalletCommand request, CancellationToken ct)
    {
        // 1. Получаем счёт
        var wallet = await _walletStorage.GetByIdAsync(request.WalletId, ct);
        if (wallet == null)
            throw new InvalidOperationException("Счёт не найден.");

        // 2. Обновление процентной ставки
        if (request.InterestRate.HasValue)
        {
            if (wallet.TypeWallet == TypeWallet.Checking)
                throw new ArgumentException("Текущий счёт не может иметь процентную ставку.");
            wallet.InterestRate = request.InterestRate;
        }

        // 3. Закрытие счёта
        if (request.DateClosed.HasValue)
        {
            if (wallet.Balance != 0)
                throw new InvalidOperationException("Счёт можно закрыть только при нулевом балансе.");
            wallet.DateClosed = request.DateClosed;
        }

        // 4. Сохраняем изменения
        await _walletStorage.UpdateAsync(wallet, ct);
    }
}