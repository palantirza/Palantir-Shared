namespace Palantir.UnitTests
{
	using Messaging;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Threading;
	using Xunit;
	using FluentAssertions;
	using Microsoft.Extensions.DependencyInjection;
	using Palantir.Messaging;
	using System.Diagnostics;
	using Testing;
	public interface IEvent1
	{
		string Text { get; set; }
	}
	public class EventImpl : IEvent1 {
		public string Text { get; set; }
	}

	public class EventHandler : IEventHandler<IEvent1>, IEventHandler<IEvent>
	{
		private IService service;

		public EventHandler(IService service)
		{
			this.service = service;
		}

		public Task Handle(IEvent ev, EventContext eventContext)
		{
			throw new NotImplementedException();
		}

		public async Task Handle(IEvent1 ev, EventContext eventContext)
		{
			service.Fired = true;
		}
	}

	public interface IService
	{
		bool Fired { get; set; }
	}

	public class Service : IService
	{
		public bool Fired { get; set; }
	}

	[Category("CI"), Time("Fast")]
	public sealed class BusPublishSubscribeTests
	{
		[Fact]
		public void PublishedEvent_ShouldBeHandled()
		{
			var serviceCollection = new ServiceCollection();
			var service = new Service();
			serviceCollection.AddSingleton<IService, Service>(x => service);

			var bus = new InMemoryBus(serviceCollection);
			bus.Subscribe<EventHandler>();

			bus.Publish<IEvent1>(x => x.Text = "Test").Wait();

			service.Fired.Should().BeTrue();
		}
	}
}
