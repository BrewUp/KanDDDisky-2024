using BrewUp.Sales.Domain.Entities;
using BrewUp.Sales.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace BrewUp.Sales.Domain.CommandHandlers;

public class CloseSalesOrderCommandHandler(IRepository repository, ILoggerFactory loggerFactory)
    : CommandHandlerAsync<CloseSalesOrder>(repository, loggerFactory)
{
    public override async Task HandleAsync(CloseSalesOrder command, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var aggregate = await Repository.GetByIdAsync<SalesOrder>(command.SalesOrderId, cancellationToken);
        aggregate!.CloseSalesOrder(command.SalesOrderNumber, command.MessageId);
        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
}