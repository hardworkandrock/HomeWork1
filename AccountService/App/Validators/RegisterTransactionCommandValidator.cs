using AccountService.App.Commands;
using FluentValidation;

namespace AccountService.App.Validators;

public class RegisterTransactionCommandValidator : AbstractValidator<RegisterTransactionCommand>
{
    public RegisterTransactionCommandValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Сумма должна быть положительной.");
        RuleFor(x => x.Currency).NotEmpty().Length(3).Matches("^[A-Z]{3}$");
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Description).MaximumLength(500);
    }
}