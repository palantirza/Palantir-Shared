namespace Palantir.Mvc.MultiTenancy
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Require a controller or action to have a tenant. 404 if a tenant isn't set.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class RequireTenantAttribute : Attribute
    {
		// TODO: Implement, maybe look at https://github.com/YoloDev/YoloDev.MultiTenant for an implementation
		// Or perhaps https://github.com/joeaudette/cloudscribe
	}
}
