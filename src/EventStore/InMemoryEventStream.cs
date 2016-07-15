using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Palantir.EventStore
{
    public sealed class InMemoryEventStream : IEventStream
    {
		private readonly List<EventMessage> committedEventsList;
		private readonly List<EventMessage> uncommittedEventsList = new List<EventMessage>();
		private readonly Guid streamId;
		private int streamRevision = -1;
		private readonly InMemoryEventStore eventStore;
		private bool disposed;

		public IEnumerable<EventMessage> CommittedEvents => committedEventsList;

		public IEnumerable<EventMessage> UncommittedEvents => uncommittedEventsList;

		public int StreamRevision => streamRevision;

		public Guid StreamId => streamId;

		internal InMemoryEventStream(InMemoryEventStore eventStore, Guid streamId, int streamRevision, List<EventMessage> committedEventsList)
			: this(eventStore, streamId, streamRevision, committedEventsList, new List<EventMessage>())
		{
		}

		internal InMemoryEventStream(InMemoryEventStore eventStore, Guid streamId, int streamRevision, List<EventMessage> committedEventsList, List<EventMessage> uncommittedEventsList)
		{
			Contract.Requires(eventStore != null);
			Contract.Requires(committedEventsList != null);
			Contract.Requires(uncommittedEventsList != null);
			Contract.Requires(streamId != Guid.NewGuid());

			this.committedEventsList = committedEventsList;
			this.streamId = streamId;
			this.streamRevision = streamRevision;
			this.uncommittedEventsList = uncommittedEventsList;
			this.eventStore = eventStore;
		}

		public void Add<TEvent>(TEvent body, Action<IDictionary<string, object>> initHeaders)
		{
			Contract.Requires(body != null);

			EnsureNotDisposed();

			var uncommittedMessage = EventMessage.CreateMessage(body, initHeaders);

			uncommittedEventsList.Add(uncommittedMessage);
		}

		public void Add<TEvent>(Action<TEvent> initMessage, Action<IDictionary<string, object>> initHeaders = null)
		{
			EnsureNotDisposed();

			var evObject = EventCreator.CreateInstanceOf(initMessage);

			Add(evObject, initHeaders);
		}

		public async Task CommitChanges()
		{
			EnsureNotDisposed();

			await eventStore.CommitStream(streamId, streamRevision, uncommittedEventsList);

			var newStreamRevision = streamRevision == -1 ? 1 : streamRevision + 1;
			committedEventsList.AddRange(uncommittedEventsList);
			uncommittedEventsList.Clear();
			streamRevision = newStreamRevision;
		}

		private void EnsureNotDisposed()
		{
			if (disposed)
				throw new ObjectDisposedException("InMemoryEventStream");
		}

		public void Dispose()
		{
			uncommittedEventsList.Clear();

			disposed = true;
		}
	}
}
