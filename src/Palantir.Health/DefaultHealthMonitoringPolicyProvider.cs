namespace Palantir.Health
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;

    public sealed class DefaultHealthMonitoringPolicyProvider : IHealthMonitoringPolicyProvider
	{
		private readonly HealthMonitoringOptions options;

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultHealthMonitoringPolicyProvider"/> class.
		/// </summary>
		/// <param name="options">The health monitoring options.</param>
		public DefaultHealthMonitoringPolicyProvider(IOptions<HealthMonitoringOptions> options)
		{
			if (options == null)
				throw new ArgumentNullException(nameof(options));

			this.options = options.Value;
		}

		/// <summary>
		/// Gets all policies.
		/// </summary>
		/// <returns>The policies.</returns>
		public Task<IReadOnlyDictionary<string, HealthMonitoringPolicy>> GetAllPoliciesAsync()
		{
			return Task.FromResult(options.GetAllPolicies());
		}

		/// <summary>
		/// Fetches the policy with the specified name.
		/// </summary>
		/// <param name="policyName">The policy name.</param>
		/// <returns>The matching policy, or null.</returns>
		public Task<HealthMonitoringPolicy> GetPolicyAsync(string policyName)
		{
			return Task.FromResult(options.GetPolicy(policyName));
		}
	}
}
