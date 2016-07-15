namespace Palantir.Testing
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Xunit.Sdk;

	[TraitDiscoverer("PriorityDiscoverer", "TraitExtensibility")]
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
	public sealed class PriorityAttribute : Attribute, ITraitAttribute
	{
		public PriorityAttribute(int priority) { }
	}
}
