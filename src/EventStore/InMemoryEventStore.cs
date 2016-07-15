namespace Palantir.EventStore
{
	using Palantir.Messaging;
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Collections.Immutable;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using PagedList;
	using Microsoft.Owin.Infrastructure;

	public sealed class InMemoryEventStore : IEventStore
	{
		private readonly IBus bus;
		private readonly ISystemClock clock;
		private Dictionary<Guid, EventStreamInfo> eventStreams = new Dictionary<Guid, EventStreamInfo>();
		private ReaderWriterLockSlim eventStreamsLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

		private class EventStreamInfo : IEventStreamInfo
		{
			private List<EventMessage> messages = new List<EventMessage>();

			public EventStreamInfo(Guid streamId, DateTimeOffset dateCreated, int streamRevision, IEnumerable<EventMessage> messages)
			{
				StreamId = streamId;
				StreamRevision = streamRevision;
				this.messages = messages.ToList();
				DateCreatedUtc = dateCreated;
				LastModifiedDateUtc = dateCreated;
			}

			public int StreamRevision { get; set; }
			public DateTimeOffset DateCreatedUtc { get; }
			public List<EventMessage> Messages => messages;
			public DateTimeOffset LastModifiedDateUtc { get; set; }
			public Guid StreamId { get; }
			public int EventCount => messages.Count();
		}

		public InMemoryEventStore(ISystemClock systemClock, IBus bus = null)
		{
			this.bus = bus;
			this.clock = systemClock;
		}

		public Task<IEventStream> OpenStream(Guid streamId, int minVersion = int.MinValue, int maxVersion = int.MaxValue)
		{
			eventStreamsLock.EnterReadLock();
			try
			{
				IEventStream eventStream;
				if (!eventStreams.ContainsKey(streamId))
					eventStream = new InMemoryEventStream(this, streamId, -1, new List<EventMessage>());
				else
				{
					var eventStreamInfo = eventStreams[streamId];

					eventStream = new InMemoryEventStream(this, streamId, eventStreamInfo.StreamRevision, new List<EventMessage>(eventStreamInfo.Messages));
				}

				return Task.FromResult(eventStream);
			}
			finally
			{
				eventStreamsLock.ExitReadLock();
			}
		}

		internal Task CommitStream(Guid streamId, int streamRevision, IEnumerable<EventMessage> uncommittedEvents)
		{
			eventStreamsLock.EnterWriteLock();
			try
			{
				if (!eventStreams.ContainsKey(streamId))
					eventStreams.Add(streamId, new EventStreamInfo(streamId, clock.UtcNow, 1, uncommittedEvents));
				else
				{
					var existingStreamInfo = eventStreams[streamId];
					if (streamRevision != existingStreamInfo.StreamRevision)
						throw new ConcurrencyException(SR.Err_EventStreamChangedSinceCreation());

					existingStreamInfo.StreamRevision++;
					existingStreamInfo.Messages.AddRange(uncommittedEvents);
					existingStreamInfo.LastModifiedDateUtc = clock.UtcNow;
				}
			}
			finally
			{
				eventStreamsLock.ExitWriteLock();
			}

			if (bus != null)
				foreach (var eventMessage in uncommittedEvents)
					bus.Publish(eventMessage.EventType, eventMessage.Body, eventMessage.Headers);

			return Task.FromResult(0);
		}

		public Task<PagedList<IEventStreamInfo>> Find(Func<IEventStreamInfo, bool> predicate, int page = 1, int pageSize = 50)
		{
			eventStreamsLock.EnterReadLock();
			try
			{
				var query = eventStreams.Values.Where(predicate);

				var list = new PagedList<IEventStreamInfo>(query, page, pageSize);

				return Task.FromResult(list);
			}
			finally
			{
				eventStreamsLock.ExitReadLock();
			}
		}
	}
}
