using AccountService.Data;
using Common.Src;
using MediatR;

namespace AccountService.App.Queries
{
    public record CheckWalletExistsQuery(Guid OwnerId, TypeWallet Type) : IRequest<bool>; 
}
