using BrewUp.Infrastructure.RabbitMq;
using BrewUp.Payments.Facade.Validators;
using BrewUp.Payments.Infrastructures.MongoDb;
using BrewUp.Payments.Infrastructures.RabbitMq;
using BrewUp.Payments.ReadModel.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BrewUp.Payments.Facade;

public static class PaymentsHelper
{
    public static IServiceCollection AddPayments(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<PaymentMovementValidator>();
        services.AddSingleton<ValidationHandler>();

        services.AddScoped<IPaymentFacade, PaymentFacade>();
        services.AddScoped<ISavingsAccountService, SavingsAccountService>();

        return services;
    }
    
    public static IServiceCollection AddPaymentsInfrastructure(this IServiceCollection services, RabbitMqSettings rabbitMqSettings)
    {
        services.AddPaymentsMongoDb();
        services.AddRabbitMqForPaymentsModule(rabbitMqSettings);

        return services;
    }
}