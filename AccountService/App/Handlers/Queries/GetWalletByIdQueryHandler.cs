using AccountService.App.DTO;
using AccountService.App.Queries;
using AccountService.Data;
using AccountService.Infrastructure.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.App.Handlers.Queries
{
    public class GetWalletByIdQueryHandler : IRequestHandler<GetWalletByIdQuery, WalletDto?>
    {
        private readonly IWalletStorageService _walletStorage;
        private readonly IOwnerService _ownerService;

        public GetWalletByIdQueryHandler(
            IWalletStorageService walletStorage,
            IOwnerService ownerService)
        {
            _walletStorage = walletStorage;
            _ownerService = ownerService;
        }

        public async Task<WalletDto?> Handle(GetWalletByIdQuery request, CancellationToken ct)
        {
            // 1. Получаем счёт
            var wallet = await _walletStorage.GetByIdAsync(request.WalletId, ct);
            if (wallet == null)
                return null;

            // 2. Получаем владельца
            var owner = await _ownerService.GetByIdAsync(wallet.OwnerId, ct);
            var ownerName = owner != null
                ? $"{owner.LastName} {owner.FirstName} {owner.FatherName}".Trim()
                : "Неизвестно";

            // 3. Возвращаем DTO
            return new WalletDto
            {
                Id = wallet.Id,
                OwnerId = wallet.OwnerId,
                OwnerName = ownerName,
                Type = wallet.TypeWallet,
                Currency = wallet.Currency,
                Balance = wallet.Balance,
                InterestRate = wallet.InterestRate,
                DateOpen = wallet.DateOpen,
                DateClosed = wallet.DateClosed
            };
        }
    }
}
