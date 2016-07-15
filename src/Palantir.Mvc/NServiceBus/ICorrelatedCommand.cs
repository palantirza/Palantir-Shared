namespace Palantir.Mvc.NServiceBus
{
	using System;
	using global::NServiceBus;

	public interface ICorrelatedCommand : ICommand
	{
		string CorrelationId { get; set; }
    }
}