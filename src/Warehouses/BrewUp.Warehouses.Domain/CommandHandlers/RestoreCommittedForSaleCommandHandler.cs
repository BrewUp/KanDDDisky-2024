using BrewUp.Warehouses.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace BrewUp.Warehouses.Domain.CommandHandlers;

public class RestoreCommittedForSaleCommandHandler(IRepository repository,
    ILoggerFactory loggerFactory) : CommandHandlerAsync<RestoreCommittedForSale>(repository, loggerFactory)
{
    public override async Task HandleAsync(RestoreCommittedForSale command, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var aggregate = await Repository.GetByIdAsync<Entities.Availability>(command.BeerId, cancellationToken);
        aggregate!.RestoreCommittedForSale(command.Quantity, command.MessageId);
        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
}