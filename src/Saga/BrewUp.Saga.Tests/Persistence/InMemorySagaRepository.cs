using Muflone.Persistence;
using Muflone.Saga.Persistence;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

//TODO: To implement in the persistence concrete code. Create also a package for MongoDB or RavenDB as an example?
namespace BrewUp.Saga.Tests.Persistence
{
	public class InMemorySagaRepository : ISagaRepository, IDisposable
	{
		private readonly ISerializer _serializer;
		internal static readonly ConcurrentDictionary<Guid, string> Data = new ConcurrentDictionary<Guid, string>();

		public InMemorySagaRepository(ISerializer serializer)
		{
			this._serializer = serializer;
		}

		public async Task<TSagaState> GetByIdAsync<TSagaState>(Guid correlationId) where TSagaState : class, new()
		{
			if (!Data.TryGetValue(correlationId, out var stateSerialized))
				return default;

			return await _serializer.DeserializeAsync<TSagaState>(stateSerialized).ConfigureAwait(false);
		}

		public async Task SaveAsync<TSagaState>(Guid id, TSagaState sagaState) where TSagaState : class, new()
		{
			var serializedData = await _serializer.SerializeAsync(sagaState);

			Data[id] = serializedData;
		}

		public Task CompleteAsync(Guid correlationId)
		{
			Data.TryRemove(correlationId, out _);
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			Data.Clear();
		}
	}
}