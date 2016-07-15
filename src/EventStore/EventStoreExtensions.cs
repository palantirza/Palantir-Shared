namespace Palantir.EventStore
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	public static class EventStoreExtensions
    {
		public static async Task AppendAndCommit<TEvent>(this IEventStore eventStore, Guid streamId, Action<TEvent> initMessage, Action<IDictionary<string, object>> initHeaders = null)
		{
			Contract.Requires(eventStore != null);
			Contract.Requires(initMessage != null);

			using (var stream = await eventStore.OpenStream(streamId))
			{
				stream.Add(initMessage, initHeaders);
				await stream.CommitChanges();
			}
		}

		public static async Task AppendAndCommit<TEvent>(this IEventStore eventStore, Action<TEvent> initMessage, Action<IDictionary<string, object>> initHeaders = null)
		{
			Contract.Requires(eventStore != null);
			Contract.Requires(initMessage != null);

			var ev = EventCreator.CreateInstanceOf(initMessage);

			var idProp = typeof(TEvent).GetProperty("Id", typeof(Guid));
			var streamId = (Guid)idProp.GetGetMethod(false).Invoke(ev, null);

			using (var stream = await eventStore.OpenStream(streamId))
			{
				stream.Add(initMessage, initHeaders);
				await stream.CommitChanges();
			}
		}
	}
}
