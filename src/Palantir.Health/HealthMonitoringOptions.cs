namespace Palantir.Health
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Provides programattic configuration used by <see cref="IHealthMonitoringService"/> and <see cref="IHealthMonitoringPolicyProvider"/>.
	/// </summary>
	public sealed class HealthMonitoringOptions
    {
		private IDictionary<string, HealthMonitoringPolicy> PolicyMap { get; } = new Dictionary<string, HealthMonitoringPolicy>(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// The initial default policy is to monitor basic system variables.
		/// </summary>
		public static HealthMonitoringPolicy DefaultPolicy { get; set; } = new HealthMonitoringPolicyBuilder().MonitorDiskSpace().Build();

		/// <summary>
		/// Add a health monitoring policy with the provided name.
		/// </summary>
		/// <param name="name">The name of the policy.</param>
		/// <param name="policy">The health monitoring policy.</param>
		public void AddPolicy(string name, HealthMonitoringPolicy policy)
		{
			Contract.Requires(name != null);
			Contract.Requires(policy != null);

			PolicyMap[name] = policy;
		}
		
		/// <summary>
		/// Add a policy that is built from a delegate with the provided name.
		/// </summary>
		/// <param name="name">The name of the policy.</param>
		/// <param name="configurePolicy">The delegate that will be used to build the policy.</param>
		public void AddPolicy(string name, Action<HealthMonitoringPolicyBuilder> configurePolicy)
		{
			Contract.Requires(name != null);
			Contract.Requires(configurePolicy != null);

			var policyBuilder = new HealthMonitoringPolicyBuilder();
			configurePolicy(policyBuilder);
			PolicyMap[name] = policyBuilder.Build();
		}

		/// <summary>
		/// Fetches all the policies for the options.
		/// </summary>
		/// <returns>The policies.</returns>
		public IReadOnlyDictionary<string, HealthMonitoringPolicy> GetAllPolicies()
		{
			return new ReadOnlyDictionary<string, HealthMonitoringPolicy>(PolicyMap);
		}

		/// <summary>
		/// Returns the policy for the specified name, or null if a policy with the name does not exist.
		/// </summary>
		/// <param name="name">The name of the policy to return.</param>
		/// <returns>The policy for the specified name, or null if a policy with the name does not exist.</returns>
		public HealthMonitoringPolicy GetPolicy(string name)
		{
			Contract.Requires(name != null);

			return PolicyMap.ContainsKey(name) ? PolicyMap[name] : null;
		}
	}
}
