using BrewUp.Payments.ReadModel.Services;
using BrewUp.Payments.SharedKernel.Commands;
using BrewUp.Payments.SharedKernel.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using Muflone.Persistence;

namespace BrewUp.Payments.Facade;

public sealed class PaymentFacade(ISavingsAccountService savingsAccountService,
    IServiceBus serviceBus) : IPaymentFacade
{
    public async Task DepositMoneyAsync(PaymentMovementJson body, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var savingsAccount = await savingsAccountService
            .CheckIfSavingsAccountExistsAsync(new CustomerId(new Guid(body.CustomerId)), cancellationToken)
            .ConfigureAwait(false);

        if (savingsAccount)
        {
            DepositMoney command = new (new CustomerId(new Guid(body.CustomerId)),
                new CustomerName(body.CustomerName),
                body.Amount);
            await serviceBus.SendAsync(command, cancellationToken).ConfigureAwait(false);            
        }
        else
        {
            CreateSavingsAccount createCommand = new(new CustomerId(new Guid(body.CustomerId)),
                new CustomerName(body.CustomerName));
            await serviceBus.SendAsync(createCommand, cancellationToken).ConfigureAwait(false);
            
            Thread.Sleep(300);
            
            DepositMoney depositCommand = new (new CustomerId(new Guid(body.CustomerId)),
                new CustomerName(body.CustomerName),
                body.Amount);
            await serviceBus.SendAsync(depositCommand, cancellationToken).ConfigureAwait(false);
        }
    }
}