using AccountService.App.Queries;
using FluentValidation;

namespace AccountService.App.Validators
{
    public class GetStatementQueryValidator : AbstractValidator<GetStatementQuery>
    {
        public GetStatementQueryValidator()
        {
            RuleFor(x => x.WalletId).NotEmpty();
            RuleFor(x => x.From).LessThan(x => x.To).WithMessage("Дата 'от' должна быть раньше даты 'до'.");
            RuleFor(x => x.To).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Дата 'до' не может быть в будущем.");
        }
    }
}
