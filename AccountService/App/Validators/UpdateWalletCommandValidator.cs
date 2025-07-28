using AccountService.App.Commands;
using FluentValidation;

namespace AccountService.App.Validators;

public class UpdateWalletCommandValidator : AbstractValidator<UpdateWalletCommand>
{
    public UpdateWalletCommandValidator()
    {
        RuleFor(x => x.WalletId).NotEmpty();

        When(x => x.InterestRate.HasValue, () =>
        {
            RuleFor(x => x.InterestRate)
                .NotNull()
                .Must(rate => rate is >= 0 and <= 100)
                .WithMessage("Процентная ставка должна быть от 0 до 100.");
        });
    }
}