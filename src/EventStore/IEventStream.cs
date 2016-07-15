namespace Palantir.EventStore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

    public interface IEventStream : IDisposable
    {
		IEnumerable<EventMessage> CommittedEvents { get; }
		IEnumerable<EventMessage> UncommittedEvents { get; }
		int StreamRevision { get; }
		Guid StreamId { get; }
		void Add<TEvent>(TEvent uncommittedMessage, Action<IDictionary<string, object>> initHeaders = null);
		void Add<TEvent>(Action<TEvent> initMessage, Action<IDictionary<string, object>> initHeaders = null);
		Task CommitChanges();
    }
}
