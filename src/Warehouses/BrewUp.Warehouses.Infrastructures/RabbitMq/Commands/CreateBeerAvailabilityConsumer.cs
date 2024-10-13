using BrewUp.Warehouses.Domain.CommandHandlers;
using BrewUp.Warehouses.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;
using Muflone.Transport.RabbitMQ.Abstracts;
using Muflone.Transport.RabbitMQ.Consumers;

namespace BrewUp.Warehouses.Infrastructures.RabbitMq.Commands;

public sealed class CreateBeerAvailabilityConsumer(IRepository repository,
    IRabbitMQConnectionFactory connectionFactory,
    ILoggerFactory loggerFactory) : CommandConsumerBase<CreateBeerAvailability>(repository, connectionFactory, loggerFactory)
{
    protected override ICommandHandlerAsync<CreateBeerAvailability> HandlerAsync { get; } = new CreateBeerAvailabilityCommandHandler(repository, loggerFactory);
}