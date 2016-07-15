namespace Palantir.Policy.Reporting
{
	using Newtonsoft.Json;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// A policy report.
	/// </summary>
	public sealed class PolicyReport
    {
		private readonly bool achieved;
		private readonly IReadOnlyDictionary<string, ServiceReport> services;

		/// <summary>
		/// Initializes a new instance of the policy report.
		/// </summary>
		/// <param name="policy">The service level policy.</param>
		/// <param name="serviceRecords">The service records.</param>
		public PolicyReport(Predicate<ServiceLevelRecord> policy, IDictionary<string, ServiceLevelRecord> serviceRecords)
		{
			Contract.Requires(policy != null);
			Contract.Requires(serviceRecords != null);

			services = new ReadOnlyDictionary<string, ServiceReport>(serviceRecords.ToDictionary(x => x.Key, x => new ServiceReport(policy, x.Value)));
			achieved = services.Values.All(x => x.Achieved);
		}

		public bool Achieved => achieved;

		public IReadOnlyDictionary<string, ServiceReport> Services => services;
	}
}
