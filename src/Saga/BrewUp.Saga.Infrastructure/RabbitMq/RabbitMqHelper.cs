using BrewUp.Infrastructure.RabbitMq;
using BrewUp.Saga.Infrastructure.RabbitMq.Commands;
using BrewUp.Saga.Infrastructure.RabbitMq.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Persistence;
using Muflone.Saga.Persistence;
using Muflone.Transport.RabbitMQ;
using Muflone.Transport.RabbitMQ.Factories;
using Muflone.Transport.RabbitMQ.Models;

namespace BrewUp.Saga.Infrastructure.RabbitMq;

public static class RabbitMqHelper
{
	public static IServiceCollection AddRabbitMqForSagaModule(this IServiceCollection services,
		RabbitMqSettings rabbitMqSettings)
	{
		var serviceProvider = services.BuildServiceProvider();
		var repository = serviceProvider.GetRequiredService<ISagaRepository>();
		var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

		var rabbitMqConfiguration = new RabbitMQConfiguration(rabbitMqSettings.Host, rabbitMqSettings.Username,
			rabbitMqSettings.Password, rabbitMqSettings.ExchangeCommandName, rabbitMqSettings.ExchangeEventName,
			rabbitMqSettings.ClientId);
		var rabbitConnectionFactory = new RabbitMQConnectionFactory(rabbitMqConfiguration, loggerFactory);

		services.AddMufloneTransportRabbitMQ(loggerFactory, rabbitMqConfiguration);

		serviceProvider = services.BuildServiceProvider();
		var consumers = serviceProvider.GetRequiredService<IEnumerable<IConsumer>>();
		consumers = consumers.Concat(new List<IConsumer>
		{
			new StartSalesOrderSagaConsumer(repository,
				rabbitConnectionFactory,
				loggerFactory,
				serviceProvider.GetRequiredService<IServiceBus>()),
			
			new BeerAvailabilityCommunicatedConsumer(rabbitConnectionFactory, loggerFactory,
				serviceProvider.GetRequiredService<IServiceBus>(),
				repository),
			new SalesOrderCreatedCommunicatedConsumer(rabbitConnectionFactory, loggerFactory,
				serviceProvider.GetRequiredService<IServiceBus>(),
				repository),
			
			new PaymentAcceptedConsumer(rabbitConnectionFactory, loggerFactory,
				serviceProvider.GetRequiredService<IServiceBus>(),
				repository),
			new PaymentRejectedConsumer(rabbitConnectionFactory, loggerFactory,
				serviceProvider.GetRequiredService<IServiceBus>(),
				repository)
		});
		services.AddMufloneRabbitMQConsumers(consumers);
		
		return services;
	}
}