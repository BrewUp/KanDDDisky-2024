using BrewUp.Saga.Messages.Commands;
using BrewUp.Saga.Validators;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Muflone.Persistence;

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
        IServiceBus serviceBus,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        await validationHandler.ValidateAsync(validator, body);
        if (!validationHandler.IsValid)
            return Results.BadRequest(validationHandler.Errors);

        if (string.IsNullOrWhiteSpace(body.SalesOrderId))
            body = body with { SalesOrderId = Guid.NewGuid().ToString() };

        StartSalesOrderSaga command = new(new SalesOrderId(new Guid(body.SalesOrderId)), Guid.NewGuid(), 
            new SalesOrderNumber(body.SalesOrderNumber),
            new OrderDate(body.OrderDate),
            new CustomerId(body.CustomerId), new CustomerName(body.CustomerName), 
            body.Rows);
        await serviceBus.SendAsync(command, cancellationToken);
        

        return Results.Created(new Uri($"/v1/sales/{body.SalesOrderId}", UriKind.Relative), body.SalesOrderId);
    }
}