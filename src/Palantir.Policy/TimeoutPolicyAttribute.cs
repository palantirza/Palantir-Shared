namespace Palantir.Policy
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Timeouts are controls that specify the longest a given operation should execute for.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class TimeoutPolicyAttribute : Attribute
	{
		public TimeoutPolicyAttribute(string policy)
		{

		}
	}
}
