using Palantir.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Palantir.EventStore
{
	public sealed class EventMessage
	{
		private readonly object body;
		private readonly Type eventType;
		private readonly ImmutableDictionary<string, object> headers;
		private readonly Guid eventId;

		private EventMessage(Type eventType, object body, ImmutableDictionary<string, object> headers)
		{
			if (eventType.IsClass || eventType.IsValueType)
				throw new NotSupportedException(SR.Err_EventMessageCannotBeConcreteType(eventType));

			this.eventType = eventType;
			this.headers = headers;
			this.body = body;
			this.eventId = Guid.NewGuid();
		}

		public IDictionary<string, object> Headers => headers;

		public object Body => body;

		public Type EventType => eventType;

		public Guid EventId => eventId;

		public TEvent GetBody<TEvent>()
		{
			if (eventType == typeof(TEvent))
				return (TEvent)body;

			throw new InvalidOperationException(SR.Err_EventMessageNotOfIndicatedType(eventType, typeof(TEvent)));
		}

		public static EventMessage CreateMessage<TEvent>(TEvent body, Action<IDictionary<string, object>> initHeaders = null)
		{
			Type eventType = typeof(TEvent);

			var headers = ImmutableDictionary<string, object>.Empty;
			if (initHeaders != null)
			{
				var builder = headers.ToBuilder();

				initHeaders(builder);

				headers = builder.ToImmutable();
			}

			return new EventMessage(eventType, body, headers);
		}

		public static EventMessage CreateMessage<TEvent>(Action<TEvent> initBody, Action<IDictionary<string, object>> initHeaders = null)
		{
			var body = EventCreator.CreateInstanceOf(initBody);

			return CreateMessage(body, initHeaders);
		}
	}
}