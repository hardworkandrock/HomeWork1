using AccountService.Data;
using Common.Src;
using MediatR;

namespace AccountService.App.Commands
{
    public record CreateWalletCommand(Guid OwnerId, TypeWallet Type, string Currency, 
        decimal InitialBalance, decimal? InterestRate) : IRequest<Guid>; 
}
