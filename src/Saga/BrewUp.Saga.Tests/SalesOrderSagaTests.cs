using BrewUp.Payments.SharedKernel.Commands;
using BrewUp.Saga.Messages.Commands;
using BrewUp.Saga.Tests.Persistence;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainIds;
using BrewUp.Shared.Messages.Sagas;
using Microsoft.Extensions.Logging.Abstractions;
using Muflone.Messages.Events;
using Muflone.Persistence;

namespace BrewUp.Saga.Tests
{
	public class SalesOrderSagaTests
	{
		private InProcessServiceBus _serviceBus;
		private InMemorySagaRepository _inMemorySagaRepository = new(new Serializer());

		public SalesOrderSagaTests()
		{
			_serviceBus = new InProcessServiceBus(typeof(StartSalesOrderSaga),
				new Dictionary<Type, Event>()
				{
					{ typeof(AskForBeerAvailability), new BeerAvailableCommunicated(_beerId, _correlationId, new Quantity(10, "Liters")) },
					{ typeof(CreateSalesOrder), new SalesOrderCreatedCommunicated(new SalesOrderId(Guid.NewGuid()), _correlationId, new SalesOrderNumber("123"), new OrderDate(DateTime.Today), _customerId,
							new CustomerName("abc"), new List<SalesOrderRowJson> { new()
							{
								BeerId = new Guid(_beerId.Value),
								BeerName = "Muflone IPA",
								Quantity = new Quantity(1, "Liters"),
								Price = new Price(8, "€")
							} }) },


					{typeof(WithdrawMoney), new PaymentAccepted(_customerId, _correlationId, new Amount(8, "€")) }
				});
		}

		private readonly BeerId _beerId = new (Guid.NewGuid());
		private readonly CustomerId _customerId = new (Guid.NewGuid());

		private readonly Guid _correlationId = Guid.NewGuid();

		[Fact]
		public async Task Saga_StartsWithCommand()
		{
			var command = new StartSalesOrderSaga(new SalesOrderId(Guid.NewGuid()), _correlationId, new SalesOrderNumber("123"), new OrderDate(DateTime.Today), _customerId,
				new CustomerName("abc"), new List<SalesOrderRowJson> { new()
				{
					BeerId = new Guid(_beerId.Value),
					BeerName = "Muflone IPA",
					Quantity = new Quantity(1, "Liters"),
					Price = new Price(8, "€")
				} });
			await _serviceBus.SendAsync(command);

			var commands = _serviceBus.SentCommands();
			Assert.Equal(4, commands.Count);
			Assert.IsType<AskForBeerAvailability>(commands[1]);
			Assert.Equal(1, ((AskForBeerAvailability)commands[1]).Quantity.Value);


			Assert.IsType<CreateSalesOrder>(commands[2]);
			//other Asserts


			Assert.IsType<WithdrawMoney>(commands[3]);
			//other Asserts

			var data = await _inMemorySagaRepository.GetByIdAsync<SalesOrderSaga.SalesOrderSagaState>(_correlationId);
			Assert.NotNull(data);
			Assert.Equal("abc", data.CustomerName.Value);
			Assert.False(data.SagaFailed);
		}
	}
}
