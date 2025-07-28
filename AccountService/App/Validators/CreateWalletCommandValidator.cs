using AccountService.App.Commands;
using Common.Src;
using FluentValidation;

namespace AccountService.App.Validators;

public class CreateWalletCommandValidator : AbstractValidator<CreateWalletCommand>
{
    public CreateWalletCommandValidator()
    {
        RuleFor(x => x.OwnerId).NotEmpty().WithMessage("OwnerId обязателен.");
        RuleFor(x => x.Type).IsInEnum().WithMessage("Некорректный тип счёта.");
        RuleFor(x => x.Currency).NotEmpty().Length(3).Matches("^[A-Z]{3}$")
            .WithMessage("Валюта должна быть ISO 4217 (например, RUB, USD).");
        RuleFor(x => x.InitialBalance).GreaterThanOrEqualTo(0).WithMessage("Баланс не может быть отрицательным.");

        When(x => x.Type == TypeWallet.Deposit || x.Type == TypeWallet.Credit, () =>
        {
            RuleFor(x => x.InterestRate).NotNull().InclusiveBetween(0, 100)
                .WithMessage("Процентная ставка обязательна для депозита и кредита и должна быть от 0 до 100.");
        });

        When(x => x.Type == TypeWallet.Checking, () =>
        {
            RuleFor(x => x.InterestRate).Null()
                .WithMessage("Текущий счёт не может иметь процентную ставку.");
        });
    }
}