namespace Palantir.Policy
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Retires indicate how many times the operation should be retried, and wait periods in between.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class RetryPolicyAttribute : Attribute
	{
		public RetryPolicyAttribute(string policy)
		{

		}
	}
}
