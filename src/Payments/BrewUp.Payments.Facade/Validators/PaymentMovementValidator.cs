using BrewUp.Payments.SharedKernel.Contracts;
using FluentValidation;

namespace BrewUp.Payments.Facade.Validators;

public sealed class PaymentMovementValidator : AbstractValidator<PaymentMovementJson>
{
    public PaymentMovementValidator()
    {
        RuleFor(v => v.CustomerId).NotEmpty().NotEqual(Guid.Empty.ToString());
        RuleFor(v => v.CustomerName).NotEmpty();
        RuleFor(v => v.Amount.Value).GreaterThan(0);
        RuleFor(v => v.Amount.Currency).NotEmpty();
    }
}