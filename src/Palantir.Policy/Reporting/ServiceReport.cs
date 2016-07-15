namespace Palantir.Policy.Reporting
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// A service report.
	/// </summary>
	public sealed class ServiceReport
    {
		private readonly bool achieved;
		private readonly ServiceLevelRecord detail;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceReport"/> class.
		/// </summary>
		/// <param name="policy">The service level policy.</param>
		/// <param name="record">The service level record.</param>
		internal ServiceReport(Predicate<ServiceLevelRecord> policy, ServiceLevelRecord record)
		{
			Contract.Requires(policy != null);
			Contract.Requires(record != null);

			achieved = policy(record);
			this.detail = record;
		}

		public bool Achieved => achieved;

		/// <summary>
		/// The service level record.
		/// </summary>
		public ServiceLevelRecord Detail => detail;
    }
}
