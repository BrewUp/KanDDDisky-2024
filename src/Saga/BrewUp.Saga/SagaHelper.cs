using BrewUp.Infrastructure.RabbitMq;
using BrewUp.Saga.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BrewUp.Saga;

public static class SagaHelper
{
	public static IServiceCollection AddSaga(this IServiceCollection services)
	{
		services.AddFluentValidationAutoValidation();
		services.AddValidatorsFromAssemblyContaining<SalesOrderContractValidator>();
		services.AddSingleton<ValidationHandler>();


		return services;
	}
}