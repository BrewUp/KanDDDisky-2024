using BrewUp.Infrastructure.RabbitMq;
using BrewUp.Payments.Infrastructures.RabbitMq.Commands;
using BrewUp.Payments.Infrastructures.RabbitMq.Events;
using BrewUp.Payments.ReadModel.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Persistence;
using Muflone.Transport.RabbitMQ;
using Muflone.Transport.RabbitMQ.Factories;
using Muflone.Transport.RabbitMQ.Models;

namespace BrewUp.Payments.Infrastructures.RabbitMq;

public static class RabbitMqHelper
{
	public static IServiceCollection AddRabbitMqForPaymentsModule(this IServiceCollection services,
		RabbitMqSettings rabbitMqSettings)
	{
		var serviceProvider = services.BuildServiceProvider();
		var repository = serviceProvider.GetRequiredService<IRepository>();
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
			new CreateSavingsAccountConsumer(repository,
				rabbitConnectionFactory,
				loggerFactory),
			new SavingsAccountCreatedConsumer(serviceProvider.GetRequiredService<ISavingsAccountService>(),
				rabbitConnectionFactory, loggerFactory),
			
			new DepositMoneyConsumer(repository,
				rabbitConnectionFactory,
				loggerFactory),
			new MoneyDepositedConsumer(serviceProvider.GetRequiredService<ISavingsAccountService>(),
				rabbitConnectionFactory, loggerFactory),
			
			new WithdrawMoneyConsumer(repository,
				rabbitConnectionFactory,
				loggerFactory),
			new MoneyWithdrawAcceptedConsumer(serviceProvider.GetRequiredService<ISavingsAccountService>(),
				serviceProvider.GetRequiredService<IEventBus>(),
				rabbitConnectionFactory, loggerFactory),
			new MoneyWithdrawRejectedConsumer(serviceProvider.GetRequiredService<IEventBus>(),
				rabbitConnectionFactory, loggerFactory)

		});
		services.AddMufloneRabbitMQConsumers(consumers);
		

		return services;
	}
}