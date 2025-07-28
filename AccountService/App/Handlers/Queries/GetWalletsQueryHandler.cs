using AccountService.App.DTO;
using AccountService.App.Queries;
using AccountService.Infrastructure.Interface;
using Common.Src;
using MediatR;

namespace AccountService.App.Handlers.Queries;

public class GetWalletsQueryHandler : IRequestHandler<GetWalletsQuery, List<WalletDto>>
{
    private readonly IOwnerService _ownerService;
    private readonly IWalletStorageService _walletStorage;

    public GetWalletsQueryHandler(
        IWalletStorageService walletStorage,
        IOwnerService ownerService)
    {
        _walletStorage = walletStorage;
        _ownerService = ownerService;
    }

    public async Task<List<WalletDto>> Handle(GetWalletsQuery request, CancellationToken ct)
    {
        // 1. Получаем счета
        IEnumerable<Wallet> wallets;
        if (request.OwnerId.HasValue)
            wallets = await _walletStorage.GetByOwnerIdAsync(request.OwnerId.Value, ct);
        else
            wallets = await _walletStorage.GetAllAsync(ct);

        // 2. Формируем DTO
        var result = new List<WalletDto>();
        foreach (var wallet in wallets)
        {
            var owner = await _ownerService.GetByIdAsync(wallet.OwnerId, ct);
            var ownerName = owner != null
                ? $"{owner.LastName} {owner.FirstName} {owner.FatherName}".Trim()
                : "Неизвестно";

            result.Add(new WalletDto
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
            });
        }

        return result;
    }
}