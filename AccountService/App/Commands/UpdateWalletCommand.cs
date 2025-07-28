using MediatR;

namespace AccountService.App.Commands;

public record UpdateWalletCommand(Guid WalletId, decimal? InterestRate, DateTime? DateClosed) : IRequest;