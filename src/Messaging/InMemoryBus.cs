namespace Palantir.Messaging
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.Extensions.DependencyInjection;
	using System.Reflection;
	public sealed class InMemoryBus : IBus
	{
		private Dictionary<Type, List<Type>> subscribers = new Dictionary<Type, List<Type>>();
		private object subscribersLock = new object();
		private IServiceProvider serviceProvider;
		private IServiceCollection serviceCollection;
		private Type handlerInterfaceType = typeof(IEventHandler<>);

		public InMemoryBus(IServiceCollection serviceCollection)
		{
			this.serviceCollection = serviceCollection;
		}

		public async Task Publish<TEvent>(Action<TEvent> ev, Action<IDictionary<string, object>> headersInit = null)
		{
			var evObject = EventCreator.CreateInstanceOf(ev);

			await Publish(evObject, headersInit);
		}

		public async Task Publish<TEvent>(TEvent ev, Action<IDictionary<string, object>> headersInit = null)
		{
			var eventType = typeof(TEvent);
			Dictionary<string, object> headers = null;
			if (headersInit != null)
				headersInit(headers = new Dictionary<string, object>());

			Publish(eventType, ev, headers);
		}

		public void Publish(Type eventType, object body, IDictionary<string, object> headers = null)
		{
			lock (subscribersLock)
			{
				if (subscribers.ContainsKey(eventType))
				{
					var handlerTypes = subscribers[eventType];
					foreach (var handlerType in handlerTypes)
					{
						var handler = ServiceProvider.GetService(handlerType);
						var interfaceType = handlerInterfaceType.MakeGenericType(eventType);
						interfaceType.InvokeMember("Handle", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public, Type.DefaultBinder, handler, new[] { body, new EventContext(headers) });
					}
				}
			}
		}

		private IServiceProvider ServiceProvider
		{
			get
			{
				if (serviceProvider == null)
					serviceProvider = serviceCollection.BuildServiceProvider();

				return serviceProvider;
			}
		}

		public async Task Send<TCommand>(TCommand command)
		{
		}

		public async Task Send<TCommand>(Action<TCommand> command)
		{
			var cmdObject = EventCreator.CreateInstanceOf<TCommand>(command);

			await Send(cmdObject);
		}

		public void Subscribe<THandler>() where THandler: class
		{
			var eventHandler = typeof(IEventHandler<>);
			var handlerType = typeof(THandler);
			foreach (var iface in handlerType.GetInterfaces())
			{
				if (iface.Name == "IEventHandler`1" && iface.GetGenericTypeDefinition() == eventHandler)
				{
					var eventType = iface.GetGenericArguments()[0];

					lock (subscribersLock)
					{
						var exists = (from s in subscribers
									  from h in s.Value
									  where h == handlerType
									  select h).Any();
						if (!exists)
						{
							serviceCollection.AddTransient<THandler>();
							serviceProvider = null;
						}

						List<Type> handlers;
						if (subscribers.ContainsKey(eventType))
							handlers = subscribers[eventType];
						else
						{
							handlers = new List<Type>();
							subscribers.Add(eventType, handlers);
						}

						handlers.Add(handlerType);
					}
				}
			}
		}
	}
}
