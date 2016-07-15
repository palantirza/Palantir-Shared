namespace Palantir.Messaging
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	public interface IEventHandler<TEvent>
    {
		Task Handle(TEvent ev, EventContext eventContext);
    }
}
