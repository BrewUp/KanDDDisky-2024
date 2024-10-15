using BrewUp.Payments.Domain.Entities;
using BrewUp.Payments.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace BrewUp.Payments.Domain.CommandHandlers;

public sealed class WithdrawingMoneyCommandHandler(IRepository repository, ILoggerFactory loggerFactory)
    : CommandHandlerBaseAsync<WithdrawingMoney>(repository, loggerFactory)
{
    public override async Task ProcessCommand(WithdrawingMoney command, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var aggregate = await Repository.GetByIdAsync<SavingsAccount>(command.CustomerId, cancellationToken);
        aggregate!.WithdrawingMoney(command.Amount, command.MessageId);
        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
}