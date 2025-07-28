using AccountService.App.DTO;
using MediatR;

namespace AccountService.App.Queries;

public record GetStatementQuery(Guid WalletId, DateTime From, DateTime To) : IRequest<StatementDto>;