using AccountService.App.Commands;
using AccountService.Data;
using AccountService.Infrastructure.Interface;
using Common.Src;
using MediatR;
     
namespace AccountService.App.Handlers.Commands
{
    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, Guid>
    {
        private readonly IWalletStorageService _walletStorage;
        private readonly IClientVerificationService _clientVerification;
        private readonly ICurrencyService _currencyService;

        public CreateWalletCommandHandler(IWalletStorageService walletStorage,
            IClientVerificationService clientVerification,
            ICurrencyService currencyService)
        {
            _walletStorage = walletStorage;
            _clientVerification = clientVerification;
            _currencyService = currencyService;
        }

        public async Task<Guid> Handle(CreateWalletCommand request, CancellationToken ct)
        {
            if (!await _clientVerification.ExistsAsync(request.OwnerId, ct))
                throw new InvalidOperationException("Владелец не найден.");

            if (!await _currencyService.IsSupportedAsync(request.Currency, ct))
                throw new InvalidOperationException($"Валюта {request.Currency} не поддерживается.");

            var wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                OwnerId = request.OwnerId,
                TypeWallet = request.Type,
                Currency = request.Currency,
                Balance = request.InitialBalance,
                InterestRate = request.InterestRate,
                DateOpen = DateTime.UtcNow
            };

            await _walletStorage.CreateAsync(wallet, ct);

            if (request.InitialBalance > 0)
            {
                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    AccountId = wallet.Id,
                    CounterPartyAccountId = null,
                    TransferAmount = request.InitialBalance,
                    Currency = request.Currency,
                    TransactionType = TransactionType.Credit,
                    Description = "Открытие счёта с начальным балансом",
                    Date = DateTime.UtcNow
                };
                await _walletStorage.AddTransactionAsync(transaction, ct);
            }

            return wallet.Id;
        }

    }
}
