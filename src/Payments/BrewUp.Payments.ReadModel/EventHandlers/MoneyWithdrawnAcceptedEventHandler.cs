using BrewUp.Payments.ReadModel.Services;
using BrewUp.Payments.SharedKernel.Events;
using Microsoft.Extensions.Logging;

namespace BrewUp.Payments.ReadModel.EventHandlers;

public sealed class MoneyWithdrawnAcceptedEventHandler(ISavingsAccountService savingsAccountService,
    ILoggerFactory loggerFactory) : DomainEventHandlerBase<MoneyWithdrawAccepted>(loggerFactory)
{
    public override async Task HandleAsync(MoneyWithdrawAccepted @event, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await savingsAccountService.WithdrawMoneyAsync(@event.CustomerId, @event.Amount, cancellationToken);
    }
}