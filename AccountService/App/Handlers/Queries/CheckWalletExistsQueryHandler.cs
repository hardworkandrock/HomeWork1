using AccountService.App.Queries;
using AccountService.Data;
using AccountService.Infrastructure.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.App.Handlers.Queries
{
    public class CheckWalletExistsQueryHandler : IRequestHandler<CheckWalletExistsQuery, bool>
    {
        private readonly IWalletStorageService _walletStorage;

        public CheckWalletExistsQueryHandler(IWalletStorageService walletStorage)
        {
            _walletStorage = walletStorage;
        }

        public async Task<bool> Handle(CheckWalletExistsQuery request, CancellationToken ct)
        {
            var wallets = await _walletStorage.GetByOwnerIdAsync(request.OwnerId, ct);
            return wallets.Any(w => w.TypeWallet == request.Type && !w.DateClosed.HasValue); 
        }
    }
}
