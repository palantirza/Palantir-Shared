namespace Palantir.Health
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Provides access to health monitoring policies.
	/// </summary>
    public interface IHealthMonitoringPolicyProvider
    {
		/// <summary>
		/// Fetches the policy with the specified name.
		/// </summary>
		/// <param name="policyName">The policy name.</param>
		/// <returns>The matching policy, or null.</returns>
		Task<HealthMonitoringPolicy> GetPolicyAsync(string policyName);

		/// <summary>
		/// Fecthes all the policies.
		/// </summary>
		/// <returns>The policies.</returns>
		Task<IReadOnlyDictionary<string, HealthMonitoringPolicy>> GetAllPoliciesAsync();
    }
}
