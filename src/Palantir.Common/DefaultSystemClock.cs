namespace Palantir
{
	using Microsoft.Owin.Infrastructure;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class DefaultSystemClock : ISystemClock
	{
		public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
	}
}
