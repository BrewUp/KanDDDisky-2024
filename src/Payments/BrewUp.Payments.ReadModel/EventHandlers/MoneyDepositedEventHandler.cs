using BrewUp.Payments.ReadModel.Services;
using BrewUp.Payments.SharedKernel.Events;
using Microsoft.Extensions.Logging;

namespace BrewUp.Payments.ReadModel.EventHandlers;

public sealed class MoneyDepositedEventHandler(ISavingsAccountService savingsAccountService,
    ILoggerFactory loggerFactory) : DomainEventHandlerBase<MoneyDeposited>(loggerFactory)
{
    public override async Task HandleAsync(MoneyDeposited @event, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        await savingsAccountService.DepositMoneyAsync(@event.CustomerId, @event.Amount, cancellationToken);
    }
}