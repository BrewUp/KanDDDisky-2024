using System.Collections.Concurrent;
using Muflone.Persistence;
using Muflone.Saga.Persistence;

namespace BrewUp.Saga.Tests.Persistence;

public class InMemorySagaRepository(ISerializer serializer) : ISagaRepository, IDisposable
{
    internal static readonly ConcurrentDictionary<Guid, string> Data = new();

    public async Task<TSagaState> GetByIdAsync<TSagaState>(Guid correlationId) where TSagaState : class, new()
    {
        if (!Data.TryGetValue(correlationId, out var stateSerialized))
            return default;

        return await serializer.DeserializeAsync<TSagaState>(stateSerialized).ConfigureAwait(false);
    }

    public async Task SaveAsync<TSagaState>(Guid id, TSagaState sagaState) where TSagaState : class, new()
    {
        var serializedData = await serializer.SerializeAsync(sagaState);

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