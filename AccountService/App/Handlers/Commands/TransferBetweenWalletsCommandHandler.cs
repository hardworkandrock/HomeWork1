using AccountService.App.Commands;
using AccountService.Data;
using AccountService.Infrastructure.Interface;
using Common.Src;
using MediatR;

namespace AccountService.App.Handlers.Commands
{
    public class TransferBetweenWalletsCommandHandler : IRequestHandler<TransferBetweenWalletsCommand>
    {
        private readonly IMediator _mediator;
        private readonly IWalletStorageService _walletStorage;

        public TransferBetweenWalletsCommandHandler(
            IMediator mediator,
            IWalletStorageService walletStorage)
        {
            _mediator = mediator;
            _walletStorage = walletStorage;
        }

        public async Task Handle(TransferBetweenWalletsCommand request, CancellationToken ct)
        {
            try
            {
                await _mediator.Send(new RegisterTransactionCommand(
                    request.FromAccountId,
                    request.ToAccountId,
                    request.Amount,
                    "RUB",
                    TransactionType.Debit,
                    request.Description + " (исходящий перевод)"), ct);

                await _mediator.Send(new RegisterTransactionCommand(
                    request.ToAccountId,
                    request.FromAccountId,
                    request.Amount,
                    "RUB",
                    TransactionType.Credit,
                    request.Description + " (входящий перевод)"), ct);
            }
            catch
            {
                // rollback
                throw;
            }
        }
    }
}
