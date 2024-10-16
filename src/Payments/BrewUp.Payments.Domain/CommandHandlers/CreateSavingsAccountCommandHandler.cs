using BrewUp.Payments.Domain.Entities;
using BrewUp.Payments.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace BrewUp.Payments.Domain.CommandHandlers;

public sealed class CreateSavingsAccountCommandHandler(IRepository repository, ILoggerFactory loggerFactory)
    : CommandHandlerAsync<CreateSavingsAccount>(repository, loggerFactory)
{
    public override async Task HandleAsync(CreateSavingsAccount command, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var aggregate = SavingsAccount.Create(command.CustomerId, command.CustomerName);
        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
}