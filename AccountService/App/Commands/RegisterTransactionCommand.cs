using AccountService.Data;
using Common.Src;
using MediatR;

namespace AccountService.App.Commands
{
    public record RegisterTransactionCommand(Guid AccountId, Guid? CounterPartyAccountId, 
        decimal Amount, string Currency, TransactionType Type, string Description) : IRequest<Guid>; 
}
