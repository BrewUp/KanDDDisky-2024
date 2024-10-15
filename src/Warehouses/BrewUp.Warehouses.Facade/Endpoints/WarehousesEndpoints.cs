using BrewUp.Warehouses.Facade.Validators;
using BrewUp.Warehouses.SharedKernel.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BrewUp.Warehouses.Facade.Endpoints;

public static class WarehousesEndpoints
{
	public static WebApplication MapWarehousesEndpoints(this WebApplication app)
	{
		var group = app.MapGroup("/v1/wareHouses/")
			.WithTags("Warehouses");

		group.MapPost("/availabilities", HandleSetAvailabilities)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status200OK)
			.WithName("SetAvailabilities");
		group.MapGet("/availabilities", HandleGetAvailabilities)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status200OK)
			.WithName("GetAvailabilities");

		return app;
	}

	private static async Task<IResult> HandleSetAvailabilities(
		IWarehousesFacade warehousesFacade,
		IValidator<SetAvailabilityJson> validator,
		ValidationHandler validationHandler,
		SetAvailabilityJson body,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
        
		await validationHandler.ValidateAsync(validator, body);
		if (!validationHandler.IsValid)
			return Results.BadRequest(validationHandler.Errors);
		
		await warehousesFacade.SetAvailabilityAsync(body, cancellationToken);

		return Results.Ok();
	}
	
	private static async Task<IResult> HandleGetAvailabilities(
		IWarehousesFacade warehousesFacade,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		
		var availabilities = await warehousesFacade.GetAvailabilitiesAsync(cancellationToken);

		return Results.Ok(availabilities);
	}
}