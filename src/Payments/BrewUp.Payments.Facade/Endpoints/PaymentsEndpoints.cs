using BrewUp.Payments.Facade.Validators;
using BrewUp.Payments.SharedKernel.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BrewUp.Payments.Facade.Endpoints;

public static class PaymentsEndpoints
{
	public static WebApplication MapPaymentsEndpoints(this WebApplication app)
	{
		var group = app.MapGroup("/v1/payments/")
			.WithTags("Payments");

		group.MapPost("/", HandleDepositMoney)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status200OK)
			.WithName("DepositMoney");

		return app;
	}

	private static async Task<IResult> HandleDepositMoney(
		IPaymentFacade paymentFacade,
		ValidationHandler validationHandler,
		PaymentMovementJson body,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		// await validationHandler.ValidateAsync(validator, body);
		// if (!validationHandler.IsValid)
		// 	return Results.BadRequest(validationHandler.Errors);

		await paymentFacade.DepositMoneyAsync(body, cancellationToken);
		
		return Results.Ok();
	}
}