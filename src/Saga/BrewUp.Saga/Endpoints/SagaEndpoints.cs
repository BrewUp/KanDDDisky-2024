using BrewUp.Saga.Validators;
using BrewUp.Shared.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BrewUp.Saga.Endpoints;

public static class SagaEndpoints
{
    public static WebApplication MapSagaEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/v1/sales/")
            .WithTags("Sales");

        group.MapPost("/", HandleCreateOrder)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status201Created)
            .WithName("CreateSalesOrder");

        return app;
    }

    private static async Task<IResult> HandleCreateOrder(
        IValidator<SalesOrderJson> validator,
        ValidationHandler validationHandler,
        SalesOrderJson body,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        await validationHandler.ValidateAsync(validator, body);
        if (!validationHandler.IsValid)
            return Results.BadRequest(validationHandler.Errors);

        var salesOrderId = "123"; //await salesUpFacade.CreateOrderAsync(body, cancellationToken);

        return Results.Created(new Uri($"/v1/sales/{salesOrderId}", UriKind.Relative), salesOrderId);
    }
}