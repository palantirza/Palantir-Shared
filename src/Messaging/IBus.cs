namespace Palantir.Messaging
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

    public interface IBus
    {
		Task Send<TCommand>(Action<TCommand> command);

		Task Send<TCommand>(TCommand command);

		Task Publish<TEvent>(TEvent ev, Action<IDictionary<string, object>> headersInit = null);

		Task Publish<TEvent>(Action<TEvent> ev, Action<IDictionary<string, object>> headersInit = null);

		void Publish(Type eventType, object body, IDictionary<string, object> headers = null);

		void Subscribe<THandler>() where THandler: class;
	}
}