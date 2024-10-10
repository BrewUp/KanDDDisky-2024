using BrewUp.Saga;
using BrewUp.Saga.Endpoints;

namespace BrewUp.Rest.Modules;

public class SagaModule : IModule
{
    public bool IsEnabled => true;
    public int Order => 0;
    public IServiceCollection Register(WebApplicationBuilder builder) => builder.Services.AddSaga();

    public WebApplication Configure(WebApplication app) => app.MapSagaEndpoints();
}