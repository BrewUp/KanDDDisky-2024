using BrewUp.Warehouses.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace BrewUp.Warehouses.Domain.CommandHandlers;

public class RestoreCommittedForSaleCommandHandler(IRepository repository,
    ILoggerFactory loggerFactory) : CommandHandlerBaseAsync<RestoreCommittedForSale>(repository, loggerFactory)
{
    public override async Task ProcessCommand(RestoreCommittedForSale command, CancellationToken cancellationToken = default)
    {
        var aggregate = await Repository.GetByIdAsync<Entities.Availability>(command.BeerId, cancellationToken);
        aggregate!.RestoreCommittedForSale(command.Quantity, command.MessageId);
        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
}