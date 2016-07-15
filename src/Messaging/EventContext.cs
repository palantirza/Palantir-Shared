namespace Palantir.Messaging
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	public sealed class EventContext
    {
		public EventContext(IDictionary<string, object> headers = null)
		{
			Headers = headers ?? new Dictionary<string, object>();
			CancellationToken = CancellationToken.None;
		}

		public CancellationToken CancellationToken { get; }
		public IDictionary<string, object> Headers { get; }
    }
}
