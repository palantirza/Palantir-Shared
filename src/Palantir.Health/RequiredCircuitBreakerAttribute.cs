namespace Palantir.Health
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Indicates a class or method is controlled by a specific breaker. If tripped, calls to the action will fail immediately.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class RequiredCircuitBreakerAttribute : Attribute
    {
		public RequiredCircuitBreakerAttribute(params string[] breakers)
		{

		}
    }
}
