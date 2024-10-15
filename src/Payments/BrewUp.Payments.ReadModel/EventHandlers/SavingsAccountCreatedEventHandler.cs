using BrewUp.Payments.ReadModel.Services;
using BrewUp.Payments.SharedKernel.Events;
using Microsoft.Extensions.Logging;

namespace BrewUp.Payments.ReadModel.EventHandlers;

public sealed class SavingsAccountCreatedEventHandler(ISavingsAccountService savingsAccountService,
    ILoggerFactory loggerFactory) : DomainEventHandlerBase<SavingsAccountCreated>(loggerFactory)
{
    public override async Task HandleAsync(SavingsAccountCreated @event, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await savingsAccountService.CreateSavingsAccountAsync(@event.CustomerId, @event.CustomerName, cancellationToken);
    }
}