namespace Palantir.Mvc.NServiceBus
{
	using System;
	using global::NServiceBus;

	public interface ICorrelatedEvent : IEvent
	{
		string CorrelationId { get; set; }
    }
}