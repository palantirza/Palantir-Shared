namespace Palantir.Policy
{
	using Reporting;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Represents a service level policy.
	/// </summary>
	public sealed class ServiceLevelOptions
    {
		private IDictionary<string, Predicate<ServiceLevelRecord>> PolicyMap { get; } = new Dictionary<string, Predicate<ServiceLevelRecord>>(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// Add a service level policy with the provided name.
		/// </summary>
		/// <param name="name">The name of the policy.</param>
		/// <param name="policy">The service level policy.</param>
		public void AddPolicy(string name, Predicate<ServiceLevelRecord> policy)
		{
			Contract.Requires(name != null);
			Contract.Requires(policy != null);

			PolicyMap[name] = policy;
		}

		/// <summary>
		/// Fetches all the policies for the options.
		/// </summary>
		/// <returns>The policies.</returns>
		public IReadOnlyDictionary<string, Predicate<ServiceLevelRecord>> GetAllPolicies()
		{
			return new ReadOnlyDictionary<string, Predicate<ServiceLevelRecord>>(PolicyMap);
		}

		/// <summary>
		/// Returns the policy for the specified name, or null if a policy with the name does not exist.
		/// </summary>
		/// <param name="name">The name of the policy to return.</param>
		/// <returns>The policy for the specified name, or null if a policy with the name does not exist.</returns>
		public Predicate<ServiceLevelRecord> GetPolicy(string name)
		{
			Contract.Requires(name != null);

			return PolicyMap.ContainsKey(name) ? PolicyMap[name] : null;
		}
	}
}
