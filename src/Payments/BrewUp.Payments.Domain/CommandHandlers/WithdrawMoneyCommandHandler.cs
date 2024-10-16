using BrewUp.Payments.Domain.Entities;
using BrewUp.Payments.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace BrewUp.Payments.Domain.CommandHandlers;

public sealed class WithdrawMoneyCommandHandler(IRepository repository, ILoggerFactory loggerFactory)
    : CommandHandlerAsync<WithdrawMoney>(repository, loggerFactory)
{
    public override async Task HandleAsync(WithdrawMoney command, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var aggregate = await Repository.GetByIdAsync<SavingsAccount>(command.CustomerId, cancellationToken);
        aggregate!.WithdrawingMoney(command.Amount, command.MessageId);
        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
}