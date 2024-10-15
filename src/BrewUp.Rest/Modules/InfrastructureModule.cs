using BrewUp.Infrastructure;
using BrewUp.Infrastructure.MongoDb;
using BrewUp.Infrastructure.RabbitMq;
using BrewUp.Payments.Facade;
using BrewUp.Saga.Infrastructure.RabbitMq;
using BrewUp.Sales.Facade;
using BrewUp.Warehouses.Facade;

namespace BrewUp.Rest.Modules;

public class InfrastructureModule : IModule
{
	public bool IsEnabled => true;
	public int Order => 90;

	public IServiceCollection Register(WebApplicationBuilder builder)
	{
		var eventStoreSettings = builder.Configuration.GetSection("BrewUp:EventStore")
			.Get<EventStoreSettings>()!;
		
		builder.Services.AddInfrastructure(builder.Configuration.GetSection("BrewUp:MongoDbSettings").Get<MongoDbSettings>()!,
			eventStoreSettings);

		var rabbitMqSettings = builder.Configuration.GetSection("BrewUp:RabbitMQ")
			.Get<RabbitMqSettings>()!;

		builder.Services.AddSalesInfrastructure(rabbitMqSettings);
		builder.Services.AddWarehousesInfrastructure(rabbitMqSettings);
		builder.Services.AddPaymentsInfrastructure(rabbitMqSettings);
		builder.Services.AddRabbitMqForSagaModule(rabbitMqSettings);

		return builder.Services;
	}

	WebApplication IModule.Configure(WebApplication app) => app;
}