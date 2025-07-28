using AccountService.App.DTO;
using MediatR;

namespace AccountService.App.Queries;

public record GetWalletByIdQuery(Guid WalletId) : IRequest<WalletDto?>;