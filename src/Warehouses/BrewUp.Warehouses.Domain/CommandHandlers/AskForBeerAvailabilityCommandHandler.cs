using BrewUp.Shared.CustomTypes;
using BrewUp.Warehouses.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace BrewUp.Warehouses.Domain.CommandHandlers;

public sealed class AskForBeerAvailabilityCommandHandler(IRepository repository,
    ILoggerFactory loggerFactory): CommandHandlerBaseAsync<AskForBeerAvailability>(repository, loggerFactory)
{
    public override async Task ProcessCommand(AskForBeerAvailability command, CancellationToken cancellationToken = default)
    {
        var aggregate = await Repository.GetByIdAsync<Entities.Availability>(command.BeerId, cancellationToken);
        aggregate!.AskForAvailability(command.MessageId);
        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
}