using BrewUp.Sales.Domain.Entities;
using BrewUp.Sales.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace BrewUp.Sales.Domain.CommandHandlers;

public class CloseSalesOrderCommandHandler : CommandHandlerBaseAsync<CloseSalesOrder>
{
    public CloseSalesOrderCommandHandler(IRepository repository, ILoggerFactory loggerFactory) : base(repository, loggerFactory)
    {
    }

    public override async Task ProcessCommand(CloseSalesOrder command, CancellationToken cancellationToken = default)
    {
        var aggregate = await Repository.GetByIdAsync<SalesOrder>(command.SalesOrderId.Value, cancellationToken);
        aggregate.CloseSalesOrder(command.SalesOrderNumber, command.MessageId);
        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
}