namespace Palantir.UnitTests
{
	using EventStore;
	using FluentAssertions;
	using Messaging;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Xunit;

	public sealed class MemoryEventStreamTests
    {
		private class TestEvent : IEvent
		{
			public Guid AggregateId { get; set; }
			public Guid EventId { get; set; }
		}

		[Fact]
		public void GetStream_ThatDoesntExist_ShouldReturnNewStream()
		{
			var eventStore = new InMemoryEventStore(new DefaultSystemClock());
			var stream = eventStore.OpenStream(Guid.NewGuid()).Result;

			stream.Should().NotBeNull();
			stream.UncommittedEvents.Count().Should().Be(0);
			stream.CommittedEvents.Count().Should().Be(0);
			stream.StreamRevision.Should().Be(-1);
		}

		[Fact]
		public void AddCommit_ToNewStream_ShouldCreateStream()
		{
			var id = Guid.NewGuid();
			var eventStore = new InMemoryEventStore(new DefaultSystemClock());
			var stream = eventStore.OpenStream(id).Result;
			stream.Add<IEvent>(new TestEvent());
			stream.CommitChanges().Wait();

			var stream1 = eventStore.OpenStream(id).Result;
			stream1.CommittedEvents.Count().Should().Be(1);
			stream1.StreamRevision.Should().Be(1);
			stream1.UncommittedEvents.Count().Should().Be(0);
		}

		[Fact]
		public void AddCommit_ToExistingStream_ShouldUpdateStream()
		{
			var id = Guid.NewGuid();
			var eventStore = new InMemoryEventStore(new DefaultSystemClock());
			var stream = eventStore.OpenStream(id).Result;
			stream.Add<IEvent>(new TestEvent());
			stream.Add<IEvent>(new TestEvent());
			stream.CommitChanges().Wait();

			var stream1 = eventStore.OpenStream(id).Result;
			stream1.Add<IEvent>(new TestEvent());
			stream1.CommitChanges().Wait();

			var stream2 = eventStore.OpenStream(id).Result;
			stream2.CommittedEvents.Count().Should().Be(3);
			stream2.StreamRevision.Should().Be(2);
			stream2.UncommittedEvents.Count().Should().Be(0);
		}

		[Fact]
		public void AddCommit_ToChangedStream_ShouldError()
		{
			var id = Guid.NewGuid();
			var eventStore = new InMemoryEventStore(new DefaultSystemClock());
			var stream = eventStore.OpenStream(id).Result;
			stream.Add<IEvent>(new TestEvent());
			stream.Add<IEvent>(new TestEvent());
			stream.CommitChanges().Wait();

			var stream1 = eventStore.OpenStream(id).Result;
			var stream2 = eventStore.OpenStream(id).Result;
			stream1.Add<IEvent>(new TestEvent());
			stream1.CommitChanges().Wait();

			stream2.Add<IEvent>(new TestEvent());

			//TODO: Change to ShouldThrow
			try
			{
				stream2.CommitChanges().Wait();
			}
			catch (AggregateException aggException)
			{
				if (aggException.InnerExceptions.Count() == 1 && (aggException.InnerExceptions[0] is EventStore.ConcurrencyException))
				{
					return;
				}
			}
			catch (EventStore.ConcurrencyException)
			{
				return;
			}

			throw new Exception("Concurrency exception not thrown as expected");
			//Action action = () => stream2.CommitChanges(eventStore).Wait();
			//action.ShouldThrow<EventStore.ConcurrencyException>();
		}
	}
}
