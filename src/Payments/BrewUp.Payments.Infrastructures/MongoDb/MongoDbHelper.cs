using BrewUp.Shared.ReadModel;
using Microsoft.Extensions.DependencyInjection;

namespace BrewUp.Payments.Infrastructures.MongoDb;

public static class MongoDbHelper
{
	public static IServiceCollection AddPaymentsMongoDb(this IServiceCollection services)
	{
		services.AddKeyedScoped<IPersister, PaymentsPersister>("payments");

		return services;
	}
}