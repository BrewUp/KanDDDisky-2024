using BrewUp.Payments.Facade;
using BrewUp.Payments.Facade.Endpoints;

namespace BrewUp.Rest.Modules;

public class PaymentModule : IModule
{
    public bool IsEnabled => true;
    public int Order => 0;
    
    public IServiceCollection Register(WebApplicationBuilder builder) => builder.Services.AddPayments();

    WebApplication IModule.Configure(WebApplication app) => app.MapPaymentsEndpoints();
}