using AccountService.Data;
using Common.Src;
using MediatR;

namespace AccountService.App.Commands
{
    public record TransferBetweenWalletsCommand(Guid FromAccountId, Guid ToAccountId, decimal Amount, string Description) : IRequest; 
}
