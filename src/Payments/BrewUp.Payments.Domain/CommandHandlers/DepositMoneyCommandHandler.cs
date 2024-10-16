using BrewUp.Payments.Domain.Entities;
using BrewUp.Payments.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace BrewUp.Payments.Domain.CommandHandlers;

public sealed class DepositMoneyCommandHandler(IRepository repository, ILoggerFactory loggerFactory)
    : CommandHandlerAsync<DepositMoney>(repository, loggerFactory)
{
    public override async Task HandleAsync(DepositMoney command, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var aggregate = await Repository.GetByIdAsync<SavingsAccount>(command.AggregateId, cancellationToken);
        aggregate!.Deposit(command.Amount);
        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
}