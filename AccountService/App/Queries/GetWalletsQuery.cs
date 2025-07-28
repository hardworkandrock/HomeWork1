using AccountService.App.DTO;
using MediatR;

namespace AccountService.App.Queries;

public record GetWalletsQuery(Guid? OwnerId = null) : IRequest<List<WalletDto>>;