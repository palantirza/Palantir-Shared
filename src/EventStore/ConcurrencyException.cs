namespace Palantir.EventStore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

    public sealed class ConcurrencyException : ApplicationException
    {
		public ConcurrencyException(string message)
			: base(message)
		{
		}
    }
}
