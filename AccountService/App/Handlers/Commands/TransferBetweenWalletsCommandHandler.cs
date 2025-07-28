using AccountService.App.Commands;
using Common.Src;
using MediatR;

namespace AccountService.App.Handlers.Commands;

public class TransferBetweenWalletsCommandHandler : IRequestHandler<TransferBetweenWalletsCommand>
{
    private readonly IMediator _mediator;

    public TransferBetweenWalletsCommandHandler(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Handle(TransferBetweenWalletsCommand request, CancellationToken ct)
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
}