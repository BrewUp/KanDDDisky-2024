using BrewUp.Sales.Domain.Entities;
using BrewUp.Sales.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Persistence;

namespace BrewUp.Sales.Domain.CommandHandlers;

public class CloseSalesOrderCommandHandler(IRepository repository, ILoggerFactory loggerFactory)
  : CommandHandlerBaseAsync<CloseSalesOrder>(repository, loggerFactory)
{
  public override async Task ProcessCommand(CloseSalesOrder command, CancellationToken cancellationToken = default)
  {
    var aggregate = await Repository.GetByIdAsync<SalesOrder>(command.SalesOrderId, cancellationToken);
    aggregate!.CloseSalesOrder(command.SalesOrderNumber, command.MessageId);
    await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
  }
}