using BrewUp.Payments.SharedKernel.Contracts;

namespace BrewUp.Payments.Facade;

public interface IPaymentFacade
{
    Task DepositMoneyAsync(PaymentMovementJson body, CancellationToken cancellationToken);
}