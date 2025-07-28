using AccountService.App.DTO;
using AccountService.Data;
using MediatR;

namespace AccountService.App.Queries
{
    public record GetWalletByIdQuery(Guid WalletId) : IRequest<WalletDto?>;
}
