using AccountService.App.DTO;
using AccountService.Data;
using Common.Src;
using MediatR;

namespace AccountService.App.Queries
{
    public record GetStatementQuery(Guid WalletId, DateTime From, DateTime To) : IRequest<StatementDto>; 
}
