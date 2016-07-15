namespace Palantir.Testing
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Xunit.Sdk;

	[TraitDiscoverer("TimeDiscoverer", "TraitExtensibility")]
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
	public sealed class TimeAttribute : Attribute, ITraitAttribute
	{
		public TimeAttribute(string timeTaken) { }
	}
}
