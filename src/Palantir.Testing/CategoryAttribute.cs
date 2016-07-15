namespace Palantir.Testing
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Xunit.Sdk;

	[TraitDiscoverer("CategoryDiscoverer", "TraitExtensibility")]
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
	public sealed class CategoryAttribute : Attribute, ITraitAttribute
	{
		public CategoryAttribute(string category) { }
	}
}
