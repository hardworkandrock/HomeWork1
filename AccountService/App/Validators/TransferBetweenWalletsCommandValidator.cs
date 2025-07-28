using AccountService.App.Commands;
using FluentValidation;

namespace AccountService.App.Validators
{
    public class TransferBetweenWalletsCommandValidator : AbstractValidator<TransferBetweenWalletsCommand>
    {
        public TransferBetweenWalletsCommandValidator()
        {
            RuleFor(x => x.FromAccountId).NotEmpty().NotEqual(x => x.ToAccountId)
                .WithMessage("Счёт отправителя и получателя не могут совпадать.");
            RuleFor(x => x.ToAccountId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        }
    }
}
